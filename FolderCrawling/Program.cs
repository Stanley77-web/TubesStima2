using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace FolderCrawling {
    internal class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        private string find_file { get; set; }
        private string root_path { get; set; }
        private List<string> path_list { get; set; }
        public Program(string find_file, string root_path) {
            this.find_file = find_file;
            this.root_path = root_path;
            this.path_list = new();
        }
        public Microsoft.Msagl.Drawing.Graph BFS(bool find_all_occurence) {
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


        public Microsoft.Msagl.Drawing.Graph DFS() {
            Graph graph = new Graph("graph");
            Boolean found = false;
            Dictionary<string, char> dict_node = new Dictionary<string, char>();
            Dictionary<(string, string), char> dict_edge = new Dictionary<(string, string), char>();
            Stack<string> stack_DFS = new Stack<string>();

            stack_DFS.Push(root_path);
            dict_node.Add(root_path, 'r');

            while (stack_DFS.Count > 0) {
                Boolean take = false;
                string current_path = stack_DFS.Peek();
                string file_name = Path.GetFileName(current_path);
                if (file_name == find_file) {
                    found = true;
                    string temp_parent = current_path;
                    do {
                        for (int i = 0; i < dict_edge.Count; i++) {
                            (string, string) key = dict_edge.ElementAt(i).Key;
                            if (temp_parent == key.Item2) {

                                string temp_children = temp_parent;
                                temp_parent = key.Item1;
                                dict_edge[(temp_parent, temp_children)] = 'f';
                                dict_node[temp_children] = 'f';
                            }
                        }
                    } while (temp_parent != root_path);
                    dict_node[temp_parent] = 'f';
                    stack_DFS.Pop();
                } else {
                    if (Directory.Exists(current_path)) {
                        string[] directories = Directory.GetDirectories(current_path);
                        string[] files = Directory.GetFiles(current_path);

                        string parent_path = current_path;
                        string children_path;
                        foreach (string directory in directories) {
                            children_path = directory;
                            if (!dict_node.ContainsKey(children_path) && !take) {
                                stack_DFS.Push(children_path);
                                dict_node.Add(children_path, !found ? 'r' : 'b');
                                dict_edge.Add((parent_path, children_path), !found ? 'r' : 'b');
                                take = true;
                                break;
                            }
                        }

                        foreach (string file in files) {
                            children_path = file;
                            if (!dict_node.ContainsKey(children_path) && !take) {
                                stack_DFS.Push(children_path);
                                dict_node.Add(children_path, !found ? 'r' : 'b');
                                dict_edge.Add((parent_path, children_path), !found ? 'r' : 'b');
                                take = true;
                                break;
                            }
                        }
                    }
                    if (!take) {
                        stack_DFS.Pop();
                    }
                }
            }

            Dictionary<string, string> list_vertice = new Dictionary<string, string>();
            foreach (KeyValuePair<string, char> knv in dict_node) {
                string key = knv.Key;
                string node = knv.Key == root_path ? root_path : Path.GetFileName(key);
                if (list_vertice.ContainsKey(node)) {
                    string[] str = key.Split(@"\");
                    int n = 2;
                    do {
                        list_vertice.Remove(node);
                        node = str[str.Length - n] + @"\" + node;
                        list_vertice.Add(node, key);
                        n++;
                    } while (list_vertice.ContainsKey(node) && n <= str.Length - 1);
                } else {
                    list_vertice.Add(node, key);
                }
            }

            Dictionary<(string, string), (string, string)> list_edge = new Dictionary<(string, string), (string, string)>();
            foreach (KeyValuePair<(string, string), char> knv in dict_edge) {
                string source = knv.Key.Item1;
                string target = knv.Key.Item2;
                string src = source == root_path ? source : Path.GetFileName(source);
                string trgt = Path.GetFileName(target);
                if (!list_vertice.ContainsKey(src)) {
                    string[] str = source.Split(@"\");
                    int n = 2;
                    do {
                        src = str[str.Length - n] + @"\" + src;
                        n++;
                    } while (!list_vertice.ContainsKey(src) && n <= str.Length - 1);
                }
                if (!list_vertice.ContainsKey(trgt)) {
                    string[] str = source.Split(@"\");
                    int n = 2;
                    do {
                        trgt = str[str.Length - n] + @"\" + trgt;
                        n++;
                    } while (!list_vertice.ContainsKey(trgt) && n <= str.Length - 1);
                }
                if (!list_edge.ContainsKey((src, trgt))) list_edge.Add((src, trgt), (source, target));
            }

            foreach (KeyValuePair<string, string> knv in list_vertice) {
                graph.AddNode(knv.Key).Attr.Color = dict_node[list_vertice[knv.Key]] == 'f' ? Microsoft.Msagl.Drawing.Color.Blue :
                                                    dict_node[list_vertice[knv.Key]] == 'r' ? Microsoft.Msagl.Drawing.Color.Red :
                                                    Microsoft.Msagl.Drawing.Color.Black;
            }
            foreach (KeyValuePair<(string, string), (string, string)> knv in list_edge) {
                graph.AddEdge(knv.Key.Item1, knv.Key.Item2).Attr.Color = dict_edge[list_edge[knv.Key]] == 'f' ? Microsoft.Msagl.Drawing.Color.Blue :
                                                                        dict_edge[list_edge[knv.Key]] == 'r' ? Microsoft.Msagl.Drawing.Color.Red :
                                                                        Microsoft.Msagl.Drawing.Color.Black; ;
            }
            return graph;
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