using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNABook_1
{
    public class Ch02Game : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;

        public Ch02Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _position = Vector2.Zero;
            _velocity = new Vector2(30, 30);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = Content.Load<Texture2D>("Media/XnaLogo");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //_position = new Vector2(_position.X + (float)gameTime.ElapsedGameTime.TotalSeconds, _position.Y + (float)gameTime.ElapsedGameTime.TotalSeconds);
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            //_spriteBatch.Draw(_texture, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.Draw(_texture, _position, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
