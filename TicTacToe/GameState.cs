namespace TicTacToe;

public interface IGameState
{
    public string Message { get; init; }
}
public class NotConcluded : IGameState
{
    public string Message { get; init; }
    public NotConcluded()
    {
        Message = string.Empty;
    }
}

public class InvalidInput : IGameState
{
    public string Message { get; init; }
    public InvalidInput(string message)
    {
        Message = message;
    }
}

public class Victory : IGameState
{
    public string Message { get; init; }
    public Victory(string message)
    {
        Message = message;
    }
}

public class Stalemate : IGameState
{
    public string Message { get; init; }
    public Stalemate(string message)
    {
        Message = message;
    }
}

public record SlotId(int Row, int Col);
