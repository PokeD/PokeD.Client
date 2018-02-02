using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.CPGL.Storage.Files.GameFiles.ContentFiles;

namespace PokeD.CPGL.BMFont
{
    public class FontRenderer : IFontRenderer
    {
        private class Font : IDisposable
        {
            public int Size => XmlFontData.Info.Size < 0 ? XmlFontData.Info.Size * -1 : XmlFontData.Info.Size;
            public int LineHeight => XmlFontData.Common.LineHeight;

            private XmlFontFile XmlFontData { get; }
            public Texture2D[] Textures { get; }
            public Dictionary<char, XmlFontChar> GlyphMap { get; } = new Dictionary<char, XmlFontChar>();

            public Font(XmlFontFile xmlFontData, Texture2D[] textures)
            {
                XmlFontData = xmlFontData;
                Textures = textures;

                foreach (var glyph in XmlFontData.Chars)
                    GlyphMap.Add((char) glyph.ID, glyph);
            }

            public Vector2 MeasureText(string text)
            {
                var width = 0;
                var height = LineHeight;
                foreach (var c in text)
                {
                    if (GlyphMap.TryGetValue(c, out XmlFontChar fc))
                        width += fc.XAdvance;

                    if (c == '\n')
                        height += LineHeight;
                }

                return new Vector2(width, height);
            }

            public void Dispose()
            {
                XmlFontData?.Dispose();
                GlyphMap?.Clear();
            }
        }
        private IList<Font> Fonts { get; } = new List<Font>();

        public FontRenderer(FontFile fontFile)
        {
            Fonts.Add(new Font(fontFile.XmlFontData, fontFile.Textures.Select(tf => (Texture2D) tf).ToArray()));
        }
        public FontRenderer(IList<FontFile> fontFiles)
        {
            foreach (var fontFile in fontFiles)
                Fonts.Add(new Font(fontFile.XmlFontData, fontFile.Textures.Select(tf => (Texture2D) tf).ToArray()));
            Fonts = Fonts.OrderByDescending(font => font.Size).ToList();
        }

        private static Texture2D _pointTexture;
        private static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        private (Font Font, Vector2 Size) GetMinFont(string text, Rectangle borders)
        {
            var maxFont = Fonts[0];
            var maxSize = maxFont.MeasureText(text);
            var maxScale = Math.Min(borders.Width / maxSize.X, borders.Height / maxSize.Y);

            var font = Fonts.OrderBy(f => Math.Abs(maxScale - (float) f.Size / (float) maxFont.Size)).First();
            var scale = (float) font.Size / (float) maxFont.Size;
            var size = maxSize * scale;
            return (font, size);

            var minFont = Fonts.Count == 1
                ? Fonts.Select(f => new { Font = f, Size = f.MeasureText(text) }).First()
                : Fonts.Select(f => new { Font = f, Size = f.MeasureText(text) }).OrderBy(tuple => Math.Abs(1.0f - Math.Min(borders.Width / tuple.Size.X, borders.Height / tuple.Size.Y))).First();

            //var minFont = Fonts.Count == 1
            //    ? Fonts.Select(f => new {Font = f, Size = f.MeasureText(text)}).First()
            //    : Fonts.Select(f => new {Font = f, Size = f.MeasureText(text)}).OrderBy(tuple => Math.Abs(1.0f - Math.Min(borders.Width / tuple.Size.X, borders.Height / tuple.Size.Y))).First();

            return (minFont.Font, minFont.Size);
        }

        public void DrawText(SpriteBatch spriteBatch, string text, Rectangle borders, Color color, bool centerHeight = true)
        {
            var minFontInfo = GetMinFont(text, borders);
            var font = minFontInfo.Font;
            var size = minFontInfo.Size;

            var dx = borders.X;
            var dy = centerHeight ? borders.Center.Y - (int) (size.Y * 0.5f) : borders.Y;

            foreach (var c in text)
            {
                if (font.GlyphMap.TryGetValue(c, out XmlFontChar fc))
                {
                    var pos = new Vector2(dx + fc.XOffset, dy + fc.YOffset);
                    var src = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);

                    spriteBatch.Draw(font.Textures[fc.Page], pos, src, color);

                    dx += fc.XAdvance;
                }

                if (c == '\n')
                {
                    dx = borders.X;
                    dy += font.LineHeight;
                }
            }
        }
        public void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            var font = Fonts.First();

            var dx = position.X;
            var dy = position.Y;

            foreach (var c in text)
            {
                if (font.GlyphMap.TryGetValue(c, out XmlFontChar fc))
                {
                    var pos = new Vector2(dx + fc.XOffset, dy + fc.YOffset);
                    var src = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);

                    spriteBatch.Draw(font.Textures[fc.Page], pos, src, color);

                    dx += fc.XAdvance;
                }

                if (c == '\n')
                {
                    dx = position.X;
                    dy += font.LineHeight;
                }
            }
        }

        public void DrawTextStretched(SpriteBatch spriteBatch, string text, Rectangle borders, Color color)
        {
            /*
            var size = MeasureText(text);
            var scale = Math.Min(borders.Width / size.X, borders.Height / size.Y);

            var dx = borders.X;
            //var dy = borders.Y;
            var dy = borders.Center.Y - (int)(size.Y * scale * 0.5f);

            foreach (var c in text)
            {
                XmlFontChar fc;
                if (GlyphMap.TryGetValue(c, out fc))
                {
                    var pos = new Vector2(dx + (int)(fc.XOffset * scale), dy + (int)(fc.YOffset * scale));
                    var src = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);

                    spriteBatch.Draw(Textures[fc.Page], pos, src, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                    dx += (int)(fc.XAdvance * scale);
                }
            }
            */
        }

        public void DrawTextCentered(SpriteBatch spriteBatch, string text, Rectangle borders, Color color)
        {
            var minFontInfo = GetMinFont(text, borders);
            var font = minFontInfo.Font;
            var size = minFontInfo.Size;

            var dx = borders.Center.X - (int) (size.X * 0.5f);
            var dy = borders.Center.Y - (int) (size.Y * 0.5f);

            foreach (var c in text)
            {
                if (font.GlyphMap.TryGetValue(c, out XmlFontChar fc))
                {
                    var pos = new Vector2(dx + fc.XOffset, dy + fc.YOffset);
                    var src = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);

                    spriteBatch.Draw(font.Textures[fc.Page], pos, src, color);

                    dx += fc.XAdvance;
                }

                if (c == '\n')
                {
                    dx = borders.Center.X - (int) (size.X * 0.5f);
                    dy += font.LineHeight;
                }
            }
        }
        public void DrawTextCenteredStretched(SpriteBatch spriteBatch, string text, Rectangle borders, Color color)
        {
            /*
            var size = MeasureText(text);
            var scale = Math.Min(borders.Width / size.X, borders.Height / size.Y);

            var dx = borders.Center.X - (int)(size.X * scale * 0.5f) - (int)(2 * text.Length * 0.5f);
            var dy = borders.Center.Y - (int)(size.Y * scale * 0.5f);

            foreach (var c in text)
            {
                XmlFontChar fc;
                if (GlyphMap.TryGetValue(c, out fc))
                {
                    var pos = new Vector2(dx + (int)(fc.XOffset * scale), dy + (int)(fc.YOffset * scale));
                    var src = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);

                    spriteBatch.Draw(Textures[fc.Page], pos, src, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                    dx += (int) (fc.XAdvance * scale);
                }
            }
            */
        }

        public void Dispose()
        {
            foreach (var font in Fonts)
                font.Dispose();
        }
    }
}