using System;
using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Core.Figures;

namespace Core
{
    public class Board : IEnumerable
    {
        Figure[,] board;

        public Figure this[int x, int y]
        {
            get
            {
                if (x > 7 || y > 7 || x < 0 || y < 0)
                    return null;
                else return board[x, y];
            }
        }
        public Figure this[Position p]
        {
            get
            {
                if (p.x > 7 || p.y > 7 || p.x < 0 || p.y < 0)
                    return null;
                else return board[p.x, p.y];
            }
        }

        // Metody
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
