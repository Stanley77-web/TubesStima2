using Microsoft.Msagl.Drawing;
using System.Threading;

namespace FolderCrawling {
    public partial class Gui : Form {
        private string dirChosen;
        private bool hasDir = false;
        private bool hasFilename = false;
        public System.Diagnostics.Process p = new System.Diagnostics.Process();
        Program prog;
        int animation_speed = 250;
        

        // main procedure if we start the program
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();
            Application.Run(new Gui());
        }

        // handles creating the gui for the app
        public Gui() {
            InitializeComponent();
            radioButton2.Checked = true;
        }

        // handles choosing a directory through a button
        private void dirButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Pick a Folder";

            if (fbd.ShowDialog() == DialogResult.OK) {
                dirChosen = fbd.SelectedPath;
                label2.Text = dirChosen;
                hasDir = true;
            }
        }

        // handles the input filename box update
        private void textBox1_TextChanged(object sender, EventArgs e) {
            // make sure that the user has inputted a filename
            if (textBox1.Text == "Input Filename" || textBox1.Text == "") {
                hasFilename = false;
            } else {
                hasFilename = true;
            }
        }

        // handles the start button
        private async void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
            this.label4.Text = "";
            if (!hasDir)
            {
                // throw message box if user has not yet chosen a dir
                MessageBox.Show("Folder Directory not chosen", "Error");
            }
            else if (!hasFilename)
            {
                // throw message box if user has not yet inputted a filename
                MessageBox.Show("File Name not set", "Error");
            }
            else
            {
                // has dir AND filename, run search algorithm

                // set graph viewer color
                gViewer1.OutsideAreaBrush = Brushes.White;
                gViewer1.PanButtonPressed = true;

                // initialize the search program
                prog = new Program(textBox1.Text, dirChosen);
                bool findAllOccurence = checkBox1.Checked;
                
                // switch case for BFS DFS
                if (radioButton2.Checked)
                {
                    prog.BFS(findAllOccurence);
                }
                else if (radioButton3.Checked)
                {
                    prog.DFS(findAllOccurence);
                }

                // animate making the graph
                await animateGraph(findAllOccurence);
                if (prog.path_list.Count == 0)
                {
                    // throw message box if file is not found
                    MessageBox.Show("File not found", "Error");
                }

                // print the run time
                label5.Text = prog.elapsedTime();

                // print the result paths through hyperlinks
                this.showResultPath(prog.path_list);
            }
        }

        // handles animating the graph
        private async Task animateGraph(Boolean find_all_occurence)
        {
            Boolean found = false;
            Graph graph = new Graph("Folder Crawling");
            List<Edge> list_edge = new();
            graph.AddNode(prog.root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            foreach ((string, string) elmt in prog.list_graph)
            {
                if (!found)
                {
                    gViewer1.Graph = graph;
                    // delay by certain time so graph building is not instantaneous
                    await Task.Delay(animation_speed);
                }
                string parent_file_name = elmt.Item1;
                string node_id_parent = parent_file_name;
                string children_file_name = elmt.Item2;
                string node_id_children = children_file_name;
                string[] str_parent = parent_file_name.Split("->");
                string[] str_children = children_file_name.Split("->");

                if (str_parent.Length > 1)
                {
                    parent_file_name = str_parent[0];
                    node_id_parent = str_parent[0] + str_parent[1];
                }

                if (str_children.Length > 1)
                {
                    children_file_name = str_children[0];
                    node_id_children = str_children[0] + str_children[1];
                }

                Node node = new Node(children_file_name);
                node.Id = node_id_children;
                node.Attr.Color =
                    children_file_name == prog.find_file && !found ? 
                        Microsoft.Msagl.Drawing.Color.Blue : 
                        !found ?
                            Microsoft.Msagl.Drawing.Color.Red :
                            Microsoft.Msagl.Drawing.Color.Black;

                graph.AddNode(node);
                Edge edge = new Edge(graph.FindNode(node_id_parent), node, ConnectionToGraph.Connected);
                edge.Attr.Color =
                    children_file_name == prog.find_file && !found ? 
                        Microsoft.Msagl.Drawing.Color.Blue : 
                        !found ?
                            Microsoft.Msagl.Drawing.Color.Red :
                            Microsoft.Msagl.Drawing.Color.Black;

                list_edge.Add(edge);
                if (children_file_name == prog.find_file)
                {                   
                    if (!found)
                    {
                        string check_parent = node_id_children;
                        do
                        {
                            foreach (Edge pair in list_edge)
                            {
                                if (pair.TargetNode.Id == check_parent)
                                {
                                    string check_children = check_parent;
                                    check_parent = pair.SourceNode.Id;
                                    pair.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                    pair.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                }
                            }
                        } while (check_parent != prog.root_path);
                        gViewer1.Graph = graph;
                        // delay by certain time so graph building is not instantaneous
                        await Task.Delay(animation_speed);  
                    }
                    if (!find_all_occurence && !found)
                    {
                        found = true;
                    }
                }
            }
            gViewer1.Graph = graph;
        }

        // handles showing the resulting path of the searching program
        // prints the resulting path using hyperlinks below the graph viewer
        private void showResultPath(List<string> resultPath) {
            string res = "";
            res += "\nPath File:\n";
            if (resultPath.Count() == 0) {
                res += "None found\n";
            }
            for (int i = 0; i < resultPath.Count(); i++) {
                res += (i + 1) + ". file:\\\\" + resultPath[i] + "\n";
            }
            this.richTextBox1.Text = res;
        }

        // handles opening file explorer if a hyperlink is clicked
        private void richTextBox1_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e) {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p = System.Diagnostics.Process.Start("Explorer.exe", e.LinkText);
        }

        // handles updating animation speed through scrollbar
        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            animation_speed = trackBar1.Value;
            
        }
    }
}
