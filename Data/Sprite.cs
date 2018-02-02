using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PokeD.CPGL.Components;
using PokeD.CPGL.Components.Camera;
using PokeD.CPGL.Extensions;
using PokeD.CPGL.Physics.Collision;

namespace PokeD.CPGL.Data
{
    public class Sprite : DrawableComponent, ICollision2D
    {
        protected IPosition2D ObjectRef { get; }
        public Vector2 Position => ObjectRef.Position;

        public Color[] TextureData { get; }
        public virtual Rectangle TextureRectangle { get; }
        public int TextureScale { get; }

        public virtual Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, TextureRectangle.Width * TextureScale, TextureRectangle.Height * TextureScale);
        public Rectangle BoundsPixel => GetPixelRectangle(this);

        private Texture2D Texture { get; }
        

        public Sprite(PortableGame game, IPosition2D objectRef, Texture2D texture, Rectangle textureRectangle, int textureScale = 1) : base(game)
        {
            ObjectRef = objectRef;

            TextureRectangle = textureRectangle;
            TextureScale = textureScale;
            TextureData = texture.GetImageData(textureRectangle, textureScale);
            Texture = texture;
        }
        public Sprite(Component component, IPosition2D objectRef, Texture2D texture, Rectangle textureRectangle, int textureScale = 1) : base(component)
        {
            ObjectRef = objectRef;

            TextureRectangle = textureRectangle;
            TextureScale = textureScale;
            TextureData = texture.GetImageData(textureRectangle, textureScale);
            Texture = texture;
        }
        public Sprite(DrawableComponent component, IPosition2D objectRef, Texture2D texture, Rectangle textureRectangle, int textureScale = 1) : base(component)
        {
            ObjectRef = objectRef;

            TextureRectangle = textureRectangle;
            TextureScale = textureScale;
            TextureData = texture.GetImageData(textureRectangle, textureScale);
            Texture = texture;
        }

        public override void Update(GameTime gameTime) { }
        public override void Draw(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { spriteBatch.Draw(Texture, Bounds, TextureRectangle, Color.White); }
        public virtual void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch) { RectangleSpriteDrawer.DrawRectangle(spriteBatch, Bounds, Color.Blue, 2); }

        public virtual bool IntersectBounds(ICollision2D other) => DefaultIntersectBounds(this, other);
        public virtual bool IntersectPixels(ICollision2D other) => DefaultIntersectPixels(this, other);
        public virtual bool IntersectPixelBounds(ICollision2D other) => DefaultIntersectPixelBounds(this, other);

        protected static bool DefaultIntersectBounds(ICollision2D a, ICollision2D b) => a.Bounds.Intersects(b.Bounds);
        protected static bool DefaultIntersectPixels(ICollision2D a, ICollision2D b)
        {
            var x1 = Math.Max(a.Bounds.X, b.Bounds.X);
            var x2 = Math.Min(a.Bounds.X + a.Bounds.Width, b.Bounds.X + b.Bounds.Width);
            var y1 = Math.Max(a.Bounds.Y, b.Bounds.Y);
            var y2 = Math.Min(a.Bounds.Y + a.Bounds.Height, b.Bounds.Y + b.Bounds.Height);

            for (var y = y1; y < y2; ++y)
            for (var x = x1; x < x2; ++x)
            {
                var ac = a.TextureData[(x - a.Bounds.X) + (y - a.Bounds.Y) * a.Bounds.Width];
                var bc = b.TextureData[(x - b.Bounds.X) + (y - b.Bounds.Y) * b.Bounds.Width];

                if (ac.A != 0 && bc.A != 0)
                    return true;
            }

            return false;
        }
        protected static bool DefaultIntersectPixelBounds(ICollision2D a, ICollision2D b) => GetPixelRectangle(a).Intersects(GetPixelRectangle(b));

        protected static Rectangle GetPixelRectangle(ICollision2D sprite)
        {
            int x1 = int.MaxValue, y1 = int.MaxValue;
            int x2 = int.MinValue, y2 = int.MinValue;

            for (var x = 0; x < sprite.TextureRectangle.Width * sprite.TextureScale; x++)
            for (var y = 0; y < sprite.TextureRectangle.Height * sprite.TextureScale; y++)
                if (sprite.TextureData[x + y * sprite.TextureRectangle.Width * sprite.TextureScale].A != 0)
                {
                    if (x1 > x) x1 = x;
                    if (x2 < x) x2 = x;

                    if (y1 > y) y1 = y;
                    if (y2 < y) y2 = y;
                }

            return new Rectangle((int)(sprite.Position.X + x1), (int)(sprite.Position.Y + y1), x2 - x1 + 1, y2 - y1 + 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Texture.Dispose();
                }
            }

