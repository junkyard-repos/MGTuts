using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MusicPlayer.Init();
            var player = MusicPlayer.Instance;

            while (true) ;
        }
    }
}
