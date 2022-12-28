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

        // Klasa abstrakcyjna
        [Serializable]
        abstract class Figure : ICloneable
        {
            // Pola
            internal Color color;
            protected int x, y;
            internal List<Position> movable;
            internal static string emptyPath = "/img/Empty.png";

            // Właściwości
            internal List<Position> MoveTable
            {
                get { return movable; }
            }

            // Metody publiczne
            public Figure(Color color, Position pos)
            {
                this.color = color;
                x = pos.x;
                y = pos.y;
            }
            public void Set(Position position)
            {
                x = position.x;
                y = position.y;
            }
            /*public List<Position> CreateMoveTable(Dictionary<Position, Figure> board)
            {
                //Jeśli tabela istnieje
                if (this.movable != null) return this.movable;

                //Tworzenie tabeli
                movable = new List<Position>();

                foreach (Figure f in board)
                {
                    Position position = new Position(f.x, f.y);
                    if (CanMove(board, position))
                    {
                        movable.Add(position);
                    }
                }
                return movable;
            }*/
            public object Clone()
            {
                return MemberwiseClone();
            }

            // Metody chronione
            protected bool Streight(Dictionary<Position, Figure> board, Position pos)
            {
                //Czy w linii prostej?
                if (pos.x == x || pos.y == y)
                {
                    //Sprawdzanie drogi
                    if (x == pos.x) //po y
                    {
                        for (int i = Math.Min(y, pos.y) + 1; i < Math.Max(y, pos.y); i++)
                        {
                            if (board.ContainsKey(new Position(x, i))) return false;
                        }
                        return true;
                    }
                    else if (y == pos.y) //po x
                    {
                        for (int i = Math.Min(x, pos.x) + 1; i < Math.Max(x, pos.x); i++)
                        {
                            if (board.ContainsKey(new Position(i, x))) return false;
                        }
                        return true;
                    }
                }
                return false;
            }
            protected bool Cross(Dictionary<Position, Figure> board, Position pos)
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
                            if (board.ContainsKey(new Position(minX + i, maxY - i))) return false;
                        }
                        return true;
                    }
                    else if ((x - pos.x) * (y - pos.y) > 0)  //przekątna z dołu na górę
                    {
                        int minX = Math.Min(x, pos.x);
                        int minY = Math.Min(y, pos.y);
                        for (int i = 1; i < Math.Abs(pos.x - x); i++)
                        {
                            if (board.ContainsKey(new Position(minX + i, minY + i))) return false;
                        }
                        return true;
                    }
                }
                return false;
            }

            // Metody wirtualne
            public abstract bool CanMove(Dictionary<Position, Figure> board, Position pos);
            // position - pozycja do której figura chce się przesunąć
            public abstract string GetPath();
        }

        //Klasy - Dzieci
        [Serializable]
        class Pawn : Figure, IMovable
        {
            internal bool wasMoved = false;
            public Pawn(Color color, Position pos) : base(color, pos) { }
            public void Move(Action cast, Action pawnAction)
            {
                wasMoved = true;
                if (y == 0 || y == 7)
                {
                    if (color == Color.Black) pawnAction.Invoke(x);
                    else pawnAction.Invoke(x + 10);
                }
            }
            public override bool CanMove(Dictionary<Position, Figure> board, Position pos)
            {
                // Czy do przodu ? (sprawdzamy przypadek odwrotny)
                if ((pos.y <= y && color == Color.Black) || (pos.y >= y && color == Color.White))
                {
                    return false;
                }
                // Czy można na wolne ?
                if (x == pos.x && !board.ContainsKey(pos))
                {
                    // pojedyńczy ruch
                    if (Math.Abs(pos.y - y) == 1) 
                    {
                        return true;
                    }
                    // podwójny ruch
                    if (Math.Abs(pos.y - y) == 2 && !wasMoved && !board.ContainsKey(new Position(pos.x, (pos.y + y) / 2)))
                    {
                        return true;
                    }
                }
                //Czy bicie ?
                if (Math.Abs(pos.y - y) == 1 && Math.Abs(pos.x - x) == 1)
                {
                    if (board.ContainsKey(pos) && board[pos].color != color)
                        return true;
                }
                return false;
            }
            public override string GetPath()
            {
                string path;
                if (color == Color.White) path = "WhitePawn";
                else path = "BlackPawn";
                return "/img/" + path + ".png";
            }
        }

        [Serializable]
        class Bishop : Figure
        {
            public Bishop(Color color, Position pos) : base(color, pos) { }
            public override bool CanMove(Dictionary<Position, Figure> board, Position pos)
            {
                //Czy wolne lub wrogie?
                if (!board.ContainsKey(pos) || board[pos].color != color)
                {
                    return Cross(board, pos);
                }
                return false;
            }
            public override string GetPath()
            {
                string path;
                if (color == Color.White) path = "WhiteBishop";
                else path = "BlackBishop";
                return "/img/" + path + ".png";
            }
        }

        [Serializable]
        class Rook : Figure, IMovable
        {
            bool wasMoved = false;
            public Rook(Color color, Position pos) : base(color, pos) { }
            public bool Moved
            {
                get { return wasMoved; }
            }
            public void Move(Action cast, Action pawnAction) { wasMoved = true; }
            public override bool CanMove(Dictionary<Position, Figure> board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (!board.ContainsKey(pos) || board[pos].color != color)
                {
                    return Streight(board, pos);
                }
                return false;
            }
            public override string GetPath()
            {
                string path;
                if (color == Color.White) path = "WhiteRook";
                else path = "BlackRook";
                return "/img/" + path + ".png";
            }
        }

        [Serializable]
        class Knight : Figure
        {
            public Knight(Color color, Position pos) : base(color, pos) { }
            public override bool CanMove(Dictionary<Position, Figure> board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (!board.ContainsKey(pos) || board[pos].color != color)
                {
                    int z = (int)(Math.Pow(x - pos.x, 2) + Math.Pow(y - pos.y, 2));
                    if (z == 5) return true;
                }
                return false;
            }
            public override string GetPath()
            {
                string path;
                if (color == Color.White) path = "WhiteKnight";
                else path = "BlackKnight";
                return "/img/" + path + ".png";
            }
        }

        [Serializable]
        class Queen : Figure
        {
            public Queen(Color color, Position pos) : base(color, pos) { }
            public override bool CanMove(Dictionary<Position, Figure> board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (!board.ContainsKey(pos) || board[pos].color != color)
                {
                    if (Streight(board, pos) || Cross(board, pos))
                        return true;
                }
                return false;
            }
            public override string GetPath()
            {
                string path;
                if (color == Color.White) path = "WhiteQueen";
                else path = "BlackQueen";
                return "/img/" + path + ".png";
            }
        }

        [Serializable]
        class King : Figure, IMovable
        {
            bool wasMoved = false, castling = false;
            public King(Color color, Position pos) : base(color, pos) { }
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
            public override bool CanMove(Dictionary<Position, Figure> board, Position pos)
            {
                //Czy wolne lub wrogie ?
                if (!board.ContainsKey(pos) || board[pos].color != color)
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
            public override string GetPath()
            {
                string path;
                if (color == Color.White) path = "WhiteKing";
                else path = "BlackKing";
                return "/img/" + path + ".png";
            }
        }
    }
}
