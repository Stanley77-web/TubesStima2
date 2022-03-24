using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace FolderCrawling {
    public partial class Gui : Form {
        private string dirChosen;
        private bool hasDir = false;
        private bool hasFilename = false;
        private bool findAllOccurence = false;
        public System.Diagnostics.Process p = new System.Diagnostics.Process();
        Program prog;

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
                    prog.DFS(checkBox1.Checked);
                    animateGraph(checkBox1.Checked);
                }
                label4.Text = prog.elapsedTime();
                this.showResultPath(prog.path_list);
            }
        }

        private void animateGraph(Boolean find_all_occurence) {
            Boolean found = false;
            Graph graph = new Graph("Folder Crawling");
            List<Edge> list_edge = new();
            graph.AddNode(prog.root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            gViewer1.Graph = graph;
            Thread.Sleep(1000);
            foreach ((string,string) elmt in prog.list_graph) {
                string parent_file_name = elmt.Item1;
                string node_id_parent = parent_file_name;
                string children_file_name = elmt.Item2;
                string node_id_children = children_file_name;
                string[] str_parent = parent_file_name.Split("->");
                string[] str_childern = children_file_name.Split("->");

                if (str_parent.Length > 1) {
                    parent_file_name = str_parent[0];
                    node_id_parent = str_parent[0] + str_parent[1];
                }

                if (str_childern.Length > 1) {              
                    children_file_name = str_childern[0];
                    node_id_children = str_childern[0] + str_childern[1];
                }

                if (children_file_name == prog.find_file) {
                    
                    Node node_children = new Node(children_file_name);
                    node_children.Id = node_id_children;
                    node_children.Attr.Color = 
                        !found ? 
                            Microsoft.Msagl.Drawing.Color.Blue :
                            Microsoft.Msagl.Drawing.Color.Black;

                    graph.AddNode(node_children);

                    Edge edge = new Edge(graph.FindNode(node_id_parent), node_children, ConnectionToGraph.Connected);
                    edge.Attr.Color = 
                        !found?
                            Microsoft.Msagl.Drawing.Color.Blue :
                            Microsoft.Msagl.Drawing.Color.Black;
                    list_edge.Add(edge);

                    if (!found) {
                        string check_parent = node_id_children;
                        do {
                            foreach (Edge pair in list_edge) {
                                if (pair.TargetNode.Id == check_parent) {
                                    string check_children = check_parent;
                                    check_parent = pair.SourceNode.Id;
                                    pair.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                    pair.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                }
                            }
                        } while (check_parent != prog.root_path);
                    }
                    if (!find_all_occurence && !found) {
                        found = true;
                    }
                } else {
                    Node node = new Node(children_file_name);
                    node.Id = node_id_children;
                    node.Attr.Color =
                        !found ?
                            Microsoft.Msagl.Drawing.Color.Red :
                            Microsoft.Msagl.Drawing.Color.Black;
                        
                    graph.AddNode(node);
                    Edge edge = new Edge(graph.FindNode(node_id_parent), node, ConnectionToGraph.Connected);
                    edge.Attr.Color =
                        !found ?
                            Microsoft.Msagl.Drawing.Color.Red :
                            Microsoft.Msagl.Drawing.Color.Black;
                    list_edge.Add(edge);
                };
                gViewer1.Graph = graph;
                Thread.Sleep(1000);
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
