namespace TicTacToe;

public class State
{
    public readonly Slots<Mark> Slots;
    public readonly Mark CurrentUserMark;
    public readonly IGameState Conclusion;
    public string Message;

    private State(
        Mark currentUserMark,
        Slots<Mark> slots,
        string message,
        IGameState gameState)
    {
        this.CurrentUserMark = currentUserMark;
        this.Slots = slots;
        this.Conclusion = gameState;
        this.Message = message;
    }

    public static State New()
    {
        var slots = new Slots<Mark>();
        return new State(Mark.X, slots, string.Empty, new NotConcluded());
    }

    public State Update(PartialState data)
    {
        return new State(
            data.CurrentUserMark ?? CurrentUserMark,
            data.Slots ?? Slots,
            data.Message ?? Message,
            data.Conclusion ?? Conclusion
        );
    }

}

public class PartialState
{
    public Slots<Mark>? Slots;
    public Mark? CurrentUserMark;
    public IGameState? Conclusion;
    internal string? Message;
}