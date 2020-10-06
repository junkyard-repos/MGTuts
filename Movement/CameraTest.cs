using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Movement
{
  public class Camera2D
  {
    private Vector2 _position;
    public Vector2 Position
    {
      get { return _position; }

      set
      {
        _position = new Vector2(MathHelper.Clamp(value.X, 0, 1280), MathHelper.Clamp(value.Y, 0, 720));
      }
    }

    public Vector2 Origin { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Vector2 Center { get; }
    public Vector2 WorldSize { get; set; }
    public float Zoom { get; set; }

    public Camera2D(int width, int height)
    {
      Width = width;
      Height = height;
      Center = new Vector2(width / 2, height / 2);
    }

    public void MoveCamera(Vector2 newPos)
    {
      Position = newPos;
    }

  }

  public class CameraTest : Game
  {
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    Texture2D playerIdleTexture;
    Texture2D playerRunTexture;
    Texture2D playerJumpTexture;
    Texture2D playerFallTexture;
    Texture2D spriteSheetTexture;

    Map _map;
    Camera2D _camera;

    private int screenWidthInWorldUnits;
    private int screenHeightInWorldUnits;
    private int worldUnitSize = 8; // in pixels squared
    private int scaledUnitSize;

    public CameraTest()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    private void BuildWorldMap()
    {
      var rand = new Random();
      int width = 500;
      int height = 500;

      int[] tiles = new int[width * height];

      for (int i = 0; i < tiles.Length; i++)
      {
        if (rand.Next() % 2 != 0)
        {
          tiles[i] = 0;
        }
        else tiles[i] = 40;
      }

      _map = new Map
      {
        Data = tiles,
        Width = width,
        Height = height
      };
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = 1280;
      _graphics.PreferredBackBufferHeight = 720;
      _graphics.ApplyChanges();

      screenWidthInWorldUnits = _graphics.PreferredBackBufferWidth / worldUnitSize; // 160
      screenHeightInWorldUnits = _graphics.PreferredBackBufferHeight / worldUnitSize; // 90

      BuildWorldMap();

      _camera = new Camera2D(500, 500);
      _camera.Position = new Vector2(8, 8);
      _camera.Zoom = 5;
      scaledUnitSize = worldUnitSize * (int)_camera.Zoom;

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
    }

    protected override void Update(GameTime gameTime)
    {
      Console.WriteLine(_camera.Position.ToString());
      if (Keyboard.GetState().IsKeyDown(Keys.W))
      {
        _camera.MoveCamera(_camera.Position + new Vector2(0, -10));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.A))
      {
        _camera.MoveCamera(_camera.Position + new Vector2(-10, 0));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {
        _camera.MoveCamera(_camera.Position + new Vector2(0, 10));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.D))
      {
        _camera.MoveCamera(_camera.Position + new Vector2(10, 0));
      }


      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(_camera.Zoom, _camera.Zoom, 1));
      DrawMap();
      _spriteBatch.End();

      base.Draw(gameTime);
    }

    private void DrawMap()
    {
      // Get camera position
      Vector2 cameraPos = _camera.Position;
      Vector2 cameraOriginBlock = new Vector2((int)Math.Floor(cameraPos.X / scaledUnitSize), (int)Math.Floor(cameraPos.Y / scaledUnitSize));
      int cameraBlockWidth = (int)Math.Ceiling(_camera.Width / scaledUnitSize * 1.0f);
      int cameraBlockHeight = (int)Math.Ceiling(_camera.Height / scaledUnitSize * 1.0f);
      // Dertimine what blocks on map to draw 160 x 90
      // each row 0 - 159
      // Draw tiles in the camera
      for (int i = (int)cameraOriginBlock.X; i < cameraBlockWidth + (int)cameraOriginBlock.X; i++)
      {
        for (int j = (int)cameraOriginBlock.Y; j < cameraBlockHeight + (int)cameraOriginBlock.Y; j++)
        {
          // Use mod to get the remainder (row)
          int tile = _map.Data[160 * i + j];
          int x = (tile % 32) * 8;
          int y = (tile / 32) * 8;

          ;

          _spriteBatch.Draw(spriteSheetTexture, new Vector2(i, j) * scaledUnitSize, new Rectangle(x, y, 8, 8), Color.White);
        }
      }

      ;

    }
  }
}
