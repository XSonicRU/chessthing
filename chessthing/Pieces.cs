using System;

namespace chessthing
{
    abstract class Piece : Cell
    {
        public bool Upper;

        public abstract bool CheckMove(Move move);

        protected Piece(char label, bool upper) : base(label)
        {
            Upper = upper;
        }
    }

    class King : Piece
    {
        public King(bool upper) : base(upper ? '♚' : '♔', upper)
        {
        }

        public override bool CheckMove(Move move)
        {
            for (int i = 0; i < 9; i++)
                if (move.Field.CheckDirection(move, 1, i))
                {
                    return true;
                }

            return false;
        }
    }

    class Queen : Piece
    {
        public Queen(bool upper) : base(upper ? '♛' : '♕', upper)
        {
        }

        public override bool CheckMove(Move move)
        {
            return new Rook(Upper).CheckMove(move) || new Bishop(Upper).CheckMove(move);
        }
    }

    class Knight : Piece
    {
        public Knight(bool upper) : base(upper ? '♞' : '♘', upper)
        {
        }

        public override bool CheckMove(Move move)
        {
            var Xdif = Math.Abs(move.Data[1] - move.Data[3]);
            var Ydif = Math.Abs(move.Data[0] - move.Data[2]);
            return Xdif == 1 && Ydif == 2 || Xdif == 2 && Ydif == 1;
        }
    }

    class Bishop : Piece
    {
        public Bishop(bool upper) : base(upper ? '♝' : '♗', upper)
        {
        }

        public override bool CheckMove(Move move)
        {
            return move.Field.CheckDirection(move, 10, 5) || move.Field.CheckDirection(move, 10, 6) ||
                   move.Field.CheckDirection(move, 10, 7) || move.Field.CheckDirection(move, 10, 8);
        }
    }

    class Rook : Piece
    {
        public Rook(bool upper) : base(upper ? '♜' : '♖', upper)
        {
        }

        public override bool CheckMove(Move move)
        {
            return move.Field.CheckDirection(move, 10, 1) || move.Field.CheckDirection(move, 10, 2) ||
                   move.Field.CheckDirection(move, 10, 3) || move.Field.CheckDirection(move, 10, 4);
        }
    }

    class Pawn : Piece
    {
        public Pawn(bool upper) : base(upper ? '♟' : '♙', upper)
        {
        }

        public override bool CheckMove(Move move)
        {
            if (Upper)
            {
                if (move.Data[0] == 2 && move.Field.CheckDirection(move, 2, 3) && !move.IsEating)
                {
                    return true;
                }

                return move.Field.CheckDirection(move, 1, 3) && !move.IsEating ||
                       move.Field.CheckDirection(move, 1, 7) && move.IsEating ||
                       move.Field.CheckDirection(move, 1, 8) && move.IsEating;
            }

            if (move.Data[0] == 7 && move.Field.CheckDirection(move, 2, 1))
            {
                return true;
            }

            return move.Field.CheckDirection(move, 1, 1) && !move.IsEating ||
                   move.Field.CheckDirection(move, 1, 5) && move.IsEating ||
                   move.Field.CheckDirection(move, 1, 6) && move.IsEating;
        }
    }
}