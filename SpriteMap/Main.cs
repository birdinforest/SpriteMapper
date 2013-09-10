using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace SpriteMap
{
    public partial class Main : Form
    {
        List<String> sprites = new List<String>();
        public Main()
        {
            InitializeComponent();
        }

        private void tsEdit_AddSprites_Click(object sender, EventArgs e)
        {
            string filepath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            ofd.Multiselect = true;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (String file in ofd.FileNames)
                {
                    filepath = file;
                    pbPreview.Load(filepath);
                    if (pbPreview.Image.Width <= pbPreview.Width && pbPreview.Image.Height <= pbPreview.Height)
                        pbPreview.SizeMode = PictureBoxSizeMode.CenterImage;
                    else
                        pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
                    sprites.Add(filepath);

                    
                    int offset = file.LastIndexOf("\\");
                    filepath = file.Substring(offset + 1, file.Length - offset - 1);
                    lbSprites.Items.Add(filepath);
                }                
            }            
        }

        private void btnAddSprites_Click(object sender, EventArgs e)
        {
            tsEdit_AddSprites_Click(sender, e);
        }
    }
}
