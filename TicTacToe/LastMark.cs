namespace TicTacToe;
public class LastMark
{
    public readonly int Row;
    public readonly int Col;
    public readonly Mark Mark;

    public LastMark(int row, int col, Mark mark)
    {
        Row = row;
        Col = col;
        Mark = mark;
    }
}

