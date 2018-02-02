using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Effects
{
    public class BRx2Effect : Effect
    {
        public static BRx2Effect Load(ContentManager content) => new BRx2Effect(content.Load<Effect>("Shaders/BRx2"));

        #region Effect Parameters

        private EffectParameter matrixParam;
        private EffectParameter textureSizeParam;
        private EffectParameter textureParam;
        
        #endregion

        public Texture2D Texture { get => textureParam.GetValueTexture2D(); set => textureParam.SetValue(value); }
        public Vector2 TextureSize { get => textureSizeParam.GetValueVector2(); set => textureSizeParam.SetValue(value); }


        public BRx2Effect(Effect cloneSource) : base(cloneSource)
        {
            CacheEffectParams();

            Texture     = cloneSource.Parameters["Texture"].GetValueTexture2D();
            TextureSize = cloneSource.Parameters["TextureSize"].GetValueVector2();
        }
        public BRx2Effect(BRx2Effect cloneSource) : base(cloneSource)
        {
            CacheEffectParams();

            Texture = cloneSource.Texture;
            TextureSize = cloneSource.TextureSize;
        }
        public BRx2Effect(GraphicsDevice graphicsDevice) : base(graphicsDevice, null)
        {
            CacheEffectParams();
        }

        private void CacheEffectParams()
        {
            textureParam        = Parameters["Texture"];
            textureSizeParam    = Parameters["TextureSize"];
            matrixParam         = Parameters["MatrixTransform"];
        }

        protected override void OnApply()
        {
            var viewport = GraphicsDevice.Viewport;

            var projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 0);
            var halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            matrixParam.SetValue(halfPixelOffset * projection);
        }

        public override Effect Clone()
        {
            return new BRx2Effect(this);
        }
    }
}
