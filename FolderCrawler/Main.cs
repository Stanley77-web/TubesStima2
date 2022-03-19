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
using "Program.cs";

namespace FolderCrawler
{
    public partial class Main : Form
    {
        string dirChosen;
        bool hasDir = false;
        bool hasFilename = false;
        bool findAllOccurence = false;

        public Main()
        {
            InitializeComponent();
            radioButton2.Checked = true;
        }

        private void dirButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog chooseFile = new CommonOpenFileDialog();
            chooseFile.IsFolderPicker = true;
            if (chooseFile.ShowDialog() == CommonFileDialogResult.Ok) {
                dirChosen = chooseFile.FileName;
                label2.Text = dirChosen;
                hasDir = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text == "Input Filename" || textBox1.Text == "")
            {
                hasFilename = false;
            }
            else
            {
                hasFilename = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (findAllOccurence) { findAllOccurence = true; }
            else { findAllOccurence = false;  }
            checkBox1.Text = "Find All Occurence"; // temp fix anj gangerti lagi jancokkkk
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!hasDir)
            {
                MessageBox.Show("Folder Directory not chosen", "Error");
            }
            else if (!hasFilename)
            {
                MessageBox.Show("File Name not set", "Error");
            }
        }
    }
}
