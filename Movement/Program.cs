using System;

namespace Movement
{
  public static class Program
  {
    [STAThread]
    static void Main()
    {
      using (var game = new SimpleCamera())
        game.Run();
    }
  }
}
