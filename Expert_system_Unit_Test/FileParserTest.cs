using System;
using Expert_System_21.Parser;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class FileParserTest
    {
        [TestCase(typeof(Exception), null)]
        [TestCase(typeof(Exception), new[] {""})]
        [TestCase(typeof(Exception), new[] {"A + B => C"})]
        [TestCase(typeof(Exception), new[] {"=A"})]
        [TestCase(typeof(Exception), new[] {"?C"})]
        [TestCase(typeof(Exception), new[] {"A + B => C", "=A"})]
        [TestCase(typeof(Exception), new[] {"A + B => C", "=A", "B + A => C"})]
        [TestCase(typeof(Exception), new[] {"A + B => C", "=A", "?C", "B + A => C"})]
        [TestCase(typeof(Exception), new[] {"A + B => C", "?C"})]
        [TestCase(typeof(Exception), new[] {"=A", "?C"})]
        public void InputErrorTest(Type expectedExceptionType, string[] lines)
        {
            Assert.Throws(expectedExceptionType, () => { new FileParser(lines); });
        }
    }
}