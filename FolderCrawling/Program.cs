using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using System.Diagnostics;

namespace FolderCrawling {
    internal class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        private string find_file { get; set; }
        private string root_path { get; set; }
        public List<string> path_list { get; set; }
        private DateTime temp = new DateTime();
        private Stopwatch programRunTime = new Stopwatch();

        public Program(string find_file, string root_path) {
            this.find_file = find_file;
            this.root_path = root_path;
            this.path_list = new();
            this.programRunTime.Restart();
        }
        public Microsoft.Msagl.Drawing.Graph BFS(bool find_all_occurence) {
            this.programRunTime.Start();
            Graph graph = new Graph("graph");
            Boolean found = false;
            Dictionary<string, Microsoft.Msagl.Drawing.Edge> dict_BFS = new();
            Dictionary<string, int> file_count = new();
            Queue<(string, string)> queue_BFS = new();
            Queue<(string, string)> queue_graph = new();

            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);
            this.path_list.Clear();
            foreach (string directory in directories) {
                queue_BFS.Enqueue((root_path, directory));
            }

            foreach (string file in files) {
                queue_BFS.Enqueue((root_path, file));
            }

            while (queue_BFS.Count > 0 && !found) {
                (string, string) current_queue = queue_BFS.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                if (file_name != find_file) {
                    if (!file_count.ContainsKey(file_name)) {
                        file_count[file_name] = 0;
                    } else {
                        file_count[file_name]++;
                        file_name += " (" + file_count[file_name] + ")";
                    }
                    if (Directory.Exists(current_path)) {
                        directories = Directory.GetDirectories(current_path);
                        files = Directory.GetFiles(current_path);
                        foreach (string directory in directories) {
                            queue_BFS.Enqueue((file_name, directory));
                        }
                        foreach (string file in files) {
                            queue_BFS.Enqueue((file_name, file));
                        }
                    }
                } else {
                    if (!find_all_occurence) {
                        found = true;
                    }
                }
                queue_graph.Enqueue((parent_path, file_name));
                // stopwatch berhenti
                this.programRunTime.Stop();
            }

            graph.AddNode(root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            while (queue_graph.Count > 0) {
                (string, string) current_queue = queue_graph.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                if (file_name != find_file) {
                    var node = graph.AddNode(file_name);
                    var edge = graph.AddEdge(parent_path, file_name);
                    if (!found) {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    } else {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                    }
                    dict_BFS[file_name] = edge;
                } else {
                    graph.AddNode(file_name).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    var edge = graph.AddEdge(parent_path, file_name);
                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    edge.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    while (dict_BFS.ContainsKey(parent_path)) {
                        dict_BFS[parent_path].Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        dict_BFS[parent_path].SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        parent_path = dict_BFS[parent_path].Source;
                    }
                    this.path_list.Add(current_path);
                    if (!find_all_occurence) {
                        found = true;
                    }
                }
            }
            return graph;
        }


        public Graph DFS() {
            Graph graph = new Graph("graph");
            Boolean found = false;
            Dictionary<string, Edge> dict_BFS = new();
            Dictionary<string, int> file_count = new();
            Queue<(string, string)> queue_BFS = new();
            Queue<(string, string)> queue_graph = new();

            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);
            this.path_list.Clear();
            foreach (string directory in directories) {
                queue_BFS.Enqueue((root_path, directory));
            }

            foreach (string file in files) {
                queue_BFS.Enqueue((root_path, file));
            }
            // start stopwatch
            while (queue_BFS.Count > 0) {
                (string, string) current_queue = queue_BFS.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                if (file_name != find_file) {
                    if (!file_count.ContainsKey(file_name)) {
                        file_count[file_name] = 0;
                    } else {
                        file_count[file_name]++;
                        file_name += " (" + file_count[file_name] + ")";
                    }
                    if (Directory.Exists(current_path)) {
                        directories = Directory.GetDirectories(current_path);
                        files = Directory.GetFiles(current_path);
                        foreach (string directory in directories) {
                            queue_BFS.Enqueue((file_name, directory));
                        }
                        foreach (string file in files) {
                            queue_BFS.Enqueue((file_name, file));
                        }
                    }
                } else {
                    if (!find_all_occurence && !found) {
                        found = true;
                        // stopwatch berhenti
                    }
                }
                queue_graph.Enqueue((parent_path, file_name));
            }
            // stop stopwatch
            graph.AddNode(root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            while (queue_graph.Count > 0) {
                (string, string) current_queue = queue_graph.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                if (file_name != find_file) {
                    var node = graph.AddNode(file_name);
                    var edge = graph.AddEdge(parent_path, file_name);
                    if (!found) {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    } else {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                    }
                    dict_BFS[file_name] = edge;
                } else {
                    graph.AddNode(file_name).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    var edge = graph.AddEdge(parent_path, file_name);
                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    edge.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    while (dict_BFS.ContainsKey(parent_path)) {
                        dict_BFS[parent_path].Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        dict_BFS[parent_path].SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        parent_path = dict_BFS[parent_path].Source;
                    }
                    this.path_list.Add(current_path);
                    if (!find_all_occurence) {
                        found = true;
                    }
                }
            }
            return graph;
        }


