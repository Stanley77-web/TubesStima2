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

            // Membersihkan graf dan list direktori hasil pencarian sebelumnya
            this.path_list.Clear();
            this.list_graph.Clear();
            this.file_count.Clear();

            // Mencari semua file dan folder dalam direktori root 
            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);

            // Menambahkan semua file dan folder ke dalam queue BFS
            foreach (string directory in directories) {
                queue_BFS.Enqueue((root_path, directory));
            }
            foreach (string file in files) {
                queue_BFS.Enqueue((root_path, file));
            }

            // start stopwatch
            this.programRunTime.Start();
            // Melakukan iterasi queue BFS sampai habis untuk skema BFS
            while (queue_BFS.Count > 0) {
                (string, string) current_queue = queue_BFS.Dequeue();
                string parent_path = current_queue.Item1;
                string current_path = current_queue.Item2;
                string file_name = Path.GetFileName(current_path);
                string temp_name = file_name; // Menyimpan sementara nama file yang akan dicek 

	            // Jika terdapat nama file/folder yang sudah ada sebelumnya, akan dibuat nama baru 
                if (!file_count.ContainsKey(file_name)) {
                    file_count[file_name] = 0;
                } else {
                    file_count[file_name]++;
                    file_name += "->" + file_count[file_name];
                }

                if (temp_name != find_file) {
                    // Jika current_path merupakan folder, tambahkan semua 
                    // file/folder (tetangga) dalam folder tersebut ke dalam
                    // queue BFS 
                    if (!found) {
                        if (Directory.Exists(current_path)) {
                            // Mencari semua file dan folder dalam direktori yang dipilih 
                            directories = Directory.GetDirectories(current_path);
                            files = Directory.GetFiles(current_path);

                            // Menambahkan semua file dan folder ke dalam queue BFS
                            foreach (string directory in directories) {
                                queue_BFS.Enqueue((file_name, directory));
                            }
                            foreach (string file in files) {
                                queue_BFS.Enqueue((file_name, file));
                            } 
                        }
                    }
                } else {
                    // Jika nama file sesuai dengan input query, direktori file
                    // tersebut dimasukkan ke dalam path_list 
                    if (!found) {
                        this.path_list.Add(Directory.GetParent(current_path).FullName + @"\");
                    }
                    // Jika pemilihan pencarian adalah 1 file saja, runtime
                    // dihentikan 
                    if (!find_all_occurence && !found) {
                        found = true;
                        this.programRunTime.Stop();
                    }
                }
                // Menambahkan file/folder dan direktorinya ke dalam list_graph 	
                // untuk penggambaran graf 
                list_graph.Add((parent_path, file_name));
            }
            // Jika pemilihan pencarian adalah all find occurance runtime 
            // dihentikan ketika queue BFS sudah habis seluruhnya
            if (this.programRunTime.IsRunning) {
                this.programRunTime.Stop();
            }
        }

        public void DFS (bool find_all_occurence) {
            Boolean found = false;
            Stack<(string, string)> stack_DFS = new();

            // Membersihkan graf dan list direktori hasil pencarian sebelumnya
            this.path_list.Clear();
            this.list_graph.Clear();
            this.file_count.Clear();

            // Mencari semua file dan folder dalam direktori root
            string[] directories = Directory.GetDirectories(root_path);
            string[] files = Directory.GetFiles(root_path);

            // Membalikan urutan folder dan file menjadi z-a dari a-z
            Array.Reverse(directories);
            Array.Reverse(files);
        
            // Menambahkan semua file dan folder ke dalam stack antrian DFS
            foreach (string file in files) {
                stack_DFS.Push((root_path, file));
            }
            foreach (string directory in directories) {
                stack_DFS.Push((root_path, directory));
            }

            // start stopwatch
            this.programRunTime.Start();
            // Melakukan iterasi stack DFS sampai habis untuk skema DFS
            while (stack_DFS.Count > 0) {
                (string, string) current_stack = stack_DFS.Pop();
                string parent_path = current_stack.Item1;
                string current_path = current_stack.Item2;
                string file_name = Path.GetFileName(current_path);
                string temp_name = file_name; // Menyimpan sementara nama file yang akan dicek 

	            // Jika terdapat nama file/folder yang sudah ada sebelumnya, akan dibuat nama baru
                file_count[file_name] = 
                    !file_count.ContainsKey(file_name) ? 0 : file_count[file_name] + 1;
                file_name = 
                    file_count[file_name] == 0 ? file_name : file_name + "->" + file_count[file_name];  

                if (temp_name == find_file) {
                    // Jika nama file sesuai dengan input query, direktori file
                    // tersebut dimasukkan ke dalam path_list 
                    if (!found) {
                        this.path_list.Add(Directory.GetParent(current_path).FullName + @"\");
                    }
                    // Jika pemilihan pencarian adalah 1 file saja, runtime
                    // dihentikan 
                    if (!find_all_occurence && !found) {
                        found = true;
                        this.programRunTime.Stop();
                    }
                } else {
                    // Jika current_path merupakan folder, tambahkan semua 				
                    // file/folder (tetangga) dalam folder tersebut ke dalam
                    // stack DFS (file/folder yang dimasukkan menggunakan
                    // aturan yang sama dengan sebelumnya
                    if (!found) {
                        if (Directory.Exists(current_path)) {
                            // Mencari semua file dan folder dalam direktori root
                            directories = Directory.GetDirectories(current_path);
                            files = Directory.GetFiles(current_path);
                            // Membalikan urutan folder dan file menjadi z-a dari a-z
                            Array.Reverse(directories);
                            Array.Reverse(files);

                            // Menambahkan semua file dan folder ke dalam stack antrian DFS
                            foreach (string file in files) {
                                stack_DFS.Push((file_name, file));
                            }
                            foreach (string directory in directories) {
                                stack_DFS.Push((file_name, directory));
                            }
                        }
                    }
                }
                // Menambahkan file/folder dan direktorinya ke dalam list_graph 	
                // untuk penggambaran graf
                list_graph.Add((parent_path, file_name));
            }
            // Jika pemilihan pencarian adalah all find occurance runtime 
            // dihentikan ketika stack DFS sudah habis seluruhnya
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
    