            base.Dispose(disposing);
        }

    }

    public sealed class PlayerSprite : Sprite
    {
        private AnimatedTile AnimatedTile { get; }
        public override Rectangle TextureRectangle => AnimatedTile;


        public Rectangle TopBounds => new Rectangle((int) Position.X, (int) Position.Y, TextureRectangle.Width * TextureScale, (int) (TextureRectangle.Height / 1.5f * TextureScale));
        public Rectangle LowBounds => new Rectangle((int) Position.X, (int) (Position.Y + TextureRectangle.Height / 1.5f * TextureScale), TextureRectangle.Width * TextureScale, (int) (TextureRectangle.Height * TextureScale - TextureRectangle.Height / 1.5f * TextureScale));
        private Rectangle DrawBounds => new Rectangle((int) Position.X, (int) Position.Y, TextureRectangle.Width * TextureScale, TextureRectangle.Height * TextureScale);
        private Rectangle Intersection => Rectangle.Intersect(LowBounds, BoundsPixel);

        private PlayerMovementState MovementState => ((Camera2DComponent) ObjectRef).MovementState;

        private bool _isMoving;
        private Keys _key;

        public PlayerSprite(Camera2DComponent camera, AnimatedTile texture, Rectangle textureRectangle, int textureScale = 1) : base(camera, camera, texture, textureRectangle, textureScale)
        {
            AnimatedTile = texture;

            camera.SetPlayerCollision(this);

            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);
            KeyboardListener.KeyReleased += (this, KeyboardListener_KeyReleased);
        }
        private void KeyboardListener_KeyPressed(object sender, Components.Input.KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    _isMoving = true;
                    _key = Keys.W;
                    break;

                case Keys.A:
                    _isMoving = true;
                    _key = Keys.A;
                    break;

                case Keys.S:
                    _isMoving = true;
                    _key = Keys.S;
                    break;

                case Keys.D:
                    _isMoving = true;
                    _key = Keys.D;
                    break;
            }
        }
        private void KeyboardListener_KeyReleased(object sender, Components.Input.KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    if(_key == Keys.W)
                        _isMoving = false;
                    break;

                case Keys.A:
                    if (_key == Keys.A)
                        _isMoving = false;
                    break;

                case Keys.S:
                    if (_key == Keys.S)
                        _isMoving = false;
                    break;

                case Keys.D:
                    if (_key == Keys.D)
                        _isMoving = false;
                    break;
            }
        }

        public override bool IntersectPixelBounds(ICollision2D other)
        {
            //var rec = new Rectangle(TextureRectangle.X, (int) (TextureRectangle.Height / 1.5f * TextureScale), TextureRectangle.Width * TextureScale, (int) (TextureRectangle.Height * TextureScale - TextureRectangle.Height / 1.5f * TextureScale));
            //var col = new Collision2D(this, ((Texture2D) AnimatedTile).GetImageData(rec, TextureScale), rec, TextureScale);
            //return col.IntersectPixelBounds(other);

            //return IntersectPixels(this, other);
            //return LowBounds.Intersects(other.BoundsPixel);
            return Intersection.Intersects(other.BoundsPixel);
        }
        private static bool IntersectPixels(PlayerSprite a, ICollision2D b)
        {
            var aBounds = a.BoundsPixel;
            var bBounds = b.Bounds;

            var x1 = Math.Max(aBounds.X, bBounds.X);
            var x2 = Math.Min(aBounds.X + aBounds.Width, bBounds.X + bBounds.Width);
            var y1 = Math.Max(aBounds.Y, bBounds.Y);
            var y2 = Math.Min(aBounds.Y + aBounds.Height, bBounds.Y + bBounds.Height);

            for (var y = y1; y < y2; ++y)
                for (var x = x1; x < x2; ++x)
                {
                    var ac = a.TextureData[(x - aBounds.X) + (y - aBounds.Y) * aBounds.Width];
                    var bc = b.TextureData[(x - bBounds.X) + (y - bBounds.Y) * bBounds.Width];

                    if (ac.A != 0 && bc.A != 0)
                        return true;
                }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            switch (MovementState)
            {
                case PlayerMovementState.Up:
                    AnimatedTile.SpriteID = 0;
                    break;

                case PlayerMovementState.Right:
                    AnimatedTile.SpriteID = 1;
                    break;

                case PlayerMovementState.Left:
                    AnimatedTile.SpriteID = 2;
                    break;

                case PlayerMovementState.Down:
                    AnimatedTile.SpriteID = 3;
                    break;
            }
        }

        private double val;
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var dt = gameTime.ElapsedGameTime.TotalSeconds * 60D;
            if (_isMoving)
            {
                val += dt;
                if (val >= 1D)
                {
                    AnimatedTile.MoveNext();
                    val = 0d;
                }
            }
            else
                AnimatedTile.MoveFirst();

            spriteBatch.Draw(AnimatedTile, DrawBounds, TextureRectangle, Color.White);
        }
        public override void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch)
        {
            RectangleSpriteDrawer.DrawRectangle(spriteBatch, Bounds, Color.Red, 2);
            RectangleSpriteDrawer.DrawRectangle(spriteBatch, TopBounds, Color.Violet, 2);
            RectangleSpriteDrawer.DrawRectangle(spriteBatch, LowBounds, Color.Blue, 2);
            RectangleSpriteDrawer.DrawRectangle(spriteBatch, BoundsPixel, Color.Yellow, 2);
            RectangleSpriteDrawer.DrawRectangle(spriteBatch, Intersection, Color.Coral, 2);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;
                    KeyboardListener.KeyReleased -= KeyboardListener_KeyReleased;

                    AnimatedTile.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}