        public Graph DFS(bool find_all_occurence) {
            Form form = new Form();
            GViewer viewer = new GViewer();
            Graph graph = new Graph("Folder Crawling DFS");

            Boolean found = false;

            Dictionary<string,int> same_file_count = new Dictionary<string,int>();
            List<string> list_vertice = new List<string>();
            List<Edge> list_edge = new List<Edge>();
            Stack<string> stack_DFS = new Stack<string>();

            stack_DFS.Push(root_path);
            graph.AddNode(root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            list_vertice.Add(root_path);
            int n = 0, m = 0;
            // start stopwatch
            while (stack_DFS.Count > 0) {
                Boolean take = false;
                string current_path = stack_DFS.Peek();
                string file_name = Path.GetFileName(current_path);
                if (file_name == find_file) {
                    if (!find_all_occurence && !found) {
                        found = true;
                        // stop stopwatch
                    }
                    if (!found) {
                        Node node = graph.FindNode(file_name);
                        string parent_file_name = node.Id;
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        do {
                            foreach (Edge elmt in list_edge) {
                                if (elmt.TargetNode.Id == parent_file_name) {
                                    string children_file_name = parent_file_name;
                                    parent_file_name = elmt.SourceNode.Id;
                                    elmt.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                    elmt.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                }
                            }
                        } while (parent_file_name != root_path);
                    }
                } else {
                if (Directory.Exists(current_path)) {
                    string[] directories = Directory.GetDirectories(current_path);
                    string[] files = Directory.GetFiles(current_path);

                    string parent_file_name = 
                    current_path == root_path ? 
                    current_path :
                    Path.GetFileName(current_path);
                    string children_path = "XXXX";
                    foreach (string directory in directories) {
                    if (list_vertice.IndexOf(directory) == -1 && !take) { 
                        children_path = directory;
                        stack_DFS.Push(children_path);
                        list_vertice.Add(children_path);
                        take = true;
                    }
                    }

                    foreach (string file in files) {
                    if (list_vertice.IndexOf(file) == -1 && !take) {
                        children_path = file;
                        stack_DFS.Push(children_path);
                        list_vertice.Add(children_path);
                        take = true;
                    }
                    }

                    if (take) {
                        string children_file_name = Path.GetFileName(children_path);
                        string id_node = children_file_name;
                        if (graph.FindNode(children_file_name) != null) {
                            same_file_count[children_file_name] = 
                            !same_file_count.ContainsKey(children_file_name) ? 
                                1 :
                                same_file_count[children_file_name]++;

                            children_file_name += " " + same_file_count[children_file_name].ToString();
                        }
                        
                        Node node = new Node(children_file_name);
                        // node.Id = id_node;
                        node.Attr.Color =
                            !found ?
                            Microsoft.Msagl.Drawing.Color.Red :
                            Microsoft.Msagl.Drawing.Color.Black;
                        graph.AddNode(node);

                        Edge edge = graph.AddEdge(parent_file_name,children_file_name);
                        // edge.TargetNode.Id = id_node;
                        edge.Attr.Color =
                            !found ?
                            Microsoft.Msagl.Drawing.Color.Red :
                            Microsoft.Msagl.Drawing.Color.Black;
                        list_edge.Add(edge);  
                    }
                }
                }
                if (!take) {
                stack_DFS.Pop();
                }
            }
            // stop stopwatch
            view(graph);
            return graph;
        }

        public string elapsedTime(){
            TimeSpan ts = programRunTime.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:000} seconds", ts.Seconds, ts.Milliseconds);
            return elapsedTime;
        }

        public static void view(Graph graph) {
            Form form = new Form();
            GViewer viewer = new GViewer();

            viewer.Graph = graph;
            // associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            // show the form 
            form.ShowDialog();
        }

    }
}