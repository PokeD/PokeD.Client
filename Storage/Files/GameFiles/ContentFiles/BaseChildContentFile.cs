using Microsoft.Xna.Framework.Content;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders;
using PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders;

namespace PokeD.CPGL.Storage.Files.GameFiles.ContentFiles
{
    public abstract class BaseChildContentFile : BaseFile
    {
        public BaseContentChildFolder Parent { get; }
        protected ContentManager ContentManager => Parent.ContentFolder.ContentManager;

        public string LocalPath => Path.Remove(0, new GameFolder().Path.Length).TrimStart('/', '\\');
        public string LocalPathWithoutExtension => LocalPath.Replace(System.IO.Path.GetExtension(Name), "");
        public string ContentLocalPath => Path.Remove(0, Parent.ContentFolder.Path.Length).TrimStart('/', '\\');
        public string ContentLocalPathWithoutExtension => ContentLocalPath.Replace(System.IO.Path.GetExtension(Name), "");
        public string InContentLocalPath => Path.Remove(0, Parent.Path.Length).TrimStart('/', '\\');
        public string InContentLocalPathWithoutExtension => InContentLocalPath.Replace(System.IO.Path.GetExtension(Name), "");
        public string NameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(Name);

        public BaseChildContentFile(IFile file, BaseContentChildFolder contentFolder) : base(file) { Parent = contentFolder; }
    }
}
