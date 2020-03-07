using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Expert_System_21.Forms;
using Expert_System_21.Parser;
using ExpertSystemTests.ExpertSystem.Log;


namespace Expert_System_21
{
	public static class Program
	{
		public const string ProjectPath = @"C:\Users\dima6\RiderProjects\Expert-System-21";

		private static void Main()
		{
			try
			{
				CheckFileParser(Path.Combine(ProjectPath, "tests/_examples/bad_files/no_init_test.txt"), true);
				
				//create a form 
				Form form = new Form();
				//create a viewer object 
				Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
				//create a graph object 
				Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
				
				//create the graph content 
				graph.AddEdge("A", "B");
				graph.AddEdge("B", "C");
				graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
				graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
				graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
				
				Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
				c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
				c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
				
				//bind the graph to the viewer 
				viewer.Graph = graph;
				//associate the viewer with the form 
				form.SuspendLayout();
				viewer.Dock = System.Windows.Forms.DockStyle.Fill;
				form.Controls.Add(viewer);
				form.ResumeLayout();
				//show the form 
				form.ShowDialog();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	
		public static bool CheckFileParser(string filePath, bool debugMode = false)
		{
			var lines = File.ReadAllLines(filePath);
			var parser = debugMode ? new FileParserWithAnswer(lines) : new FileParser(lines);
			var tree = new ESTree(parser);
			Dictionary<char, bool?> results = tree.ResolveQuerys(parser.Queries);
			bool result = !debugMode || CheckResults(results, (FileParserWithAnswer) parser);
			Log.PrintResults(results, result);
			return result;
		}

		private static bool CheckResults(Dictionary<char, bool?> results, FileParserWithAnswer parser)
		{
			var check = true;
			foreach (var result in results)
			{
				var inTrue = parser.ExpectedTrueResults.Contains(result.Key);
				var inFalse = parser.ExpectedFalseResults.Contains(result.Key);
				if (inTrue || inFalse)
				{
					check &= (result.Value == true && inTrue) || (result.Value == false && inFalse);
				}
			}

			return check;
		}
	}
}
