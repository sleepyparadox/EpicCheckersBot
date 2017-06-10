using EpicCheckersBot.Checkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot.Renderer
{
    public class Renderer
    {
        public static void RenderToConsole(Board board)
        {
            Console.Clear();
            for (int row = 0; row < Board.Width; row++)
            {
                for (int col = 0; col < Board.Width; col++)
                {
                    if (board.WithinBoard(row, col))
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else // flag out of bounds as black
                        Console.BackgroundColor = ConsoleColor.Black;

                    WritePieceToConsole(board.GetPieceAt(row, col));
                }
                Console.Write("\n");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void WritePieceToConsole(Piece? piece)
        {
            if (piece == Piece.Red)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("R");
            }
            else if (piece == Piece.Blue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("B");
            }
            else
            {
                Console.Write(" ");
            }
        }
    }
}
