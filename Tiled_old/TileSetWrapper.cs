using System.Xml.Serialization;

using Aragas.Core.Wrappers;

using Microsoft.Xna.Framework.Graphics;

using PCLStorage;

using TmxMapperPCL;

namespace PokeD.PGL.Tiled
{
    public class TileSetWrapper
    {
        private static IFolder TilesetsFolder => FileSystemWrapper.ContentFolder.GetFolderAsync("Maps").Result.GetFolderAsync("Tilesets").Result;

        public int FirstGID { get; }
        public Texture2D Texture { get; }


        public TileSetWrapper(GraphicsDevice graphicsDevice, TileSet tileSet)
        {
            FirstGID = tileSet.FirstGID;

            var serializer = new XmlSerializer(typeof(TileSet));
            var tileSetLoaded = (TileSet) serializer.Deserialize(TilesetsFolder.GetFileAsync(tileSet.Source).Result.OpenAsync(FileAccess.Read).Result);

            Texture = Texture2D.FromStream(graphicsDevice, TilesetsFolder.GetFileAsync(tileSetLoaded.Image.Source).Result.OpenAsync(FileAccess.Read).Result);
        }
    }
}