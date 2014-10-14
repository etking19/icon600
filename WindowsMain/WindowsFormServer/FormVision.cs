using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormClient.RgbInput;

namespace WindowsFormClient
{
    public partial class FormVision : Form
    {
        private Window window;
        private Input input;
        private OnScreenDisplay osd;

        private FontDialog fontDlg = new FontDialog();
        private ColorDialog colorDlg = new ColorDialog();

        public Window WindowObj 
        { 
            get
            {
                return window;
            } 
            set
            {
                window = value;
            }
        }

        public Input InputObj
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        public OnScreenDisplay OnScreenDisplayObj
        {
            get
            {
                return osd;
            }
            set
            {
                osd = value;
            }
        }

        public int NumberOfInputs { get; set; }

        public FormVision()
        {
            InitializeComponent();
        }

        private void FormVision_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            wndAspectRatioCustom.CheckedChanged += wndAspectRatioCustom_CheckedChanged;
            wndCustomCaptureRate.CheckedChanged += wndCustomCaptureRate_CheckedChanged;

            wndCaptureRateCustom.Enabled = false;
            wndAspectRatioCustomWidth.Enabled = false;
            wndAspectRatioCustomHeight.Enabled = false;

            inputCroppingOn.CheckedChanged += inputCroppingOn_CheckedChanged;

            panelSimpleText.Enabled = false;
            osdColorBtn.Enabled = false;
            osdSimpleText.CheckedChanged += osdSimpleText_CheckedChanged;
            osdBgndOpaque.CheckedChanged += osdBgndOpaque_CheckedChanged;

            loadWindowAttributes();
            loadInputAttributes();
            loadOSDAttributes();
        }

