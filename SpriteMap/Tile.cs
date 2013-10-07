using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SpriteMap
{
    [Serializable]
    public class Tile : IComparable<Tile>
    {
        public string Name;
        public string Filepath;
        public Image Sprite;
        public Point Position;
        public Point Size;

        public Tile(string _Filepath, Point _Position)
        {
            Filepath = _Filepath;

            int offset = _Filepath.LastIndexOf("\\");
            Name = _Filepath.Substring(offset + 1, _Filepath.Length - offset - 1);

            Position = _Position;

            Sprite = new Image();
            BitmapImage image = new BitmapImage(new Uri(Filepath));
            Sprite.Source = image;
            Canvas.SetLeft(Sprite, Position.X);
            Canvas.SetTop(Sprite, Position.Y);

            Size.X = image.Width;
            Size.Y = image.Height;
        }

        public int CompareTo(Tile other)
        {
            //  -1* Forces Descending Order
            if (Size.X == other.Size.X)
                return -1*Size.Y.CompareTo(other.Size.Y);
            return -1*Size.X.CompareTo(other.Size.X);
        }
    }
}
