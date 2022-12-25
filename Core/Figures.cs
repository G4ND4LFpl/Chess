using System;
using System.Collections.Generic;

namespace Core
{
    namespace Figures
    {
        //Interfejs
        internal interface IMovable
        {
            void Move(Action castling, Action toQueen);
        }

        //Klasa abstrakcyjna
        [Serializable]
        public abstract class Figure : ICloneable
        {
            // Pola
            public ChessColor color;
            protected int x, y;
            List<Position> movable;

            // Metody publiczne
            public Figure(ChessColor color)
            {
                this.color = color;
            }
            public void Set(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public void Set(Position position)
            {
                x = position.x;
                y = position.y;
            }
            public List<Position> CanMoveTable(Board board)
            {
                //Jeśli tabela istnieje
                if (this.movable != null) return this.movable;

                //Tworzenie tabeli
                List<Position> movable = new List<Position>();

                foreach (Figure f in board)
                {
                    Position position = new Position(f.x, f.y);
                    if (CanMove(board, position))
                    {
                        movable.Add(position);
                    }
                }
                this.movable = movable;
                return movable;
            }
            public object Clone()
            {
                return MemberwiseClone();
            }


            // Metody chronione
            protected bool Streight(Board board, Position pos)
            {
                //Czy w linii prostej?
                if (pos.x == x || pos.y == y)
                {
                    //Sprawdzanie drogi
                    if (x == pos.x) //po y
                    {
                        for (int i = Math.Min(y, pos.y) + 1; i < Math.Max(y, pos.y); i++)
                        {
                            if (board[x, i] != null) return false;
                        }
                        return true;
                    }
                    else if (y == pos.y) //po x
                    {
                        for (int i = Math.Min(x, pos.x) + 1; i < Math.Max(x, pos.x); i++)
                        {
                            if (board[i, x] != null) return false;
                        }
                        return true;
                    }
                }
                return false;
            }
            protected bool Cross(Board board, Position pos)
            {
                //Czy po krosie?
                if (Math.Abs(pos.x - x) == Math.Abs(pos.y - y))
                {
                    //Sprawdzanie drogi
                    if ((x - pos.x) * (y - pos.y) < 0) //przekątna z góry na dół
                    {
                        int minX = Math.Min(x, pos.x);
                        int maxY = Math.Max(y, pos.y);
                        for (int i = 1; i < Math.Abs(pos.x - x); i++)
                        {
                            if (board[minX + i, maxY - i] != null) return false;
                        }
                        return true;
                    }
                    else if ((x - pos.x) * (y - pos.y) > 0)  //przekątna z dołu na górę
                    {
                        int minX = Math.Min(x, pos.x);
                        int minY = Math.Min(y, pos.y);
                        for (int i = 1; i < Math.Abs(pos.x - x); i++)
                        {
                            if (board[minX + i, minY + i] != null) return false;
                        }
                        return true;
                    }
                }
                return false;
            }

            // Metody wirtualne
            public abstract bool CanMove(Board board, Position pos);
            // position - pozycja do której figura chce się przesunąć
            public abstract string GetPath(bool LightUp = false); //?
        }

        //Klasy - Dzieci
        [Serializable]
        class Pawn : Figure, IMovable
        {
            bool wasMoved = false;
            public Pawn(ChessColor color) : base(color) { }
            public void Move(Action cast, Action pawnAction)
            {
                wasMoved = true;
                if (y == 0 || y == 7)
                {
                    if (color == ChessColor.Black) pawnAction.Invoke(x);
                    else pawnAction.Invoke(x + 10);
                }
            }
            public override bool CanMove(Board board, Position pos)
            {
                //Czy do przodu ? (sprawdzamy przypadek odwrotny)
                if ((pos.y <= y && color == ChessColor.White) || (pos.y >= y && color == ChessColor.Black))
                {
                    return false;
                }
                //Czy wolne ?
                if (x == pos.x && board[pos] == null)
                {
                    if (Math.Abs(pos.y - y) == 1) //pojedyńczy ruch
                    {
                        return true;
                    }
                    //podwójny ruch
                    if (Math.Abs(pos.y - y) == 2 && !wasMoved && board[pos.x, (pos.y + y) / 2] == null)
                    {
                        return true;
                    }
                }
                //Czy bicie ?
                if (Math.Abs(pos.y - y) == 1 && Math.Abs(pos.x - x) == 1)
                {
                    if (board[pos] != null && board[pos].color != color)
                        return true;
                }
                return false;
            }
            public override string GetPath(bool LightUp = false)
            {
                string path;
                if (color == ChessColor.White) path = "WhitePawn";
                else path = "BlackPawn";
                if (LightUp == true) path += "LU";
                return path + ".png";
            }
        }

