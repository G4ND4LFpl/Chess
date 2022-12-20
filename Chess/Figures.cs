using System;

namespace MyClasses
{
    namespace Figures
    {
        interface IMovable
        {
            public void Move(Action castling, Action toQueen);
        }

        [Serializable]
        public abstract class Figure : ICloneable
        {
            //pola
            public ChessColor color;
            protected int x, y;
            //metody
            public Figure(bool Iswhite)
            {
                if (Iswhite) color = ChessColor.White;
                else color = ChessColor.Black;
            }
            public void Set(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public object Clone()
            {
                return MemberwiseClone();
            }
            //metody wirtualne
            public abstract bool CanMove(Area area, Area[,] tab);
            public abstract string GetPath(bool LightUp = false);
        }

        //Figury szachowe
        [Serializable]
        class Pawn : Figure, IMovable
        {
            bool wasMoved = false;
            public Pawn(bool Iswhite) : base(Iswhite) { }
            public void Move(Action cast, Action pawnAction)
            {
                wasMoved = true;
                if (y == 0 || y == 7)
                {
                    if (color == ChessColor.Black) pawnAction.Invoke(x);
                    else pawnAction.Invoke(x + 10);
                }
            }
            public override bool CanMove(Area area, Area[,] tab)
            {
                int x2 = area.positionX;
                int y2 = area.positionY;
                //Pierwszy ruch
                if (!wasMoved && x == x2 && Math.Abs(y2 - y) == 2 && area.F == null && tab[x, (y + y2) / 2].F == null)
                {
                    //wasMoved = true;
                    return true;
                }
                //czy ruch do przodu o 1?
                if ((y - y2 == 1 && color == ChessColor.White) || (y - y2 == -1 && color == ChessColor.Black))
                {
                    //Czy wolne do przodu?
                    if (x == x2 && area.F == null)
                    {
                        //wasMoved = true;
                        return true;
                    }
                    //Czy można bić?
                    else if (Math.Abs(x - x2) == 1 && area.F != null)
                    {
                        if (area.F.color != color)
                        {
                            //wasMoved = true;
                            return true;
                        }
                    }
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
            public Bishop(bool Iswhite) : base(Iswhite) { }
            public override bool CanMove(Area area, Area[,] tab)
            {
                int x2 = area.positionX;
                int y2 = area.positionY;
                //czy nie zajęte?
                if (area.F == null || area.F.color != color)
                {
                    //Czy po krosie?
                    if (Math.Abs(x2 - x) == Math.Abs(y2 - y))
                    {
                        //Sprawdzanie drogi
                        if ((x - x2) * (y - y2) > 0) //przekątna z góry na dół
                        {
                            for (int i = 1; i < Math.Max(x, x2) - Math.Min(x, x2); i++)
                            {
                                if (tab[Math.Min(x, x2) + i, Math.Min(y, y2) + i].F != null) return false;
                            }
                            return true;
                        }
                        else if ((x - x2) * (y - y2) < 0)  //przekątna z dołu na górę
                        {
                            for (int i = 1; i < Math.Max(x, x2) - Math.Min(x, x2); i++)
                            {
                                if (tab[Math.Min(x, x2) + i, Math.Max(y, y2) - i].F != null) return false;
                            }
                            return true;
                        }
                    }
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
            public Rook(bool Iswhite) : base(Iswhite) { }
            public bool Moved
            {
                get { return wasMoved; }
            }
            public void Move(Action cast, Action pawnAction) { wasMoved = true; }
            public override bool CanMove(Area area, Area[,] tab)
            {
                int x2 = area.positionX;
                int y2 = area.positionY;
                //czy nie zajęte?
                if (area.F == null || area.F.color != color)
                {
                    //Czy w linii prostej?
                    if (x2 == x || y2 == y)
                    {
                        //Sprawdzanie drogi
                        if (x == x2) //po y
                        {
                            for (int i = Math.Min(y, y2) + 1; i < Math.Max(y, y2); i++)
                            {
                                if (tab[x, i].F != null) return false;
                            }
                            return true;
                        }
                        else if (y == y2) //po x
                        {
                            for (int i = Math.Min(x, x2) + 1; i < Math.Max(x, x2); i++)
                            {
                                if (tab[i, y].F != null) return false;
                            }
                            return true;
                        }
                    }
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
            public Knight(bool Iswhite) : base(Iswhite) { }
            public override bool CanMove(Area area, Area[,] tab)
            {
                int x2 = area.positionX;
                int y2 = area.positionY;
                //Czy wolne?
                if (area.F == null || area.F.color != color)
                {
                    int z = (int)(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
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
            public Queen(bool Iswhite) : base(Iswhite) { }
            public override bool CanMove(Area area, Area[,] tab)
            {
                int x2 = area.positionX;
                int y2 = area.positionY;
                //Czy wolne?
                if (area.F == null || area.F.color != color)
                {
                    //Czy w linii prostej?
                    if (x2 == x || y2 == y)
                    {
                        //Sprawdzanie drogi v1
                        if (x == x2) //po y
                        {
                            for (int i = Math.Min(y, y2) + 1; i < Math.Max(y, y2); i++)
                            {
                                if (tab[x, i].F != null) return false;
                            }
                            return true;
                        }
                        else if (y == y2) //po x
                        {
                            for (int i = Math.Min(x, x2) + 1; i < Math.Max(x, x2); i++)
                            {
                                if (tab[i, y].F != null) return false;
                            }
                            return true;
                        }
                    }
                    //Czy po krosie?
                    else if (Math.Abs(x2 - x) == Math.Abs(y2 - y))
                    {
                        //Sprawdzanie drogi
                        if ((x - x2) * (y - y2) > 0) //przekątna z góry na dół
                        {
                            for (int i = 1; i < Math.Max(x, x2) - Math.Min(x, x2); i++)
                            {
                                if (tab[Math.Min(x, x2) + i, Math.Min(y, y2) + i].F != null) return false;
                            }
                            return true;
                        }
                        else if ((x - x2) * (y - y2) < 0)  //przekątna z dołu na górę
                        {
                            for (int i = 1; i < Math.Max(x, x2) - Math.Min(x, x2); i++)
                            {
                                if (tab[Math.Min(x, x2) + i, Math.Max(y, y2) - i].F != null) return false;
                            }
                            return true;
                        }
                    }
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
            public King(bool Iswhite) : base(Iswhite) { }
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
            public override bool CanMove(Area area, Area[,] tab)
            {
                int x2 = area.positionX;
                int y2 = area.positionY;
                //Normalny ruch
                if (area.F == null || area.F.color != color)
                {
                    int z = (int)(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
                    if (z <= 2) return true;
                }
                //Roszada
                if (area.F == null && Math.Abs(x - x2) == 2 && wasMoved == false && !Area.Attacked(tab[x, y], tab) &&
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
                }
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
