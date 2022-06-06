namespace TicTacToe;

public class State
{
    public readonly Slots<Mark> Slots;
    public readonly Mark CurrentUserMark;

    // public readonly GameState GameState;
    public string Message;

    private State(Mark currentUserMark, Slots<Mark> slots, string message)
    {
        this.CurrentUserMark = currentUserMark;
        this.Slots = slots;
        this.Message = message;
    }

    public static State New()
    {
        var slots = new Slots<Mark>();
        return new State(Mark.X, slots, string.Empty);
    }

    public State Update(PartialState data)
    {
        return new State(
            data.CurrentUserMark ?? CurrentUserMark,
            data.Slots ?? Slots,
            data.Message ?? Message
        );
    }

}

public class PartialState
{
    public Slots<Mark>? Slots;
    public Mark? CurrentUserMark;
    internal string? Message;
}