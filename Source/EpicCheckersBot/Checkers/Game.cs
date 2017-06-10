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

        public Move? GetBestMove(Piece color)
        {
            var allMoves = GetAllMoves(color).ToArray();

            if (allMoves.Length == 0)
                return null;

            // todo logical choice
            var rand = new Random();
            return allMoves[rand.Next(allMoves.Length)];
        }

        public static void RunPracticeGame(int maxMoves = 70)
        {
            var game = new Game(new Board(newGame: true));
            Action redraw = () =>
            {
                Renderer.Renderer.RenderToConsole(game.Board);
                Console.WriteLine("Round {0}", game.Board.Round);
                Thread.Sleep(100);
            };

            redraw();
            while (game.Board.Round < Board.EndsAtRound)
            {
                // blue turn
                {
                    var blueMove = game.GetBestMove(Piece.Blue);
                    if (blueMove == null)
                        break;

                    // apply
                    game.Board.PushMove(blueMove.Value);
                    redraw();
                }
               
                // red turn
                {
                    var redMove = game.GetBestMove(Piece.Red);
                    if (redMove == null)
                        break;

                    // apply
                    game.Board.PushMove(redMove.Value);
                    redraw();
                }
            }
            redraw();

            var bluePieces = game.Board.GetAllPieces(Piece.Blue).Count();
            var redPieces = game.Board.GetAllPieces(Piece.Red).Count();

            if (bluePieces > redPieces)
                Console.WriteLine("Blue Wins");
            else if (redPieces > bluePieces)
                Console.WriteLine("Red Wins");
            else
                Console.WriteLine("It's a tie");

            Thread.Sleep(500);

            // Undo all moves
            while (game.Board.MoveStackSize > 0)
            {
                redraw();
                game.Board.PopMove();
            }

            redraw();
        }

        public IEnumerable<Move> GetAllMoves(Piece color)
        {
            foreach(var movablePiece in Board.GetAllPieces().Where(p => p.Value == color))
            {
                var from = movablePiece.Key;
                foreach(var direction in BoardPoint.GetAllDirections())
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
