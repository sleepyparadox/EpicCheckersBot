using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot.Checkers
{
    public class Board
    {
        public const int Width = 8;
        public int _round;
        Piece?[][] _pieces;

        public Board(int round, Piece?[][] pieces)
        {
            _round = round;
            _pieces = pieces;
        }

        public Board(bool newGame)
        {
            _round = 0;
            _pieces = new Piece?[8][];
            for (int row = 0; row < Width; row++)
                _pieces[row] = new Piece?[Width];

            if(newGame)
            {
                for (int col = 0; col < Width; col++)
                {
                    SetPieceAt(0, col, Piece.Red);
                    SetPieceAt(7, col, Piece.Blue);
                }
            }
        }

        public void RenderToConsole()
        {
            Renderer.Renderer.RenderToConsole(_pieces);
        }

        public void Move(BoardPoint from, BoardPoint to)
        {
            var piece = GetPieceAt(from);

            SetPieceAt(from, null);
            SetPieceAt(to, piece);

            _round++;
        }

        public IEnumerable<BoardPoint> GetEmptySpots(BoardPoint piecePoint, BoardPoint direction)
        {
            var point = piecePoint;

            point += direction;
            while (WithinBoard(point) && GetPieceAt(point) == null)
            {
                yield return point;
                point += direction;
            }
        }

        public IEnumerable<KeyValuePair<BoardPoint, Piece?>> GetAllPieces()
        {
            var shrink = GetShrink(_round);
            for (int row = shrink; row < Width - shrink; row++)
            {
                for (int col = shrink; col < Width - shrink; col++)
                {
                    var point = new BoardPoint(row, col);
                    var piece = GetPieceAt(point);

                    yield return new KeyValuePair<BoardPoint, Piece?>(point, piece);
                }
            }
        }

        public Piece? GetPieceAt(BoardPoint point)
        {
            return GetPieceAt(point.row, point.col);
        }

        public Piece? GetPieceAt(int row, int col)
        {
            return _pieces[row][col];
        }

        public void SetPieceAt(BoardPoint point, Piece? piece)
        {
            SetPieceAt(point.row, point.col, piece);
        }

        public void SetPieceAt(int row, int col, Piece? piece)
        {
            if (WithinBoard(row, col) == false)
                throw new Exception(row + "," + col + " isn't on the board");

            _pieces[row][col] = piece;
        }

        public bool WithinBoard(BoardPoint point)
        {
            return WithinBoard(point.row, point.col);
        }

        public bool WithinBoard(int row, int col)
        {
            var shrink = GetShrink(_round);
            return row >= shrink
                && row < Width - shrink
                && col >= shrink
                && col < Width - shrink;
        }

        static int GetShrink(int round)
        {
            // kills do resolve first
            // board shrinks as final step of 30th, 50th and 60th turns

            if (round >= 61) 
                return 3;
            else if (round >= 51)
                return 2;
            else if (round >= 31)
                return 1;
            else
                return 0;
        }
    }
}
