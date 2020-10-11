using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Movement
{


  public class CameraTest : Game
  {
    #region properties
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    Texture2D playerIdleTexture;
    Texture2D playerRunTexture;
    Texture2D playerJumpTexture;
    Texture2D playerFallTexture;
    Texture2D spriteSheetTexture;
    private SpriteFont _font;

    Map _map = new Map();

    #endregion

    public CameraTest()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    private void BuildWorldMap()
    {
      var rand = new Random();
      int width = 50;
      int height = 50;

      int[] tiles = new int[width * height];
      Tile[] tileData = new Tile[width * height];

      for (int i = 0; i < tiles.Length; i++)
      {
        if (rand.Next() % 2 != 0)
        {
          tiles[i] = 0;
          tileData[i] = new Tile { IsVisable = false, SpriteTile = 0 };
        }
        else
        {

          tiles[i] = 40;
          tileData[i] = new Tile { IsVisable = true, SpriteTile = 40 };
        }
      }

      _map = new Map
      {
        Data = tiles,
        Width = width,
        Height = height,
        TileData = tileData,
      };
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = 800;
      _graphics.PreferredBackBufferHeight = 600;
      _graphics.ApplyChanges();

      WorldDetails.WorldUnitSize = 8;

      WorldDetails.ScreenWidthInWorldUnits = _graphics.PreferredBackBufferWidth / WorldDetails.WorldUnitSize; // 160
      WorldDetails.ScreenHeightInWorldUnits = _graphics.PreferredBackBufferHeight / WorldDetails.WorldUnitSize; // 90

      _map.BuildRandomMap(505, 505);

      Camera2D.Width = _graphics.PreferredBackBufferWidth;
      Camera2D.Height = _graphics.PreferredBackBufferHeight;
      Camera2D.Position = new Vector2(0, 0);
      Camera2D.Zoom = 4;


      WorldDetails.ScaledUnitSize = WorldDetails.WorldUnitSize * (int)Camera2D.Zoom;

      WorldDetails.WordWidthInPixles = _map.Width + WorldDetails.ScreenWidthInWorldUnits;
      WorldDetails.WordHeightInPixles = _map.Height * (int)WorldDetails.ScreenHeightInWorldUnits;

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      playerIdleTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_idle");
      playerRunTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_run");
      playerJumpTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_jump");
      playerFallTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_jump_midair");
      spriteSheetTexture = Content.Load<Texture2D>("Minivania/s4m_ur4i_minivania_tilemap");
      _font = Content.Load<SpriteFont>("Fonts/Magdalena");
    }

    protected override void Update(GameTime gameTime)
    {
      //Console.WriteLine(Camera2D.Position.ToString());
      if (Keyboard.GetState().IsKeyDown(Keys.W))
      {
        Camera2D.MoveCamera(Camera2D.Position + new Vector2(0, -0.3f));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.A))
      {
        //Console.WriteLine(Camera2D.Position.X);
        Camera2D.MoveCamera(Camera2D.Position + new Vector2(-0.3f, 0));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {
        Camera2D.MoveCamera(Camera2D.Position + new Vector2(0, 0.3f));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.D))
      {
        //Console.WriteLine(Camera2D.Position.X);
        //Console.WriteLine(Camera2D.Width / scaledUnitSize);
        //Console.WriteLine(screenWidthInWorldUnits - (Camera2D.Width / scaledUnitSize));

        //if (Camera2D.Position.X < screenWidthInWorldUnits - (Camera2D.Width / scaledUnitSize * 2))
        Camera2D.MoveCamera(Camera2D.Position + new Vector2(0.3f, 0));
      }


      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(Camera2D.Zoom, Camera2D.Zoom, 1));
      //_spriteBatch.Begin();


      _map.Draw(_spriteBatch, spriteSheetTexture);

      DrawString(WorldDetails.ScreenWidthInWorldUnits.ToString(), new Vector2(20, 20));
      DrawString(Camera2D.Position.ToString(), new Vector2(20, 30));


      _spriteBatch.End();

      base.Draw(gameTime);
    }


    private void DrawString(SpriteFont font, string text, Vector2 position, Color color)
    {

      _spriteBatch.DrawString(font, text, position, color);
    }

    private void DrawString(string text, Vector2 position)
    {
      _spriteBatch.DrawString(_font, text, position, Color.White);

    }
  }
}
