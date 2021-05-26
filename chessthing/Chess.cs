using System;

namespace chessthing
{
    class Field
    {
        private Cell[,] _field;

        public Field()
        {
            reset_field();
            Timer.Instance.ResetTimer();
        }

        public Piece GetPiece(int x, int y)
        {
            return (Piece) _field[x, y];
        }

        public bool CheckInBounds(Move m)
        {
            var flag = true;
            for (var i = 0; i < 4; i++)
            {
                flag &= m.Data[0] <= 8 && m.Data[0] >= 1;
            }

            return flag;
        }

        public bool IsEmpty(int x, int y)
        {
            return _field[x, y] == null;
        }

        public bool GetSide(int x, int y)
        {
            return ((Piece) _field[x, y]).Upper;
        }

        // 1 - top, 2 - right, 3 - down, 4 - left 5 - left top 6 - right top 7 - bottom right 8 - bottom left 
        public bool CheckDirection(Move m, int k, int direction)
        {
            int x_off, y_off;
            switch (direction)
            {
                case 1:
                    y_off = 0;
                    x_off = -1;
                    break;
                case 2:
                    y_off = 1;
                    x_off = 0;
                    break;
                case 3:
                    y_off = 0;
                    x_off = 1;
                    break;
                case 4:
                    y_off = -1;
                    x_off = 0;
                    break;
                case 5:
                    x_off = -1;
                    y_off = -1;
                    break;
                case 6:
                    x_off = -1;
                    y_off = 1;
                    break;
                case 7:
                    x_off = 1;
                    y_off = 1;
                    break;
                case 8:
                    x_off = 1;
                    y_off = -1;
                    break;
                default:
                    return false;
            }

            var cur_x = m.Data[0];
            var cur_y = m.Data[1];
            int i;
            for (i = 0; i < k; i++)
            {
                cur_x += x_off;
                cur_y += y_off;
                if (!IsEmpty(cur_x, cur_y))
                {
                    return (m.Data[2] == cur_x && m.Data[3] == cur_y) && m.IsEating;
                }

                if (m.Data[2] == cur_x && m.Data[3] == cur_y)
                {
                    return true;
                }
            }

            return false;
        }

        public void MakeMove(Move move)
        {
            if (move.Field != null)
            {
                Timer.Instance.AddTurn();
                _field[move.Data[2], move.Data[3]] = _field[move.Data[0], move.Data[1]];
                _field[move.Data[0], move.Data[1]] = null;
            }
        }

        public void MakeMove(int x1, int y1, int x2, int y2)
        {
            MakeMove(new Move(x1, y1, x2, y2, this));
        }

        private void reset_field()
        {
            _field = new Cell[10, 10];
            for (var i = 1; i < 9; i++)
            {
                _field[i, 0] = new Cell((char) ('9' - i));
                _field[i, 9] = new Cell((char) ('9' - i));
                _field[0, i] = new Cell((char) ('`' + i));
                _field[9, i] = new Cell((char) ('`' + i));
                _field[2, i] = new Pawn(true);
                _field[7, i] = new Pawn(false);
                _field[1, i] = Util.GetPieceByNumber(i - 1, true);
                _field[8, i] = Util.GetPieceByNumber(i - 1, false);
            }
        }

        public void draw_field()
        {
            for (var i = 0; i < _field.GetLength(0); i++)
            {
                for (var j = 0; j < _field.GetLength(1); j++)
                {
                    if ((i == 0 && j == 0) || (i == 0 && j == 9) || (i == 9 && j == 9) || (i == 9 && j == 0))
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write(_field[i, j] == null ? '.' : _field[i, j].Label);
                    }

                    Console.Write(' ');
                }

                Console.Write('\n');
            }
        }
    }

    class Cell
    {
        public char Label { get; }

        public Cell(char label)
        {
            Label = label;
        }
    }

    class Move
    {
        public readonly int[] Data;
        public Field Field;
        public readonly bool IsEating;

        public Move(int x1, int y1, int x2, int y2, Field field)
        {
            Data = new[] {x1, y1, x2, y2};
            Field = field;
            if (!field.IsEmpty(Data[0], Data[1]))
            {
                if (!field.IsEmpty(x2, y2))
                {
                    if (field.GetSide(x2, y2) == field.GetSide(x1, y1))
                    {
                        Console.WriteLine("Trying to eat your ally!");
                        wrongTurn();
                        return;
                    }
                }
                if (Timer.Instance.getTurns() % 2 == 0
                    ? field.GetPiece(Data[0], Data[1]).Upper
                    : !field.GetPiece(Data[0], Data[1]).Upper)
                {
                    if (field.CheckInBounds(this))
                    {
                        IsEating = !field.IsEmpty(x2, y2);
                        if (!field.GetPiece(Data[0], Data[1]).CheckMove(this))
                        {
                            Console.WriteLine("Such move is not permitted!");
                            wrongTurn();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Index out of bounds!");
                        wrongTurn();
                    }
                }
                else
                {
                    Console.WriteLine("Wrong color!");
                    wrongTurn();
                }
            }
            else
            {
                Console.WriteLine("There is no piece at that cell...");
                wrongTurn();
            }
        }

        private void wrongTurn()
        {
            Field = null;
        }
    }
}