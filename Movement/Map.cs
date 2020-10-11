using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Movement
{
  class Map
  {
    public int[] Data { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public Tile[] TileData { get; set; }

    public void BuildRandomMap(int mapWidth, int mapHeight)
    {
      var rand = new Random();

      WorldDetails.WordWidthInPixles = mapWidth * (int)WorldDetails.ScaledUnitSize;
      WorldDetails.WordHeightInPixles = mapHeight * (int)WorldDetails.ScaledUnitSize;

      int[] tiles = new int[mapWidth * mapHeight];
      Tile[] tileData = new Tile[mapWidth * mapHeight];

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

      Data = tiles;
      Width = mapWidth;
      Height = mapHeight;
      TileData = tileData;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
      // Get camera position
      Vector2 cameraPos = Camera2D.Position;
      //Vector2 cameraOriginBlock = new Vector2((int)Math.Floor(cameraPos.X / WorldDetails.ScaledUnitSize), (int)Math.Floor(cameraPos.Y / WorldDetails.ScaledUnitSize));
      Vector2 cameraOriginBlock = new Vector2(cameraPos.X, cameraPos.Y);
      int cameraBlockWidth = (int)Math.Ceiling(Camera2D.Width / WorldDetails.ScaledUnitSize * 1.0f);
      int cameraBlockHeight = (int)Math.Ceiling(Camera2D.Height / WorldDetails.ScaledUnitSize * 1.0f);
      // Dertimine what blocks on map to draw 160 x 90
      // each row 0 - 159
      // Draw tiles in the camera

      for (int i = (int)cameraOriginBlock.X; i <= cameraBlockWidth + (int)cameraOriginBlock.X; i++)
      {
        for (int j = (int)cameraOriginBlock.Y; j <= cameraBlockHeight + (int)cameraOriginBlock.Y; j++)
        {
          // Use mod to get the remainder (row)
          //if (WorldDetails.ScreenWidthInWorldUnits * i + j < Data.Length)
          //{
          int tile = Data[(int)WorldDetails.ScreenWidthInWorldUnits * i + j];
          int x = (tile % 32) * 8;
          int y = (tile / 32) * 8;

          spriteBatch.Draw(texture, new Vector2(i - cameraOriginBlock.X, j - cameraOriginBlock.Y) * 8, new Rectangle(x, y, 8, 8), Color.White);
          //}

        }
      }

    }
  }
}
