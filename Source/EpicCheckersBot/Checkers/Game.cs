using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EpicCheckersBot.Checkers
{
    public class Game
    {
        public readonly Board Board;

        public Game(Board board)
        {
            Board = board;
        }

        public Move GetBestMove(Piece color)
        {
            var allMoves = GetAllMoves(color).ToArray();

            // todo logical choice
            var rand = new Random();
            return allMoves[rand.Next(allMoves.Length)];
        }

        public static void RunPracticeGame()
        {
            var game = new Game(new Board(newGame: true));
            Action drawToConsole = () =>
            {
                game.Board.RenderToConsole();
                Thread.Sleep(500);
            };

            while (true)
            {
                drawToConsole();

                var blueMove = game.GetBestMove(Piece.Blue);
                game.Board.Move(blueMove.From, blueMove.To);

                drawToConsole();

                var redMove = game.GetBestMove(Piece.Red);
                game.Board.Move(redMove.From, redMove.To);
            }
        }

        public IEnumerable<Move> GetAllMoves(Piece color)
        {
            foreach(var movablePiece in Board.GetAllPieces().Where(p => p.Value == color))
            {
                var from = movablePiece.Key;
                foreach(var direction in BoardPoint.GetAllPoints())
                {
                    foreach (var to in Board.GetEmptySpots(from, direction))
                    {
                        yield return new Move(from, to);
                    }
                }
            }
        }
    }
}
