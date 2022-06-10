using TicTacToe;

var inputMap = new Dictionary<char, int>();
var rowLetter = new Dictionary<int, char>();

inputMap['a'] = 0;
inputMap['b'] = 1;
inputMap['c'] = 2;
inputMap['1'] = 0;
inputMap['2'] = 1;
inputMap['3'] = 2;

rowLetter[0] = 'A';
rowLetter[1] = 'B';
rowLetter[2] = 'C';

var game = new Game(State.New(), inputMap);
var run = true;
var errorMessage = string.Empty;

while (run)
{
    Render(game.State);
    var input = Console.ReadLine();
    if (input is null) return;
    if (IsFinished(game.State))
    {
        game = new Game(State.New(), inputMap);
        continue;
    }
    else if (input.Length == 0) continue;

    if (input == "exit") return;
    game.MarkSlot(input[0], input[1]);
}

static bool IsFinished(State state)
{
    return state.Conclusion is Victory || state.Conclusion is Stalemate;
}

static void Render(State state)
{
    var a1 = GetValue(state.Slots.Val(0, 0));
    var a2 = GetValue(state.Slots.Val(0, 1));
    var a3 = GetValue(state.Slots.Val(0, 2));
    var b1 = GetValue(state.Slots.Val(1, 0));
    var b2 = GetValue(state.Slots.Val(1, 1));
    var b3 = GetValue(state.Slots.Val(1, 2));
    var c1 = GetValue(state.Slots.Val(2, 0));
    var c2 = GetValue(state.Slots.Val(2, 1));
    var c3 = GetValue(state.Slots.Val(2, 2));

    Console.Clear();

    Console.WriteLine("Tic Tac Toe");
    Console.WriteLine(string.Empty);
    Console.WriteLine("    1   2   3  ");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine($"A | {a1} | {a2} | {a3} |");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine($"B | {b1} | {b2} | {b3} |");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine($"C | {c1} | {c2} | {c3} |");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine(string.Empty);
    Console.WriteLine("- Type cell code to mark.");
    Console.WriteLine("  (ex a1 for first cell in the table)");
    Console.WriteLine("- Type exit to exit the game.");
    Console.WriteLine(string.Empty);
    if (state.Conclusion is not NotConcluded)
    {
        Console.WriteLine(state.Conclusion.Message);
    }
    if (IsFinished(state))
    {
        Console.Write("Press enter to restart");
    }
    if (state.Conclusion is NotConcluded || state.Conclusion is InvalidInput)
    {
        Console.Write($"{state.CurrentUserMark}'s turn: ");
    }

    string GetValue(IMaybe<Mark> val) => val.Filter(m => m != Mark.NONE)
        .Map(m => m.ToString())
        .OrSome(" ");
}