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
        public static void RenderToConsole(Piece?[][] _pieces)
        {
            Console.Clear();
            for (int row = 0; row < Board.Width; row++)
            {
                for (int col = 0; col < Board.Width; col++)
                {
                    WritePieceToConsole(_pieces[row][col]);
                }
                Console.Write("\n");
            }
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
