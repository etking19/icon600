using Datapath.RGBEasy;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WcfServiceLibrary1;
using WindowsFormClient.RgbInput;

namespace WindowsFormClient.Server
{
    public class ServerVisionHelper
    {
        private static ServerVisionHelper sInstance = null;
        private string rgbExecutablePath;

        private static readonly Random getrandom = new Random();

        private ServerVisionHelper()
        {

        }

        public static ServerVisionHelper getInstance()
        {
            if (sInstance == null)
            {
                sInstance = new ServerVisionHelper();
            }

            return sInstance;
        }

        public void InitializeVisionDB()
        {
            // get the installed rgb executable path
            //if (Properties.Settings.Default.VisionPath == string.Empty)
            //{
                // auto search the rgb exe
                foreach (String matchPath in Utils.Files.DirSearch(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "rgbxcmd.com"))
                {
                    rgbExecutablePath = matchPath;
                    break;
                }

                if (rgbExecutablePath == String.Empty)
                {
                    foreach (String matchPath in Utils.Files.DirSearch(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "rgbxcmd.com"))
                    {
                        rgbExecutablePath = matchPath;
                        break;
                    }
                }

                //Properties.Settings.Default.VisionPath = rgbExecutablePath;
                //Properties.Settings.Default.Save();
            //}

            // check if the db has data
            // if have return and do nothing, else proceed initialization
            if (GetAllVisionInputs().Count() > 0)
            {
                Trace.WriteLine("Already initialize");
                return;
            }

            // get number of input from API
            uint numberInputs = GetNumberOfInputs();
            if(numberInputs == 0)
            {
                Trace.WriteLine("Failed to get input count: initialize");
                return;
            }

            // contruct default input entry in DB
            RGBERROR error = RGBERROR.NO_ERROR;
            for (uint i = 1; i <= numberInputs; i++ )
            {
                // get the input type
                SIGNALTYPE signalType;
                uint captureWidth;
                uint captureHeight;
                uint refreshRate;
                if((error = RGB.GetInputSignalType(i, out signalType, out captureWidth, out captureHeight, out refreshRate)) != RGBERROR.NO_ERROR)
                {
                    Trace.WriteLine("failed to get signal of input: " + i);
                }

                IntPtr hRgb;
                uint croppingMode = 0;
                int top = 0;
                int left = 0;
                uint width = 0;
                uint height = 0;
                if((error = RGB.OpenInput(i, out hRgb)) != RGBERROR.NO_ERROR)
                {
                    if((error = RGB.IsCroppingEnabled(hRgb, out croppingMode)) != RGBERROR.NO_ERROR)
                    {
                        RGB.GetCropping(hRgb, out top, out left, out width, out height);
                    }
                    
                    // clean up
                    RGB.CloseInput(hRgb);
                }
                
                Input visionInput = new Input()
                {
                    InputNumber = i,
                    LabelName = String.Format("{0}:{1}", System.Enum.GetName(typeof(Input.EInputType), signalType), i),
                    InputCaptureWidth = captureWidth,
                    InputCaptureHeight = captureHeight,
                    InputCropping = croppingMode > 0 ? true:false,
                    InputCropLeft = left,
                    InputCropTop = top,
                    InputCropWidth = width,
                    InputCropHeight = height
                };

                // get windows and OSD details
                Window visionWnd = new Window();
                OnScreenDisplay visionOSD = new OnScreenDisplay();

                AddVisionInput(visionWnd, visionInput, visionOSD);
            }

            ServerDbHelper.GetInstance().AddSystemInputCount((int)numberInputs);
        }

