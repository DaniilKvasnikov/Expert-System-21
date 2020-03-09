using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Expert_System_21.Logs;
using Expert_System_21.Parser;
using Expert_System_21.Visualizer;


namespace Expert_System_21
{
	public static class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
				{
					CheckFileParser(options.FileName, options.DebugMode, options.Visualisation);
				});
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	
		public static bool CheckFileParser(string filePath, bool debugMode = false, bool graphVisualise = false)
		{
			if (filePath == null) throw new Exception("filePath must be not null!");
			var lines = File.ReadAllLines(filePath);
			var parser = debugMode ? new FileParserWithAnswer(lines) : new FileParser(lines);
			var tree = new ExpertSystemTree(parser);
			Dictionary<char, bool?> results = tree.ResolveQuerys(parser.Queries);
			bool result = !debugMode || CheckResults(results, (FileParserWithAnswer) parser);
			LogResult.PrintResults(results, result);
			if (graphVisualise)
			{
				new GraphVisualizer(tree, parser);
			}

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
