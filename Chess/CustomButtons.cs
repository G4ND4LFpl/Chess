using System;
using System.Windows.Controls;
//using System.Collections.Generic;
//using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace Chess
{
    // Klasa abstrakcyjna Custom Button
    abstract class CustomButton : Button
    {
        public CustomButton() : base()
        {
            MouseEnter += Hover;
            MouseLeave += UnHover;
        }
        internal abstract void Hover(object sender, MouseEventArgs args);
        internal abstract void UnHover(object sender, MouseEventArgs args);
    }

    // Klasa Menu Button
    class MenuButton : CustomButton
    {
        public MenuButton() : base()
        {
            Background = Brushes.SaddleBrown;
            Foreground = Brushes.White;
        }
        internal override void Hover(object sender, MouseEventArgs args)
        {
            Background = Brushes.SandyBrown;
        }
        internal override void UnHover(object sender, MouseEventArgs args)
        {
            Background = Brushes.SaddleBrown;
        }
    }

    // Klasa Area Button
    class AreaButton : MenuButton
    {
        Image img;
        Border border;
        bool borderlock = false;
        public AreaButton(int colorId) : base()
        {
            if (colorId % 2 == 0) Background = Brushes.SandyBrown;
            else Background = Brushes.SaddleBrown;

            img = new Image();
            border = new Border()
            {
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.Transparent
            };
            border.Child = img;
            AddChild(border);
        }
        public bool LockBorderOn
        {
            set
            {
                borderlock = value;
                if (value) border.BorderBrush = Brushes.Yellow;
                else border.BorderBrush = Brushes.Transparent;
            }
        }
        internal override void Hover(object sender, MouseEventArgs args)
        {
            border.BorderBrush = Brushes.Yellow;
        }
        internal override void UnHover(object sender, MouseEventArgs args)
        {
            if (!borderlock) border.BorderBrush = Brushes.Transparent;
        }
        public void SourceSet(string source)
        {
            img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(source, UriKind.Relative));
        }
    }
}
