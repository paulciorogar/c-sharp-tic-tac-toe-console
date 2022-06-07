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
        .Map(ResolveGame)
        .OrSome(State);
    }

    private State ResolveGame(State state)
    {
        return Maybe.Just(state)
            .Filter(state => state.Conclusion is not InvalidInput)
            .Map(Victory)
            .OrSome(state);
    }

    private State Victory(State state)
    {
        return state.LastMark.Map(lastMark =>
        {
            var count = countWestMarks(lastMark, state.Slots) + countEastMarks(lastMark, state.Slots);
            if (count == 2)
            {
                return state.Update(data =>
                {
                    data.Conclusion = new Victory($"{lastMark.Mark} won the game");
                    return data;
                });
            }
            return state;
        }).OrSome(state);
    }

    private static int countWestMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<int>() { lastMark.Col - 1, lastMark.Col - 2 };
        return CountMarks(lastMark, slots, columnIdList);
    }
    private static int countEastMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<int>() { lastMark.Col + 1, lastMark.Col + 2 };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int CountMarks(LastMark lastMark, Slots<Mark> slots, IEnumerable<int> indexList)
    {
        return indexList.Select(colIndex => slots.Val(lastMark.Row, colIndex)
            .Filter(mark => mark == lastMark.Mark)
            .Map(_ => 1)
            .OrSome(0)
        ).Sum();
    }

    private Func<State> ImpossibleSlot(int row, int col)
    {
        return () =>
        {
            return State.Update(data =>
            {
                data.Conclusion = new InvalidInput($"Slot {row}.{col} is not in this game");
                return data;
            });
        };
    }

    private State SlotAlreadyMarked(int row, int col, Mark slot)
    {
        return State.Update(data =>
        {
            data.Conclusion = new InvalidInput($"Slot {row}.{col} already marked by: {slot}");
            return data;
        });
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
        return State.Update(data =>
        {
            if (State.Conclusion is InvalidInput)
            {
                data.Conclusion = new NotConcluded();
            }

            data.LastMark = Maybe.Just(new LastMark(row, col, State.CurrentUserMark));
            data.Slots = State.Slots.Update(row, col, State.CurrentUserMark);
            data.CurrentUserMark = State.CurrentUserMark == Mark.X ? Mark.O : Mark.X;
            return data;
        });
    }

}
