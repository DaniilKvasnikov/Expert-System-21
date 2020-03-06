using System.IO;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class Tests
    {
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
            string path = Path.Combine(Expert_System_21.Program.ProjectPath, filePath);
            var res = Expert_System_21.Program.CheckFileParser(path, false);
            Assert.True(res);
        }
    }
}