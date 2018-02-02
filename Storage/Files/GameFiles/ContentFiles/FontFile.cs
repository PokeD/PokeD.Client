using System.IO;
using System.Xml.Serialization;

using PCLExt.FileStorage;

using PokeD.CPGL.BMFont;
using PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders;

namespace PokeD.CPGL.Storage.Files.GameFiles.ContentFiles
{
    public class FontFile : BaseChildContentFile
    {
        public XmlFontFile XmlFontData { get; }
        public TextureFile[] Textures { get; }

        public FontFile(IFile file, BaseContentChildFolder parent) : base(file, parent)
        {
            using (var stream = Open(FileAccess.Read))
            using (var textReader = new StreamReader(stream))
                XmlFontData = (XmlFontFile) new XmlSerializer(typeof(XmlFontFile)).Deserialize(textReader);

            Textures = new TextureFile[XmlFontData.Pages.Count];
            foreach (var fontPage in XmlFontData.Pages)
            {
                var xnbFileName = $"{PortablePath.GetFileNameWithoutExtension(fontPage.File)}.xnb";
                var pngFileName = $"{PortablePath.GetFileNameWithoutExtension(fontPage.File)}.png";
                if (Parent.CheckExists(xnbFileName) != ExistenceCheckResult.FileExists)
                {
                    if (Parent.CheckExists(pngFileName) != ExistenceCheckResult.FileExists)
                    {
                        throw new IOException();
                    }
                    else
                        Textures[fontPage.ID] = new TextureFile(Parent.GetFile(pngFileName), Parent);
                }
                else
                    Textures[fontPage.ID] = new TextureFile(Parent.GetFile(xnbFileName), Parent);
            }
        }
    }
}