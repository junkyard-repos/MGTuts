using System;

namespace XNABook_1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Ch05Game())
                game.Run();
        }
    }
}
