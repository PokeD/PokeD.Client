using System;

using Microsoft.Xna.Framework.Graphics;

using TMXParserPCL;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders.GameFolders;

namespace PokeD.CPGL.Tiled
{
    public class TileSetWrapper : IDisposable
    {
        private static IFolder TilesetsFolder => new OldContentFolder().GetFolder("Maps").GetFolder("Tilesets");

        public int FirstGID { get; }
        public Texture2D Texture { get; }


        public TileSetWrapper(GraphicsDevice graphicsDevice, TileSet tileSet)
        {
            var tileSetLoaded = TileSet.LoadExternal(TilesetsFolder.GetFile(tileSet.Source).Open(FileAccess.Read), tileSet);
            
            FirstGID = tileSetLoaded.FirstGID;
            Texture = Texture2D.FromStream(graphicsDevice, TilesetsFolder.GetFile(tileSetLoaded.Image.Source).Open(FileAccess.Read));

            //FirstGID = tileSet.FirstGID;
            //Texture = Texture2D.FromStream(graphicsDevice, TilesetsFolder.GetFileAsync(tileSet.Image.Source).Result.OpenAsync(FileAccess.Read).Result);
            //var path = PortablePath.Combine(@"C:\Users\Aragas\Documents\RPGXP\Project1", tileSet.Image.Source.Replace(@"../", ""));
            //Texture = Texture2D.FromStream(graphicsDevice, FileSystem.Current.GetFileFromPathAsync(path).Result.OpenAsync(FileAccess.Read).Result);
        }

        public void Dispose()
        {
            Texture?.Dispose();
        }
    }
}