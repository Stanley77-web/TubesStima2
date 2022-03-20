namespace FolderCrawling
{
  internal class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    private string find_file { get; set; }
    private string root_path { get; set; }

    public Program()
    {
      this.find_file = null;
      this.root_path = null;
    }

    public Program(string find_file, string root_path)
    {
      this.find_file = find_file;
      this.root_path = root_path;
    }

    public Microsoft.Msagl.Drawing.Graph BFS()
    {
      //create a form 
      System.Windows.Forms.Form form = new System.Windows.Forms.Form();
      //create a viewer object 
      Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
      //create a graph object 
      Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("BFS Search");


      Boolean found = false;
      //string find_file = "jemoy.txt";
      Dictionary<string, Microsoft.Msagl.Drawing.Edge> dict_BFS = new();
      Dictionary<string, int> file_count = new();
      Queue<(string, string)> queue_BFS = new();

      // Input path disini
      //string root_path = @"D:\Testing";
      string[] directories = Directory.GetDirectories(root_path);
      string[] files = Directory.GetFiles(root_path);
      graph.AddNode(root_path).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

      foreach (string directory in directories)
      {
        queue_BFS.Enqueue((root_path, directory));
      }

      foreach (string file in files)
      {
        queue_BFS.Enqueue((root_path, file));
      }

      while (queue_BFS.Count > 0)
      {
        (string, string) current_queue = queue_BFS.Dequeue();
        string parent_path = current_queue.Item1;
        string current_path = current_queue.Item2;
        string file_name = Path.GetFileName(current_path);
        if (file_name != find_file)
        {
          if (graph.FindNode(file_name) != null)
          {
            if (!file_count.ContainsKey(file_name))
            {
              file_count[file_name] = 1;
            }
            else
            {
              file_count[file_name]++;
            }
            file_name += " (" + file_count[file_name] + ")";
          }
          var node = graph.AddNode(file_name);
          var edge = graph.AddEdge(parent_path, file_name);
          if (!found)
          {
            node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
            edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
          }
          else
          {
            node.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
            edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
          }
          dict_BFS[file_name] = edge;

          if (Directory.Exists(current_path))
          {
            directories = Directory.GetDirectories(current_path);
            files = Directory.GetFiles(current_path);
            foreach (string directory in directories)
            {
              queue_BFS.Enqueue((file_name, directory));
            }
            foreach (string file in files)
            {
              queue_BFS.Enqueue((file_name, file));
            }
          }
        }
        else
        {
          graph.AddNode(file_name).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
          var edge = graph.AddEdge(parent_path, file_name);
          edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
          edge.SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
          while (dict_BFS.ContainsKey(parent_path))
          {
            dict_BFS[parent_path].Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            dict_BFS[parent_path].SourceNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            parent_path = dict_BFS[parent_path].Source;
          }
          found = true;
        }
      }
      //bind the graph to the viewer
      graph.Attr.BackgroundColor = new Microsoft.Msagl.Drawing.Color(byte.MaxValue, 192, 192, 192);
      return graph;
      //viewer.Graph = graph;
      //associate the viewer with the form 
      //form.SuspendLayout();
      //viewer.Dock = System.Windows.Forms.DockStyle.Fill;
      //form.Controls.Add(viewer);
      //form.ResumeLayout();
      //show the form 
      //form.ShowDialog();
    }


    public void DFS()
    {
      Boolean found = false;
      Boolean take = false;

      //this.find_file = "Main.cs";
      List<string> list_Simpul = new List<string>();
      Stack<string> stack_DFS = new Stack<string>();

      //this.root_path = @"E:\Stanley\Semester 4\Stima\Tubes\TubesStima2\Test";

      stack_DFS.Push(this.root_path);
      list_Simpul.Add(this.root_path);

      string[] directories = Directory.GetDirectories(this.root_path);
      string[] files = Directory.GetFiles(this.root_path);

      string current_file = null;
      string file_taken = null;

      while (stack_DFS.Count > 0 && !found)
      {
        current_file = stack_DFS.Peek();
        Console.WriteLine("1 " + current_file); //bisa jdi bagian visualisasi                
        string file_name = Path.GetFileName(current_file);
        if (file_name == this.find_file)
        {
          found = true;
        }
        else
        {
          take = false;
          if (Directory.Exists(current_file))
          {
            directories = Directory.GetDirectories(current_file);
            files = Directory.GetFiles(current_file);
            foreach (string directory in directories)
            {
              if (list_Simpul.IndexOf(directory) == -1 && !take)
              {
                file_taken = directory;
                stack_DFS.Push(file_taken);
                list_Simpul.Add(file_taken);
                take = true;
                break;
              }
            }

            foreach (string file in files)
            {
              if (list_Simpul.IndexOf(file) == -1 && !take)
              {
                file_taken = file;
                stack_DFS.Push(file_taken);
                list_Simpul.Add(file_taken);
                take = true;
                break;
              }
            }
          }
          if (!take)
          {
            stack_DFS.Pop();
          }
        }
      }
      // if (found) {
      //     Console.WriteLine(find_file + " ditemukan di ");
      // } 
    }

    //[STAThread]
    //static void Main()
    //{
    //To customize application configuration such as set high DPI settings or default font,
    //see https://aka.ms/applicationconfiguration.
    //ApplicationConfiguration.Initialize();
    //Application.Run(new Gui());
    //BFS();
    // }

  }
}