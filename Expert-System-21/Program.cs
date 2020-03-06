using System;
using System.Collections.Generic;
using System.IO;
using Expert_System_21.Exceptions;
using Expert_System_21.Parser;
using ExpertSystemTests.ExpertSystem.Log;


namespace Expert_System_21
{
	public static class Program
	{
		public const string ProjectPath = "/home/rrhaenys/RiderProjects/Expert-System-21";

		private static void Main()
		{
			CheckFileParser(Path.Combine(ProjectPath, "tests/_examples/good_files/empty_init_test.txt"), true);
		}
	
		public static bool CheckFileParser(string filePath, bool debugMode = false)
		{
			try
			{
				var lines = File.ReadAllLines(filePath);
				var parser = debugMode ? new FileParserWithAnswer(lines) : new FileParser(lines);
				var tree = new ESTree(parser);
				Dictionary<char, bool?> results = tree.ResolveQuerys(parser.Queries);
				bool result = !debugMode || CheckResults(results, (FileParserWithAnswer) parser);
				Log.PrintResults(results, result);
				return result;
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("FileNotFoundException: " + filePath);
				throw new FileNotFoundException("FileNotFoundException", e);
			}
			catch (NodeConflictException e)
			{
				Console.WriteLine(e);
				throw new NodeConflictException("FileNotFoundException");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw new Exception("Exception", e);
			}
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
