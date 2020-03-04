using System;
using System.Collections.Generic;
using System.IO;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.Notation;
using ExpertSystemTests.Parser;
using ExpertSystemTests.Preprocessing;

namespace ExpertSystemTests
{
	class Program
	{
		static void Main(string[] args)
		{
			//TODO: parser args
			
			var infos = new List<(string filename, string trueAnswer, string falseAnswer)>();
			infos.Add(("tests/_examples/good_files/and.txt", "C", "F"));
			infos.Add(("tests/_examples/good_files/and_in_conclusions.txt", "FCDU", ""));
			infos.Add(("tests/_examples/good_files/comments.txt", "CDF", ""));
			infos.Add(("tests/_examples/good_files/double_implies.txt", "FCD", ""));
			infos.Add(("tests/_examples/good_files/mix.txt", "G", "TX"));
			foreach (var info in infos)
			{
				CheckFileParser(info.filename, info.trueAnswer, info.falseAnswer);
			}
		}

		private static void CheckFileParser(string filePath, string trueStates, string falseStates)
		{
			try
			{
				string[] lines = File.ReadAllLines(filePath);
				var parser = new FileParser(lines);
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
			var inputCopy = StringPreprocessing.GetString(input);
			Console.WriteLine(string.Equals(inputCopy, expectedString) + "\t" + input + " => " + inputCopy + " (" + expectedString + ")");
		}

		static void CheckNotation(ReversePolishNotation notation, string input, string expectedString, bool expectedResult)
		{
			var inputCopy = StringPreprocessing.GetString(input);
			string notationResult = notation.Convert(inputCopy);
			Console.WriteLine(string.Equals(notationResult, expectedString) + "\t" + input + " => " + notationResult + " (" + expectedString + ")");
		}
	}
}
