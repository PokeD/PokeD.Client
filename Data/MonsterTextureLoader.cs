using System;

using Microsoft.Xna.Framework.Graphics;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders.GameFolders;

namespace PokeD.CPGL.Data
{
    public static class MonsterTextureLoader
    {
        private static IFolder Front { get; }
        private static IFolder Back { get; }
        private static IFolder Info { get; }

        static MonsterTextureLoader()
        {
            Front = new OldContentFolder().GetFolder("Textures").GetFolder("Monsters").GetFolder("Battle").GetFolder("Front");
            Back = new OldContentFolder().GetFolder("Textures").GetFolder("Monsters").GetFolder("Battle").GetFolder("Back");
            Info = new OldContentFolder().GetFolder("Textures").GetFolder("Monsters").GetFolder("Info");
        }

        public static AnimatedGif LoadMonster(GraphicsDevice graphicsDevice, int id /*, MonsterMetadata MonsterPosition */)
        {
            int index = 0;

            /*
            if (MonsterPosition.HasFlag(MonsterMetadata.MoveOne))
                index = 1;
            if (MonsterPosition.HasFlag(MonsterMetadata.MoveTwo))
                index = 2;
            if (MonsterPosition.HasFlag(MonsterMetadata.MoveThree))
                index = 3;
            if (MonsterPosition.HasFlag(MonsterMetadata.MoveFour))
                index = 4;
            if (MonsterPosition.HasFlag(MonsterMetadata.MoveFive))
                index = 5;


            if (MonsterPosition.HasFlag(MonsterMetadata.Back))
                using (var stream = GetBackFolder(id, index).Open(FileAccess.Read))
                    return new AnimatedGif(stream, 3, graphicsDevice);

            if (MonsterPosition.HasFlag(MonsterMetadata.Front))
                using (var stream = GetFrontFolder(id, index).Open(FileAccess.Read))
                    return new AnimatedGif(stream, 3, graphicsDevice);

            throw new ArgumentException($"{nameof(MonsterPosition)} should have MonsterPosition.Front or MonsterPosition.Back!");
            */
            return null;
        }

        private static IFile GetFrontFolder(int id, int index)
        {
            var number = id.ToString("000");
            var name = index > 0 ? $"{number}-{index}" : number;

            return GetGeneration(Front, id).GetFile($"{name}.gif");
        }
        private static IFile GetBackFolder(int id, int index)
        {
            var number = id.ToString("000");
            var name = index > 0 ? $"{number}-{index}" : number;

            return GetGeneration(Back, id).GetFile($"{name}.gif");
        }
        private static IFile GetInfoFolder(int id, int index)
        {
            var number = id.ToString("000");
            var name = index > 0 ? $"{number}-{index}" : number;

            return GetGeneration(Info, id).GetFile($"{name}.gif");
        }

        private static IFolder GetGeneration(IFolder folder, int id)
        {
            if (id <= 151)
                return folder.GetFolder("Gen1");
            if (id <= 251)
                return folder.GetFolder("Gen2");
            if (id <= 386)
                return folder.GetFolder("Gen3");
            if (id <= 493)
                return folder.GetFolder("Gen4");
            if (id <= 649)
                return folder.GetFolder("Gen5");
            if (id <= 721)
                return folder.GetFolder("Gen6");

            throw new ArgumentOutOfRangeException(nameof(id));
        }
    }
}