using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FolderCrawler
{
    
    internal class Program
    {
        private string find_file {get; set;}
        private string root_path {get; set;}

        public Program() {
            this.find_file = null;
            this.root_path = null;
        }
        public Program(string find_file, string root_path) {
            this.find_file = find_file;
            this.root_path = root_path;
        }
        public void BFS()
        {
            Boolean found = false;
            this.find_file = "hehe.txt";
            Queue<string> queue_BFS = new Queue<string>();
            // Input path disini
            this.root_path = @"D:\Video Recording";
            string[] directories = Directory.GetDirectories(this.root_path);
            string[] files = Directory.GetFiles(this.root_path);
            foreach (string directory in directories)
            {
                queue_BFS.Enqueue(directory);
            }
            foreach (string file in files)
            {
                queue_BFS.Enqueue(file);
            }

            while (queue_BFS.Count > 0 && !found)
            {
                string current_file = queue_BFS.Dequeue();
                string file_name = Path.GetFileName(current_file);
                
                Console.WriteLine(file_name);

                if (Directory.Exists(current_file))
                {
                    directories = Directory.GetDirectories(current_file);
                    files = Directory.GetFiles(current_file);
                    foreach (string directory in directories)
                    {
                        queue_BFS.Enqueue(directory);
                    }
                    foreach (string file in files)
                    {
                        queue_BFS.Enqueue(file);
                    }
                }
                else if (file_name == this.find_file)
                {
                    found = true;
                }
            }
            
        }

        public void DFS() {
            Boolean found = false;
            Boolean take = false;

            this.find_file = "Main.cs";
            List<string> list_Simpul = new List<string>();
            Stack<string> stack_DFS = new Stack<string>();

            this.root_path = @"E:\Stanley\Semester 4\Stima\Tubes\TubesStima2\Test";

            stack_DFS.Push(this.root_path);
            list_Simpul.Add(this.root_path);

            string[] directories = Directory.GetDirectories(this.root_path);
            string[] files = Directory.GetFiles(this.root_path);

            string current_file = null;
            string file_taken = null;

            while (stack_DFS.Count > 0 && !found) {
                current_file = stack_DFS.Peek();
                Console.WriteLine("1 " + current_file); //bisa jdi bagian visualisasi                
                string file_name = Path.GetFileName(current_file);
                if (file_name == this.find_file) {
                    found = true;
                } else {
                    take = false;
                    if (Directory.Exists(current_file)) {
                        directories = Directory.GetDirectories(current_file);
                        files = Directory.GetFiles(current_file);
                        foreach (string directory in directories) {
                            if (list_Simpul.IndexOf(directory) == -1 && !take) {
                                file_taken = directory;
                                stack_DFS.Push(file_taken);    
                                list_Simpul.Add(file_taken);
                                take = true;
                                break;
                            }
                        }

                        foreach (string file in files) {    
                            if (list_Simpul.IndexOf(file) == -1 && !take) {
                                file_taken = file;
                                stack_DFS.Push(file_taken);
                                list_Simpul.Add(file_taken);
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
        // if (found) {
        //     Console.WriteLine(find_file + " ditemukan di ");
        // } 
        }    


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
