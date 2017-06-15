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
        public const int EndsAtRound = 61;

        public int MoveStackSize { get { return _moveStack.Count; } }
        public int Round { get; private set; }

        Piece?[][] _pieces;
        Dictionary<int, List<KeyValuePair<BoardPoint, Piece>>> _captureHistory = new Dictionary<int, List<KeyValuePair<BoardPoint, Piece>>>();
        Stack<Move> _moveStack = new Stack<Move>();
        //Stack<string> _hashStack = new Stack<string>();

        public Piece? GetWinner()
        {
            var reds = 0;
            var blues = 0;
            foreach(var piece in GetAllPieces())
            {
                if (piece == Piece.Red)
                    reds++;
                if (piece == Piece.Blue)
                    blues++;
            }

            if (blues == 0 && reds > 0)
                return Piece.Red;

            if(reds == 0 && blues > 0)
                return Piece.Blue;

            if (Round == EndsAtRound)
            {
                if (blues > reds)
                    return Piece.Blue;
                else if (reds > blues)
                    return Piece.Red;
                else
                    return Piece.Winner_Tie;
            }

            // no winner
            return null;
        }

        public Board(int round, Piece?[][] pieces)
        {
            Round = round;
            _pieces = pieces;
        }

        public Board(bool newGame)
        {
            Round = 0;
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
        
        public void PushMove(Move move)
        {
            //var hash = GetBoardHash();
            //_hashStack.Push(hash);

            var piece = GetPieceAt(move.From);

            if (piece == null)
                throw new NullReferenceException("Move piece");

            SetPieceAt(move.From, null);
            SetPieceAt(move.To, piece);

            var captures = new List<KeyValuePair<BoardPoint, Piece>>();

            foreach (var direction in BoardPoint.GetAllDirections())
            {
                // did it chain any combos?
                var killsThisDirection = CheckForChain(move.To, direction);
                if (killsThisDirection != null)
                    captures.AddRange(killsThisDirection);
            }

            //rows
            {
                var rowDoubles = CheckForDoubles(move.To, move.To + BoardPoint.Left, move.To + BoardPoint.Right);
                if (rowDoubles != null)
                    captures.AddRange(rowDoubles);
            }

            //cols
            {
                var colDoubles = CheckForDoubles(move.To, move.To + BoardPoint.Up, move.To + BoardPoint.Down);
                if (colDoubles != null)
                    captures.AddRange(colDoubles);
            }

            if (captures.Any())
            {
                // remove duplicated captures
                var distinctPointPiece = new BoardPoint.ComparePointPiece();
                captures = captures.Distinct(distinctPointPiece).ToList();

                foreach (var capturePoint in captures.Select(p => p.Key))
                {
                    SetPieceAt(capturePoint, null);
                }

                _captureHistory.Add(Round, captures);
            }

            Round++;
            _moveStack.Push(move);
        }

        public void PopMove()
        {
            var move = _moveStack.Pop();
            Round--;

            var movedPiece = GetPieceAt(move.To).Value;

            // move it back
            SetPieceAt(move.To, null);
            SetPieceAt(move.From, movedPiece);

            // uncapture pieces
            if(_captureHistory.ContainsKey(Round))
            {
                foreach(var capture in _captureHistory[Round])
                {
                    SetPieceAt(capture.Key, capture.Value);
                }
                _captureHistory.Remove(Round);
            }

            //var hash = GetBoardHash();
            //var expectedHash = _hashStack.Pop();

            //if (hash != expectedHash)
            //    throw new Exception("Failed to pop state correctly");
        }

        public IEnumerable<KeyValuePair<BoardPoint, Piece>> CheckForChain(BoardPoint piecePoint, BoardPoint direction)
        {
            var myColor = GetPieceAt(piecePoint).Value;
            var theirColor = myColor.GetOpponentColor();

            var chain = new List<KeyValuePair<BoardPoint, Piece>>();
            var targetPoint = piecePoint;

            targetPoint += direction;
            Piece? targetPiece;
            while (WithinBoard(targetPoint) && (targetPiece = GetPieceAt(targetPoint)).HasValue)
            {
                if (targetPiece == theirColor)
                {
                    chain.Add(new KeyValuePair<BoardPoint, Piece>(targetPoint, targetPiece.Value));
                }
                else if (targetPiece == myColor)
                {
                    if (chain.Any())
                        return chain;
                    else
                        return null;
                }
                targetPoint += direction;
            }

            // Never finished chain fail
            return null;
        }

        public IEnumerable<KeyValuePair<BoardPoint, Piece>> CheckForDoubles(BoardPoint piecePoint, BoardPoint sideA, BoardPoint sideB)
        {
            var myColor = GetPieceAt(piecePoint).Value;
            var theirColor = myColor.GetOpponentColor();

            if (GetPieceAt(sideA) == theirColor && GetPieceAt(sideB) == theirColor)
            {
                return new[]
                {
                    new KeyValuePair<BoardPoint, Piece>(sideA, theirColor),
                    new KeyValuePair<BoardPoint, Piece>(sideB, theirColor),
                };
            }
            else
            {
                return null;
            }
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

        public IEnumerable<KeyValuePair<BoardPoint, Piece?>> GetAllPiecePoints(Piece color)
        {
            return GetAllPiecePoints().Where(p => p.Value == color);
        }

        public IEnumerable<KeyValuePair<BoardPoint, Piece?>> GetAllPiecePoints()
        {
            var shrink = GetShrink(Round);
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

        public IEnumerable<Piece> GetAllPieces()
        {
            var shrink = GetShrink(Round);
            for (int row = shrink; row < Width - shrink; row++)
            {
                for (int col = shrink; col < Width - shrink; col++)
                {
                    var piece = GetPieceAt(row, col);
                    if(piece.HasValue)
                        yield return piece.Value;
                }
            }
        }

        public Piece? GetPieceAt(BoardPoint point)
        {
            return GetPieceAt(point.row, point.col);
        }

        public Piece? GetPieceAt(int row, int col)
        {
            if (WithinBoard(row, col) == false)
                return null; // board is shrinking

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
            var shrink = GetShrink(Round);
            return row >= shrink
                && row < Width - shrink
                && col >= shrink
                && col < Width - shrink;
        }

        static int GetShrink(int round)
        {
            // kills do resolve first
            // board shrinks as final step of 30th, 50th and 60th turns

            if (round >= EndsAtRound) 
                return 3;
            else if (round >= 51)
                return 2;
            else if (round >= 31)
                return 1;
            else
                return 0;
        }

        string GetBoardHash()
        {
            var builder = new StringBuilder((Width) * Width);
            var shrink = GetShrink(Round);
            for (int row = shrink; row < Width - shrink; row++)
            {
                for (int col = shrink; col < Width - shrink; col++)
                {
                    var piece = GetPieceAt(row, col);
                    if (piece == Piece.Red)
                        builder.Append('R');
                    else if (piece == Piece.Blue)
                        builder.Append('B');
                    else
                        builder.Append(' ');
                }
                //builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
