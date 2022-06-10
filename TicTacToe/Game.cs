namespace TicTacToe;

public class Game
{
    public State State { get; internal set; }
    public Dictionary<char, int> InputMap { get; init; }
    public Game(State state, Dictionary<char, int> inputMap)
    {
        State = state;
        InputMap = inputMap;
    }


    public void MarkSlot(char rowId, char colId)
    {
        var impossibleSlot = ImpossibleSlot(rowId, colId);
        if (!InputMap.ContainsKey(rowId) || !InputMap.ContainsKey(colId))
        {
            State = impossibleSlot();
            return;
        }
        var row = InputMap[rowId];
        var col = InputMap[colId];
        var maybeSlot = State.Slots.Val(row, col);

        State = maybeSlot.Map(slot => slot switch
        {
            Mark.NONE => ApplyMarkOn(row, col),
            _ => SlotAlreadyMarked(rowId, colId, slot)
        })
        .CatchMap(impossibleSlot)
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

    private Func<State> ImpossibleSlot(char row, char col)
    {
        return () =>
        {
            return State.Update(data =>
            {
                data.Conclusion = new InvalidInput($"Slot {row}{col} is not in this game");
                return data;
            });
        };
    }

    private State SlotAlreadyMarked(char row, char col, Mark slot)
    {
        return State.Update(data =>
        {
            data.Conclusion = new InvalidInput($"Slot {row}{col} already marked by: {slot}");
            return data;
        });
    }

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