        public uint GetNumberOfInputs()
        {
            uint numberInputs = 0;
            try
            {
                RGBERROR error = RGB.GetNumberOfInputs(out numberInputs);
                if (error != RGBERROR.NO_ERROR)
                {
                    Trace.WriteLine("failed to get number of inputs");
                    return 0;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return numberInputs;
        }

        public List<Tuple<int, Window, Input, OnScreenDisplay>> GetAllVisionInputs()
        {
            List<Tuple<int, Window, Input, OnScreenDisplay>> result = new List<Tuple<int, Window, Input, OnScreenDisplay>>();
            
            // get data from database
            List<VisionData> visionList = new List<VisionData>(ServerDbHelper.GetInstance().GetAllVisionInputs());
            foreach (VisionData data in visionList)
            {
                System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Window));
                TextReader wndReader = new StringReader(data.windowStr);
                
                System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Input));
                TextReader inputReader = new StringReader(data.inputStr);

                System.Xml.Serialization.XmlSerializer osdSerializer = new System.Xml.Serialization.XmlSerializer(typeof(OnScreenDisplay));
                TextReader osdReader = new StringReader(data.osdStr);

                Tuple<int, Window, Input, OnScreenDisplay> resultData = new Tuple<int, Window, Input, OnScreenDisplay>
                (
                    data.id,
                    (Window)wndSerializer.Deserialize(wndReader),
                    (Input)inputSerializer.Deserialize(inputReader),
                    (OnScreenDisplay)osdSerializer.Deserialize(osdReader)
                );

                result.Add(resultData);
            }

