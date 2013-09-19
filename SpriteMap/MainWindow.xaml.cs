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
        //  Sprite Sheet Containters
        Dictionary<string, string> dSprites = new Dictionary<string, string>();
        List<Tile> lSpriteSheet = new List<Tile>();
        Tile tSelectedTile = null;
        Tile tMoveTile = null;
        bool clicked = false;
        Point distMouseCanvas = new Point();
        Point distMouseSheet = new Point();
        int GridSize = 32;
        int GridSnap = 16;

        //  Preview Containers
        Image iTilePreview = new Image();

        public MainWindow()
        {
            InitializeComponent();
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
        }

        //  Window
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clicked = false;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clicked = true;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double WW = (double)GetValue(Window.ActualWidthProperty);
            double WH = (double)GetValue(Window.ActualHeightProperty);

            double CW = (double)cSpriteSheet.GetValue(Canvas.WidthProperty);
            double CH = (double)cSpriteSheet.GetValue(Canvas.HeightProperty);

            cSpriteSheet.SetValue(Canvas.LeftProperty, (WW/2) - (CW/2));
            cSpriteSheet.SetValue(Canvas.TopProperty, (WH / 2) - (CH / 2));
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
                    //if (dSprites.ContainsKey(name))
                    dSprites.Add(name, filepath);
                }
            }
            lbSprites.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
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
            lbSprites.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
        }
        private void miEditDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        //  Sprite Add Button
        private void btnSpriteAdd_Click(object sender, RoutedEventArgs e)
        {
            miEditAddSprites_Click(sender, e);
        }

        //  Folder Add Button
        private void btnFolderAdd_Click(object sender, RoutedEventArgs e)
        {
            miEditAddFolder_Click(sender, e);
        }

        //  Sprites Listbox
        private void lbSprites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cTilePreview.Children.Remove(iTilePreview);

            if (lbSprites.SelectedItem == null)
                return;

            string filepath = dSprites[lbSprites.SelectedItem.ToString()];
            BitmapImage image = new BitmapImage(new Uri(filepath));

            iTilePreview.Source = image;
            iTilePreview.Width = (image.Width > cTilePreview.Width) ? cTilePreview.Width : image.Width;
            iTilePreview.Height = (image.Height > cTilePreview.Height) ? cTilePreview.Height : image.Height;


            cTilePreview.Children.Add(iTilePreview);
            Canvas.SetLeft(iTilePreview, (cTilePreview.Width / 2) - (iTilePreview.Width / 2));
            Canvas.SetTop(iTilePreview, (cTilePreview.Height / 2) - (iTilePreview.Height / 2));            
        }     
        private void lbSprites_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object data = GetDataFromListBox(parent, e.GetPosition(parent));
            lbSprites.SelectedItem = data;

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        //  SpriteSheet Canvas
        private void cSpriteSheet_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(string));
            Point point = e.GetPosition((Canvas)sender);
            point.X = (int)point.X / GridSnap * GridSnap;
            point.Y = (int)point.Y / GridSnap * GridSnap;

            string filepath = dSprites[data.ToString()];

            Tile tile = new Tile(filepath, point);
            lSpriteSheet.Add(tile);
            lbLayers.Items.Add(tile.Name);
            cSpriteSheet.Children.Add(tile.Sprite);
        }      

        private void cSpriteBackground_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse = e.MouseDevice.GetPosition((Canvas)sender);
            Point mouseB = e.MouseDevice.GetPosition(cSpriteSheet);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && clicked)
            {
                cSpriteSheet.SetValue(Canvas.LeftProperty, mouse.X + distMouseCanvas.X);
                cSpriteSheet.SetValue(Canvas.TopProperty, mouse.Y + distMouseCanvas.Y);
            }
            else if (tMoveTile != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                tMoveTile.Position.X = (int)(mouseB.X + distMouseSheet.X) / GridSnap * GridSnap;
                tMoveTile.Position.Y = (int)(mouseB.Y + distMouseSheet.Y) / GridSnap * GridSnap;

                Canvas.SetLeft(tMoveTile.Sprite, tMoveTile.Position.X);
                Canvas.SetTop(tMoveTile.Sprite, tMoveTile.Position.Y);
            }
        }
        private void cSpriteBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mouse = e.MouseDevice.GetPosition((Canvas)sender);
            Point mouseB = e.MouseDevice.GetPosition(cSpriteSheet);

            if (!clicked)
            {
                for (int i = 0; i < lSpriteSheet.Count; ++i)
                {
                    if ((mouseB.X < (lSpriteSheet[i].Position.X + lSpriteSheet[i].Size.X)) && (mouseB.X > lSpriteSheet[i].Position.X) && (mouseB.Y < (lSpriteSheet[i].Position.Y + lSpriteSheet[i].Size.Y)) && (mouseB.Y > lSpriteSheet[i].Position.Y))
                    {
                        tMoveTile = lSpriteSheet[i];
                        lbLayers.SelectedIndex = i;
                    }
                }
            }

            //  Finds the distance between the top corner of Object and the Mouse
            if (tMoveTile != null)
                distMouseSheet = (Point)(tMoveTile.Position - mouseB);
            Point sheetPos = new Point((double)cSpriteSheet.GetValue(Canvas.LeftProperty), (double)cSpriteSheet.GetValue(Canvas.TopProperty));
            distMouseCanvas = (Point)(sheetPos - mouse);
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
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                tMoveTile = null;
        }

        private void lbLayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tMoveTile = lSpriteSheet[lbLayers.SelectedIndex];
        }

























    }
}
