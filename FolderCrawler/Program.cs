using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FolderCrawler
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        static public void BFS()
        {
            Boolean found = false;
            string find_file = "hehe.txt";
            Queue<string> queue_BFS = new Queue<string>();
            // Input path disini
            string path = @"D:\Video Recording";
            string[] directories = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
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
                if (file_name == find_file)
                {
                    found = true;
                }
                else if (Directory.Exists(current_file))
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
            }
        }

        public static void DFS() {
        Boolean found = false;
        Boolean take = false;

        string find_file = "Main.cs";
        List<string> list_Simpul = new List<string>();
        Stack<string> stack_DFS = new Stack<string>();

        string path = @"E:\Stanley\Semester 4\Stima\Tubes\TubesStima2\Test";

        stack_DFS.Push(path);
        list_Simpul.Add(path);

        string[] directories = Directory.GetDirectories(path);
        string[] files = Directory.GetFiles(path);

        string current_file = files[0];
        string file_taken = files[0];

        while (stack_DFS.Count > 0 && !found) {
            current_file = stack_DFS.Peek();
            // Console.WriteLine("1 " + current_file); bisa jdi bagian visualisasi                
            string file_name = Path.GetFileName(current_file);
            if (file_name == find_file) {
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
        // return list_Simpul;
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

