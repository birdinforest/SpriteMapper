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
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Windows.Markup;
using System.Xml;
using System.IO.Compression;

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
        Tile tMoveTile = null;
        bool clicked = false;
        Point distMouseCanvas = new Point();
        Point distMouseSheet = new Point();
        public Vector GridSnap;
        public Vector CanvasSize;
        public bool GridSnapX;
        public bool GridSnapY;

        //  Preview Containers
        Image iTilePreview = new Image();

        public MainWindow()
        {
            InitializeComponent();
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            GridSnap = new Vector(8, 8);
            CanvasSize = new Vector(bSpriteSheet.Width, bSpriteSheet.Height);
            GridSnapX = true;
            GridSnapY = true;
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

            double CW = (double)bSpriteSheet.GetValue(Border.WidthProperty);
            double CH = (double)bSpriteSheet.GetValue(Border.HeightProperty);

            bSpriteSheet.SetValue(Canvas.LeftProperty, (WW / 2) - (CW / 2));
            bSpriteSheet.SetValue(Canvas.TopProperty, (WH / 2) - (CH / 2));
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                tMoveTile = null;
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
        private void miFileExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.InitialDirectory = Directory.GetCurrentDirectory();
            sfd.Filter = "PNG|*.png";
            sfd.DefaultExt = ".png";

            if (sfd.ShowDialog() == true)
            {
                string path = sfd.FileName;
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)bSpriteSheet.Width, (int)bSpriteSheet.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                cSpriteSheet.Background.Opacity = 0d;
                rtb.Render(cSpriteSheet);

                var stm = System.IO.File.Create(path);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                pngEncoder.Save(stm);
                cSpriteSheet.Background.Opacity = 1d;
                stm.Close();

                if (lSpriteSheet.Count > 0)
                    WriteXML(path);
            }
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
                    LoadSprite(filepath);
                }
                lbSprites.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
                SortTiles(lSpriteSheet, bSpriteSheet);
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
                    LoadSprite(filepath);
                }
                lbSprites.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
                SortTiles(lSpriteSheet, bSpriteSheet);
            }
        }
        private void miEditDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        //  Tools Menu Items
        private void miToolsSettings_Click(object sender, RoutedEventArgs e)
        {
            var SettingWindow = new Settings(GridSnapX, GridSnapY, GridSnap, CanvasSize);
            SettingWindow.Show();
        }

        //  Sprites ListView
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

        //  Layers  ListView
        private void lbLayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tMoveTile = lSpriteSheet[lbLayers.SelectedIndex];
        }

        //  SpriteSheet Canvas
        private void cSpriteSheet_Drop(object sender, DragEventArgs e)
        {
            int X = 1; int Y = 1;
            if (GridSnapX)
                X = (int)GridSnap.X;
            if (GridSnapY)
                Y = (int)GridSnap.Y;

            object data = e.Data.GetData(typeof(string));
            Point point = e.GetPosition((Canvas)sender);

            point.X = (int)point.X / X * X;
            point.Y = (int)point.Y / Y * Y;

            string filepath = dSprites[data.ToString()];

            Tile tile = new Tile(filepath, point);
            lSpriteSheet.Add(tile);
            lbLayers.Items.Add(tile.Name);
            cSpriteSheet.Children.Add(tile.Sprite);
        }      

        private void cSpriteBackground_MouseMove(object sender, MouseEventArgs e)
        {
            int X = 1; int Y = 1;
            if (GridSnapX)
                X = (int)GridSnap.X;
            if (GridSnapY)
                Y = (int)GridSnap.Y;

            Point mouse = e.MouseDevice.GetPosition((Canvas)sender);
            Point mouseB = e.MouseDevice.GetPosition(bSpriteSheet);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && clicked)
            {
                bSpriteSheet.SetValue(Canvas.LeftProperty, mouse.X + distMouseCanvas.X);
                bSpriteSheet.SetValue(Canvas.TopProperty, mouse.Y + distMouseCanvas.Y);
            }
            else if (tMoveTile != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                tMoveTile.Position.X = (int)(mouseB.X + distMouseSheet.X) / X * X;
                tMoveTile.Position.Y = (int)(mouseB.Y + distMouseSheet.Y) / Y * Y;

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
            Point sheetPos = new Point((double)bSpriteSheet.GetValue(Canvas.LeftProperty), (double)bSpriteSheet.GetValue(Canvas.TopProperty));
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
        private static void SortTiles(List<Tile> Tiles, Border canvas)
        {
            List<Tile> descending = new List<Tile>(Tiles);
            descending.Sort();

            //  Work out the square size needed for the Canvas
            double sqr = Math.Sqrt((double)Tiles.Count);
            int size = (int)sqr;
            if (sqr % 1 != 0)
                size++;

            //  How wide the Canvas will need to be            
            int width = (int)descending[0].Size.X;
            
            //  How high the Canvas will need to be
            int height = (int)descending[0].Size.Y;
            foreach (Tile tile in descending)
                if (tile.Size.Y > height)
                    height = (int)tile.Size.Y;

            //bSpriteSheet
            canvas.SetValue(Canvas.WidthProperty, (double)(width * size));
            canvas.SetValue(Canvas.HeightProperty, (double)height * size);

            //  Sort the Tiles onto the Canvas
            int x = 0, y = 0;
            for (int i = 0; i < Tiles.Count; ++i)
            {

                Tiles[i].Position.X = width * x;
                Tiles[i].Position.Y = height * y;

                Canvas.SetLeft(Tiles[i].Sprite, Tiles[i].Position.X);
                Canvas.SetTop(Tiles[i].Sprite, Tiles[i].Position.Y);

                x++;
                if (x >= size)
                {
                    x = 0;
                    y++;
                }
            }
        }
        private void LoadSprite(string filepath)
        {
            int offset = filepath.LastIndexOf("\\");
            string name = filepath.Substring(offset + 1, filepath.Length - offset - 1);
            if (!dSprites.ContainsKey(name))
            {
                lbSprites.Items.Add(name);
                dSprites.Add(name, filepath);
                Tile tile = new Tile(filepath, new Point(0, 0));
                lSpriteSheet.Add(tile);
                lbLayers.Items.Add(tile.Name);
                cSpriteSheet.Children.Add(tile.Sprite);
            }
        }
        private void WriteXML(string path)
        {
            int offset = path.LastIndexOf("\\");
            string name = path.Substring(offset + 1, path.Length - offset - 1);
            path = path.Substring(0, path.LastIndexOf("."));

            //Start the settings and set Indent to true for cleaner looking code
            XmlWriterSettings XWSettings = new XmlWriterSettings();
            XWSettings.Indent = true;
            //start the writer
            using (XmlWriter XMLWriter = XmlWriter.Create(path + ".xml", XWSettings))
            {
                //Start the XML writer
                XMLWriter.WriteStartDocument();
                XMLWriter.WriteStartElement("SpriteSheet");

                //Export Canvas Properties
                XMLWriter.WriteAttributeString("Count", lSpriteSheet.Count.ToString());
                XMLWriter.WriteAttributeString("Width", cSpriteSheet.ActualWidth.ToString());
                XMLWriter.WriteAttributeString("Height", cSpriteSheet.ActualHeight.ToString());
                XMLWriter.WriteAttributeString("Image", name);

                //For each image, export its relevant content
                for (int i = 0; i < lSpriteSheet.Count; ++i)
                {
                    XMLWriter.WriteStartElement("Image");
                    XMLWriter.WriteAttributeString("X", lSpriteSheet[i].Position.X.ToString());
                    XMLWriter.WriteAttributeString("Y", lSpriteSheet[i].Position.Y.ToString());
                    XMLWriter.WriteAttributeString("Width", lSpriteSheet[i].Size.X.ToString());
                    XMLWriter.WriteAttributeString("Height", lSpriteSheet[i].Size.Y.ToString());
                    XMLWriter.WriteAttributeString("Name", lSpriteSheet[i].Name.Substring(0, lSpriteSheet[i].Name.LastIndexOf(".")));
                    XMLWriter.WriteAttributeString("Id", i.ToString());
                    //XMLWriter.WriteAttributeString("Path", lSpriteSheet[i].Filepath.ToString());
                    XMLWriter.WriteEndElement();
                }
                XMLWriter.WriteEndDocument();
            }
        }
    }
}
