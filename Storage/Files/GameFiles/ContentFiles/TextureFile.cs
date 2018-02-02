using Microsoft.Xna.Framework.Graphics;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders;

namespace PokeD.CPGL.Storage.Files.GameFiles.ContentFiles
{
    public class TextureFile : BaseChildContentFile
    {
        public static implicit operator Texture2D(TextureFile textureFile) => textureFile.Texture2D;

        private Texture2D _texture2D;
        private Texture2D Texture2D => _texture2D ?? (_texture2D = ContentManager.Load<Texture2D>(LocalPathWithoutExtension));

        public TextureFile(IFile file, BaseContentChildFolder parent) : base(file, parent) { }
    }
}