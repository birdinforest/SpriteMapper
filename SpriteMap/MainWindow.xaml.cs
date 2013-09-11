using System;
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

        public MainWindow()
        {
            InitializeComponent();
        }


        // File Menu Items
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

        // Edit Menu Items
        private void miEditAddSprites_Click(object sender, RoutedEventArgs e)
        {
            string filepath;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            ofd.FileName = "image";
            ofd.DefaultExt = ".png";

            if (ofd.ShowDialog() == true)
            {
                foreach (String file in ofd.FileNames)
                {
                    cTilePreview.LoadImage(file);


                    int offset = file.LastIndexOf("\\");
                    filepath = file.Substring(offset + 1, file.Length - offset - 1);
                    lbSprites.Items.Add(filepath);
                }
            }
        }
        private void miEditAddFolder_Click(object sender, RoutedEventArgs e)
        {

        }
        private void miEditDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
