using System;
using System.Drawing;
using MyClasses;
using MyClasses.Figures;

namespace Chess
{
    [Serializable]
    struct SaveAreas
    {
        public ChessColor turn;
        public Figure[,] tab;
        public string[] slots1;
        public string[] slots2;
        public ChessTime clock1, clock2;
        public bool IsTime;
        public string version;
        public void Initialize()
        {
            tab = new Figure[8, 8];
            slots1 = new string[15];
            slots2 = new string[15];
        }
    }

    class PlayBoard
    {
        //Zmienne
        public ChessColor Turn;
        //SaveAreas areas;
        Area[,] tab;
        ImageFrame[] slots1;
        ImageFrame[] slots2;
        MyClasses.Action resoult;
        MyClasses.Action switchtime;
        int step = 1;
        Area From;

        //konstruktory
        public PlayBoard()
        {
            AreaTabInitialization();
        }
        public PlayBoard(MyClasses.Action resoult, MyClasses.Action switchtime,Size Size)
        {
            AreaTabInitialization();
            this.resoult = resoult;
            this.switchtime = switchtime;

            slots1 = new ImageFrame[15];
            slots2 = new ImageFrame[15];
            int slotsite = 40;
            for (int i = 0; i < 15; i++)
            {
                //Górne sloty
                slots1[i] = new ImageFrame();
                slots1[i].Size = new Size(slotsite, slotsite);
                if (i <= 7) slots1[i].Location = new Point(650 + i * slotsite, 75);
                else slots1[i].Location = new Point(650 + (i - 8) * slotsite, 115);
                //Dolne sloty
                slots2[i] = new ImageFrame();
                slots2[i].Size = new Size(slotsite, slotsite);
                if (i <= 7) slots2[i].Location = new Point(650 + i * slotsite, 555);
                else slots2[i].Location = new Point(650 + (i - 8) * slotsite, 595);

                //slots1[i].Image = Image.FromFile(@"images\ErrorImage.png");
                //slots2[i].Image = Image.FromFile(@"images\ErrorImage.png");
                slots1[i].Visible = false;
                slots2[i].Visible = false;
            }
            AdjustSize(Size);
        }
        void AreaTabInitialization()
        {
            tab = new Area[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tab[i, j] = new Area(i, j);
                }
            }
        }
        //właściwości
        public ImageFrame[] UpSlots
        {
            get { return slots1; }
        }
        public ImageFrame[] DownSlots
        {
            get { return slots2; }
        }
        public Area this[int i, int j]
        {
            get { return tab[i, j]; }
        }
        //funkcje
        public void OnClick(object sender, EventArgs e)
        {
            Area area = (Area)sender;
            if (step == 0) return;

            //Etap 1 : Wybór figury
            if (step == 1 && area.F != null)
            {
                if (area.F.color == Turn)
                {
                    From = area;
                    area.LightUp();
                    step++;
                }
            }

            //Etap 2 : Ruch
            else if (step == 2)
            {
                if (From.F.CanMove(area, tab))
                {
                    Figure bufor;
                    //Wykonywanie ruchu
                    bufor = area.F;
                    area.SetFigure(From.F);
                    From.SetFigure(null);

                    //Sprawdzanie szacha - czy nasz król jest bezpieczny
                    if (KingInDanger(Turn))
                    {
                        From.SetFigure(area.F);
                        area.SetFigure(bufor);
                    }
                    //Kończenie tury
                    else
                    {
                        //Zmiana gracza
                        step = 1;
                        if (Turn == ChessColor.White)
                        {
                            switchtime(2);
                            Turn = ChessColor.Black; //turn == gracz szachowany
                        }
                        else
                        {
                            Turn = ChessColor.White;
                            switchtime(1);
                        }
                        //Aktualizowanie statusu figury
                        if (area.F is Pawn) Moved<Pawn>(area.F);
                        if (area.F is King) Moved<King>(area.F);
                        if (area.F is Rook) Moved<Rook>(area.F);

                        //Wyświetlanie zbitej figury
                        if (bufor != null && bufor.color == ChessColor.White)
                        {
                            int k = 0;
                            while (slots1[k].Image != null && k < 15) k++;
                            slots1[k].SetFigure(bufor);
                            slots1[k].Visible = true;
                        }
                        else if (bufor != null)
                        {
                            int k = 0;
                            while (slots2[k].Image != null && k < 15) k++;
                            slots2[k].SetFigure(bufor);
                            slots2[k].Visible = true;
                        }
                        //Sprawdzanie czy kończy się gra
                        bool endgame = true;
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (tab[i, j].F != null && tab[i, j].F.color == Turn)
                                {
                                    if (CanHelp(tab[i, j])) endgame = false;
                                }
                            }
                        }
                        if (endgame)
                        {
                            //Koniec gry
                            step = 0;
                            if (KingInDanger(Turn))
                            {
                                if (Turn == ChessColor.White) resoult(-1);   // Wygrywa czarny
                                else resoult(1);    // Wygrywa biały
                            }
                            else resoult(0);    // Remis
                        }
                    }
                }
                else
                {
                    step = 1;
                    From.LightDown();
                }
            }
        }
        bool CanHelp(Area from)
        {
            //Figure bufor;
            PlayBoard quicktab = Copy();
            Figure f;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (from.F.CanMove(tab[i, j], tab))
                    {
                        //quicktab = (Area[,])tab.Clone();
                        //bufor = quicktab[i, j].F;
                        f = quicktab[i, j].F;
                        quicktab[i, j].SetFigure(quicktab[from.positionX, from.positionY].F);
                        quicktab[from.positionX, from.positionY].SetFigure(null);
                        if (!quicktab.KingInDanger(Turn)) return true;
                        else
                        {
                            quicktab[from.positionX, from.positionY].SetFigure(quicktab[i, j].F);
                            quicktab[i, j].SetFigure(f);
                        }
                    }
                }
            }
            return false;
        }
        bool KingInDanger(ChessColor color)
        {
            Area king = null;
            //Gdzie jest król?
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tab[i, j].F is King && tab[i, j].F.color == color)
                    {
                        king = tab[i, j];
                        break;
                    }
                }
                if (king != null) break;
            }
            //czy jest atakowany?
            return Area.Attacked(king, tab);
        }
        public PlayBoard Copy()
        {
            PlayBoard newBoard = new PlayBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tab[i, j].F != null)
                    {
                        newBoard[i, j].SetFigure((Figure)tab[i, j].F.Clone());
                    }
                }
            }
            return newBoard;
        }
        public void AdjustSize(Size S)
        {
            //szachownica
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tab[i, j].Resize(S);
                }
            }
            //sloty
            int slotsite = S.Width * 40 / 1200;
            for (int i = 0; i < 15; i++)
            {
                slots1[i].Size = new Size(slotsite, slotsite);
                if (i <= 7) slots1[i].Location = new Point(S.Width * 650 / 1200 + i * slotsite, S.Height * 75 / 675);
                else slots1[i].Location = new Point(S.Width * 650 / 1200 + (i - 8) * slotsite, S.Height * 115 / 675);

                slots2[i].Size = new Size(slotsite, slotsite);
                if (i <= 7) slots2[i].Location = new Point(S.Width * 650 / 1200 + i * slotsite, S.Height * 555 / 675);
                else slots2[i].Location = new Point(S.Width * 650 / 1200 + (i - 8) * slotsite, S.Height * 595 / 675);
            }
        }
        public void Moved<T>(Figure f) where T : Figure, IMovable
        {
            MyClasses.Action a = Castling;
            MyClasses.Action b = ToQueen;
            T t = (T)f;
            t.Move(a, b);
        }
        void Castling(int x)
        {
            switch (x)
            {
                case 1:
                    {
                        tab[3, 7].SetFigure(tab[0, 7].F);
                        tab[0, 7].SetFigure(null);
                        break;
                    }
                case 2:
                    {
                        tab[5, 7].SetFigure(tab[7, 7].F);
                        tab[7, 7].SetFigure(null);
                        break;
                    }
                case 3:
                    {
                        tab[3, 0].SetFigure(tab[0, 0].F);
                        tab[0, 0].SetFigure(null);
                        break;
                    }
                case 4:
                    {
                        tab[5, 0].SetFigure(tab[7, 0].F);
                        tab[7, 0].SetFigure(null);
                        break;
                    }
            }
        }
        void ToQueen(int x)
        {
            if (x > 10) tab[x - 10, 0].SetFigure(new Queen(true));
            else tab[x, 7].SetFigure(new Queen(false));
        }
        //rzutowanie
        public static explicit operator SaveAreas(PlayBoard board)
        {
            SaveAreas areas = new SaveAreas();
            areas.Initialize();
            areas.turn = board.Turn;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    areas.tab[i, j] = board.tab[i, j].F;
                }
            }
            for (int i = 0; i < 15; i++)
            {
                areas.slots1[i] = board.slots1[i].filename;
                areas.slots2[i] = board.slots2[i].filename;
            }
            return areas;
        }
    }
}
