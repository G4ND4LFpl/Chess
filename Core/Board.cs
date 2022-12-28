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
        Color playerTurn;
        bool figureChoosenFlag;
        Position from;

        // Konstruktor
        public Board()
        {
            board = new Figure[8, 8];

            // figury
            for(int i=0;i<8;i++)
            {
                board[i, 0] = SetDefaultFigure(Color.Black, i, 0);
                board[i, 1] = new Pawn(Color.Black, new Position(i, 1));
                // pusta plansza
                board[i, 6] = new Pawn(Color.White, new Position(i, 6));
                board[i, 7] = SetDefaultFigure(Color.White, i, 7);
            }
            
            playerTurn = Color.White;
            figureChoosenFlag = false;

            // dev test
            //board[0, 0] = new Pawn(Color.White, new Position(0, 0));
            //board[7, 7] = new Pawn(Color.Black, new Position(7, 7));
        }
        private Figure SetDefaultFigure(Color c, int x, int y)
        {
            if (x == 0 || x == 7) return new Rook(c, new Position(x, y));
            if (x == 1 || x == 6) return new Knight(c, new Position(x, y));
            if (x == 2 || x == 5) return new Bishop(c, new Position(x, y));
            if (x == 3) return new Queen(c, new Position(x, y));
            if (x == 4) return new King(c, new Position(x, y));
            else return null;
        }

        internal Figure this[Position p]
        {
            get
            {
                if (p.x > 7 || p.y > 7 || p.x < 0 || p.y < 0)
                    return null;
                else return board[p.x, p.y];
            }
            set
            {
                if (p.x <= 7 || p.y <= 7 || p.x >= 0 || p.y >= 0)
                    board[p.x, p.y] = value;
            }
        }

        // Metody publiczne
        public string GetFigureImgSource(int x, int y)
        {
            if (x > 7 || y > 7 || x < 0 || y < 0)
                return null;
            else if (board[x, y] != null) return board[x, y].GetPath(); 
            else return Figure.emptyPath;
        }
        public bool IsChoosen(int x, int y)
        {
            if (figureChoosenFlag && from == new Position(x, y)) return true;
            else return false;
        }
        public void Click(int x, int y)
        {
            if (x > 7 || y > 7 || x < 0 || y < 0) return;
            Position position = new Position(x, y);

            // Wybór figury
            if (!figureChoosenFlag)
            {
                // Kliknięcie w figurę
                if (this[position] != null && this[position].color == playerTurn)
                {
                    from = position;
                    figureChoosenFlag = true;
                }
                return;
            }

            // Wykonywanie ruchu
            if (this[from].CanMove(this, position))
            {
                // Ruch
                this[position] = this[from];
                this[position].Set(position);
                this[from] = null;

                if (playerTurn == Color.White) playerTurn = Color.Black;
                else playerTurn = Color.White;
            }

            // Końcowe zmiany
            figureChoosenFlag = false;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();  
        }
    }
}
