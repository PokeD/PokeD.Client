using PCLExt.FileStorage;

namespace PokeD.CPGL.Storage.Folders
{
    public sealed class MainFolder : BaseFolder
    {
        public MainFolder() : base(FileSystem.SpecialStorage) { }
    }
}