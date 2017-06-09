using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EpicCheckersBot.Checkers
{
    public struct BoardPoint
    {
        public int col;
        public int row;

        public static BoardPoint Left { get { return new BoardPoint(0, -1); } }
        public static BoardPoint Right { get { return new BoardPoint(0, 1); } }
        public static BoardPoint Up { get { return new BoardPoint(-1, 0); } }
        public static BoardPoint Down { get { return new BoardPoint(1, 0); } }

        public BoardPoint(int row, int column)
        {
            this.row = row;
            this.col = column;
        }

        public static BoardPoint operator +(BoardPoint p1, BoardPoint p2)
        {
            return new BoardPoint(p1.row + p2.row, p1.col + p2.col);
        }

        public static IEnumerable<BoardPoint> GetAllPoints()
        {
            yield return Left;
            yield return Right;
            yield return Up;
            yield return Down;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", row, col);
        }
    }
}
