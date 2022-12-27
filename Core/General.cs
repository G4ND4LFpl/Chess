using System;
using System.Collections.Generic;
using System.Linq;
namespace Core
{
    //Delegat
    delegate void Action(int x);

    //Typy wartościowe
    internal enum Color
    {
        White, Black
    }

    //Struktury
    public struct Position
    {
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
    }

    class xx
    {
        public xx()
        {
            Dictionary<Position, Figures.Figure> dict = new Dictionary<Position, Figures.Figure>();

            dict.Add(new Position(3, 4), new Figures.Knight(Color.White, new Position(3, 4)));
            dict.Add(new Position(2, 4), new Figures.Pawn(Color.White, new Position(2, 4)));
            dict.Add(new Position(3, 3), new Figures.Bishop(Color.Black, new Position(3, 3)));

            Position old_pos = new Position(1, 5);
            Position new_pos = new Position(1, 1);

            dict.Add(old_pos, new Figures.Rook(Color.Black, old_pos));

            if (!dict.ContainsKey(new_pos))
            {
                dict.Add(new_pos, dict[old_pos]);
                dict.Remove(old_pos);
            }
        }
    }
}
