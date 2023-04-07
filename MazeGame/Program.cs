using System;
using System.Collections.Generic;

namespace SimpleMazeGame
{
    class Program
    {
        // configuration
        static int width = 20;
        static int height = 20;
        static int seed = 132131123;
        
        static void Main(string[] args)
        {
            Game game = new Game(new StandartInput())
            {
                width = width,
                height = height,
                randomSeed = true,
            };
            game.Run();
        }
        
        
    }
}