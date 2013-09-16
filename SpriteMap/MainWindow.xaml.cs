using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> dSprites = new Dictionary<string, string>();
        Image iTilePreview = new Image();

        public MainWindow()
        {
            InitializeComponent();

            Image image = new Image();
            Image image2 = new Image();
            Image image3 = new Image();

            image.Source = new BitmapImage(new Uri("C:\\Users\\nicholas.zaharias\\Git\\SpriteMapper\\SpriteMap\\Resources\\images\\blue.png"));
            image2.Source = new BitmapImage(new Uri("C:\\Users\\nicholas.zaharias\\Git\\SpriteMapper\\SpriteMap\\Resources\\images\\red.png"));
            image3.Source = new BitmapImage(new Uri("C:\\Users\\nicholas.zaharias\\Git\\SpriteMapper\\SpriteMap\\Resources\\images\\blue.png"));

            cSpriteSheet.Children.Add(image);
            cSpriteSheet.Children.Add(image2);
            cSpriteSheet.Children.Add(image3);

            Canvas.SetLeft(image, 100);
            Canvas.SetTop(image2, 100);
        }

        //  File Menu Items
        private void miFileNew_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miFileOpen_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miFileSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miFileSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miFileExport_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miFileExportAs_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miFileQuit_Click(object sender, RoutedEventArgs e)
        {

        }

        //  Edit Menu Items
        private void miEditAddSprites_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            ofd.FileName = "image";
            ofd.DefaultExt = ".png";

            if (ofd.ShowDialog() == true)
            {
                foreach (string filepath in ofd.FileNames)
                {
                    int offset = filepath.LastIndexOf("\\");
                    string name = filepath.Substring(offset + 1, filepath.Length - offset - 1);
                    lbSprites.Items.Add(name);
                    dSprites.Add(name, filepath);
                }
            }
        }
        private void miEditAddFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderpath;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = Environment.CurrentDirectory;

            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if (result.ToString() == "OK")
                folderpath = fbd.SelectedPath;
            else
                folderpath = "";

            if (folderpath != "")
            {
                string[] files = GetFiles(folderpath, "*.jpg;*.jpeg;*.png;", SearchOption.AllDirectories);
                foreach (string filepath in files)
                {
                    int offset = filepath.LastIndexOf("\\");
                    string name = filepath.Substring(offset + 1, filepath.Length - offset - 1);
                    lbSprites.Items.Add(name);
                }
            }            
        }
        private void miEditDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        //  Sprites Listbox
        private void lbSprites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cTilePreview.Children.Remove(iTilePreview);

            string filepath = dSprites[lbSprites.SelectedItem.ToString()];
            iTilePreview.Source = new BitmapImage(new Uri(filepath));
            cTilePreview.Children.Add(iTilePreview);
            Canvas.SetLeft(iTilePreview, cTilePreview.Width / 2);
            Canvas.SetTop(iTilePreview, cTilePreview.Height / 2);
            
        }

        //  SpriteSheet InkCanvas
        private void cSpriteSheet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        //  Functions
        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            //  Splits the search patterns up so that each can be called individually
            string[] searchPatterns = searchPattern.Split(';');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(Directory.GetFiles(path, sp, searchOption));
            files.Sort();
            return files.ToArray();
        }
    }
}
