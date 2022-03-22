using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace FolderCrawling {
    public partial class Gui : Form {
        private string dirChosen = null;
        private bool hasDir = false;
        private bool hasFilename = false;
        private bool findAllOccurence = false;
        private DateTime temp = new DateTime();
        private Stopwatch programRunTime = new Stopwatch();
        private string elapsedTime = null;

        Program prog;

        [STAThread]
        static void Main() {
            // inisialisasi ngentot
            ApplicationConfiguration.Initialize();
            Application.Run(new Gui());
        }


        public Gui() {
            InitializeComponent();
            radioButton2.Checked = true;

        }

        private void dirButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Custom Description";

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
                programRunTime.Restart();
                programRunTime.Start();
                prog = new Program(textBox1.Text, dirChosen);
                if (radioButton2.Checked) {
                    gViewer1.Graph = prog.BFS(checkBox1.Checked);
                } else if (radioButton3.Checked) {
                    prog.DFS();
                    gViewer1.Graph = prog.DFS();
                }
                programRunTime.Stop();
                TimeSpan ts = programRunTime.Elapsed;
                elapsedTime = String.Format("{0:00}.{1:00} seconds", ts.Seconds, ts.Milliseconds / 10);
                label4.Text = elapsedTime;
            }
        }
    }
}
