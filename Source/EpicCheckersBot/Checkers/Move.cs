using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot.Checkers
{
    public struct Move
    {
        public BoardPoint From;
        public BoardPoint To;

        public Move(BoardPoint from, BoardPoint to)
        {
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return string.Format("from {0} to {1}", From, To);
        }
    }
}
