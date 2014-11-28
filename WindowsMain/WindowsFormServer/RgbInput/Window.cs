using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.RgbInput
{
    public class Window
    {
        public enum EAspectRatioMode
        {
            No = 0,
            Source,
            Custom,
        }

        public enum EStyle
        {
            NoBorderAndTitle = 0,
            BorderAndTitle,
            Border,
        }

        public enum ECursor
        {
            AlwaysShow = 0,
            AlwaysHide,
            HideWhenWndActive,
        }

        public enum ECaptureFormat
        {
            Auto = 0,
            FiveFiveFive,
            FiveSixFive,
            EightEightEight,
        }

        public enum EDeinterlace
        {
            Bob = 0,
            Weave,
            None,
        }

        public enum ETransferData
        {
            Direct = 0,
            SystemMemory,
        }

        public enum EScaling
        {
            Fast = 0,
            Slow,
        }

        private int captureRate = 100;
        private int inactiveCaptureRate = 100;

        public int WndPostLeft { get; set; }
        public int WndPosTop { get; set; }
        public int WndPostWidth { get; set; }
        public int WndPosHeight { get; set; }
        public bool WndPosExcludeBorder { get; set; }
        public EAspectRatioMode WndAspectRatioMode { get; set; }
        public int WndAspectRatioWidth { get; set; }
        public int WndAspectRatioHeight { get; set; }
        public EStyle WndStyle { get; set; }
        public bool WndMenuBar { get; set; }
        public bool WndAlwaysOnTop { get; set; }
        public ECursor WndCursorMode { get; set; }

        private string defaultWndCaption = "Input Window";
        public string WndCaption 
        { 
            get
            {
                return defaultWndCaption;
            }
            set
            {
                defaultWndCaption = value;
            }
        }
        public int WndInvalidInputMsgTime { get; set; }
        public ECaptureFormat WndCaptureMode { get; set; }
        public EDeinterlace WndDeinterlaceMode { get; set; }
        public ETransferData WndTransferDataMode { get; set; }
        public EScaling WndScalingMode { get; set; }
        public int WndCaptureRate
        {
            get
            {
                return captureRate;
            }
            set
            {
                captureRate = value;
            }
        }

        public bool WndDiffCaptureMode { get; set; }
        
        public int WndInactiveCaptureRate
        {
            get
            {
                return inactiveCaptureRate;
            }
            set
            {
                inactiveCaptureRate = value;
            }
        }
    }
}