        void osdBgndOpaque_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            osdColorBtn.Enabled = button.Checked;
        }

        void osdSimpleText_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            panelSimpleText.Enabled = button.Checked;
        }

        void inputCroppingOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            panelCropping.Enabled = button.Checked;
        }

        void wndCustomCaptureRate_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            wndCaptureRateCustom.Enabled = button.Checked;
        }

        void wndAspectRatioCustom_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton) sender;
            wndAspectRatioCustomWidth.Enabled = button.Checked;
            wndAspectRatioCustomHeight.Enabled = button.Checked;
        }

        private void setOSDAttributes()
        {
            if (osdNoDisplay.Checked)
            {
                OnScreenDisplayObj.OSDType = OnScreenDisplay.EDisplayType.NoOnScreenDisplay;
            }
            else
            {
                OnScreenDisplayObj.OSDType = OnScreenDisplay.EDisplayType.SimpleText;
            }

            string text = string.Empty;
            if (osdTextContent.Text.Count() != 0)
            {
                text = "\"" + osdTextContent.Text + "\"";
            }
            OnScreenDisplayObj.OSDText = text;

            OnScreenDisplayObj.OSDTextWrap = osdLineWrapping.Checked;
            OnScreenDisplayObj.OSDFont = new Session.Common.SerializableFont(fontDlg.Font).SerializeFontAttribute;

            OnScreenDisplayObj.OSDIsScaling = osdScaleAuto.Checked;

            if (osdBgndTransparent.Checked)
            {
                OnScreenDisplayObj.OSDBgndMOde = OnScreenDisplay.EBackgroundMode.Transparent;
            }
            else
            {
                OnScreenDisplayObj.OSDBgndMOde = OnScreenDisplay.EBackgroundMode.Opaque;
            }

            OnScreenDisplayObj.OSDColor = System.Drawing.ColorTranslator.ToHtml(colorDlg.Color);

            OnScreenDisplayObj.OSDMarginLeft = Convert.ToInt32(osdMarginsLeft.Value);
            OnScreenDisplayObj.OSDMarginTop = Convert.ToInt32(osdMarginsTop.Value);
            OnScreenDisplayObj.OSDMarginRight = Convert.ToInt32(osdMarginsRight.Value);
            OnScreenDisplayObj.OSDMarginBottom = Convert.ToInt32(osdMarginsBottom.Value);

            if(osdScaleVerticalTop.Checked)
            {
                OnScreenDisplayObj.OSDAlignVerticalMode = OnScreenDisplay.EAlignmentVertical.Top;
            }
            else if (osdScaleVerticalCenter.Checked)
            {
                OnScreenDisplayObj.OSDAlignVerticalMode = OnScreenDisplay.EAlignmentVertical.Center;
            }
            else
            {
                OnScreenDisplayObj.OSDAlignVerticalMode = OnScreenDisplay.EAlignmentVertical.Bottom;
            }

            if (osdScaleHorizontalLeft.Checked)
            {
                OnScreenDisplayObj.OSDAlignHorizontalMode = OnScreenDisplay.EAlignmentHorizontal.Left;
            }
            else if (osdScaleHorizontalCenter.Checked)
            {
                OnScreenDisplayObj.OSDAlignHorizontalMode = OnScreenDisplay.EAlignmentHorizontal.Center;
            }
            else
            {
                OnScreenDisplayObj.OSDAlignHorizontalMode = OnScreenDisplay.EAlignmentHorizontal.Right;
            }
        }

        private void loadOSDAttributes()
        {
            switch (OnScreenDisplayObj.OSDType)
            {
                case OnScreenDisplay.EDisplayType.NoOnScreenDisplay:
                    osdNoDisplay.Checked = true;
                    break;
                case OnScreenDisplay.EDisplayType.SimpleText:
                    osdSimpleText.Checked = true;
                    break;
            }

            osdTextContent.Text = OnScreenDisplayObj.OSDText;

            osdLineWrapping.Checked = OnScreenDisplayObj.OSDTextWrap;
            fontDlg.Font = new Session.Common.SerializableFont() { SerializeFontAttribute = OnScreenDisplayObj.OSDFont }.FontValue;

            osdScaleAuto.Checked = OnScreenDisplayObj.OSDIsScaling;
            osdScaleFixed.Checked = !OnScreenDisplayObj.OSDIsScaling;

            switch(OnScreenDisplayObj.OSDBgndMOde)
            {
                case OnScreenDisplay.EBackgroundMode.Transparent:
                    osdBgndTransparent.Checked = true;
                    break;
                case OnScreenDisplay.EBackgroundMode.Opaque:
                    osdBgndOpaque.Checked = true;
                    break;
            }

            colorDlg.Color = System.Drawing.ColorTranslator.FromHtml(OnScreenDisplayObj.OSDColor);

            osdMarginsLeft.Value = OnScreenDisplayObj.OSDMarginLeft;
            osdMarginsTop.Value = OnScreenDisplayObj.OSDMarginTop;
            osdMarginsRight.Value = OnScreenDisplayObj.OSDMarginRight;
            osdMarginsBottom.Value = OnScreenDisplayObj.OSDMarginBottom;

            switch (OnScreenDisplayObj.OSDAlignVerticalMode)
            {
                case OnScreenDisplay.EAlignmentVertical.Top:
                    osdScaleVerticalTop.Checked = true;
                    break;
                case OnScreenDisplay.EAlignmentVertical.Center:
                    osdScaleVerticalCenter.Checked = true;
                    break;
                case OnScreenDisplay.EAlignmentVertical.Bottom:
                    osdScaleVerticalBottom.Checked = true;
                    break;
            }

            switch (OnScreenDisplayObj.OSDAlignHorizontalMode)
            {
                case OnScreenDisplay.EAlignmentHorizontal.Left:
                    osdScaleHorizontalLeft.Checked = true;
                    break;
                case OnScreenDisplay.EAlignmentHorizontal.Center:
                    osdScaleHorizontalCenter.Checked = true;
                    break;
                case OnScreenDisplay.EAlignmentHorizontal.Right:
                    osdScaleHorizontalRight.Checked = true;
                    break;
            }
        }

        private void loadInputAttributes()
        {
            List<int> inputList = new List<int>();
            for (int i = 1; i <= NumberOfInputs; i++ )
            {
                inputList.Add(i);
            }
            inputSource.DataSource = new BindingSource(inputList.ToList(), null);

            inputSource.SelectedIndex = inputSource.FindStringExact(string.Format("{0}", InputObj.InputNumber));
            inputDisplayName.Text = InputObj.LabelName;

            inputResolutionWidth.Value = InputObj.InputCaptureWidth;
            inputResolutionHeight.Value = InputObj.InputCaptureHeight;

            inputCroppingOn.Checked = InputObj.InputCropping;
            inputCroppingOff.Checked = !InputObj.InputCropping;
            inputCroppingLeft.Value = InputObj.InputCropLeft;
            inputCroppingTop.Value = InputObj.InputCropTop;
            inputCroppingWidth.Value = InputObj.InputCropWidth;
            inputCroppingHeight.Value = InputObj.InputCropHeight;

            panelCropping.Enabled = inputCroppingOn.Checked;
        }

        private void setInputAttributes()
        {
            InputObj.InputNumber = Convert.ToUInt32(inputSource.SelectedItem);
            InputObj.LabelName = inputDisplayName.Text;

            InputObj.InputCaptureWidth = Convert.ToUInt32(inputResolutionWidth.Value);
            InputObj.InputCaptureHeight = Convert.ToUInt32(inputResolutionHeight.Value);

            InputObj.InputCropping = inputCroppingOn.Checked;

            InputObj.InputCropLeft = Convert.ToInt32(inputCroppingLeft.Value);
            InputObj.InputCropTop = Convert.ToInt32(inputCroppingTop.Value);
            InputObj.InputCropWidth = Convert.ToUInt32(inputCroppingWidth.Value);
            InputObj.InputCropHeight = Convert.ToUInt32(inputCroppingHeight.Value);
        }

        private void loadWindowAttributes()
        {
            wndPosLeft.Value = window.WndPostLeft;
            wndPosTop.Value = window.WndPosTop;
            wndPosWidth.Value = window.WndPostWidth;
            wndPosHeight.Value = window.WndPosHeight;
            wndExcludeBorder.Checked = window.WndPosExcludeBorder;

            switch(window.WndAspectRatioMode)
            {
                case Window.EAspectRatioMode.No:
                    wndAspectRatioNo.Checked = true;
                    break;
                case Window.EAspectRatioMode.Source:
                    wndAspectRatioSource.Checked = true;
                    break;
                case Window.EAspectRatioMode.Custom:
                    wndAspectRatioCustom.Checked = true;
                    break;
            }

            wndAspectRatioCustomWidth.Value = window.WndAspectRatioWidth;
            wndAspectRatioCustomHeight.Value = window.WndAspectRatioHeight;

            switch(window.WndStyle)
            {
                case Window.EStyle.BorderAndTitle:
                    wndStyleBorderTitle.Checked = true;
                    break;
                case Window.EStyle.Border:
                    wndStyleBorder.Checked = true;
                    break;
                case Window.EStyle.NoBorderAndTitle:
                    wndStyleNoBorderTitle.Checked = true;
                    break;
            }

            wndStyleMenu.Checked = window.WndMenuBar;
            wndStyleAlwaysOnTop.Checked = window.WndAlwaysOnTop;

            switch (window.WndCursorMode)
            {
                case Window.ECursor.AlwaysShow:
                    wndCursorShow.Checked = true;
                    break;
                case Window.ECursor.AlwaysHide:
                    wndCursorHide.Checked = true;
                    break;
                case Window.ECursor.HideWhenWndActive:
                    wndCursorHideActive.Checked = true;
                    break;
            }

            wndCaption.Text = window.WndCaption;
            wndInvalidDisplayMsgTime.Value = window.WndInvalidInputMsgTime;
            
            switch(window.WndCaptureMode)
            {
                case Window.ECaptureFormat.Auto:
                    wndCaptureFormatAuto.Checked = true;
                    break;
                case Window.ECaptureFormat.FiveFiveFive:
                    wndCaptureFormat555.Checked = true;
                    break;
                case Window.ECaptureFormat.FiveSixFive:
                    wndCaptureFormat565.Checked = true;
                    break;
                case Window.ECaptureFormat.EightEightEight:
                    wndCaptureFormat888.Checked = true;
                    break;
            }

            switch(window.WndDeinterlaceMode)
            {
                case Window.EDeinterlace.Bob:
                    wndDeinterlaceBob.Checked = true;
                    break;
                case Window.EDeinterlace.Weave:
                    wndDeinterlaceWeave.Checked = true;
                    break;
                case Window.EDeinterlace.None:
                    wndDeinterlaceNone.Checked = true;
                    break;
            }

            switch(window.WndTransferDataMode)
            {
                case Window.ETransferData.Direct:
                    wndTransferGraphic.Checked = true;
                    break;
                case Window.ETransferData.SystemMemory:
                    wndTransferMemory.Checked = true;
                    break;
            }

            switch(window.WndScalingMode)
            {
                case Window.EScaling.Fast:
                    wndScaleFast.Checked = true;
                    break;
                case Window.EScaling.Slow:
                    wndScaleSlow.Checked = true;
                    break;
            }

            wndActiveCaptureRate.SelectedIndex = wndActiveCaptureRate.FindStringExact(string.Format("{0}%", window.WndCaptureRate));
            wndCaptureRateCustom.SelectedIndex = wndActiveCaptureRate.FindStringExact(string.Format("{0}%", window.WndInactiveCaptureRate));

            wndCustomCaptureRate.Checked = window.WndDiffCaptureMode;
            wndCaptureRate.Checked = !window.WndDiffCaptureMode;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // set all the values
            setWindowsAttributes();
            setInputAttributes();
            setOSDAttributes();
        }

        private void setWindowsAttributes()
        {
            window.WndPostLeft = Convert.ToInt32(wndPosLeft.Value);
            window.WndPosTop = Convert.ToInt32(wndPosTop.Value);
            window.WndPostWidth = Convert.ToInt32(wndPosWidth.Value);
            window.WndPosHeight = Convert.ToInt32(wndPosHeight.Value);
            window.WndPosExcludeBorder = wndExcludeBorder.Checked;

            if(wndAspectRatioNo.Checked)
            {
                window.WndAspectRatioMode = Window.EAspectRatioMode.No;
            }
            else if (wndAspectRatioSource.Checked)
            {
                window.WndAspectRatioMode = Window.EAspectRatioMode.Source;
            }
            else
            {
                window.WndAspectRatioMode = Window.EAspectRatioMode.Custom;
            }

            window.WndAspectRatioWidth = Convert.ToInt32(wndAspectRatioCustomWidth.Value);
            window.WndAspectRatioHeight = Convert.ToInt32(wndAspectRatioCustomHeight.Value);

            if(wndStyleBorderTitle.Checked)
            {
                window.WndStyle = Window.EStyle.BorderAndTitle;
            }
            else if (wndStyleBorder.Checked)
            {
                window.WndStyle = Window.EStyle.Border;
            }
            else
            {
                window.WndStyle = Window.EStyle.NoBorderAndTitle;
            }

            window.WndMenuBar = wndStyleMenu.Checked;
            window.WndAlwaysOnTop = wndStyleAlwaysOnTop.Checked;

            if(wndCursorShow.Checked)
            {
                window.WndCursorMode = Window.ECursor.AlwaysShow;
            }
            else if (wndCursorHide.Checked)
            {
                window.WndCursorMode = Window.ECursor.AlwaysHide;
            }
            else
            {
                window.WndCursorMode = Window.ECursor.HideWhenWndActive;
            }

            window.WndCaption = wndCaption.Text;
            window.WndInvalidInputMsgTime = Convert.ToInt32(wndInvalidDisplayMsgTime.Value);

            if(wndCaptureFormatAuto.Checked)
            {
                window.WndCaptureMode = Window.ECaptureFormat.Auto;
            }
            else if(wndCaptureFormat555.Checked)
            {
                window.WndCaptureMode = Window.ECaptureFormat.FiveFiveFive;
            }
            else if (wndCaptureFormat565.Checked)
            {
                window.WndCaptureMode = Window.ECaptureFormat.FiveSixFive;
            }
            else
            {
                window.WndCaptureMode = Window.ECaptureFormat.EightEightEight;
            }

            if(wndDeinterlaceBob.Checked)
            {
                window.WndDeinterlaceMode = Window.EDeinterlace.Bob;
            }
            else if (wndDeinterlaceWeave.Checked)
            {
                window.WndDeinterlaceMode = Window.EDeinterlace.Weave;
            }
            else
            {
                window.WndDeinterlaceMode = Window.EDeinterlace.None;
            }

            if (wndTransferGraphic.Checked)
            {
                window.WndTransferDataMode = Window.ETransferData.Direct;
            }
            else
            {
                window.WndTransferDataMode = Window.ETransferData.SystemMemory;
            }

            if (wndScaleFast.Checked)
            {
                window.WndScalingMode = Window.EScaling.Fast;
            }
            else
            {
                window.WndScalingMode = Window.EScaling.Slow;
            }

            Regex rgx = new Regex(@"\d+", RegexOptions.IgnoreCase);
            window.WndCaptureRate = int.Parse(rgx.Match((string)wndActiveCaptureRate.SelectedItem).Value);
            window.WndInactiveCaptureRate = int.Parse(rgx.Match((string)wndCaptureRateCustom.SelectedItem).Value);

            window.WndDiffCaptureMode = wndCustomCaptureRate.Checked;
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            fontDlg.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDlg.ShowDialog(this);
        }
    }
}
