using System.Numerics;

namespace SimpleMazeGame;

public abstract class InputProvider
{
    public Action onResetPressed;
    public abstract Vector2 GetInput();
    
}