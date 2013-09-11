using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpriteMap
{
    public class MyCanvas : Canvas
    {
        BitmapImage background = null;

        public void LoadImage(string filename)
        {
            background = new BitmapImage(new Uri(filename));
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (background != null)
            {
                int width = background.PixelWidth,
                    height = background.PixelHeight;
                if (width > Width)
                    width = (int)Width;
                if (height > Height)
                    height = (int)Height;
                dc.DrawImage(background, new Rect((Width / 2) - (width / 2), (Height / 2) - (height / 2), width, height));
            }
        }
    }
}
