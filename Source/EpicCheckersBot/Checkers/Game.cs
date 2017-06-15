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
        static int MovesChecked = 0;
        static int EndingMovesChecked = 0;
        public Game(Board board)
        {
            Board = board;
        }

        public Move? GetBestMove(Piece color)
        {
            MovesChecked = 0;
            EndingMovesChecked = 0;

            var totalPieces = Board.GetAllPieces().Count();

            // increase search depth as board shrinks
            var earlyGame = Board.GetShrink(Board.Round + 3) < 2 && totalPieces > 5;

            var depth = earlyGame ? 3 : 5;
            var allMoves = GetAllMoves(color, earlyGame).ToArray();

            if(earlyGame && allMoves.Any() == false)
            {
                // forget cheap early game I need some moves
                allMoves = GetAllMoves(color, false).ToArray();
            }

            Move? bestMove = null;
            var bestScore = float.MinValue;

            foreach(var move in allMoves)
            {
                var score = GetMoveMiniMax(move, color, depth, float.MinValue, float.MinValue, earlyGame);
                if(score >= bestScore)
                {
                    bestMove = move;
                    bestScore = score;
                }
            }

            return bestMove;
        }

        public static bool IsGreatScore(float score, float parentScore, bool earlyGame)
        {
            if (score >= 1f)
                return true;
            if (earlyGame && score > 0.15f)
                return true;
            if (parentScore > -100f && score > parentScore + 0.10f)
                return true;
            return false;
        }

        float GetMoveMiniMax(Move move, Piece color, int depthToCheck, float parentScore, float otherParentScore, bool earlyGame)
        {
            MovesChecked++;

            Board.PushMove(move);

            var score = Board.GetScore(color);
            if (score >= 1 // win
                || score < parentScore // worse than parent
                || IsGreatScore(score, parentScore, earlyGame)
                || depthToCheck <= 1) 
            {
                EndingMovesChecked++;

                // game ended or this is last node we can aford to check
                Board.PopMove();
                return score;
            }

            Move bestChild = new Move();
            var bestChildScore = float.MinValue;

            var childDepth = depthToCheck - 1;
            var childColor = color.GetOpponentColor();
            var children = GetAllMoves(childColor, earlyGame);
            foreach (var child in children)
            {
                var childScore = GetMoveMiniMax(child, childColor, childDepth,
                                                // flip the parent scores
                                                otherParentScore, score, 
                                                earlyGame);

                if (childScore > bestChildScore)
                {
                    bestChild = child;
                    bestChildScore = childScore;
                }

                if(IsGreatScore(bestChildScore, otherParentScore, earlyGame))
                {
                    // best for opponent
                    break;
                }
            }

            Board.PopMove();
            return (bestChildScore * -1);
        }

        public static void RunPracticeGame()
        {
            var game = new Game(new Board(newGame: true));
            Action redraw = () =>
            {
                Renderer.Renderer.RenderToConsole(game.Board);
                Console.WriteLine("Round {0}", game.Board.Round);
                Thread.Sleep(500);
            };

            redraw();
            Piece? winner;
            while (true)
            {
                // blue turn
                {
                    var blueMove = game.GetBestMove(Piece.Blue);
                    if ((winner = game.Board.GetWinner()).HasValue || blueMove == null)
                        break;

                    // apply
                    game.Board.PushMove(blueMove.Value);
                    redraw();
                }
               
                // red turn
                {
                    var redMove = game.GetBestMove(Piece.Red);
                    if ((winner = game.Board.GetWinner()).HasValue || redMove == null)
                        break;

                    // apply
                    game.Board.PushMove(redMove.Value);
                    redraw();
                }
            }
            redraw();

            Console.WriteLine("Winner is " + winner.Value);

            Thread.Sleep(2500);
        }

        public IEnumerable<Move> GetAllMoves(Piece color, bool earlyGame)
        {
            var i = (Board.Round / 2) % 2;
            foreach(var movablePiece in Board.GetAllPiecePoints().Where(p => p.Value == color))
            {
                // horrible hack to reduce the number of moves by skipping every second move
                if (earlyGame && i++ % 2 == 0) 
                    continue;

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
