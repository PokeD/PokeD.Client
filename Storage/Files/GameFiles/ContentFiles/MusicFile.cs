using System;

using Microsoft.Xna.Framework.Media;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders;

namespace PokeD.CPGL.Storage.Files.GameFiles.ContentFiles
{
    public class MusicFile : BaseChildContentFile
    {
        public static implicit operator Song(MusicFile musicFile) => musicFile.Song;

        private Song _song;
        private Song Song => _song ?? (_song = ContentManager.Load<Song>(LocalPathWithoutExtension));
        public TimeSpan Duration => Song.Duration;

        public MusicFile(IFile file, BaseContentChildFolder parent) : base(file, parent) { }
    }
}
