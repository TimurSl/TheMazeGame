using System.Numerics;

namespace SimpleMazeGame;

public class LevelGenerator
{
    private Vector2 finishPos;
    public bool[,] GetLevel(int width, int height, int seed)
    {
        bool[,] maze = new bool[width, height];

        // Initialize all cells as walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = false;
            }
        }

        // make walls around the maze
        for (int x = 0; x < width; x++)
        {
            maze[x, 0] = true;
            maze[x, height - 1] = true;
        }
        for (int y = 0; y < height; y++)
        {
            maze[0, y] = true;
            maze[width - 1, y] = true;
        }

        // initialize the random number generator with the given seed value
        Random rand = new Random(seed);

        // level generation using recursive backtracking
        int startX = rand.Next(0, width - 2);
        int startY = rand.Next(0, height - 2);
        RecursiveBacktracking(maze, startX, startY, rand);

        // add random points to the maze with open paths
        for (int i = 0; i < width / 2; i++)
        {
            int x = rand.Next(1, width - 2);
            int y = rand.Next(1, height - 2);
            maze[x, y] = false;
        }
        
        // add finish point to the maze
        int finishX = rand.Next(1, width - 2);
        int finishY = rand.Next(1, height - 2);
        maze[finishX, finishY] = false;
        finishPos = new Vector2(finishX, finishY);
        
        return maze;
    }

    public Vector2 GetFinishPos()
    {
        if (finishPos.X == 0 && finishPos.Y == 0)
        {
            finishPos = new Vector2(1, 1);
        }
        return finishPos;
    }
    
    private static void RecursiveBacktracking(bool[,] maze, int x, int y, Random rand)
    {
        maze[x, y] = true;

        List<int[]> directions = new List<int[]>
        {
            new int[] { -1, 0 }, // left
            new int[] { 1, 0 }, // right
            new int[] { 0, -1 }, // up
            new int[] { 0, 1 } // down
        };

        // shuffle the directions randomly
        for (int i = 0; i < directions.Count; i++)
        {
            int r = rand.Next(i, directions.Count);
            (directions[r], directions[i]) = (directions[i], directions[r]);
        }

        // recursive backtracking
        foreach (int[] direction in directions)
        {
            int dx = direction[0];
            int dy = direction[1];

            int newX = x + dx * 2;
            int newY = y + dy * 2;

            if (newX >= 1 && newX < maze.GetLength(0) - 1 &&
                newY >= 1 && newY < maze.GetLength(1) - 1 &&
                !maze[newX, newY])
            {
                maze[x + dx, y + dy] = true;
                RecursiveBacktracking(maze, newX, newY, rand);
            }
        }
    }
}

