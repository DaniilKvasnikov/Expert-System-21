using System;
using System.Collections.Generic;
using System.IO;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.MyExtensions;
using ExpertSystemTests.Notation;
using ExpertSystemTests.Parser;

namespace ExpertSystemTests
{
	class Program
	{
		static void Main()
		{
			CheckFileParser("../../../tests/_examples/good_files/parenthesis.txt", "C", "F", true);
		}

		private static void CheckFileParser(string filePath, string trueStates, string falseStates, bool debugMode = false)
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
					// Console.WriteLine(result.Key + " : " + result.Value);
					var inTrue = trueStates.Contains(result.Key.ToString());
					var inFalse = falseStates.Contains(result.Key.ToString());
					if (inTrue || inFalse)
						check &= (result.Value == true && inTrue) || (result.Value == false && inFalse);
				}
				Console.ForegroundColor = check ? ConsoleColor.Green : ConsoleColor.Red;
				Console.WriteLine(filePath + ": " + check + ". The end!");
				Console.ResetColor();
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("FileNotFoundException: " + filePath);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
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
