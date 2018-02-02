using System.Collections.Generic;
using System.Linq;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Files.GameFiles.ContentFiles;

namespace PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders
{
    public class SoundEffectsFolder : BaseContentChildFolder
    {
        public SoundEffectsFolder(IFolder folder, BaseContentFolder parent) : base(folder, parent) { }

        public SoundEffectFile GetSoundEffectFile(string fileName) => GetAllSoundEffectFiles().FirstOrDefault(file => file.InContentLocalPathWithoutExtension == fileName);
        public IList<SoundEffectFile> GetAllSoundEffectFiles() => GetFiles("*.xnb", FolderSearchOption.AllFolders).Select(file => new SoundEffectFile(file, this)).ToList();
    }
}
