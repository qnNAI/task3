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

        IWinnerService winnerService = new WinnerService(args);
        IWinnerTableService winnerTableService = new WinnerTableService(winnerService, args);
        IKeyGenerator keyGenerator = new KeyGenerator();
        var rng = new Random();

        while(true) {
            //Console.Clear();

            var computerMove = args[rng.Next(args.Length)];
            var key = keyGenerator.GenerateKey();
            var hmac = keyGenerator.GenerateHmac(key, computerMove);

            Console.WriteLine($"HMAC - {Convert.ToHexString(hmac)}");
            Console.WriteLine("Available moves:");

            foreach(var move in moveMap) {
                Console.WriteLine($"{move.Key} - {move.Value}");
                counter++;
            }
            Console.WriteLine("0 - exit");
            Console.WriteLine("? - help");
            Console.Write("Enter your move: ");

            var input = Console.ReadLine();
            if (input == "?") {
                Console.WriteLine(winnerTableService.GetWinnerTable());
                continue;
            }

            if(!int.TryParse(input, out var choice)) continue;

            if (choice == 0) break;

            if (!moveMap.ContainsKey(choice)) continue;

            var userMove = moveMap[choice];
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

    }

    private static bool _ValidateInputArgs(string[] args) {
        if (args.Length < 3) {
            Console.WriteLine("Should be at least 3 moves available!");
            _ShowExample();
            return false;
        }

        if (args.Length % 2 == 0) {
            Console.WriteLine("Should be an odd number of moves available!");
            _ShowExample();
            return false;
        }

        var set = new HashSet<string>();
        foreach(var arg in args) {
            if (!set.Add(arg)) {
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
}



