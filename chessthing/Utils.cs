using System;

namespace chessthing
{
    static class Util
    {
        public static Move StringToMove(string s, Field field)
        {
            s = s.ToLower();
            return new Move(-((int) char.GetNumericValue(s[1]) - 9), s[0] - 'a' + 1,
                -((int) char.GetNumericValue(s[^1]) - 9),
                s[^2] - 'a' + 1, field);
        }


        public static Piece GetPieceByNumber(int i, bool upper)
        {
            return i switch
            {
                0 => new Rook(upper),
                1 => new Knight(upper),
                2 => new Bishop(upper),
                3 => new Queen(upper),
                4 => new King(upper),
                5 => new Bishop(upper),
                6 => new Knight(upper),
                7 => new Rook(upper),
                _ => new Rook(upper)
            };
        }
    }

    class Timer
    {
        private int _turnCount;
        private static Timer _instance;

        public static Timer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Timer();
                    return _instance;
                }
                else
                {
                    return _instance;
                }
            }
        }


        private Timer()
        {
            _turnCount = 0;
        }

        public void AddTurn()
        {
            _turnCount++;
        }

        public int getTurns()
        {
            return _turnCount;
        }

        public void ResetTimer()
        {
            _turnCount = 0;
        }
    }
}