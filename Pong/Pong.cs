using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Pong : Game
    {
        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 200f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("Images\\ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here

            var kstate = Keyboard.GetState();
            MovePlayer(gameTime, kstate);

            base.Update(gameTime);

        }

        private void MovePlayer(GameTime gameTime, KeyboardState kstate)
        {
            var bp = ballPosition;

            if (kstate.IsKeyDown(Keys.Up))
            {
                ballPosition.Y = ClampY(bp.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                ballPosition.Y = ClampY(bp.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                ballPosition.X = ClampX(bp.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                ballPosition.X = ClampX(bp.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }



        }

        private float ClampY(float position)
        {
            return Math.Clamp(position, ballTexture.Height / 2, graphics.PreferredBackBufferHeight - ballTexture.Height / 2);
        }

        private float ClampX(float position)
        {
            return Math.Clamp(position, ballTexture.Width / 2, graphics.PreferredBackBufferWidth - ballTexture.Width / 2);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f, new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
