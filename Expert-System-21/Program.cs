using System;
using System.Collections.Generic;
using System.IO;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.MyExtensions;
using ExpertSystemTests.Notation;
using ExpertSystemTests.Parser;

namespace ExpertSystemTests
{
	public class Program
	{
		public const string ProjectPath = "/home/rrhaenys/RiderProjects/Expert-System-21";

		static void Main()
		{
			var check = CheckFileParser(Path.Combine(ProjectPath, "tests/_examples/good_files/parenthesis.txt"), true);
			Console.ForegroundColor = check ? ConsoleColor.Green : ConsoleColor.Red;
			Console.WriteLine(ProjectPath + ": " + check + ". The end!");
			Console.ResetColor();;
		}
		
		public static bool CheckFileParser(string filePath, bool debugMode = false)
		{
			try
			{
				string[] lines = File.ReadAllLines(filePath);
				FileParser parser = debugMode ? new FileParserWithAnswer(lines) : new FileParser(lines);
				var tree = new ESTree(parser);
				var results = tree.ResolveQuerys(parser.Queries);
				var check = true;
				foreach (var result in results)
				{
					Console.WriteLine(result.Key + " : " + result.Value);
					if (debugMode)
					{
						var inTrue = ((FileParserWithAnswer) parser).ExpectedTrueResults.Contains(result.Key);
						var inFalse = ((FileParserWithAnswer) parser).ExpectedFalseResults.Contains(result.Key);
						if (inTrue || inFalse)
							check &= (result.Value == true && inTrue) || (result.Value == false && inFalse);
					}
				}
				return check;
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("FileNotFoundException: " + filePath);
				throw new FileNotFoundException("FileNotFoundException", e);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw new Exception("Exception", e);
			}
		}

		private static void CheckStringPreprocessing(string input, string expectedString)
		{
			var inputCopy = input.PostProcess();
			Console.WriteLine(string.Equals(inputCopy, expectedString) + "\t" + input + " => " + inputCopy + " (" + expectedString + ")");
		}

		static void CheckNotation(ReversePolishNotation notation, string input, string expectedString, bool expectedResult)
		{
			var inputCopy = input.PostProcess();
			string notationResult = notation.Convert(inputCopy);
			Console.WriteLine(string.Equals(notationResult, expectedString) + "\t" + input + " => " + notationResult + " (" + expectedString + ")");
		}
	}
}
