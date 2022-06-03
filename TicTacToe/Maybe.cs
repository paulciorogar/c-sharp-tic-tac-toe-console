namespace TicTacToe;

public interface IMaybe<T>
{
    public IMaybe<TResult> Map<TResult>(Func<T, TResult> callback);
    public IMaybe<T> CatchMap(Func<T> callback);
    public T OrSome(T orValue);

}

public static class Maybe
{
    public static IMaybe<T> Of<T>(T val)
    {
        if (val is null) return new None<T>();
        return new Some<T>(val);
    }
}

public class Some<T> : IMaybe<T>
{
    private T _val;
    public Some(T val)
    {
        _val = val;
    }

    public IMaybe<TResult> Map<TResult>(Func<T, TResult> callback)
    {
        return new Some<TResult>(callback(_val));
    }

    public IMaybe<T> CatchMap(Func<T> callback)
    {
        return this;
    }

    public T OrSome(T orValue)
    {
        return _val;
    }
}

public class None<T> : IMaybe<T>
{
    public IMaybe<TResult> Map<TResult>(Func<T, TResult> callback)
    {
        return new None<TResult>();
    }

    public IMaybe<T> CatchMap(Func<T> callback)
    {
        return new Some<T>(callback());
    }

    public T OrSome(T orValue)
    {
        return orValue;
    }
}