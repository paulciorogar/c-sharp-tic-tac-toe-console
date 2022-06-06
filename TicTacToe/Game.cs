namespace TicTacToe;

public class Game
{
    public Game(State state)
    {
        State = state;
    }

    public State State { get; internal set; }

    public void MarkSlot(int row, int col)
    {
        var maybeSlot = State.Slots.Val(row, col);

        State = maybeSlot.Map(slot => slot switch
        {
            Mark.NONE => ApplyMarkOn(row, col),
            _ => SlotAlreadyMarked(row, col, slot)
        })
        .CatchMap(ImpossibleSlot(row, col))
        .Map(ResolveGame(row, col))
        .OrSome(State);
    }

    private Func<State, State> ResolveGame(int row, int col)
    {
        return state => Maybe.Just(state)
            .Filter(state => state.Message == string.Empty)
            .Map(Victory)
            .OrSome(state);
    }

    private State Victory(State state)
    {
        return state;
    }

    private Func<State> ImpossibleSlot(int row, int col)
    {
        return () =>
        {
            var data = new PartialState();
            data.Conclusion = new InvalidInput($"Slot {row}.{col} is not in this game");
            return State.Update(data);
        };
    }

    private State SlotAlreadyMarked(int row, int col, Mark slot)
    {
        var data = new PartialState();
        data.Conclusion = new InvalidInput($"Slot {row}.{col} already marked by: {slot}");
        return State.Update(data);
    }

    // public void Mark(string slotId)
    // {
    //     var slotIdParts = slotId.Split(".");
    //     if (slotIdParts.Length != 2)
    //     {
    //         throw new Exception("TODO: move this shit out of here");
    //     }

    //     // TODO: move this shit also
    //     int row;
    //     int col;
    //     var rowParseSuccess = int.TryParse(slotIdParts[0], out row);
    //     var colParseSuccess = int.TryParse(slotIdParts[1], out col);


    // }
    private State ApplyMarkOn(int row, int col)
    {
        var data = new PartialState();
        if (State.Conclusion is InvalidInput)
        {
            data.Conclusion = new NotConcluded();
        }

        data.Slots = State.Slots.Update(row, col, State.CurrentUserMark);
        data.CurrentUserMark = State.CurrentUserMark == Mark.X ? Mark.O : Mark.X;
        return State.Update(data);
    }

}
