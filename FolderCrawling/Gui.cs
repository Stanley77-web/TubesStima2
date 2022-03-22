using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderCrawling {
    public partial class Gui : Form {
        private string dirChosen = null;
        private bool hasDir = false;
        private bool hasFilename = false;
        private bool findAllOccurence = false;
        public System.Diagnostics.Process p = new System.Diagnostics.Process();
        Program prog = null;

        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();
            Application.Run(new Gui());
        }


        public Gui() {
            InitializeComponent();
            radioButton2.Checked = true;
        }

        private void dirButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Pick a Folder";

            if (fbd.ShowDialog() == DialogResult.OK) {
                dirChosen = fbd.SelectedPath;
                label2.Text = dirChosen;
                hasDir = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            if (textBox1.Text == "Input Filename" || textBox1.Text == "") {
                hasFilename = false;
            } else {
                hasFilename = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (findAllOccurence) { findAllOccurence = true; } else { findAllOccurence = false; }
            checkBox1.Text = "Find All Occurence"; // temp fix anj gangerti lagi jancokkkk
        }

        private void button1_Click(object sender, EventArgs e) {
            if (!hasDir) {
                MessageBox.Show("Folder Directory not chosen", "Error");
            } else if (!hasFilename) {
                MessageBox.Show("File Name not set", "Error");
            } else {
                gViewer1.OutsideAreaBrush = Brushes.Silver;
                gViewer1.PanButtonPressed = true;
                prog = new Program(textBox1.Text, dirChosen);
                if (radioButton2.Checked) {
                    gViewer1.Graph = prog.BFS(checkBox1.Checked);
                } else if (radioButton3.Checked) {
                    gViewer1.Graph = prog.DFS(checkBox1.Checked);
                }
                label4.Text = prog.elapsedTime();
                this.showResultPath(prog.path_list);
            }
        }

        private void showResultPath(List<string> resultPath)
        {
            string res = "";
            res += "\n\nPath File:\n";
            if(resultPath.Count() == 0)
            {
                res += "None found\n";
            }
            for(int i = 0; i < resultPath.Count(); i++)
            {
                res += (i + 1) + ". file://" + resultPath[i] + "\n";
            }
            this.richTextBox1.Text = res;
        }

        private void richTextBox1_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p = System.Diagnostics.Process.Start("Explorer.exe", e.LinkText);
        }
    }
}
