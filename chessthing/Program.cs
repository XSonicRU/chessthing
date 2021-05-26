using System;

namespace chessthing
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new Field();
            f.draw_field();
            while (true)
            {
                f.MakeMove(Util.StringToMove(Console.ReadLine(), f));
                f.draw_field();
            }
        }
    }
}