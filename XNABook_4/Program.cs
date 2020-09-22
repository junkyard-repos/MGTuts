using System;

namespace XNABook_4
{
  public static class Program
  {
    [STAThread]
    static void Main()
    {
      using (var game = new Game6())
        game.Run();
    }
  }
}
