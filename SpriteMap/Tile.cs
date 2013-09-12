using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace SpriteMap
{
    public class Tile
    {
        public double X, Y;
        public double Width, Height;
        public BitmapImage Sprite;
        public string Name;
        public string Filepath;

        public Tile(string _Filepath, double _X, double _Y)
        {
            Filepath = _Filepath;

            int offset = _Filepath.LastIndexOf("\\");
            Name = _Filepath.Substring(offset + 1, _Filepath.Length - offset - 1);

            Sprite = new BitmapImage(new Uri(Filepath));

            Width = Sprite.Width;
            Height = Sprite.Height;

            X = _X;
            Y = _Y;
            
        }
    }
}
