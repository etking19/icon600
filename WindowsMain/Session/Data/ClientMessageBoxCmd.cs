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
        public string TextFont { get; set; }
        public Color TextColor { get; set; }
        public int Duration { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

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

public static class FontXmlConverter
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