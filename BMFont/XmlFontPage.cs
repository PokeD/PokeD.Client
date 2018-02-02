using System;
using System.Xml.Serialization;

namespace PokeD.CPGL.BMFont
{
    public class XmlFontPage
    {
        [XmlAttribute("id")]
        public Int32 ID { get; set; }

        [XmlAttribute("file")]
        public String File { get; set; }
    }
}
