namespace SimpleMazeGame
{
    class Program
    {
        // configuration
        static int width = 10;
        static int height = 10;
        static int seed = 0;

        static void Main(string[] args)
        {
            Game game = new Game(width, height, seed, new StandartInput(), new LevelGenerator(), new StandartLevelSystem());
            game.Run();
        }
    }
}