using System.Collections.Generic;
using System.Linq;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Files.GameFiles.ContentFiles;

namespace PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders
{
    public class FontFolder : BaseContentChildFolder
    {
        public FontFolder(IFolder folder, BaseContentFolder parent) : base(folder, parent) { }

        public FontFile GetFontFile(string fileName)
        {
            var t = GetFiles($"{fileName}_*.fnt").Select(file => new FontFile(file, this)).ToList();

            return GetFontFiles().FirstOrDefault(file => file.InContentLocalPathWithoutExtension == fileName);
        }
        public IList<FontFile> GetFontFiles(string fontName) => GetFiles($"{fontName}_*.fnt").Select(file => new FontFile(file, this)).ToList();

        public IList<FontFile> GetFontFiles() => GetFiles("*.fnt").Select(file => new FontFile(file, this)).ToList();
    }
}
