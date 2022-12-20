using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MyClasses;
using MyClasses.Figures;

namespace Chess
{
    [Serializable]
    public class Area : ImageFrame
    {
        //stałe
        Color white = Color.FromArgb(220, 200, 170);
        Color black = Color.FromArgb(90, 50, 25);
        //Color lightwhite = Color.FromArgb(250, 240, 210);
        //Color lightblack = Color.FromArgb(120, 60, 25);
        //pozostałe
        public int positionX;
        public int positionY;
        Figure figure;
        Color basic;
        public Area(int x, int y) : base()
        {
            positionX = x;
            positionY = y;
            if ((x + y) % 2 == 0) basic = white;
            else basic = black;
            BackColor = basic;
        }
        public Figure F
        {
            get { return figure; }
        }
        public override void SetFigure(Figure f)
        {
            figure = f;
            if (figure != null)
            {
                figure.Set(positionX, positionY);
                SetImage(figure.GetPath());
            }
            else
            {
                Image = null;
                filename = null;
            }
        }
        public new void Resize(Size S) //ukrywa domyślną funkcję
        {
            int site = S.Height * 70 / 675;
            int valueX = S.Width * 40 / 1200 + site * positionX;
            int valueY = S.Height * 70 / 675 + site * positionY;
            Location = new Point(valueX, valueY);
            Size = new Size(site, site);
        }
        public void LightUp()
        {
            SetImage(figure.GetPath(true));
        }
        public void LightDown()
        {
            SetImage(figure.GetPath(false));
        }
        public static bool Attacked(Area area, Area[,] tab, ChessColor color = ChessColor.White)
        {
            if (area.F != null) color = area.F.color;
            //Właściwa część
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tab[i, j].F != null && tab[i, j].F.color != color)
                    {
                        if (tab[i, j].F.CanMove(area, tab))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
