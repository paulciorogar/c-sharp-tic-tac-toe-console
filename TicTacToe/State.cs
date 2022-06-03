namespace TicTacToe;

public class State
{
    public readonly Slots<Mark> slots;
    public readonly Mark currentUserMark;
    public string message;

    private State(Mark currentUserMark, Slots<Mark> slots, string message)
    {
        this.currentUserMark = currentUserMark;
        this.slots = slots;
        this.message = message;
    }

    public static State New()
    {
        var slots = new Slots<Mark>();
        return new State(Mark.X, slots, string.Empty);
    }

    public State Update(PartialState data)
    {
        return new State(
            data.currentUserMark ?? currentUserMark,
            data.slots ?? slots,
            data.message ?? message
        );
    }

}

public class PartialState
{
    public Slots<Mark>? slots;
    public Mark? currentUserMark;
    internal string? message;
}