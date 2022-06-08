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

    private static State ResolveGame(State state)
    {
        return state
            .Pipe(Stalemate)
            .Pipe(Victory);
    }

    private static State Stalemate(State state)
    {
        var movesLeft = state.Slots.Reduce(0)((result, mark) => result + ((mark == Mark.NONE) ? 1 : 0));
        if (movesLeft > 0) return state;
        return state.Update(data =>
        {
            data.Conclusion = new Stalemate("Stalemate, play again");
            return data;
        });

    }

    private static State Victory(State state)
    {
        if (state.Conclusion is Stalemate) return state;
        return state.LastMark.Map(lastMark =>
        {
            var axis = new List<Func<int>>() {
                () => countWestMarks(lastMark, state.Slots) + countEastMarks(lastMark, state.Slots),
                () => countNorthMarks(lastMark, state.Slots) + countSouthMarks(lastMark, state.Slots),
                () => countNWSEMarks(lastMark, state.Slots) + countSENWMarks(lastMark, state.Slots),
                () => countSWNEMarks(lastMark, state.Slots) + countNESWMarks(lastMark, state.Slots)
            };

            if (axis.Any(count => count() == 2))
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
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row, lastMark.Col - 1),
            new SlotId(lastMark.Row, lastMark.Col - 2)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }
    private static int countEastMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row, lastMark.Col + 1),
            new SlotId(lastMark.Row, lastMark.Col + 2)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int countNorthMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row -1, lastMark.Col),
            new SlotId(lastMark.Row -2, lastMark.Col)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int countSouthMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row + 1, lastMark.Col),
            new SlotId(lastMark.Row + 2, lastMark.Col)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int countNWSEMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row + 1, lastMark.Col + 1),
            new SlotId(lastMark.Row + 2, lastMark.Col + 2)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int countSENWMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row - 1, lastMark.Col - 1),
            new SlotId(lastMark.Row - 2, lastMark.Col - 2)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int countSWNEMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row - 1, lastMark.Col + 1),
            new SlotId(lastMark.Row - 2, lastMark.Col + 2)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int countNESWMarks(LastMark lastMark, Slots<Mark> slots)
    {
        var columnIdList = new List<SlotId>() {
            new SlotId(lastMark.Row + 1, lastMark.Col - 1),
            new SlotId(lastMark.Row + 2, lastMark.Col - 2)
        };
        return CountMarks(lastMark, slots, columnIdList);
    }

    private static int CountMarks(LastMark lastMark, Slots<Mark> slots, IEnumerable<SlotId> slotIdList)
    {
        return slotIdList.Select(id => slots.Val(id.Row, id.Col)
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
