using System;
using System.Collections.Generic;
using System.IO;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.Notation;
using ExpertSystemTests.Parser;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class Tests
    {
        string startupPath = "/home/rrhaenys/RiderProjects/Expert-System-21";
        
        [TestCase("tests/_examples/good_files/and.txt", "C", "F")]
        [TestCase("tests/_examples/good_files/and_in_conclusions.txt", "FCDU", "")]
        [TestCase("tests/_examples/good_files/comments.txt", "CDF", "")]
        [TestCase("tests/_examples/good_files/double_implies.txt", "FCD", "")]
        [TestCase("tests/_examples/good_files/mix.txt", "G", "TX")]
        public void CheckFileParser(string filePath, string trueStates, string falseStates)
        {
            try
            {
                string[] lines = File.ReadAllLines(Path.Combine(startupPath, filePath));
                var parser = new FileParser(lines);
                var tree = new ESTree(parser);
                var results = tree.ResolveQuerys(parser.Queries);
                var check = true;
                foreach (var result in results)
                {
                    var inTrue = trueStates.Contains(result.Key.ToString());
                    var inFalse = falseStates.Contains(result.Key.ToString());
                    if (inTrue || inFalse)
                        check &= (result.Value == true && inTrue) || (result.Value == false && inFalse);
                }
                Assert.True(check);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + filePath + "(" + e + ")");
                Assert.True(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.True(false);
            }
        }
        
        [TestCase("tests/_examples/good_files/and.txt", "C", "F")]
        [TestCase("tests/_examples/good_files/and_in_conclusions.txt", "FCDU", "")]
        [TestCase("tests/_examples/good_files/comments.txt", "CDF", "")]
        [TestCase("tests/_examples/good_files/double_implies.txt", "FCD", "")]
        [TestCase("tests/_examples/good_files/mix.txt", "G", "TX")]
        public void CheckFileParserDebug(string filePath, string trueStates, string falseStates)
        {
            try
            {
                string[] lines = File.ReadAllLines(Path.Combine(startupPath, filePath));
                var parser = new FileParserWithAnswer(lines);
                var tree = new ESTree(parser);
                var results = tree.ResolveQuerys(parser.Queries);
                var check = true;
                foreach (var result in results)
                {
                    var inTrue = trueStates.Contains(result.Key.ToString());
                    var inFalse = falseStates.Contains(result.Key.ToString());
                    if (inTrue || inFalse)
                        check &= (result.Value == true && inTrue) || (result.Value == false && inFalse);
                }
                Assert.True(check);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + filePath + "(" + e + ")");
                Assert.True(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.True(false);
            }
        }
        
    }
}