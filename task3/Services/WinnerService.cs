using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task3.Abstract;

namespace task3.Services
{
    public class WinnerService : IWinnerService
    {
        private readonly Dictionary<string, int> _moves = new();

        public WinnerService(IEnumerable<string> moves)
        {
            var counter = 1;

            foreach (var move in moves)
            {
                _moves.Add(move, counter);
                counter++;
            }
        }

        public int GetWinner(string first, string second)
        {
            if (first == second)
                return 0;

            var firstValue = _moves[first];
            var secondValue = _moves[second];
            var diff = firstValue - secondValue;
            if (Math.Abs(diff) > _moves.Count / 2)
            {
                diff = -diff;
            }

            return diff > 0 ? 1 : -1;
        }

    }
}
