using System;
using System.IO;

using ImageTools.IO;
using ImageTools.IO.Gif;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Data
{
    public class AnimatedTile : IEquatable<AnimatedTile>, IEquatable<Texture2D>, IDisposable
    {
        public SequenceMode SequenceMode { get; }

        public int SpriteID { private get; set; }
        public int CurrentFrame { get; private set; }
        public int FrameCount { get; private set; }
        public int Speed { get; }

        private Texture2D Tile { get; }
        private Rectangle[][] Frames { get; set; }
        private Rectangle CurrentRectangle => Frames[SpriteID][CurrentFrame];

        private Random Random { get; }
        private int cd, stp;


        public static implicit operator Texture2D(AnimatedTile animatedTile) => animatedTile.Tile;
        public static implicit operator Rectangle(AnimatedTile animatedTile) => animatedTile.CurrentRectangle;


        static AnimatedTile() { Decoders.AddDecoder<GifDecoder>(); }
        public AnimatedTile(Stream stream, int speed, GraphicsDevice graphicsDevice, Point data, SequenceMode sequenceMode = SequenceMode.Normal)
        {
            CurrentFrame = 0;
            stp = 1;
            cd = speed;
            Speed = speed;
            SequenceMode = sequenceMode;

            if(SequenceMode == SequenceMode.Random)
                Random = new Random();

            Tile = Texture2D.FromStream(graphicsDevice, stream);

            LoadTile(Tile, data);
        }
        public AnimatedTile(Texture2D texture, int speed, Point data, SequenceMode sequenceMode = SequenceMode.Normal)
        {
            Tile = texture;
            CurrentFrame = 0;
            stp = 1;
            cd = speed;
            Speed = speed;
            SequenceMode = sequenceMode;

            if (SequenceMode == SequenceMode.Random)
                Random = new Random();

            LoadTile(Tile, data);
        }

        public void MoveFirst()
        {
            CurrentFrame = 0;
        }
        public void MoveNext()
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

        private void LoadTile(Texture2D tile, Point data)
        {
            var spriteCount = tile.Height / data.Y;
            FrameCount = tile.Width / data.X;

            Frames = new Rectangle[spriteCount][];
            for (var i = 0; i < spriteCount; i++)
                Frames[i] = new Rectangle[FrameCount];

            for (var y = 0; y < spriteCount; y++)
                for (var x = 0; x < FrameCount; x++)
                    Frames[y][x] = new Rectangle(x * data.X, y * data.Y, data.X, data.Y);
        }


        public override string ToString() { return typeof(AnimatedTile).FullName + "[" + FrameCount + "]"; }
        public override int GetHashCode() { return CurrentFrame.GetHashCode() + cd.GetHashCode() + Frames.GetHashCode() + FrameCount.GetHashCode() + Speed.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj is AnimatedGif)
                return (AnimatedTile) obj == this;
            if (obj is Texture2D)
                return (Texture2D) obj == Tile;
            
            return false;
        }

        public bool Equals(AnimatedTile gif) { return gif.Tile == Tile && gif.Frames == Frames && gif.SequenceMode == SequenceMode; }
        public bool Equals(Texture2D tile) { return tile == Tile; }


        public void Dispose()
        {
            Tile.Dispose();
        }
    }
}
