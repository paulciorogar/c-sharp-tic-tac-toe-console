namespace TicTacToe;

public class State
{
    public readonly Slots<Mark> slots;
    public readonly Mark currentUserMark;

    private State(Mark currentUserMark, Slots<Mark> slots)
    {
        this.currentUserMark = currentUserMark;
        this.slots = slots;
    }

    public static State New()
    {
        var slots = new Slots<Mark>();
        return new State(Mark.X, slots);
    }

    public State Update(PartialState data)
    {
        return new State(
            data.currentUserMark ?? currentUserMark,
            data.slots ?? slots
        );
    }

}

public class PartialState
{
    public Slots<Mark>? slots;
    public Mark? currentUserMark;
}