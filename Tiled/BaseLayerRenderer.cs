using Microsoft.Xna.Framework.Graphics;

using PokeD.CPGL.Components;

namespace PokeD.CPGL.Tiled
{
    public abstract class  BaseLayerRenderer : DrawableComponent
    {
        public string Name => Layer.Name;

        protected MapWrapper Map { get; }
        public LayerWrapper Layer { get; }
        protected SpriteBatch SpriteBatch { get; }

        public BaseLayerRenderer(MapWrapper map, LayerWrapper layer, SpriteBatch spriteBatch) : base(map)
        {
            Map = map;
            Layer = layer;
            SpriteBatch = spriteBatch;
        }
    }
}