using System;
using System.Collections.Generic;
using System.IO;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.Notation;
using ExpertSystemTests.Parser;
using ExpertSystemTests.Preprocessing;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ReversePolishNotationTest()
        {
            var notation = new ReversePolishNotation();
            Console.WriteLine("CheckStringPreprocessing");
            CheckStringPreprocessing("A+B", "A+B");
            CheckStringPreprocessing("A+B#123", "A+B");
            CheckStringPreprocessing("A#+B", "A");
            CheckStringPreprocessing("#A+B", "");
            
            Console.WriteLine("");
            Console.WriteLine("CheckStringPreprocessing");
            CheckNotation(notation, "A+B+Z", "ABZ++", true);
            CheckNotation(notation, "(A+B)+Z", "AB+Z+", true);
            CheckNotation(notation, "A|B+Z", "ABZ+|", true);
            CheckNotation(notation, "!(A|B)", "AB|!", true);
            CheckNotation(notation, "!(A|B)#123", "AB|!", true);
            CheckNotation(notation, "#!(A|B)", "", false);
        }
        
        private void CheckStringPreprocessing(string input, string expectedString)
        {
            var inputCopy = StringPreprocessing.GetString(input);
            Assert.True(string.Equals(inputCopy, expectedString));
        }

        private void CheckNotation(ReversePolishNotation notation, string input, string expectedString, bool expectedResult)
        {
            var inputCopy = StringPreprocessing.GetString(input);
            string notationResult = notation.Convert(inputCopy);
            Assert.True(string.Equals(notationResult, expectedString));
        }
        
        [Test]
        public void Test1()
        {
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
                Assert.True(check);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}