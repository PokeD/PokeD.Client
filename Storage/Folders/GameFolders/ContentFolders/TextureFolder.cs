using System.Collections.Generic;
using System.Linq;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Files.GameFiles.ContentFiles;

namespace PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders
{
    public class TextureFolder : BaseContentChildFolder
    {
        public TextureFolder(IFolder folder, BaseContentFolder parent) : base(folder, parent) { }

        public TextureFile GetTextureFile(string fileName) => GetAlTextureFiles().FirstOrDefault(file => file.InContentLocalPathWithoutExtension == fileName);
        public IList<TextureFile> GetAlTextureFiles() => GetFiles("*.xnb", FolderSearchOption.AllFolders).Select(file => new TextureFile(file, this)).ToList();
    }
}