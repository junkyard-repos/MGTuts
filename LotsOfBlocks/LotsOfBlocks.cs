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
    private Camera2d _camera2d;
    private SpriteFont _font;
    private string _gameTime;

    private const int BLOCK_BASE_SIZE = 8;
    private int _tilesDrawn = 0;

    public LotsOfBlocks()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = 1280;
      _graphics.PreferredBackBufferHeight = 720;
      _graphics.ApplyChanges();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);
      _texture = Content.Load<Texture2D>("Minivania/s4m_ur4i_minivania_tilemap");
      _tile = new Rectangle(8, 8, 8, 8);
      _tiles = new Tile[5000, 5000];
      //int temp = _tiles.Length;
      _camera2d = new Camera2d();
      _font = Content.Load<SpriteFont>("Fonts/Magdalena");
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

      if (Keyboard.GetState().IsKeyDown(Keys.A))
      {

        _camera2d.Zoom += 0.1f;
      }

      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {

        _camera2d.Zoom -= 0.1f;
      }

      _gameTime = gameTime.ElapsedGameTime.ToString();

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(new Color(50, 50, 50));

      Matrix globalTransformation = Matrix.CreateScale(_camera2d.Zoom, _camera2d.Zoom, 1);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, globalTransformation);
      DrawTiles();
      _spriteBatch.End();

      _spriteBatch.Begin();
      DrawString(_font, _gameTime, new Vector2(300, 100), new Color(255, 255, 255));
      DrawString(_font, gameTime.TotalGameTime.ToString(), new Vector2(300, 120), new Color(255, 255, 255));
      DrawString(_tilesDrawn.ToString(), new Vector2(300, 140));
      _spriteBatch.End();

      base.Draw(gameTime);
    }

    private void DrawTiles()
    {
      int tilesToDraw = 0;
      int numBlocksWidth = 0;
      int numBlocksHeight = 0;

      numBlocksHeight = _graphics.PreferredBackBufferHeight / BLOCK_BASE_SIZE / (int)_camera2d.Zoom + 2;
      numBlocksWidth = _graphics.PreferredBackBufferWidth / BLOCK_BASE_SIZE / (int)_camera2d.Zoom + 2;

      for (int i = 0; i < numBlocksWidth; i++)
      {
        for (int j = 0; j < numBlocksHeight; j++)
        {
          tilesToDraw++;
          if (_tiles[i, j].Visable)
          {
            _spriteBatch.Draw(_texture, new Vector2(i * BLOCK_BASE_SIZE, j * BLOCK_BASE_SIZE), _tile, Color.White);
          }
        }
      }

      if (tilesToDraw != _tilesDrawn)
      {
        _tilesDrawn = tilesToDraw;
      }
    }

    private void DrawString(SpriteFont font, string text, Vector2 position, Color color)
    {

      _spriteBatch.DrawString(font, text, position, color);
    }

    private void DrawString(string text, Vector2 position)
    {
      DrawString(_font, text, position, Color.White);

    }
  }
}
