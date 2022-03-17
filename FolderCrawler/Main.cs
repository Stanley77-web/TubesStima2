using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FolderCrawler
{
    public partial class Main : Form
    {
        string dirChosen;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dirButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog chooseFile = new CommonOpenFileDialog();
            chooseFile.IsFolderPicker = true;
            if (chooseFile.ShowDialog() == CommonFileDialogResult.Ok) {
                dirChosen = chooseFile.FileName;
                if(dirChosen.Length > 32)
                {
                    directoryTextbox.Text = ".." + dirChosen.Substring(dirChosen.Length - 32);
                }
            }
        }

        private void directoryTextbox_TextChanged(object sender, EventArgs e)
        {
            // does nothing, actually
        }
    }
}
