using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MyClasses
{
    delegate void Action(int x);

    public enum ChessColor
    {
        White, Black
    }

    [Serializable]
    public struct ChessTime
    {
        public int minutes;
        public int seconds;
        public ChessTime(int minutes, int seconds)
        {
            this.minutes = minutes;
            this.seconds = seconds;
        }
    }

    [Serializable]
    public class ImageFrame : PictureBox
    {
        string basepath = @"images\";
        public string filename;
        public ImageFrame()
        {
            BorderStyle = BorderStyle.None;
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.FromArgb(140, 80, 35);
        }
        public void SetImage(string filename)
        {
            if (filename == null) Image = null;
            else try
            {
                Image = Image.FromFile(basepath + filename);
                this.filename = filename;
            }
            catch
            {
                Image = Image.FromFile(basepath + "ErrorImage.png");
                this.filename = "ErrorImage.png";
            }
        }
        public virtual void SetFigure(Figures.Figure f)
        {
            SetImage(f.GetPath());
        }
    }

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
        Figures.Figure figure;
        Color basic;
        public Area(int x, int y) : base()
        {
            positionX = x;
            positionY = y;
            if ((x + y) % 2 == 0) basic = white;
            else basic = black;
            BackColor = basic;
        }
        public Figures.Figure F
        {
            get { return figure; }
        }
        public override void SetFigure(Figures.Figure f)
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

    class Clock : Label
    {
        ChessTime time;
        public Clock(ChessTime time)
        {
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point);
            Location = new Point(730, 290);
            Size = new Size(155, 65);
            Visible = false;
            Text = "No time";

            this.time = time;
        }
        public ChessTime Time
        {
            get { return time; }
        }
        public void Tick(object sender, EventArgs e)
        {
            time.seconds--;
            if(time.seconds < 0)
            {
                time.minutes--;
                time.seconds = 59;
            }
            UpdateText();
        }
        public void UpdateText()
        {
            StringBuilder build = new StringBuilder();
            if (time.minutes < 10) build.Append('0');
            build.Append(time.minutes);
            build.Append(':');
            if (time.seconds < 10) build.Append('0');
            build.Append(time.seconds);
            Text = build.ToString();
        }
        //Statyczne
        public static bool IsTimeActive = false;
        public static int GetTime(string text)
        {
            string StringTime = text.Substring(0, 2);
            return Convert.ToInt32(StringTime);
        }
    }
}
