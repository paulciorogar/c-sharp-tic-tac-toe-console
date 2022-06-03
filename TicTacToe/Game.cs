namespace TicTacToe;

public class Game
{
    // private State _state;
    public Game(State state)
    {
        State = state;
    }

    public State State { get; internal set; }

    public void MarkSlot(int row, int col)
    {
        var maybeSlot = State.slots.Val(row, col);

        State = maybeSlot.Map(slot =>
        {
            var data = new PartialState();
            if (slot != Mark.NONE)
            {
                data.message = $"Slot {row}.{col} already marked by: {slot}";
                return State.Update(data);
            }

            if (State.message != string.Empty)
            {
                data.message = string.Empty;
            }

            data.slots = State.slots.Update(row, col, State.currentUserMark);
            data.currentUserMark = State.currentUserMark == Mark.X ? Mark.O : Mark.X;
            return State.Update(data);

        })
        .CatchMap(() =>
        {
            var data = new PartialState();
            data.message = $"Slot {row}.{col} is not in this game";
            return State.Update(data);
        })
        .OrSome(State);
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
}
