using System.Numerics;

namespace SimpleMazeGame;

public class StandartInput : InputProvider
{
    public override Vector2 GetInput()
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        
        if (key.Key == ConsoleKey.R)
        {
            onResetPressed?.Invoke();
        }
        
        var dir = key.Key switch
        {
            ConsoleKey.UpArrow => new Vector2(0, -1),
            ConsoleKey.DownArrow => new Vector2(0, 1),
            ConsoleKey.LeftArrow => new Vector2(-1, 0),
            ConsoleKey.RightArrow => new Vector2(1, 0),
            _ => new Vector2(0, 0)
        };
        
        return dir;
    }
    
}