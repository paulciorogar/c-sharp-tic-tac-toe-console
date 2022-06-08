namespace TicTacToe;

public static class PipeExtension
{
    public static TResult Pipe<TParam, TResult>(this TParam input, Func<TParam, TResult> func)
            => func(input);
}