using PCLExt.FileStorage;

namespace PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders
{
    public abstract class BaseContentChildFolder : BaseFolder
    {
        public BaseContentFolder ContentFolder { get; }

        protected BaseContentChildFolder(IFolder folder, BaseContentFolder contentFolder) : base(folder) { ContentFolder = contentFolder; }
    }
}