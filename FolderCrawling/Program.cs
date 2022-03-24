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

        public Graph BFS(bool find_all_occurence) {
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
                        file_name += "->" + file_count[file_name];
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
                    if (!found) {
                        this.path_list.Add(current_path);
                    }

                    if (!find_all_occurence && !found) {
                        found = true;
                        // stopwatch berhenti
                    }
                }
                queue_graph.Enqueue((parent_path, file_name));
            }
            // stop stopwatch
            graph.AddNode(root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            found = false;
            while (queue_graph.Count > 0) {
                (string, string) current_queue = queue_graph.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                string parent_file_name = parent_path;
                string node_id_parent = parent_file_name;
                string children_file_name = file_name;
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

                if (file_name != find_file) {
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
                        
                    dict_BFS[node_id_children] = edge;

                } else {
                    Node node_children = new Node(children_file_name);
                    node_children.Id = node_id_children;
                    node_children.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;

                    graph.AddNode(node_children);

                    Edge edge = new Edge(graph.FindNode(node_id_parent), node_children, ConnectionToGraph.Connected);
                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    edge.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;

                    dict_BFS[node_id_children] = edge;
                    
                    while (dict_BFS.ContainsKey(node_id_parent)) {
                        dict_BFS[node_id_parent].Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        dict_BFS[node_id_parent].SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        node_id_parent = dict_BFS[node_id_parent].Source;
                    }
                    
                    if (!find_all_occurence && !found) {
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

            Dictionary<string,int> file_count = new Dictionary<string,int>();
            List<string> list_vertice = new List<string>();
            List<string> list_node = new List<string>();
            List<(string,string)> list_graph = new List<(string, string)>();
            List<Edge> list_edge = new List<Edge>();
            Stack<string> stack_DFS = new Stack<string>();

            stack_DFS.Push(root_path);
            
            list_vertice.Add(root_path);
            file_count.Add(root_path,0);

            // start stopwatch
            while (stack_DFS.Count > 0) {
                Boolean take = false;
                string current_path = stack_DFS.Peek();
                string file_name = Path.GetFileName(current_path);
                if (file_name == find_file) {
                    if (!found) {
                        this.path_list.Add(current_path);
                    }
                    if (!find_all_occurence && !found) {
                        found = true;
                        // stop stopwatch
                    }
                    list_node.Add(file_name);
                } else {
                if (Directory.Exists(current_path)) {
                    string[] directories = Directory.GetDirectories(current_path);
                    string[] files = Directory.GetFiles(current_path);

                    string parent_file_name = 
                    current_path == root_path ? 
                    current_path :
                    Path.GetFileName(current_path);
                    string children_path = "";
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

                            parent_file_name = 
                                file_count[parent_file_name] == 0 ? 
                                    parent_file_name :
                                    parent_file_name +  "->" + file_count[parent_file_name].ToString();
                            
                            file_count[children_file_name] = 
                                !file_count.ContainsKey(children_file_name) ? 
                                    0 :
                                    file_count[children_file_name] + 1; 

                            children_file_name = 
                                file_count[children_file_name] == 0 ? 
                                    children_file_name :
                                    children_file_name +  "->" + file_count[children_file_name].ToString();
                            
                            list_node.Add(children_file_name);
                            list_graph.Add((parent_file_name,children_file_name)); 
                        }
                    }
                }
                if (!take) {
                stack_DFS.Pop();
                }
            }
            // stop stopwatch

            // visualisasi graph
            found = false;
            graph.AddNode(root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            foreach ((string,string) elmt in list_graph) {
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

                if (children_file_name == find_file) {
                    Node node_children = new Node(children_file_name);
                    node_children.Id = node_id_children;
                    node_children.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;

                    graph.AddNode(node_children);

                    Edge edge = new Edge(graph.FindNode(node_id_parent), node_children, ConnectionToGraph.Connected);
                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    list_edge.Add(edge);

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
                    } while (check_parent != root_path);

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
                }

            }
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