using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Session.Data
{
    public class ClientMessageBoxCmd : BaseCmd
    {
        public string Message { get; set; }

        /// <summary>
        /// string textFont = new SerializableFont(font).SerializeFontAttribut;
        /// Font font = new SerializableFont() { SerializeFontAttribute = messageBoxData.TextFont }.FontValue;
        /// </summary>
        public string TextFont { get; set; }

        /// <summary>
        /// string TextColor = System.Drawing.ColorTranslator.ToHtml(MyColorInstance);
        /// Color MyColor = System.Drawing.ColorTranslator.FromHtml(MyColorString);
        /// </summary>
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }

        public int Duration { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

