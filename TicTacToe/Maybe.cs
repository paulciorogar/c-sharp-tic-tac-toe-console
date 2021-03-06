namespace TicTacToe;

public interface IMaybe<T> where T : notnull
{
    public IMaybe<TResult> Map<TResult>(Func<T, TResult> callback) where TResult : notnull;
    public IMaybe<T> CatchMap(Func<T> callback);
    public T OrSome(T orValue);

    public IMaybe<T> Filter(Func<T, bool> callback);

}

public static class Maybe
{
    public static IMaybe<T> Of<T, U>(U val) where U : T where T : notnull
    {
        if (val is null) return new Nothing<T>();
        return new Some<T>(val);
    }

    public static IMaybe<T> Just<T>(T val) where T : notnull
    {
        return new Some<T>(val);
    }
}

public class Some<T> : IMaybe<T> where T : notnull
{
    private T _val;
    public Some(T val)
    {
        _val = val;
    }

    public IMaybe<TResult> Map<TResult>(Func<T, TResult> callback) where TResult : notnull
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

    public IMaybe<T> Filter(Func<T, bool> callback)
    {
        if (callback(_val)) return this;
        return new Nothing<T>();
    }
}

public class Nothing<T> : IMaybe<T> where T : notnull
{
    public IMaybe<TResult> Map<TResult>(Func<T, TResult> callback) where TResult : notnull
    {
        return new Nothing<TResult>();
    }

    public IMaybe<T> CatchMap(Func<T> callback)
    {
        return new Some<T>(callback());
    }

    public T OrSome(T orValue)
    {
        return orValue;
    }

    public IMaybe<T> Filter(Func<T, bool> callback)
    {
        return this;
    }

}