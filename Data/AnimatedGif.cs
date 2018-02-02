using System;
using System.Collections.Generic;
using System.IO;

using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Gif;

using Microsoft.Xna.Framework.Graphics;

using PCLExt.Thread;

namespace PokeD.CPGL.Data
{
    public class AnimatedGif : IEquatable<AnimatedGif>, IEquatable<Texture2D[]>, IEquatable<Texture2D>, IDisposable
    {
        public SequenceMode SequenceMode { get; }

        public int CurrentFrame { get; private set; }
        public int FrameCount => Frames.Length;
        public int Speed { get; }

        private Texture2D[] Frames { get; }
        private Texture2D CurrentFrames { get { MoveNext(); return Frames[CurrentFrame]; } }

        private Random Random { get; }
        private int cd, stp;


        public static implicit operator Texture2D(AnimatedGif gif) { return gif.CurrentFrames; }


        static AnimatedGif() { Decoders.AddDecoder<GifDecoder>(); }
        public AnimatedGif(Stream stream, int speed, GraphicsDevice graphicsDevice, SequenceMode sequenceMode = SequenceMode.Normal)
        {
            CurrentFrame = 0;
            stp = 1;
            cd = speed;
            Speed = speed;
            SequenceMode = sequenceMode;

            if(SequenceMode == SequenceMode.Random)
                Random = new Random();

            var img = new ExtendedImage();
            img.SetSource(stream);

            while (img.IsLoading)
                Thread.Sleep(100);
            
            Frames = LoadGif(graphicsDevice, img);
        }

        
        private void MoveNext()
        {
            cd--;
            if (cd <= 0)
            {
                switch (SequenceMode)
                {
                    case SequenceMode.Normal:
                        CurrentFrame++;
                        if (CurrentFrame >= FrameCount)
                            CurrentFrame = 0;
                        break;
                    case SequenceMode.Random:
                        {
                            int i = CurrentFrame;
                            while (CurrentFrame == i)
                                CurrentFrame = Random.Next(FrameCount);
                            break;
                        }
                    case SequenceMode.ReverseAtEnd:
                        CurrentFrame += stp;
                        if (CurrentFrame >= FrameCount)
                        {
                            CurrentFrame -= stp;
                            stp *= -1;
                        }
                        break;
                }
                cd = Speed;
            }
        }

        private static Texture2D[] LoadGif(GraphicsDevice graphicsDevice, ExtendedImage image)
        {
            var textureList = new List<Texture2D>();

            for (var i = 0; i < image.Frames.Count; i++)
            {
                var texture = new Texture2D(graphicsDevice, image.PixelWidth, image.PixelHeight);
                texture.SetData(image.Frames[i].Pixels);
                textureList.Add(texture);
            }

            return textureList.ToArray();
        }


        public override string ToString() { return typeof(AnimatedGif).FullName + "[" + FrameCount + "]"; }
        public override int GetHashCode() { return Frames.GetHashCode() + FrameCount.GetHashCode() + Speed.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj is AnimatedGif)
                return (AnimatedGif) obj == this;
            if (obj is Texture2D[])
                return (Texture2D[]) obj == Frames;
            if (obj is Texture2D)
                return (Texture2D)   obj == Frames[CurrentFrame];

            return false;
        }

        public bool Equals(AnimatedGif gif) { return gif.FrameCount == FrameCount && gif.Frames == Frames && gif.SequenceMode == SequenceMode; }
        public bool Equals(Texture2D[] gif) { return gif == Frames; }
        public bool Equals(Texture2D texture) { return texture == Frames[CurrentFrame]; }


        public void Dispose()
        {
            for (int i = 0; i < FrameCount; i++)
                Frames[i].Dispose();
        }
    }
}
