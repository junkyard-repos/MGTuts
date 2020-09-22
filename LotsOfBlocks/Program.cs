using System;

namespace LotsOfBlocks
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new LotsOfBlocks())
                game.Run();
        }
    }
}
