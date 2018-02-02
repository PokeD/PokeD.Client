using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokeD.CPGL.BMFont
{
    [XmlRoot("font")]
    public class XmlFontFile : IDisposable
    {
        [XmlElement("info")]
        public XmlFontInfo Info { get; set; }

        [XmlElement("common")]
        public XmlFontCommon Common { get; set; }

        [XmlArray("pages")]
        [XmlArrayItem("page")]
        public List<XmlFontPage> Pages { get; set; }

        [XmlArray("chars")]
        [XmlArrayItem("char")]
        public List<XmlFontChar> Chars { get; set; }

        [XmlArray("kernings")]
        [XmlArrayItem("kerning")]
        public List<XmlFontKerning> Kernings { get; set; }

        public void Dispose()
        {
            Pages?.Clear();
            Chars?.Clear();
            Kernings?.Clear();
        }
    }
}
