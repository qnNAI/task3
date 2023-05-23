using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task3.Abstract;

namespace task3.Services
{

    public class WinnerTableService : IWinnerTableService
    {
        private readonly IWinnerService _winnerService;
        private readonly List<string> _moves;

        public WinnerTableService(IWinnerService winnerService, IEnumerable<string> moves)
        {
            _winnerService = winnerService;
            _moves = moves as List<string> ?? moves.ToList();
        }

        public string GetWinnerTable() {
            var sb = new StringBuilder();
            var align = _GetTableAlignment();

            _BuildTableHead(sb, align);
            _BuildTableBody(sb, align);

            return sb.ToString();
        }

        private int _GetTableAlignment() {
            var align = 0;

            foreach(var move in _moves) {
                if(align < move.Length)
                    align = move.Length;
            }

            return align;
        }

        private void _BuildTableBody(StringBuilder sb, int align) {
            var counter = 0;
            while(counter < _moves.Count) {
                sb.Append(_GetFormattedString(_moves[counter], align));

                foreach(var move in _moves) {
                    var result = _winnerService.GetWinner(_moves[counter], move);
                    sb.Append(_GetFormattedString(
                        result < 1 ?
                            result == 0 ? "draw" : "lose"
                            : "win",
                        align));
                }

                sb.AppendLine("|");
                counter++;
            }
        }

        private void _BuildTableHead(StringBuilder sb, int align) {
            sb.AppendFormat("  {0, align} ".Replace("align", align.ToString()), "");

            foreach(var move in _moves) {
                sb.Append(_GetFormattedString(move, align));
            }

            sb.AppendLine("|");
        }

        private string _GetFormattedString(string value, int align) => string.Format("| {0, -align} ".Replace("align", align.ToString()), value);
    }
}
