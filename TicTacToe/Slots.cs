namespace TicTacToe;

public class Slots<T> where T : notnull
{
    private T[,] _slots;
    private int _size;

    public Slots()
    {
        _size = 3; // TODO: make this dynamic
        _slots = new T[_size, _size];
    }

    private Slots(T[,] initData)
    {
        _size = 3; // TODO: make this dynamic
        _slots = initData;
    }

    public void ForEach(Action<T, int, int> callback)
    {
        for (int row = 0; row < _size; row++)
        {
            for (int col = 0; col < _size; col++)
            {
                callback(_slots[row, col], row, col);
            }
        }
    }

    internal Slots<T> Update(int updateRow, int updateCol, T val)
    {
        return Map<T>((oldVal, row, col) =>
        {
            if (row == updateRow && col == updateCol) return val;
            return oldVal;
        });
    }

    public IMaybe<T> Val(int row, int col)
    {
        if (row < _size && col < _size)
        {
            return new Some<T>(_slots[row, col]);
        }
        else
        {
            return new None<T>();

        }
    }

    public Slots<U> Map<U>(MapCallback<U, T> callback) where U : notnull
    {
        var newData = new U[_size, _size];
        ForEach((val, row, col) => newData[row, col] = callback(_slots[row, col], row, col));
        return new Slots<U>(newData);
    }
}

public delegate TResult MapCallback<TResult, TVal>(TVal val, int row, int col);

public enum Mark
{
    NONE, X, O
}