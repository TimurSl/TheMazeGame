using System.Diagnostics;
using System.Numerics;
using Figgle;

namespace SimpleMazeGame;

public class Game
{
    public char Wall = '#';
    public char Path = ' ';
    public char Border = '│';
    public char Player = '■';
    public char Exit = '▒';
    
    public ConsoleColor PlayerColor = ConsoleColor.Red;
    public ConsoleColor ExitColor = ConsoleColor.Green;
    public ConsoleColor WallColor = ConsoleColor.Yellow;
    public ConsoleColor PathColor = ConsoleColor.Black;
    public ConsoleColor BorderColor = ConsoleColor.Black;
    
    
    public int width = 20;
    public int height = 20;
    public int seed = 0;
    public bool randomSeed = false;
    
    private static bool[,] maze;
    private static Vector2 finishPos;
    
    public Vector2 playerStart = new Vector2(1, 1);
    private Vector2 playerPosition = new Vector2(1, 1);
    private InputProvider inputProvider = new StandartInput();
    public LevelGenerator levelGenerator = new LevelGenerator();
    public LevelSystem levelSystem;
    
    private bool gameOver = false;
    
    public Game(InputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }
    
    public void Run()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        WelcomeMessage();

        StartLevel();
        
        inputProvider.onResetPressed += (() =>
        {
            StartLevel();
        });
        
        while (!gameOver)
        {
            Console.Clear();
            // draw
            DrawMaze(maze, playerPosition, finishPos);
            // update
            UpdateInput();
            
            // check if player is on exit
            gameOver = playerPosition == finishPos;
        }
        
        FinishMessage(sw);

        Console.ReadKey();
        // stop the program
        Environment.Exit(0);
    }

    private void FinishMessage(Stopwatch sw)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            Console.SetWindowSize(80, 25);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(FiggleFonts.Standard.Render("You Win!"));
        Console.WriteLine("Time: " + sw.Elapsed);
        Console.WriteLine("Congratulations! You won!");
        Console.WriteLine("Press any key to exit");

        if (levelSystem != null)
            levelSystem.LevelUp();
    }

    private static void WelcomeMessage()
    {
        Console.WriteLine(FiggleFonts.Standard.Render("Maze Game"));
        Console.WriteLine("Press any key to start");
        Console.ReadKey();
    }

    private void StartLevel()
    {
        if (randomSeed)
            seed = DateTime.Now.Millisecond * DateTime.Now.Second * DateTime.Now.Minute / DateTime.Now.Hour;
        if (levelSystem != null)
        {
            int currentLevel = levelSystem.GetLevel();
            maze = levelGenerator.GetLevel(currentLevel <= 10 ? 10 : width + currentLevel,
                currentLevel <= 10 ? 10 : height + currentLevel, seed + currentLevel);
        }
        else
        {
            maze = levelGenerator.GetLevel(width, height, seed);
        }
        finishPos = levelGenerator.GetFinishPos();
        playerPosition = playerStart;
        gameOver = false;
    }

    private void UpdateInput()
    {
        Vector2 input = inputProvider.GetInput();
        
        if (input != Vector2.Zero)
        {
            bool canPass = IsPath(playerPosition + input);
            if (canPass)
            {
                playerPosition += input;
            }
        }
    }
    
    private bool IsPath(Vector2 pos)
    {
        // check if the pos not the wall
        bool isWall = maze[(int) pos.Y, (int) pos.X];
        return !isWall;
    }
    
    private  void DrawMaze(bool[,] walls, Vector2 playerPos, Vector2 exitPos)
    {
        // if platform is windows 
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            // set the console window size to the maze size
#pragma warning disable CA1416 // im tired of this warnings
            Console.SetWindowSize(width * 2, height + 1);
            // Console.SetBufferSize(width * 3, height * 3);
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // if the current cell is the player cell
                if (x == playerPosition.X && y == playerPosition.Y)
                {
                    WriteToConsoleWithColor(PlayerColor, Player.ToString());
                }
                // if the current cell is the exit cell
                else if (x == exitPos.X && y == exitPos.Y)
                {
                    walls[y, x] = false;
                    WriteToConsoleWithColor(ExitColor, Exit.ToString());
                }
                else if (walls[y, x])
                {
                    WriteToConsoleWithColor(WallColor, Wall.ToString());
                }
                else
                {
                    WriteToConsoleWithColor(PathColor, Path.ToString());
                }

                WriteToConsoleWithColor(BorderColor, Border.ToString());
            }
            Console.WriteLine();
        }
    }

    private void WriteToConsoleWithColor(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    
    
}