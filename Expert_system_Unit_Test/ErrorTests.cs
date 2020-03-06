using System;
using System.IO;
using Expert_System_21.Exceptions;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class ErrorTests
    {
        [TestCase(typeof(FileNotFoundException), "tests/nofile")]
        [TestCase(typeof(NodeConflictException), "tests/_examples/good_files/error_0.txt")]
        [TestCase(typeof(NodeConflictException), "tests/_examples/good_files/error_1.txt")]
        [TestCase(typeof(NodeConflictException), "tests/_examples/good_files/NEGATION_SIMPLE_2")]
        [TestCase(typeof(NodeConflictException), "tests/_examples/good_files/raise_me_daddy.txt")]
        
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