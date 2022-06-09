using TicTacToe;

var game = new Game(State.New());
var run = true;
var errorMessage = string.Empty;

while (run)
{
    Render();
    var input = Console.ReadLine();
    if (input is null) return;

    if (input == "exit") return;

    int? row = input[0] switch
    {
        'a' => 0,
        'b' => 1,
        'c' => 2,
        _ => null
    };


    int? col = input[1] switch
    {
        '1' => 0,
        '2' => 1,
        '3' => 2,
        _ => null
    };

    if (row is null || col is null)
    {
        errorMessage = $"Invalid input: {input[0]}{input[1]}";
    }
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

static void Render()
{
    Console.Clear();

    Console.WriteLine("Tic Tac Toe");
    Console.WriteLine(string.Empty);
    Console.WriteLine("    1   2   3  ");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine("A |   |   |   |");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine("B |   |   |   |");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine("C |   |   |   |");
    Console.WriteLine("  +---+---+---+");
    Console.WriteLine(string.Empty);
    Console.WriteLine("Type cell code to mark.");
    Console.WriteLine("(ex a1 for first cell in the table)");
    Console.WriteLine(string.Empty);
    Console.Write("X's turn: ");
}