        [Serializable]
        class Bishop : Figure
        {
            public Bishop(ChessColor color) : base(color) { }
            public override bool CanMove(Board board, Position pos)
            {
                //Czy wolne lub wrogie?
                if (board[pos] == null || board[pos].color != color)
                {
                    return Cross(board, pos);
                }
                return false;
            }
            public override string GetPath(bool LightUp = false)
            {
                string path;
                if (color == ChessColor.White) path = "WhiteBishop";
                else path = "BlackBishop";
                if (LightUp == true) path += "LU";
                return path + ".png";
            }
        }

        [Serializable]
        class Rook : Figure, IMovable
        {
            bool wasMoved = false;
            public Rook(ChessColor color) : base(color) { }
            public bool Moved
            {
                get { return wasMoved; }
            }
            public void Move(Action cast, Action pawnAction) { wasMoved = true; }
            public override bool CanMove(Board board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (board[pos] == null || board[pos].color != color)
                {
                    return Streight(board, pos);
                }
                return false;
            }
            public override string GetPath(bool LightUp = false)
            {
                string path;
                if (color == ChessColor.White) path = "WhiteRook";
                else path = "BlackRook";
                if (LightUp == true) path += "LU";
                return path + ".png";
            }
        }

        [Serializable]
        class Knight : Figure
        {
            public Knight(ChessColor color) : base(color) { }
            public override bool CanMove(Board board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (board[pos] == null || board[pos].color != color)
                {
                    int z = (int)(Math.Pow(x - pos.x, 2) + Math.Pow(y - pos.y, 2));
                    if (z == 5) return true;
                }
                return false;
            }
            public override string GetPath(bool LightUp = false)
            {
                string path;
                if (color == ChessColor.White) path = "WhiteKnight";
                else path = "BlackKnight";
                if (LightUp == true) path += "LU";
                return path + ".png";
            }
        }

        [Serializable]
        class Queen : Figure
        {
            public Queen(ChessColor color) : base(color) { }
            public override bool CanMove(Board board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (board[pos] == null || board[pos].color != color)
                {
                    if (Streight(board, pos) || Cross(board, pos))
                        return true;
                }
                return false;
            }
            public override string GetPath(bool LightUp = false)
            {
                string path;
                if (color == ChessColor.White) path = "WhiteQueen";
                else path = "BlackQueen";
                if (LightUp == true) path += "LU";
                return path + ".png";
            }
        }

        [Serializable]
        class King : Figure, IMovable
        {
            bool wasMoved = false, castling = false;
            public King(ChessColor color) : base(color) { }
            public void Move(Action cast, Action pawnAction)
            {
                wasMoved = true;
                if (castling == true)
                {
                    castling = false;
                    if (x == 2 && y == 7) cast.Invoke(1);
                    else if (x == 6 && y == 7) cast.Invoke(2);
                    else if (x == 2 && y == 0) cast.Invoke(3);
                    else if (x == 6 && y == 0) cast.Invoke(4);
                    else throw new Exception("Błędna lokalizacja");
                }
            }
            public override bool CanMove(Board board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (board[pos] == null || board[pos].color != color)
                {
                    int z = (int)(Math.Pow(x - pos.x, 2) + Math.Pow(y - pos.y, 2));
                    if (z <= 2) return true;
                }
                //Roszada
                /*
                if (board[pos] == null && Math.Abs(x - pos.x) == 2 && !wasMoved && !Area.Attacked(tab[x, y], tab) &&
                    tab[(x + x2) / 2, y].F == null && !Area.Attacked(tab[(x + x2) / 2, y], tab, color))
                {
                    if (x2 > x) //Prawa roszada
                    {
                        if (tab[7, y].F is Rook)
                        {
                            Rook k = (Rook)tab[7, y].F;
                            if (!k.Moved) { castling = true; return true; }
                        }
                    }
                    else //Lewa roszada
                    {
                        if (tab[1, y].F == null && tab[0, y].F is Rook)
                        {
                            Rook k = (Rook)tab[0, y].F;
                            if (!k.Moved) { castling = true; return true; }
                        }
                    }
                } */
                return false;
            }
            public override string GetPath(bool LightUp = false)
            {
                string path;
                if (color == ChessColor.White) path = "WhiteKing";
                else path = "BlackKing";
                if (LightUp == true) path += "LU";
                return path + ".png";
            }
        }
    }
}
