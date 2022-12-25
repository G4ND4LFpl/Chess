using System;
//using System.Collections.Generic;
using System.Windows.Controls;
//using System.Text;
//using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using General;

namespace Chess
{
    class AreaButton : Button
    {
        Image img;
        public AreaButton(ChessColor color) : base()
        {
            if(color == ChessColor.White)
                Background = Brushes.SandyBrown;
            if (color == ChessColor.Black)
                Background = Brushes.SaddleBrown;

            img = new Image();
            AddChild(img);
        }
        internal void Hover(object sender, MouseEventArgs args)
        {
            // pass
        }
        internal void UnHover(object sender, MouseEventArgs args)
        {
            // pass
        }
        public void SourceSet(string source)
        {
            img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(source, UriKind.Relative));
        }
    }
}
