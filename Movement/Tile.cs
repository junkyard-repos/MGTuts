using Microsoft.Xna.Framework;

namespace Movement
{
  public struct Tile
  {
    public int SpriteTile { get; set; }
    public bool IsVisable { get; set; }
    public Vector2 Position { get; set; }
  }
}
