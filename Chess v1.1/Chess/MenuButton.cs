//using System;
using System.Windows.Controls;
//using System.Collections.Generic;
//using System.Text;
using System.Windows.Media;
using System.Windows.Input;

namespace Chess
{
    class MenuButton : Button
    {
        public MenuButton() : base()
        {
            Background = Brushes.SaddleBrown;
            Foreground = Brushes.White;

            MouseEnter += Hover;
            MouseLeave += UnHover;
        }
        internal void Hover(object sender, MouseEventArgs args)
        {
            Background = Brushes.SandyBrown;
        }
        internal void UnHover(object sender, MouseEventArgs args)
        {
            Background = Brushes.SaddleBrown;
        }
    }
}
