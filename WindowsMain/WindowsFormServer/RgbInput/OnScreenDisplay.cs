using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.RgbInput
{
    public class OnScreenDisplay
    {
        public enum EDisplayType
        {
            NoOnScreenDisplay = 0,
            SimpleText,
        }

        public enum EBackgroundMode
        {
            Transparent = 0,
            Opaque,
        }

        public enum EAlignmentVertical
        {
            Top = 0,
            Center,
            Bottom,
        }

        public enum EAlignmentHorizontal
        {
            Left = 0,
            Center,
            Right,
        }


        public EDisplayType OSDType { get; set; }

        public string OSDText { get; set; }

        /// <summary>
        /// string textFont = new SerializableFont(font).SerializeFontAttribut;
        /// Font font = new SerializableFont() { SerializeFontAttribute = messageBoxData.TextFont }.FontValue;
        /// </summary>
        public string OSDFont { get; set; }

        /// <summary>
        /// string TextColor = System.Drawing.ColorTranslator.ToHtml(MyColorInstance);
        /// Color MyColor = System.Drawing.ColorTranslator.FromHtml(MyColorString);
        /// </summary>pass
        public string OSDColor { get; set; }
        public bool OSDIsScaling { get; set; }
        public EBackgroundMode OSDBgndMOde { get; set; }

        public bool OSDTextWrap { get; set; }

        /// <summary>
        /// string TextColor = System.Drawing.ColorTranslator.ToHtml(MyColorInstance);
        /// Color MyColor = System.Drawing.ColorTranslator.FromHtml(MyColorString);
        /// </summary>
        public string OSDBgndColor { get; set; }
        public int OSDMarginLeft { get; set; }
        public int OSDMarginTop { get; set; }
        public int OSDMarginRight { get; set; }
        public int OSDMarginBottom { get; set; }
        public EAlignmentVertical OSDAlignVerticalMode { get; set; }
        public EAlignmentHorizontal OSDAlignHorizontalMode { get; set; }
    }
}
