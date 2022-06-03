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
        var data = new PartialState();
        data.slots = State.slots.Update(row, col, State.currentUserMark);
        data.currentUserMark = State.currentUserMark == Mark.X ? Mark.O : Mark.X;
        State = State.Update(data);
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
