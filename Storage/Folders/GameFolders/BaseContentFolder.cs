using System;

using Microsoft.Xna.Framework.Content;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders;

namespace PokeD.CPGL.Storage.Folders.GameFolders
{
    public abstract class BaseContentFolder : BaseGameChildFolder, IDisposable
    {
        public FontFolder FontFolder => new FontFolder(CreateFolder("Fonts", CreationCollisionOption.OpenIfExists), this);
        public TextureFolder TextureFolder => new TextureFolder(CreateFolder("Textures", CreationCollisionOption.OpenIfExists), this);
        public MusicFolder MusicFolder => new MusicFolder(CreateFolder("Music", CreationCollisionOption.OpenIfExists), this);
        public SoundEffectsFolder SoundEffectsFolder => new SoundEffectsFolder(CreateFolder("Sound Effects", CreationCollisionOption.OpenIfExists), this);

        public ContentManager ContentManager { get; private set; }

        protected BaseContentFolder(ContentManager contentManager, IFolder folder) : base(folder) { ContentManager = contentManager; }

        /*
        public bool TextureExist(string path)
        {
            path = path.Replace(@"\", @"|");
            string[] arr = path.Split('|');
            string filename = arr.Last();
            string[] folders = arr.Length > 1 ? arr.Take(arr.Length - 1).ToArray() : null;

            IFolder folder = new OldContentFolder();
            if (folders != null)
                foreach (var folderName in folders)
                    folder = folder.GetFolder(folderName);

            return folder.CheckExists($"{filename}.xnb") == ExistenceCheckResult.FileExists || folder.CheckExists($"{filename}.png") == ExistenceCheckResult.FileExists;
        }

        public Texture2D GetTexture(string path)
        {
            path = path.Replace(@"\", @"|");
            string[] arr = path.Split('|');
            string filename = arr.Last();
            string[] folders = arr.Length > 1 ? arr.Take(arr.Length - 1).ToArray() : null;

            IFile file = null;
            IFolder folder = new ContentFolder();
            if (folders != null)
                foreach (var folderName in folders)
                    folder = folder.GetFolder(folderName);

            if (folder.CheckExists($"{filename}.xnb") != ExistenceCheckResult.FileExists)
            {
                if (folder.CheckExists($"{filename}.png") == ExistenceCheckResult.FileExists)
                    file = folder.GetFile($"{filename}.png");
            }
            else
                file = folder.GetFile($"{filename}.xnb");

            if (file != null)
                return new TextureFile(file, new TextureFolder(folder, this));

            return TextureManager.DefaultTexture;
        }
        */

        public void Dispose()
        {
            ContentManager.Dispose();
            ContentManager = null; // Best wasy to see if something is leaking
            //GC.Collect(); // -- TODO: May be slow. Depends on how often screens will be changed
            //GC.WaitForPendingFinalizers();
        }
    }
}