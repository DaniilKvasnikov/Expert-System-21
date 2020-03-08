using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Expert_System_21.Parser;
using Expert_System_21.Visualizer;
using ExpertSystemTests.ExpertSystem.Log;


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
				Console.WriteLine(e);
				throw;
			}
		}
	
		public static bool CheckFileParser(string filePath, bool debugMode = false, bool graphVisualise = false)
		{
			var lines = File.ReadAllLines(filePath);
			var parser = debugMode ? new FileParserWithAnswer(lines) : new FileParser(lines);
			var tree = new ESTree(parser);
			Dictionary<char, bool?> results = tree.ResolveQuerys(parser.Queries);
			bool result = !debugMode || CheckResults(results, (FileParserWithAnswer) parser);
			Log.PrintResults(results, result);
			if (graphVisualise)
			{
				PraphVisualizer visualizer = new PraphVisualizer(tree, results);
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
