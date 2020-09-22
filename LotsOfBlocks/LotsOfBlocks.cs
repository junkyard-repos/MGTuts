using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LotsOfBlocks
{
    public class LotsOfBlocks : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Rectangle _tile;
        private Tile[,] _tiles;

        public LotsOfBlocks()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texture = Content.Load<Texture2D>("Minivania/s4m_ur4i_minivania_tilemap");
            _tile = new Rectangle(8, 8, 8, 8);
            _tiles = new Tile[5000, 5000];
            //int temp = _tiles.Length;
            PopulateTiles();
        }

        private void PopulateTiles()
        {
            var rand = new Random();

            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    _tiles[i, j] = new Tile { TileSheetPos = new Rectangle(8, 8, 8, 8), Visable = rand.Next() % 5 != 0 };
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(50, 50, 50));

            Matrix globalTransformation = Matrix.CreateScale(3, 3, 1);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, globalTransformation);

            DrawTiles();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawTiles()
        {


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_tiles[i, j].Visable)
                    {
                        _spriteBatch.Draw(_texture, new Vector2(i * 8, j * 8), _tile, Color.White);
                    }
                }
            }
        }
    }
}
