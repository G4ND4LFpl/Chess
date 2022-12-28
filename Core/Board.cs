using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Core.Figures;

namespace Core
{
    public class Board : IEnumerable
    {
        Dictionary<Position, Figure> board;
        Color playerTurn;
        bool figureChoosenFlag;
        Position from, whiteKing, blackKing;

        // Konstruktor
        public Board()
        {
            board = new Dictionary<Position, Figure>();

            // figury
            for(int i=0;i<8;i++)
            {
                board.Add(new Position(i, 0), SetDefaultFigure(Color.Black, i, 0));
                board.Add(new Position(i, 1), new Pawn(Color.Black, new Position(i, 1)));
                // pusta plansza
                board.Add(new Position(i, 6), new Pawn(Color.White, new Position(i, 6)));
                board.Add(new Position(i, 7), SetDefaultFigure(Color.White, i, 7));
            }
            
            playerTurn = Color.White;
            figureChoosenFlag = false;
            blackKing = new Position(4, 0);
            whiteKing = new Position(4, 7);
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

        private Figure this[Position p]
        {
            get
            {
                if (board.ContainsKey(p)) return board[p];
                else return null;
            }
            set
            {
                if (p.x <= 7 || p.y <= 7 || p.x >= 0 || p.y >= 0)
                    board.Add(p, value);
            }
        }

        // Metody publiczne
        public string GetFigureImgSource(int x, int y)
        {
            if (x > 7 || y > 7 || x < 0 || y < 0)
                return null;
            else if (board.ContainsKey(new Position(x, y))) return board[new Position(x, y)].GetPath(); 
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
                if (board.ContainsKey(position) && board[position].color == playerTurn)
                {
                    from = position;
                    figureChoosenFlag = true;
                }
                return;
            }

            // Wykonywanie ruchu
            if (board[from].CanMove(board, position))
            {
                // Ruch
                board[position] = board[from];
                board[position].Set(position);
                board.Remove(from);

                // Czy gracz odsłania swojego króla
                // Czy król jest atakowany

                // Koniec tury
                if (playerTurn == Color.White) playerTurn = Color.Black;
                else playerTurn = Color.White;
            }

            // Po kliknięciu
            figureChoosenFlag = false;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();  
        }
    }
}
