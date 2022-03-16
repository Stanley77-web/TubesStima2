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

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
