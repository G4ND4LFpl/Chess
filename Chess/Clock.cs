using System;
//using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
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
            if (time.seconds < 0)
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
