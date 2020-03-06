using System;
using System.IO;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class ErrorTests
    {
        [TestCase(typeof(FileNotFoundException), "tests/nofile")]
        [TestCase(typeof(Exception), "tests/_examples/good_files/error_0.txt")]
        [TestCase(typeof(Exception), "tests/_examples/good_files/error_1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/good_files/NEGATION_SIMPLE_2")]
        [TestCase(typeof(Exception), "tests/_examples/good_files/raise_me_daddy.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/and1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/and2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/and3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion4.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion5.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion6.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion7.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion8.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion9.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion10.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion11.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion12.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion13.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion14.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion15.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion16.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion17.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion18.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion19.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion20.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion21.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion22.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_conclusion23.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_implies1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_implies2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_implies3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_implies4.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_initial_facts1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_initial_facts2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_initial_facts3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_initial_facts4.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_query1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_query2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_query3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule4.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule5.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule6.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/bad_rule7.txt")]
        [TestCase(typeof(ArgumentNullException), "tests/_examples/bad_files/Crash_test_1")]
        [TestCase(typeof(ArgumentNullException), "tests/_examples/bad_files/Crash_test_2")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/Crash_test_3")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/Crash_test_4")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/double_init.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/empty_file.sssss")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/empty_file_test.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/invalid_chars_test.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/INVALID_FOR_TESTS_1")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/INVALID_FOR_TESTS_2")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/nl")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/no_init_test.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/no_query_test.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/no_rules_test.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/not1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/not2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/not3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/or1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/or2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/or3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/or4.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis10.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis11.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis12.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis3.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis4.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis5.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis6.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis7.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis8.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/parenthesis9.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/STRANGE_OUTPUT_1")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/STRANGE_OUTPUT_2")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/test_abbensid1")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/xor1.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/xor2.txt")]
        [TestCase(typeof(Exception), "tests/_examples/bad_files/xor3.txt")]
        
        public void RunErrorFileNotFoundException(Type expectedExceptionType, string filePath)
        {
            Assert.Throws(expectedExceptionType, () =>
                {
                    string path = Path.Combine(Expert_System_21.Program.ProjectPath, filePath);
                    var res = Expert_System_21.Program.CheckFileParser(path);
                }
                );
        }
    }
}