            return result;
        }

        public List<InputAttributes> GetAllVisionInputsAttributes()
        {
            List<InputAttributes> attributesList = new List<InputAttributes>();
            foreach (Tuple<int, Window, Input, OnScreenDisplay> data in GetAllVisionInputs())
            {
                InputAttributes inputAttr = new InputAttributes()
                {
                    InputId = data.Item1,
                    DisplayName = data.Item3.LabelName,
                };

                attributesList.Add(inputAttr);
            }

            return attributesList;
        }

        public bool AddVisionInput(Window window, Input input, OnScreenDisplay osd)
        {
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(window.GetType());
            StringWriter wndTextWriter = new StringWriter();
            wndSerializer.Serialize(wndTextWriter, window);

            System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(input.GetType());
            StringWriter inputTextWriter = new StringWriter();
            inputSerializer.Serialize(inputTextWriter, input);

            System.Xml.Serialization.XmlSerializer osdSerializer = new System.Xml.Serialization.XmlSerializer(osd.GetType());
            StringWriter osdTextWriter = new StringWriter();
            osdSerializer.Serialize(osdTextWriter, osd);
            
            return ServerDbHelper.GetInstance().AddVisionInput(wndTextWriter.ToString(), inputTextWriter.ToString(), osdTextWriter.ToString());
        }

        public bool EditVisionInput(uint id, Window window, Input input, OnScreenDisplay osd)
        {
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(window.GetType());
            StringWriter wndTextWriter = new StringWriter();
            wndSerializer.Serialize(wndTextWriter, window);

            System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(input.GetType());
            StringWriter inputTextWriter = new StringWriter();
            inputSerializer.Serialize(inputTextWriter, input);

            System.Xml.Serialization.XmlSerializer osdSerializer = new System.Xml.Serialization.XmlSerializer(osd.GetType());
            StringWriter osdTextWriter = new StringWriter();
            osdSerializer.Serialize(osdTextWriter, osd);

            return ServerDbHelper.GetInstance().EditVisionInput((int)id, wndTextWriter.ToString(), inputTextWriter.ToString(), osdTextWriter.ToString());
        }

        public bool RemoveVisionInput(uint id)
        {
            return ServerDbHelper.GetInstance().RemoveVisionInput((int)id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns> window main module id matched with wnd id in client mimic screen</returns>
        public int LaunchVisionWindow(int dbId)
        {
            int returnValue = -1;
            // get the info from db
            var result = ServerDbHelper.GetInstance().GetAllVisionInputs().First(t => t.id == dbId);
            if (result == null)
            {
                Trace.WriteLine("unable to launch vision window with db id: " + dbId);
                return returnValue;
            }

            // create the lauching parameters
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Window));
            TextReader wndReader = new StringReader(result.windowStr);
            Window window = (Window)wndSerializer.Deserialize(wndReader);

            System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Input));
            TextReader inputReader = new StringReader(result.inputStr);
            Input input = (Input)inputSerializer.Deserialize(inputReader);

            System.Xml.Serialization.XmlSerializer osdSerializer = new System.Xml.Serialization.XmlSerializer(typeof(OnScreenDisplay));
            TextReader osdReader = new StringReader(result.osdStr);
            OnScreenDisplay osd = (OnScreenDisplay)osdSerializer.Deserialize(osdReader);

            // construct the param list
            string argumentList = string.Empty;
            argumentList += constructWinParams(window);
            argumentList += constructInputParams(input);
            argumentList += constructOSDParams(osd);
            argumentList += string.Format("-ID={0} ", getrandom.Next(1, 65535));

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = rgbExecutablePath;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = argumentList;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    returnValue = exeProcess.Id;
                }
            }
            catch
            {
                // Log error.
            }

            return returnValue;
        }

        public int LaunchVisionWindow(int dbId, int left, int top, int width, int height)
        {
            int returnValue = -1;
            // get the info from db
            var result = ServerDbHelper.GetInstance().GetAllVisionInputs().First(t => t.id == dbId);
            if (result == null)
            {
                Trace.WriteLine("unable to launch vision window with db id: " + dbId);
                return returnValue;
            }

            // create the lauching parameters
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Window));
            TextReader wndReader = new StringReader(result.windowStr);
            Window window = (Window)wndSerializer.Deserialize(wndReader);
            
            /* TODO: matched the latest pos
            // modify to match
            window.WndPostLeft = left;
            window.WndPosTop = top;
            window.WndPostWidth = width;
            window.WndPosHeight = height;
            */

            System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Input));
            TextReader inputReader = new StringReader(result.inputStr);
            Input input = (Input)inputSerializer.Deserialize(inputReader);

            System.Xml.Serialization.XmlSerializer osdSerializer = new System.Xml.Serialization.XmlSerializer(typeof(OnScreenDisplay));
            TextReader osdReader = new StringReader(result.osdStr);
            OnScreenDisplay osd = (OnScreenDisplay)osdSerializer.Deserialize(osdReader);

            // construct the param list
            string argumentList = string.Empty;
            argumentList += constructWinParams(window);
            argumentList += constructInputParams(input);
            argumentList += constructOSDParams(osd);
            argumentList += string.Format("-ID={0} ", getrandom.Next(1, 65535));

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = rgbExecutablePath;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = argumentList;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    if (exeProcess.WaitForInputIdle())
                    {
                        Thread.Sleep(1000);
                        returnValue = Utils.Windows.NativeMethods.GetForegroundWindow().ToInt32();
                    }
                }
            }
            catch(Exception e)
            {
                // Log error.
                Trace.WriteLine(e);
            }

            return returnValue;
        }

        private string constructOSDParams(OnScreenDisplay osd)
        {
            string argumentList = string.Empty;
            argumentList += string.Format("-OSDType={0} ", osd.OSDType == OnScreenDisplay.EDisplayType.NoOnScreenDisplay? "Disabled":"SimpleText");

            argumentList += string.Format("-OSDText={0} ", osd.OSDText);
            argumentList += string.Format("-OSDScaling={0} ", osd.OSDIsScaling?"ScaleWithWindow":"FixedSize");

            Font textFont = new Session.Common.SerializableFont() { SerializeFontAttribute = osd.OSDFont }.FontValue;
            if (null != textFont)
            {
                argumentList += string.Format("-OSDFontName={0} ", textFont.Name.Replace(" ", string.Empty));
                argumentList += string.Format("-OSDFontSize={0} ", (int)textFont.SizeInPoints);

                if (textFont.Style == FontStyle.Regular)
                {
                    argumentList += string.Format("-OSDFontStyle={0} ", "Regular");
                }
                else if (textFont.Style == FontStyle.Bold)
                {
                    argumentList += string.Format("-OSDFontStyle={0} ", "Bold");
                }

                if (textFont.Italic)
                {
                    argumentList += string.Format("-OSDFontItalic={0} ", "On");
                }
                else
                {
                    argumentList += string.Format("-OSDFontItalic={0} ", "Off");
                }

                if (textFont.Strikeout)
                {
                    argumentList += string.Format("-OSDFontStrikeout={0} ", "On");
                }
                else
                {
                    argumentList += string.Format("-OSDFontStrikeout={0} ", "Off");
                }

                if (textFont.Underline)
                {
                    argumentList += string.Format("-OSDFontUnderline={0} ", "On");
                }
                else
                {
                    argumentList += string.Format("-OSDFontUnderline={0} ", "Off");
                }
            }
            

            Color fontColor = System.Drawing.ColorTranslator.FromHtml(osd.OSDColor);
            if (null != fontColor)
            {
                argumentList += string.Format("-OSDFontColour={0},{1},{2} ", fontColor.R, fontColor.G, fontColor.B);
                argumentList += string.Format("-OSDBackground={0} ", osd.OSDBgndMOde == OnScreenDisplay.EBackgroundMode.Opaque ? "Opaque" : "Transparent");
            }
            

            Color bgndColor = System.Drawing.ColorTranslator.FromHtml(osd.OSDBgndColor);
            if (null != bgndColor)
            {
                argumentList += string.Format("-OSDBackgroundColour={0},{1},{2} ", bgndColor.R, bgndColor.G, bgndColor.B);
                argumentList += string.Format("-OSDMargins={0},{1},{2},{3} ", osd.OSDMarginTop, osd.OSDMarginLeft, osd.OSDMarginRight, osd.OSDMarginBottom);
            }
            
            argumentList += string.Format("-OSDTextWrap={0} ", osd.OSDTextWrap?"On":"Off");

            switch(osd.OSDAlignHorizontalMode)
            {
                case OnScreenDisplay.EAlignmentHorizontal.Center:
                    argumentList += string.Format("-OSDHorizontalAlignment={0} ", "Centre");
                    break;
                case OnScreenDisplay.EAlignmentHorizontal.Left:
                    argumentList += string.Format("-OSDHorizontalAlignment={0} ", "Left");
                    break;
                case OnScreenDisplay.EAlignmentHorizontal.Right:
                    argumentList += string.Format("-OSDHorizontalAlignment={0} ", "Right");
                    break;
            }

            switch (osd.OSDAlignVerticalMode)
            {
                case OnScreenDisplay.EAlignmentVertical.Center:
                    argumentList += string.Format("-OSDVerticalAlignment={0} ", "Centre");
                    break;
                case OnScreenDisplay.EAlignmentVertical.Top:
                    argumentList += string.Format("-OSDVerticalAlignment={0} ", "Top");
                    break;
                case OnScreenDisplay.EAlignmentVertical.Bottom:
                    argumentList += string.Format("-OSDVerticalAlignment={0} ", "Bottom");
                    break;
            }

            return argumentList;
        }

        private string constructInputParams(Input input)
        {
            string argumentList = string.Empty;

            argumentList += string.Format("-Input={0} ", input.InputNumber);
            if (input.InputCropping)
            {
                argumentList += string.Format("-Cropping={0},{1},{2},{3},{4} ", "On", input.InputCropTop, input.InputCropLeft, input.InputCaptureWidth, input.InputCropHeight);
            }
            else
            {
                argumentList += string.Format("-Cropping={0} ", "Off");
            }
            argumentList += string.Format("-CaptureWidth={0} ", input.InputCaptureWidth);
            argumentList += string.Format("-CaptureHeight={0} ", input.InputCaptureHeight);

            return argumentList;
        }

        private string constructWinParams(Window window)
        {
            string argumentList = string.Empty;
            argumentList += string.Format("-AlwaysOnTop={0} ", window.WndAlwaysOnTop ? "On" : "Off");

            switch (window.WndAspectRatioMode)
            {
                case Window.EAspectRatioMode.No:
                    argumentList += string.Format("-AspectRatio={0} ", "Off");
                    break;
                case Window.EAspectRatioMode.Source:
                    argumentList += string.Format("-AspectRatio={0} ", "Source");
                    break;
                case Window.EAspectRatioMode.Custom:
                    argumentList += string.Format("-AspectRatio={0},{1},{2} ", "On", window.WndAspectRatioWidth, window.WndAspectRatioHeight);
                    break;
            }

            argumentList += string.Format("-Caption={0} ", window.WndCaption);
            argumentList += string.Format("-Window={0},{1},{2},{3} ", window.WndPosTop, window.WndPostLeft, window.WndPostWidth, window.WndPosHeight);
            
            switch (window.WndStyle)
            {
                case Window.EStyle.Border:
                    argumentList += string.Format("-WindowStyle={0} ", "BorderOnly");
                    break;
                case Window.EStyle.BorderAndTitle:
                    argumentList += string.Format("-WindowStyle={0} ", "BorderAndTitleBar");
                    break;
                case Window.EStyle.NoBorderAndTitle:
                    argumentList += string.Format("-WindowStyle={0} ", "NoBorderOrTitleBar");
                    break;
            }

            argumentList += string.Format("-ShowMenuBar={0} ", window.WndMenuBar ? "On" : "Off");
            argumentList += string.Format("-ExcludeBorders={0} ", window.WndPosExcludeBorder ? "On" : "Off");
            argumentList += string.Format("-DisplayMessageAfter={0} ", window.WndInvalidInputMsgTime);
            
            switch (window.WndCaptureMode)
            {
                case Window.ECaptureFormat.Auto:
                    argumentList += string.Format("-CaptureFormat={0} ", "Automatic");
                    break;
                case Window.ECaptureFormat.EightEightEight:
                    argumentList += string.Format("-CaptureFormat={0} ", "8-8-8");
                    break;
                case Window.ECaptureFormat.FiveFiveFive:
                    argumentList += string.Format("-CaptureFormat={0} ", "5-5-5");
                    break;
                case Window.ECaptureFormat.FiveSixFive:
                    argumentList += string.Format("-CaptureFormat={0} ", "5-6-5");
                    break;
            }
            argumentList += string.Format("-TransferData={0} ", window.WndTransferDataMode == Window.ETransferData.Direct ? "DirectToGraphicsCard" : "ViaSystemMemory");
            argumentList += string.Format("-Scaling={0} ", window.WndScalingMode == Window.EScaling.Fast ? "Fast" : "Slow");
            argumentList += string.Format("-ActiveCaptureRate={0} ", window.WndCaptureRate);
            argumentList += string.Format("-InactiveCaptureRate={0} ", window.WndInactiveCaptureRate);
            argumentList += string.Format("-DifferentCaptureRate={0} ", window.WndDiffCaptureMode ? "On" : "Off");
            
            switch (window.WndDeinterlaceMode)
            {
                case Window.EDeinterlace.Bob:
                    argumentList += string.Format("-Deinterlace={0} ", "Bob");
                    break;
                case Window.EDeinterlace.Weave:
                    argumentList += string.Format("-Deinterlace={0} ", "Weave");
                    break;
                case Window.EDeinterlace.None:
                    argumentList += string.Format("-Deinterlace={0} ", "Field0");
                    break;
            }

            switch (window.WndCursorMode)
            {
                case Window.ECursor.AlwaysHide:
                    argumentList += string.Format("-CursorStyle={0} ", "Hide");
                    break;
                case Window.ECursor.AlwaysShow:
                    argumentList += string.Format("-CursorStyle={0} ", "Show");
                    break;
                case Window.ECursor.HideWhenWndActive:
                    argumentList += string.Format("-CursorStyle={0} ", "HideWhenActive");
                    break;
            }

            return argumentList;
        }
    }
}
