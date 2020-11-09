using System;

namespace Riemer
{
  public static class Program
  {
    [STAThread]
    private static void Main()
    {
      using (var game = new Game2())
      {
        game.Run();
      }
    }
  }
}