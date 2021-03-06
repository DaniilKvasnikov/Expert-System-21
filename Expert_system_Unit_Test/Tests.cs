﻿using Expert_System_21;
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
        [TestCase("tests/_examples/good_files/AND_LIST")]
        [TestCase("tests/_examples/good_files/AND_OR")]
        [TestCase("tests/_examples/good_files/BI_IF")]
        [TestCase("tests/_examples/good_files/easy_test.txt")]
        [TestCase("tests/_examples/good_files/empty_init_test.txt")]
        [TestCase("tests/_examples/good_files/HAfffff_.txt")]
        [TestCase("tests/_examples/good_files/hard_imply_2.txt")]
        [TestCase("tests/_examples/good_files/HARDDDDDER_.txt")]
        [TestCase("tests/_examples/good_files/imply_and.txt")]
        [TestCase("tests/_examples/good_files/just_a_test.txt")]
        [TestCase("tests/_examples/good_files/just_a_test2.txt")]
        [TestCase("tests/_examples/good_files/NEGATION_SIMPLE_1")]
        [TestCase("tests/_examples/good_files/NEGATION_SIMPLE_3")]
        [TestCase("tests/_examples/good_files/NEGATION_SIMPLE_4")]
        [TestCase("tests/_examples/good_files/parentheses_test.txt")]
        [TestCase("tests/_examples/good_files/test_blyat.txt")]
        [TestCase("tests/_examples/good_files/test_blyat1.txt")]
        [TestCase("tests/_examples/good_files/test_neg_3333.txt")]
        [TestCase("tests/_examples/good_files/test_not")]
        [TestCase("tests/_examples/good_files/test_parents_priority.txt")]
        [TestCase("tests/_examples/good_files/test_parents_priority2.txt")]
        [TestCase("tests/_examples/bad_files/INVALID_FOR_TESTS_2")]
        public void CheckFileParserDebug(string filePath)
        {
            var res = Program.CheckFileParser(filePath, true);
            Assert.True(res);
        }

        [TestCase("tests/schoolTests/01")]
        [TestCase("tests/schoolTests/02")]
        [TestCase("tests/schoolTests/03")]
        [TestCase("tests/schoolTests/04")]
        [TestCase("tests/schoolTests/05")]
        [TestCase("tests/schoolTests/06")]
        [TestCase("tests/schoolTests/07")]
        [TestCase("tests/schoolTests/08")]
        [TestCase("tests/schoolTests/09")]
        [TestCase("tests/schoolTests/10")]
        [TestCase("tests/schoolTests/11")]
        [TestCase("tests/schoolTests/12")]
        [TestCase("tests/schoolTests/13")]
        [TestCase("tests/schoolTests/14")]
        [TestCase("tests/schoolTests/15")]
        [TestCase("tests/schoolTests/16")]
        [TestCase("tests/schoolTests/17")]
        [TestCase("tests/schoolTests/18")]
        [TestCase("tests/schoolTests/19")]
        [TestCase("tests/schoolTests/20")]
        [TestCase("tests/schoolTests/21")]
        [TestCase("tests/schoolTests/22")]
        [TestCase("tests/schoolTests/23")]
        [TestCase("tests/schoolTests/24")]
        [TestCase("tests/schoolTests/25")]
        [TestCase("tests/schoolTests/26")]
        [TestCase("tests/schoolTests/27")]
        [TestCase("tests/schoolTests/28")]
        [TestCase("tests/schoolTests/29")]
        [TestCase("tests/schoolTests/37")]
        [TestCase("tests/schoolTests/38")]
        public void SchoolTest(string filePath)
        {
            var res = Program.CheckFileParser(filePath);
            Assert.True(res);
        }

        [TestCase("tests/schoolTests/38")]
        [TestCase("tests")]
        public void ArgsTest(string filePath)
        {
            var args = new[] {"-f", filePath, "-l", "-d", "-v"};
            Program.Main(args);
        }
    }
}