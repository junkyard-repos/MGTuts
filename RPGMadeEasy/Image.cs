using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RPGMadeEasy
{
    public class Image
    {
        private Vector2 origin;
        private ContentManager content;
        private RenderTarget2D renderTarget;
        private SpriteFont font;

        [XmlIgnore] public float Alpha;
        [XmlIgnore] public Texture2D Texture;
        [XmlIgnore] public Vector2 Scale;
        [XmlIgnore] public Rectangle SourceRectangle;
        [XmlIgnore] public string Text;
        [XmlIgnore] public string FontName;

        public Vector2 Position;
        public string Path;


        public Image()
        {
            Path = String.Empty;
            FontName = @"Fonts\Arial";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Text = "Finally!!!";
            Alpha = 1.0f;
            SourceRectangle = Rectangle.Empty;
        }

        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            if (!string.IsNullOrEmpty(Path))
            {
                Texture = content.Load<Texture2D>(Path);
            }

            font = content.Load<SpriteFont>(FontName);
            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X += Texture.Width;
                dimensions.Y += Texture.Height;
                //dimensions.Y += font.MeasureString(Text).X;
            }

            //if (Texture != null)
            //{
            //    dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            //}
            //else
            //{
            //    dimensions.Y = font.MeasureString(Text).Y;
            //}


            if (SourceRectangle == Rectangle.Empty)
            {
                SourceRectangle = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            Texture = renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            spriteBatch.Draw(Texture, Position + origin, SourceRectangle, Color.White * Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
