using System.Collections.Generic;
using System.Linq;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Files.GameFiles.ContentFiles;

namespace PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders
{
    public class MusicFolder : BaseContentChildFolder
    {
        public MusicFolder(IFolder folder, BaseContentFolder parent) : base(folder, parent) { }

        public MusicFile GetMusicFile(string fileName) => GetAllMusicFiles().FirstOrDefault(file => file.InContentLocalPathWithoutExtension == fileName);
        public IList<MusicFile> GetAllMusicFiles() => GetFiles("*.ogg", FolderSearchOption.AllFolders).Select(file => new MusicFile(file, this)).ToList();
    }
}
