using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Session
{
    public class Common
    {
        public class SerializableFont
        {
            public SerializableFont()
            {
                FontValue = null;
            }

            public SerializableFont(Font font)
            {
                FontValue = font;
            }

            [XmlIgnore]
            public Font FontValue { get; set; }

            [XmlElement("FontValue")]
            public string SerializeFontAttribute
            {
                get
                {
                    return FontXmlConverter.ConvertToString(FontValue);
                }
                set
                {
                    FontValue = FontXmlConverter.ConvertToFont(value);
                }
            }

            public static implicit operator Font(SerializableFont serializeableFont)
            {
                if (serializeableFont == null)
                    return null;
                return serializeableFont.FontValue;
            }

            public static implicit operator SerializableFont(Font font)
            {
                return new SerializableFont(font);
            }
        }

        internal class FontXmlConverter
        {
            public static string ConvertToString(Font font)
            {
                try
                {
                    if (font != null)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                        return converter.ConvertToString(font);
                    }
                    else
                        return null;
                }
                catch { System.Diagnostics.Debug.WriteLine("Unable to convert"); }
                return null;
            }
            public static Font ConvertToFont(string fontString)
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                    return (Font)converter.ConvertFromString(fontString);
                }
                catch { System.Diagnostics.Debug.WriteLine("Unable to convert"); }
                return null;
            }
        }

        internal class ColorXmlConverter
        {
            public static string ConvertToString(Color color)
            {
                try
                {
                    if (color != null)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                        return converter.ConvertToString(color);
                    }
                    else
                        return null;
                }
                catch { System.Diagnostics.Debug.WriteLine("Unable to convert color"); }
                return null;
            }

            public static Color ConvertToColor(string colorString)
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                    return (Color)converter.ConvertFromString(colorString);
                }
                catch { System.Diagnostics.Debug.WriteLine("Unable to convert color"); }
                return new Color();
            }
        }
    }
}
