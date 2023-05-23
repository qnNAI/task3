using System.Collections.Generic;
using System.Text;
using task3.Abstract;
using task3.Services;

namespace task3;

internal class Program {
    static void Main(string[] args) {
        if (!_ValidateInputArgs(args)) return;

        var counter = 1;
        var moveMap = args.ToDictionary(move => counter++);

        var winnerService = new WinnerService(args);
        var winnerTableService = new WinnerTableService(winnerService, args);
        var keyGenerator = new KeyGenerator();
        var rng = new Random();

        while(_RunMatch(args, moveMap, rng, winnerService, winnerTableService, keyGenerator)) {}
    }

    private static bool _RunMatch(
                            string[] moves,
                            Dictionary<int, string> moveMap,
                            Random rng,
                            IWinnerService winnerService,
                            IWinnerTableService winnerTableService,
                            IKeyGenerator keyGenerator) {

        var computerMove = moves[rng.Next(moves.Length)];
        var key = keyGenerator.GenerateKey();
        var hmac = keyGenerator.GenerateHmac(key, computerMove);

        _PrintMenu(moveMap, hmac);

        var userMoveIndex = _GetUserMove(winnerTableService, out var isExitRequired);

        if(isExitRequired)
            return false;

        if(!moveMap.ContainsKey(userMoveIndex))
            return true;

        var userMove = moveMap[userMoveIndex];

        _PrintMatchResult(userMove, computerMove, key, winnerService);
        return true;
    }

    #region UserInteraction

    private static void _PrintMatchResult(string userMove, string computerMove, byte[] key, IWinnerService winnerService) {
        Console.WriteLine($"Your move: {userMove}");
        Console.WriteLine($"Computer move: {computerMove}");

        var result = winnerService.GetWinner(userMove, computerMove);
        Console.WriteLine(result < 1 ?
            result == 0 ? "Draw!" : "You lose!"
            : "You win!");
        Console.WriteLine($"HMAC key: {Convert.ToHexString(key)}");
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void _PrintMenu(Dictionary<int, string> moveMap, byte[] hmac) {
        Console.WriteLine($"HMAC - {Convert.ToHexString(hmac)}");
        Console.WriteLine("Available moves:");

        foreach(var move in moveMap) {
            Console.WriteLine($"{move.Key} - {move.Value}");
        }
        Console.WriteLine("0 - exit");
        Console.WriteLine("? - help");
        Console.Write("Enter your move: ");
    }

    private static int _GetUserMove(IWinnerTableService winnerTableService, out bool isExitRequired) {
        isExitRequired = false;

        var input = Console.ReadLine();
        if(input == "?") {
            Console.WriteLine(winnerTableService.GetWinnerTable());
            return -1;
        }

        if(!int.TryParse(input, out var choice))
            return -1;

        if(choice == 0) {
            isExitRequired = true;
            return -1;
        }

        return choice;
    }

    #endregion

    #region Validation

    private static bool _ValidateInputArgs(string[] args) {
        if(!_ValidateAtLeastThreeMoves(args)) return false;

        if(!_ValidateOddNumberOfMoves(args)) return false;

        if (!_ValidateNoDuplicateMoves(args)) return false;

        return true;
    }

    private static bool _ValidateAtLeastThreeMoves(string[] moves) {
        if(moves.Length < 3) {
            Console.WriteLine("Should be at least 3 moves available!");
            _ShowExample();
            return false;
        }
        return true;
    }

    private static bool _ValidateOddNumberOfMoves(string[] moves) {
        if(moves.Length % 2 == 0) {
            Console.WriteLine("Should be an odd number of moves available!");
            _ShowExample();
            return false;
        }
        return true;
    }

    private static bool _ValidateNoDuplicateMoves(string[] moves) {
        var set = new HashSet<string>();
        foreach(var move in moves) {
            if(!set.Add(move)) {
                Console.WriteLine("Available moves could not be duplicate!");
                _ShowExample();
                return false;
            }
        }
        return true;
    }

    private static void _ShowExample() {
        Console.WriteLine("Example: rock paper scissors");
    }

    #endregion

}



