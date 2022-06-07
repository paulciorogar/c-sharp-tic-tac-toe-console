namespace TicTacToe;

public class State
{
    public readonly IMaybe<LastMark> LastMark;
    public readonly Slots<Mark> Slots;
    public readonly Mark CurrentUserMark;
    public readonly IGameState Conclusion;

    private State(
        IMaybe<LastMark> lastMark,
        Mark currentUserMark,
        Slots<Mark> slots,
        IGameState gameState)
    {
        this.LastMark = lastMark;
        this.CurrentUserMark = currentUserMark;
        this.Slots = slots;
        this.Conclusion = gameState;
    }

    public static State New()
    {
        var slots = new Slots<Mark>();
        return new State(new Nothing<LastMark>(), Mark.X, slots, new NotConcluded());
    }

    public State Update(Func<PartialState, PartialState> callback)
    {
        var data = new PartialState();
        var result = callback(data);
        return new State(
            result.LastMark ?? LastMark,
            result.CurrentUserMark ?? CurrentUserMark,
            result.Slots ?? Slots,
            result.Conclusion ?? Conclusion
        );
    }

}

public class PartialState
{
    public Slots<Mark>? Slots;
    public Mark? CurrentUserMark;
    public IGameState? Conclusion;
    public IMaybe<LastMark>? LastMark;
}