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

    public static Vector2 Clamp =>
      new Vector2(
        (worldRectangle.Width - viewPortSize.X) / (Zoom * 8),
        (worldRectangle.Height - viewPortSize.Y) / (Zoom * 8)
      );
    //set { clamp = value; }

    public static void UpdatePosition(Vector2 value)
    {
      position = new Vector2(
        MathHelper.Clamp(value.X, 0, (worldRectangle.Width - viewPortSize.X) / (Zoom * 8)),
        MathHelper.Clamp(value.Y, 0, (worldRectangle.Height - viewPortSize.Y) / (Zoom * 8))
      );
      //Console.WriteLine(position);
    }


    public static Vector2 Position => position;
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
    public static Color[,] TileColorArray;
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

            if (i == (int)BasicPlayer.PlayerBlockPosition().X && j == (int)BasicPlayer.PlayerBlockPosition().Y)
            {
              sb.Draw(texture, new Vector2(i - cameraOriginBlock.X, j - cameraOriginBlock.Y) * 8, new Rectangle(x, y, wu, wu), Color.Red);
            }
            else if (i == (int)BasicPlayer.PlayerBlockPosition().X || j == (int)BasicPlayer.PlayerBlockPosition().Y)
            {
              sb.Draw(texture, new Vector2(i - cameraOriginBlock.X, j - cameraOriginBlock.Y) * 8, new Rectangle(x, y, wu, wu), Color.Green);
            }
            else
            {
              sb.Draw(texture, new Vector2(i - cameraOriginBlock.X, j - cameraOriginBlock.Y) * 8, new Rectangle(x, y, wu, wu), Color.White);
            }
          }
        }
      }
      TilesDrawn = tileCount;
    }
  }

  public static class BasicPlayer
  {


    public static Rectangle spriteLocation = new Rectangle(0, 224, 8, 8);

    private static Vector2 position = new Vector2(20, 20);

    public static int WU = 8;

    public static Color[,] PlayerColorArray;

    public static Vector2 playerOriginBlock;

    public static Vector2 nextOriginBlock;

    public static Vector2 Position => position;
    //set { position = value; }

    public static Vector2 PlayerBlockPosition()
    {
      return new Vector2((position.X + 4) / WU, (position.Y + 4) / WU);
    }

    public static void UpdatePosition(Vector2 pos, BasicMap map)
    {


      Vector2 topLeft = new Vector2((pos.X + 0) / WU, (pos.Y + 0) / WU);
      Vector2 middleLeft = new Vector2((pos.X + 0) / WU, (pos.Y + 4) / WU);
      Vector2 bottomLeft = new Vector2((pos.X + 0) / WU, (pos.Y + 8) / WU);

      Vector2 topMiddle = new Vector2((pos.X + 4) / WU, (pos.Y + 0) / WU);
      Vector2 center = new Vector2((pos.X + 4) / WU, (pos.Y + 4) / WU);
      Vector2 bottomMiddle = new Vector2((pos.X + 4) / WU, (pos.Y + 8) / WU);

      Vector2 topRight = new Vector2((pos.X + 8) / WU, (pos.Y + 0) / WU);
      Vector2 middleRight = new Vector2((pos.X + 8) / WU, (pos.Y + 4) / WU);
      Vector2 bottomRight = new Vector2((pos.X + 8) / WU, (pos.Y + 8) / WU);




      //Vector2 nextOriginBlock = new Vector2(pos.X / WU * BasicCamera.Zoom, pos.Y / WU * BasicCamera.Zoom);

      //if (

      //  map.TileData[(int)nextOriginBlock.X, (int)nextOriginBlock.Y].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X + 1, (int)nextOriginBlock.Y].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X - 1, (int)nextOriginBlock.Y].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X, (int)nextOriginBlock.Y + 1].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X, (int)nextOriginBlock.Y - 1].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X + 1, (int)nextOriginBlock.Y + 1].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X - 1, (int)nextOriginBlock.Y - 1].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X + 1, (int)nextOriginBlock.Y - 1].SpriteTile == 0 &&

      //  map.TileData[(int)nextOriginBlock.X - 1, (int)nextOriginBlock.Y + 1].SpriteTile == 0
      //  )
      //{
      //  position = pos;
      //}




      Vector2 tilePos = map.TileData[(int)topLeft.X, (int)topLeft.Y].Position * WU;
      Vector2 overlap = pos - tilePos;
      Console.WriteLine();
      Console.WriteLine(tilePos.ToString());
      Console.WriteLine(pos.ToString());
      Console.WriteLine(overlap);
      Console.WriteLine();

      if (pos.X > position.X)
      {
        // Get all collidable tiles to the right of player

        // check for right side tile collisions
        bool collidedRight = false;

        if (map.TileData[(int)topRight.X, (int)topRight.Y].SpriteTile == 40)
        {
          // do per pixel check
          // get tile overlap
          // set collidedRight
          //collidedRight = true;
        }

        if (!collidedRight)
        {
          if (map.TileData[(int)bottomRight.X, (int)bottomRight.Y].SpriteTile == 40)
          {
            // do per pixel check
            // set collidedRight
            //collidedRight = true;
          }
        }

        if (!collidedRight)
        {
          position.X = pos.X;
        }
      }
      // player moving left
      else if (pos.X < position.X)
      {


        // check for right side tile collisions
        bool collidedLeft = false;

        if (map.TileData[(int)topLeft.X, (int)topLeft.Y].SpriteTile == 40)
        {

          // create new color arrays
          Color[,] tileOverlap = new Color[(int)overlap.X, (int)overlap.Y];
          int i = 0;
          int j = 0;
          for (int x = (int)overlap.X; x < WU; x++)
          {
            for (int y = (int)overlap.Y; y < WU; y++)
            {
              tileOverlap[i, j] = BasicMap.TileColorArray[x, y];
              j++;
            }
            i++;
          }





          ;
          //collidedLeft = true;
        }

        if (!collidedLeft)
        {
          if (map.TileData[(int)bottomLeft.X, (int)bottomLeft.Y].SpriteTile == 40)
          {
            // do per pixel check
            // set collidedLeft

          }
        }

        if (!collidedLeft)
        {
          position.X = pos.X;
        }
      }


      if (pos.Y > position.Y)
      {
        // check for right side tile collisions
        bool collidedBottom = false;

        if (map.TileData[(int)bottomRight.X, (int)bottomRight.Y].SpriteTile == 40)
        {
          // do per pixel check
          // get tile overlap
          // set collidedRight
          //collidedBottom = true;
        }

        if (!collidedBottom)
        {
          if (map.TileData[(int)bottomLeft.X, (int)bottomLeft.Y].SpriteTile == 40)
          {
            // do per pixel check
            // set collidedRight
            //collidedBottom = true;
          }
        }

        if (!collidedBottom)
        {
          position.Y = pos.Y;
        }
      }
      // player moving left
      else if (pos.Y < position.Y)
      {
        // check for right side tile collisions
        bool collidedTop = false;

        if (map.TileData[(int)topRight.X, (int)topRight.Y].SpriteTile == 40)
        {
          // do per pixel check
          // set collidedLeft
          //collidedTop = true;
        }

        if (!collidedTop)
        {
          if (map.TileData[(int)topLeft.X, (int)topLeft.Y].SpriteTile == 40)
          {
            // do per pixel check
            // set collidedLeft
            //collidedTop = true;
          }
        }

        if (!collidedTop)
        {
          position.Y = pos.Y;
        }
      }

    }

    public static void Draw(SpriteBatch sb, Texture2D texture)
    {
      sb.Draw(texture, Position, spriteLocation, Color.White);
    }

    public static void DebugDraw(SpriteBatch sb, Texture2D texture)
    {
      int normX = (int)(Position.X * 1);
      int normY = (int)(Position.Y * 1);
      // get larger left side
      int x1 = MathHelper.Max(normX, 40);
      // get larger top side
      int y1 = MathHelper.Max(normY, 40);
      // get smaller right side
      int x2 = MathHelper.Min(normX + 8, 60);
      // get smaller bottom side
      int y2 = MathHelper.Min(normY + 8, 60);

      // x2 - x1
      int width = x2 - x1;
      // y2 = y1
      int height = y2 - y1;

      if (width > 0 && height > 0)
      {
        sb.Draw(texture, new Rectangle(x1, y1, width, height), new Rectangle(60, 4, 1, 1), new Color(175, 50, 20, 255));
      }
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

      BasicCamera.Zoom = 6;
      BasicCamera.viewPortSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);



      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _spriteSheet = Content.Load<Texture2D>("Minivania/s4m_ur4i_minivania_tilemap");
      _font = Content.Load<SpriteFont>("Fonts/Magdalena");

      //Color[] player1D = new Color[64];
      //_spriteSheet.GetData(0, BasicPlayer.spriteLocation, player1D, 0, 64);

      //Color[,] player2D = new Color[8, 8];

      //for (int x = 0; x < 8; x++)
      //{
      //  for (int y = 0; y < 8; y++)
      //  {
      //    //int arrPos = x + y * 8;
      //    int arrPos = (x * 8) + y;
      //    Color color = player1D[arrPos];
      //    player2D[x, y] = color;
      //  }
      //}

      //BasicPlayer.PlayerColorArray = player2D;

      Color[] tile1D = new Color[64];
      _spriteSheet.GetData(0, new Rectangle(64, 8, 8, 8), tile1D, 0, 64);

      Color[,] tile2D = new Color[8, 8];

      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
          //int arrPos = x + y * 8;
          int arrPos = (x * 8) + y;
          Color color = tile1D[arrPos];
          tile2D[x, y] = color;
        }
      }

      BasicMap.TileColorArray = tile2D;


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
      BasicPlayer.DebugDraw(_spriteBatch, _spriteSheet);
      _spriteBatch.End();

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(2, 2, 1));
      //DrawString(BasicPlayer.Position.ToString(), new Vector2(20, 20));
      //DrawString(BasicPlayer.nextOriginBlock.ToString(), new Vector2(20, 30));
      //DrawString(BasicPlayer.PlayerBlockPosition().ToString(), new Vector2(20, 40));

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
            tileData[i, j] = new Tile { IsVisable = false, SpriteTile = 0, Position = new Vector2(i, j) };
          }
          else
          {

            //tiles[i] = 40;
            tileData[i, j] = new Tile { IsVisable = true, SpriteTile = 40, Position = new Vector2(i, j) };
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
