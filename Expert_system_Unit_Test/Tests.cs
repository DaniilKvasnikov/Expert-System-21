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
        string startupPath = @"C:\Users\Labs_09\RiderProjects\Expert-System-21";
        
        [TestCase("tests/_examples/good_files/and.txt", "C", "F")]
        [TestCase("tests/_examples/good_files/and_in_conclusions.txt", "FCDU", "")]
        [TestCase("tests/_examples/good_files/comments.txt", "CDF", "")]
        [TestCase("tests/_examples/good_files/double_implies.txt", "FCD", "")]
        [TestCase("tests/_examples/good_files/mix.txt", "G", "TX")]
        public void CheckFileParser(string filePath, string trueStates, string falseStates)
        {
            try
            {
                string path = Path.Combine(startupPath, filePath);
                string[] lines = File.ReadAllLines(path);
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
        
        [TestCase("tests/_examples/good_files/and.txt")]
        [TestCase("tests/_examples/good_files/and_in_conclusions.txt")]
        [TestCase("tests/_examples/good_files/comments.txt")]
        [TestCase("tests/_examples/good_files/double_implies.txt")]
        [TestCase("tests/_examples/good_files/mix.txt")]
        [TestCase("tests/_examples/good_files/mix2.txt")]
        [TestCase("tests/_examples/good_files/multiple_initial_facts.txt")]
        [TestCase("tests/_examples/good_files/multiple_initial_facts2.txt")]
        [TestCase("tests/_examples/good_files/multiple_initial_facts3.txt")]
        [TestCase("tests/_examples/good_files/multiple_initial_facts4.txt")]
        [TestCase("tests/_examples/good_files/multiple_initial_facts5.txt")]
        [TestCase("tests/_examples/good_files/multiple_initial_facts6.txt")]
        [TestCase("tests/_examples/good_files/no_initial_facts1.txt")]
        [TestCase("tests/_examples/good_files/no_initial_facts2.txt")]
        [TestCase("tests/_examples/good_files/not.txt")]
        [TestCase("tests/_examples/good_files/or.txt")]
        [TestCase("tests/_examples/good_files/parenthesis.txt")]
        [TestCase("tests/_examples/good_files/xor.txt")]
        [TestCase("tests/_correction/test_and1")]
        [TestCase("tests/_correction/test_and2")]
        [TestCase("tests/_correction/test_or1")]
        [TestCase("tests/_correction/test_or2")]
        [TestCase("tests/_correction/test_or3")]
        [TestCase("tests/_correction/test_or4")]
        [TestCase("tests/_correction/test_paran1")]
        [TestCase("tests/_correction/test_paran2")]
        [TestCase("tests/_correction/test_paran3")]
        [TestCase("tests/_correction/test_paran4")]
        [TestCase("tests/_correction/test_paran5")]
        [TestCase("tests/_correction/test_paran6")]
        [TestCase("tests/_correction/test_paran7")]
        [TestCase("tests/_correction/test_paran8")]
        [TestCase("tests/_correction/test_paran9")]
        [TestCase("tests/_correction/test_paran10")]
        [TestCase("tests/_correction/test_paran11")]
        [TestCase("tests/_correction/test_same1")]
        [TestCase("tests/_correction/test_same2")]
        [TestCase("tests/_correction/test_same3")]
        [TestCase("tests/_correction/test_same4")]
        [TestCase("tests/_correction/test_xor1")]
        [TestCase("tests/_correction/test_xor2")]
        [TestCase("tests/_correction/test_xor3")]
        [TestCase("tests/_correction/test_xor4")]
        [TestCase("tests/_correction/test_neg1")]
        [TestCase("tests/_correction/test_neg2")]
        [TestCase("tests/_correction/test_neg3")]
        [TestCase("tests/_correction/test_neg4")]
        [TestCase("tests/_correction/test_neg5")]
        [TestCase("tests/_examples/good_files/test_slack1")]
        [TestCase("tests/_examples/good_files/test_slack2")]
        [TestCase("tests/_examples/good_files/test_slack3")]
        [TestCase("tests/_examples/good_files/test_slack4")]
        public void CheckFileParserDebug(string filePath)
        {
            try
            {
                string path = Path.Combine(startupPath, filePath);
                string[] lines = File.ReadAllLines(path);
                var parser = new FileParserWithAnswer(lines);
                var tree = new ESTree(parser);
                var results = tree.ResolveQuerys(parser.Queries);
                var check = true;
                foreach (var result in results)
                {
                    var inTrue = parser.ExpectedTrueResults.Contains(result.Key);
                    var inFalse = parser.ExpectedFalseResults.Contains(result.Key);
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