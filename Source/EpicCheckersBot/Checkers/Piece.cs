using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot.Checkers
{
    public enum Piece
    {
        Blue,
        Red,

        Winner_Tie,
    }

    public static class PieceExtensionMethods
    {
        public static Piece GetOpponentColor(this Piece piece)
        {
            switch(piece)
            {
                case Piece.Blue:
                    return Piece.Red;
                case Piece.Red:
                    return Piece.Blue;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
