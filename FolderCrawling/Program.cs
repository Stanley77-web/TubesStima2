using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using System.Diagnostics;

namespace FolderCrawling {
    internal class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public string find_file { get; private set; }
        public string root_path { get; private set; }
        public List<string> path_list { get; private set; }
        public Dictionary<string, int> file_count {get; private set;}
        public List<(string,string)> list_graph { get; private set; }
        private DateTime temp = new DateTime();
        private Stopwatch programRunTime = new Stopwatch();

        public Program(string find_file, string root_path) {
            this.find_file = find_file;
            this.root_path = root_path;
            this.path_list = new();
            this.list_graph = new();
            this.file_count = new();      
            this.programRunTime.Restart();
        }

        public void BFS(bool find_all_occurence) {
            Boolean found = false;
            Queue<(string, string)> queue_BFS = new();

            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);

            this.path_list.Clear();
            this.list_graph.Clear();
            this.file_count.Clear();

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
                    if (!file_count.ContainsKey(file_name)) {
                        file_count[file_name] = 0;
                    } else {
                        file_count[file_name]++;
                        file_name += "->" + file_count[file_name];
                    }
                    if (!found) {
                        this.path_list.Add(Directory.GetParent(current_path).FullName);
                    }

                    if (!find_all_occurence && !found) {
                        found = true;
                        // stopwatch berhenti
                    }
                }
                list_graph.Add((parent_path, file_name));
            }
            // stop stopwatch
        }

       public void DFS(bool find_all_occurence) {
            Boolean found = false;

            this.path_list.Clear();
            this.list_graph.Clear();
            this.file_count.Clear();

            List<Edge> list_edge = new List<Edge>();
            List<string> list_vertice = new List<string>();
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
                        this.path_list.Add(Directory.GetParent(current_path).FullName);
                    }
                    if (!find_all_occurence && !found) {
                        found = true;
                        // stop stopwatch
                    }
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
                            
                            // list_node.Add(children_file_name);
                            list_graph.Add((parent_file_name,children_file_name)); 
                        }
                    }
                }
                if (!take) {
                stack_DFS.Pop();
                }
            }
            // stop stopwatch
        }
        public string elapsedTime(){
            TimeSpan ts = programRunTime.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:000} seconds", ts.Seconds, ts.Milliseconds);
            return elapsedTime;
        }
    }
}