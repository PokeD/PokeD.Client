using Microsoft.Xna.Framework.Content;

using PCLExt.FileStorage;

namespace PokeD.CPGL.Storage.Folders.GameFolders
{
    public class ContentFolder : BaseContentFolder
    {
        // CoreResources
        public ContentFolder(ContentManager contentManager) : base(contentManager, new GameFolder().CreateFolder("Content", CreationCollisionOption.OpenIfExists)) { }
        // GameMode Content && ContentPack Content
        public ContentFolder(ContentManager contentManager, IFolder folder) : base(contentManager, folder) { }


        public ContentFolder(ContentManager contentManager, string localPath) : base(contentManager, FromLocalPath(localPath)) { }
    }

    public class OldContentFolder : BaseFolder
    {
        public OldContentFolder() : base(new GameFolder().CreateFolder("Content", CreationCollisionOption.OpenIfExists)) { }
    }
}
