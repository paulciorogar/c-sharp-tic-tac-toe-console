namespace TicTacToe;

public class Slots<T>
{
    private T[,] _slots;
    private int _size;

    public Slots()
    {
        _size = 3;
        _slots = new T[_size, _size];
    }

    private Slots(T[,] initData)
    {
        _slots = initData;
    }

    public void ForEach(Action<T> callback)
    {
        foreach (var item in _slots)
        {
            callback(item);
        }
    }

    public Slots<U> Map<U>(MapCallback<U, T> callback)
    {
        var newData = new U[_size, _size];
        for (int row = 0; row < _size; row++)
        {
            for (int col = 0; col < _size; col++)
            {
                newData[row, col] = callback(_slots[row, col], row, col);
            }
        }
        return new Slots<U>(newData);
    }
}

public delegate T MapCallback<T, U>(U val, int row, int col);

public enum Mark
{
    NONE, X, O
}