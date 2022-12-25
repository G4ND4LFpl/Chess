using System;
using System.Collections.Generic;
using System.Linq;
namespace General
{
    //Delegat
    delegate void Action(int x);

    //Typy wartościowe
    public enum ChessColor
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
}
