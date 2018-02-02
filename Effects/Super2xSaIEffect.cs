using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Effects
{
    public class Super2xSaIEffect : Effect
    {
        public static Super2xSaIEffect Load(ContentManager content) => new Super2xSaIEffect(content.Load<Effect>("Shaders/Super2xSaI"));

        #region Effect Parameters

        private EffectParameter matrixParam;
        private EffectParameter textureSizeParam;
        private EffectParameter textureParam;
        
        #endregion

        public Texture2D Texture { get { return textureParam.GetValueTexture2D(); } set { textureParam.SetValue(value); } }
        public Vector2 TextureSize { get { return textureSizeParam.GetValueVector2(); } set { textureSizeParam.SetValue(value); } }


        public Super2xSaIEffect(Effect cloneSource) : base(cloneSource)
        {
            CacheEffectParams();

            Texture     = cloneSource.Parameters["Texture"].GetValueTexture2D();
            TextureSize = cloneSource.Parameters["TextureSize"].GetValueVector2();
        }
        public Super2xSaIEffect(Super2xSaIEffect cloneSource) : base(cloneSource)
        {
            CacheEffectParams();

            Texture = cloneSource.Texture;
            TextureSize = cloneSource.TextureSize;
        }
        public Super2xSaIEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, null)
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

            var projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            var halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            matrixParam.SetValue(halfPixelOffset * projection);
        }

        public override Effect Clone()
        {
            return new Super2xSaIEffect(this);
        }
    }
}
