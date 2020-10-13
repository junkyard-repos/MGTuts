using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Movement
{
  public static class BasicCamera
  {
    private static Vector2 position = Vector2.Zero;
    public static Vector2 viewPortSize = Vector2.Zero;
    public static Rectangle worldRectangle = new Rectangle(0, 0, 0, 0);
    public static int Zoom = 1;

    private static Vector2 clamp;

    public static Vector2 Clamp
    {
      get
      {
        return new Vector2(
          (worldRectangle.Width - viewPortSize.X) / (Zoom * 8),
          (worldRectangle.Height - viewPortSize.Y) / (Zoom * 8)
        );
      }

      //set { clamp = value; }
    }


    public static void UpdatePosition(Vector2 value)
    {
      position = new Vector2(
        MathHelper.Clamp(value.X, 0, (worldRectangle.Width - viewPortSize.X) / (Zoom * 8)),
        MathHelper.Clamp(value.Y, 0, (worldRectangle.Height - viewPortSize.Y) / (Zoom * 8))
      );
      //Console.WriteLine(position);
    }


    public static Vector2 Position
    {
      get { return position; }

    }


  }

  public class BasicMap
  {
    public int[] Data { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public Tile[,] TileData { get; set; }

    public Vector2 cameraOriginBlock { get; set; }
    public int cameraBlockWidth { get; set; }
    public int cameraBlockHeight { get; set; }

    public int TilesDrawn { get; private set; }

    public void Draw(SpriteBatch sb, Texture2D texture, int wu)
    {
      int tileCount = 0;
      cameraOriginBlock = new Vector2(BasicCamera.Position.X / wu * BasicCamera.Zoom, BasicCamera.Position.Y / wu * BasicCamera.Zoom);
      cameraBlockWidth = (int)Math.Ceiling(BasicCamera.viewPortSize.X / (wu * BasicCamera.Zoom));
      cameraBlockHeight = (int)Math.Ceiling(BasicCamera.viewPortSize.Y / (wu * BasicCamera.Zoom));

      for (int i = (int)cameraOriginBlock.X; i <= cameraBlockWidth + (int)cameraOriginBlock.X; i++)
      {
        for (int j = (int)cameraOriginBlock.Y; j <= cameraBlockHeight + (int)cameraOriginBlock.Y; j++)
        {
          if (i < TileData.GetUpperBound(0) && j < TileData.GetUpperBound(1))
          {
            int tile = TileData[i, j].SpriteTile;
            int x = (tile % 32) * 8;
            int y = (tile / 32) * 8;
            tileCount++;
            sb.Draw(texture, new Vector2(i - cameraOriginBlock.X, j - cameraOriginBlock.Y) * 8, new Rectangle(x, y, wu, wu), Color.White);
          }
        }
      }
      TilesDrawn = tileCount;
    }
  }

  public static class BasicPlayer
  {


    public static Rectangle spriteLocation = new Rectangle(0, 209, 8, 8);

    private static Vector2 position = new Vector2(20, 20);

    public static int WU = 8;

    public static Vector2 playerOriginBlock;

    public static Vector2 Position
    {
      get { return position; }
      //set { position = value; }
    }



    public static void UpdatePosition(Vector2 pos, BasicMap map)
    {

      Vector2 nextOriginBlock = new Vector2(pos.X / WU * BasicCamera.Zoom, pos.Y / WU * BasicCamera.Zoom);

      if (
        /*
         * 0 0 0
         * 0 1 0
         * 0 0 0
        */
        map.TileData[(int)nextOriginBlock.X, (int)nextOriginBlock.Y].SpriteTile == 0 &&
        /*
         * 0 0 0
         * 0 0 1
         * 0 0 0
        */
        map.TileData[(int)nextOriginBlock.X + 1, (int)nextOriginBlock.Y].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X - 1, (int)nextOriginBlock.Y].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X, (int)nextOriginBlock.Y + 1].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X, (int)nextOriginBlock.Y - 1].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X + 1, (int)nextOriginBlock.Y + 1].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X - 1, (int)nextOriginBlock.Y - 1].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X + 1, (int)nextOriginBlock.Y - 1].SpriteTile == 0 &&

        map.TileData[(int)nextOriginBlock.X - 1, (int)nextOriginBlock.Y + 1].SpriteTile == 0
        )
      {
        position = pos;
      }

    }

    public static void Draw(SpriteBatch sb, Texture2D texture)
    {
      sb.Draw(texture, Position, spriteLocation, Color.White);
    }
  }

  public class SimpleCamera : Game
  {
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    Texture2D _spriteSheet;
    SpriteFont _font;
    BasicMap _map;
    int _worldUnit = 8;

    public SimpleCamera()
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

      BasicCamera.Zoom = 4;
      BasicCamera.viewPortSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);



      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _spriteSheet = Content.Load<Texture2D>("Minivania/s4m_ur4i_minivania_tilemap");
      _font = Content.Load<SpriteFont>("Fonts/Magdalena");

      BuildRandom2DMap(500, 500);
      BasicCamera.worldRectangle = new Rectangle(0, 0, _map.Width * _worldUnit * BasicCamera.Zoom, _map.Height * _worldUnit * BasicCamera.Zoom);
      //BasicCamera.worldRectangle = new Rectangle(0, 0, _spriteSheet.Width * BasicCamera.Zoom, _spriteSheet.Height * BasicCamera.Zoom);
      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (Keyboard.GetState().IsKeyDown(Keys.W))
      {
        //BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(0, -50f * deltaTime));
        BasicPlayer.UpdatePosition(BasicPlayer.Position + new Vector2(0, -50f * deltaTime), _map);
      }
      if (Keyboard.GetState().IsKeyDown(Keys.A))
      {
        //BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(-50f * deltaTime, 0));
        BasicPlayer.UpdatePosition(BasicPlayer.Position + new Vector2(-50f * deltaTime, 0), _map);
      }
      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {
        //BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(0, 50f * deltaTime));
        BasicPlayer.UpdatePosition(BasicPlayer.Position + new Vector2(0, 50f * deltaTime), _map);
      }
      if (Keyboard.GetState().IsKeyDown(Keys.D))
      {
        //BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(50f * deltaTime, 0));
        BasicPlayer.UpdatePosition(BasicPlayer.Position + new Vector2(50f * deltaTime, 0), _map);
      }


      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(BasicCamera.Zoom, BasicCamera.Zoom, 1));

      //_spriteBatch.Draw(_spriteSheet, Vector2.Zero, new Rectangle((int)BasicCamera.Position.X, (int)BasicCamera.Position.Y, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
      _map.Draw(_spriteBatch, _spriteSheet, _worldUnit);
      BasicPlayer.Draw(_spriteBatch, _spriteSheet);

      //DrawString(_spriteSheet.Width.ToString(), new Vector2(20, 10));
      //DrawString((_spriteSheet.Width * BasicCamera.Zoom).ToString(), new Vector2(20, 20));
      //DrawString((BasicCamera.Position).ToString(), new Vector2(20, 30));
      //DrawString((_map.cameraOriginBlock).ToString(), new Vector2(20, 40));
      //DrawString((_map.cameraBlockWidth).ToString(), new Vector2(20, 50));
      //DrawString((_map.cameraBlockHeight).ToString(), new Vector2(20, 60));
      //DrawString((_map.TilesDrawn).ToString(), new Vector2(20, 70));
      //DrawString((_map.TileData.GetUpperBound(0)).ToString(), new Vector2(20, 80));
      //DrawString((_map.TileData.GetUpperBound(1)).ToString(), new Vector2(20, 90));
      //DrawString(BasicCamera.Clamp.ToString(), new Vector2(20, 100));

      _spriteBatch.End();

      base.Draw(gameTime);
    }

    public void DrawPlayer()
    {
      _spriteBatch.Draw(_spriteSheet, BasicPlayer.Position, BasicPlayer.spriteLocation, Color.White);
    }

    public void DrawString(string text, Vector2 position)
    {
      _spriteBatch.DrawString(_font, text, position, Color.White);

    }

    //public void BuildRandomMap(int mapWidth, int mapHeight)
    //{
    //  var rand = new Random();

    //  //WorldDetails.WordWidthInPixles = mapWidth * (int)WorldDetails.ScaledUnitSize;
    //  //WorldDetails.WordHeightInPixles = mapHeight * (int)WorldDetails.ScaledUnitSize;

    //  int[] tiles = new int[mapWidth * mapHeight];
    //  Tile[] tileData = new Tile[mapWidth * mapHeight];

    //  for (int i = 0; i < tiles.Length; i++)
    //  {
    //    if (rand.Next() % 5 != 0)
    //    {
    //      tiles[i] = 0;
    //      tileData[i] = new Tile { IsVisable = false, SpriteTile = 0 };
    //    }
    //    else
    //    {

    //      tiles[i] = 40;
    //      tileData[i] = new Tile { IsVisable = true, SpriteTile = 40 };
    //    }
    //  }

    //  _map = new BasicMap
    //  {
    //    Data = tiles,
    //    Width = mapWidth,
    //    Height = mapHeight,
    //    TileData = tileData
    //  };

    //}

    public void BuildRandom2DMap(int mapWidth, int mapHeight)
    {
      var rand = new Random();

      //WorldDetails.WordWidthInPixles = mapWidth * (int)WorldDetails.ScaledUnitSize;
      //WorldDetails.WordHeightInPixles = mapHeight * (int)WorldDetails.ScaledUnitSize;

      int[] tiles = new int[mapWidth * mapHeight];
      Tile[,] tileData = new Tile[mapWidth, mapHeight];

      for (int i = 0; i < tileData.GetUpperBound(0); i++)
      {
        for (int j = 0; j < tileData.GetUpperBound(1); j++)
        {

          if (rand.Next() % 133 != 0)
          {
            //tiles[i] = 0;
            tileData[i, j] = new Tile { IsVisable = false, SpriteTile = 0 };
          }
          else
          {

            //tiles[i] = 40;
            tileData[i, j] = new Tile { IsVisable = true, SpriteTile = 40 };
          }
        }

      }

      _map = new BasicMap
      {
        Data = tiles,
        Width = mapWidth,
        Height = mapHeight,
        TileData = tileData
      };

    }
  }
}
