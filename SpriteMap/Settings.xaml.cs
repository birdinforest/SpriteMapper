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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace SpriteMap
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        MainWindow main;
        public Settings(bool _SnapX, bool _SnapY, Vector _SnapSize, Vector _CanvasSize)
        {
            InitializeComponent();

            main = Application.Current.Windows[0] as MainWindow;

            cSnapX.IsChecked = _SnapX;
            cSnapY.IsChecked = _SnapY;
            tSnapX.Text = _SnapSize.X.ToString();
            tSnapY.Text = _SnapSize.Y.ToString();

            tCanvasX.Text = _CanvasSize.X.ToString();
            tCanvasY.Text = _CanvasSize.Y.ToString();

        }

        private void cSnapX_Checked(object sender, RoutedEventArgs e)
        {
            main.GridSnapX = true;
        }
        private void cSnapX_Unchecked(object sender, RoutedEventArgs e)
        {
            main.GridSnapX = false;
        }
        private void tSnapX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumber(tSnapX.Text))
                main.GridSnap.X = double.Parse(tSnapX.Text);
        }

        private void cSnapY_Checked(object sender, RoutedEventArgs e)
        {
            main.GridSnapY = true;
        }
        private void cSnapY_Unchecked(object sender, RoutedEventArgs e)
        {
            main.GridSnapY = false;
        }
        private void tSnapY_TextChanged(object sender, TextChangedEventArgs e)
        {
            main.GridSnap.Y = double.Parse(tSnapY.Text);
        }

        bool IsNumber(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }
    }
}
