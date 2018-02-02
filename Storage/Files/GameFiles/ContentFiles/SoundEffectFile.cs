using System;

using Microsoft.Xna.Framework.Audio;

using PCLExt.FileStorage;

using PokeD.CPGL.Storage.Folders.GameFolders.ContentFolders;

namespace PokeD.CPGL.Storage.Files.GameFiles.ContentFiles
{
    public class SoundEffectFile : BaseChildContentFile
    {
        public static implicit operator SoundEffect(SoundEffectFile soundEffectFile) => soundEffectFile.SoundEffect;

        private SoundEffect _soundEffect;
        private SoundEffect SoundEffect => _soundEffect ?? (_soundEffect = ContentManager.Load<SoundEffect>(LocalPathWithoutExtension));
        public TimeSpan Duration => SoundEffect.Duration;

        public SoundEffectFile(MusicFile file) : base(file, file.Parent) { }
        public SoundEffectFile(IFile file, BaseContentChildFolder parent) : base(file, parent) { }

        public SoundEffectInstance CreateInstance() => SoundEffect.CreateInstance();
    }
}