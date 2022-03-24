using System.Diagnostics;

namespace FolderCrawling {
    internal class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public string find_file { get; private set; }
        public string root_path { get; private set; }
        public List<string> path_list { get; private set; }
        public Dictionary<string, int> file_count { get; private set; }
        public List<(string, string)> list_graph { get; private set; }
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

            this.path_list.Clear();
            this.list_graph.Clear();
            this.file_count.Clear();

            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);

            foreach (string directory in directories) {
                queue_BFS.Enqueue((root_path, directory));
            }
            foreach (string file in files) {
                queue_BFS.Enqueue((root_path, file));
            }

            // start stopwatch
            this.programRunTime.Start();
            while (queue_BFS.Count > 0) {
                (string, string) current_queue = queue_BFS.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                string temp_name = file_name;
                if (!file_count.ContainsKey(file_name)) {
                    file_count[file_name] = 0;
                } else {
                    file_count[file_name]++;
                    file_name += "->" + file_count[file_name];
                }
                if (temp_name != find_file) {
                    if (!found) {
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
                    }
                } else {
                    if (!found) {
                        this.path_list.Add(Directory.GetParent(current_path).FullName + @"\");
                    }
                    if (!find_all_occurence && !found) {
                        found = true;
                        this.programRunTime.Stop();
                    }
                }
                list_graph.Add((parent_path, file_name));
            }
            if (this.programRunTime.IsRunning) {
                this.programRunTime.Stop();
            }
        }

        public void DFS (bool find_all_occurence) {
            Boolean found = false;
            Stack<(string, string)> stack_DFS = new();

            this.path_list.Clear();
            this.list_graph.Clear();
            this.file_count.Clear();

            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);

            Array.Reverse(directories);
            Array.Reverse(files);

            foreach (string directory in directories) {
                stack_DFS.Push((root_path, directory));
            }
            foreach (string file in files) {
                stack_DFS.Push((root_path, file));
            }

            this.programRunTime.Start();
            while (stack_DFS.Count > 0) {
                (string, string) current_queue = stack_DFS.Pop();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                string temp_name = file_name;
                if (!file_count.ContainsKey(file_name)) {
                    file_count[file_name] = 0;
                } else {
                    file_count[file_name]++;
                    file_name += "->" + file_count[file_name];
                }
                if (temp_name != find_file) {
                    if (!found) {
                        if (Directory.Exists(current_path)) {
                            directories = Directory.GetDirectories(current_path);
                            files = Directory.GetFiles(current_path);

                            Array.Reverse(directories);
                            Array.Reverse(files);
                            
                            foreach (string directory in directories) {
                                stack_DFS.Push((file_name, directory));
                            }
                            foreach (string file in files) {
                                stack_DFS.Push((file_name, file));
                            }
                        }
                    }
                } else {
                    if (!found) {
                        this.path_list.Add(Directory.GetParent(current_path).FullName + @"\");
                    }
                    if (!find_all_occurence && !found) {
                        found = true;
                        this.programRunTime.Stop();
                    }
                }
                list_graph.Add((parent_path, file_name));
            }
            if (this.programRunTime.IsRunning) {
                this.programRunTime.Stop();
            }
        }
        public string elapsedTime() {
            TimeSpan ts = programRunTime.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:000} seconds", ts.Seconds, ts.Milliseconds);
            return elapsedTime;
        }
    }
}
    