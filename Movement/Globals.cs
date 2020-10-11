using Microsoft.Xna.Framework;

namespace Movement
{
  public class Globals
  {

  }

  public static class WorldDetails
  {
    private static float zoom;

    public static int ScreenWidthInWorldUnits { get; set; }
    public static int ScreenHeightInWorldUnits { get; set; }
    public static int WorldUnitSize { get; set; } = 8;
    public static int ScaledUnitSize { get; set; }
    public static int WordWidthInPixles { get; set; }
    public static int WordHeightInPixles { get; set; }

    public static float Zoom
    {
      get => zoom;

      // Add zoom constraints
      set => zoom = value;
    }
  }

  public static class Camera2D
  {
    private static Vector2 _position;

    public static Vector2 Position
    {
      get { return _position; }

      set
      {
        _position = new Vector2(MathHelper.Clamp(value.X, 0, WorldDetails.WordWidthInPixles), MathHelper.Clamp(value.Y, 0, WorldDetails.WordHeightInPixles));
      }
    }

    public static Vector2 Origin { get; set; }
    public static int Width { get; set; }
    public static int Height { get; set; }
    public static Vector2 Center { get; }
    public static Vector2 WorldSize { get; set; }
    public static float Zoom { get; set; }

    public static void MoveCamera(Vector2 newPos)
    {
      Position = newPos;
    }
  }
}
