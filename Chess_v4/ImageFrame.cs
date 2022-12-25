using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MyClasses.Figures;


namespace Chess
{
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
        public virtual void SetFigure(Figure f)
        {
            SetImage(f.GetPath());
        }
    }
}
