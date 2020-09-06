using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNABook_1
{
    public class BouncingImage : DrawableGameComponent
    {
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        public BouncingImage(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            position = Vector2.Zero;
            velocity = new Vector2(30, 30);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Media/XnaLogo");
            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(
                typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            position += (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (!GraphicsDevice.Viewport.Bounds.Contains(new Rectangle(
                (int)position.X, (int)position.Y, texture.Width, texture.Height)))
            {
                bool negateX = false;
                bool negateY = false;
                // Update velocity based on where you crossed the border
                if ((position.X < 0) || ((position.X + texture.Width) >
                    GraphicsDevice.Viewport.Width))
                {
                    negateX = true;
                }
                if ((position.Y < 0) || ((position.Y + texture.Height) >
                    GraphicsDevice.Viewport.Height))
                {
                    negateY = true;
                }
                // Move back to where you were before
                position -= (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (negateX) velocity.X *= -1;
                if (negateY) velocity.Y *= -1;
                // Finally do the correct update
                position += (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }
    }
}
