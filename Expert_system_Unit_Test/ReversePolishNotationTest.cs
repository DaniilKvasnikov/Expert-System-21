using System;
using Expert_System_21.MyExtensions;
using Expert_System_21.Notation;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    public class ReversePolishNotationTest
    {
        [TestCase("A+B", "A+B")]
        [TestCase("A+B#123", "A+B")]
        [TestCase("A#+B", "A")]
        [TestCase("#A+B", "")]
        public void CheckStringPreprocessing(string input, string expectedString)
        {
            var inputCopy = input.PreProcess();
            Assert.True(string.Equals(inputCopy, expectedString));
        }

        [TestCase("A+B+Z", "ABZ++")]
        [TestCase("(A+B)+Z", "AB+Z+")]
        [TestCase("A|B+Z", "ABZ+|")]
        [TestCase("!(A|B)", "AB|!")]
        [TestCase("!(A|B)", "AB|!")]
        [TestCase("#!(A|B)", "")]
        public void CheckNotation(string input, string expectedString)
        {
            var notation = new ReversePolishNotation();
            var inputCopy = input.PreProcess();
            var notationResult = notation.Convert(inputCopy);
            Assert.True(string.Equals(notationResult, expectedString));
        }

        [TestCase('A', CharType.Fact)]
        [TestCase('+', CharType.Operation)]
        [TestCase('!', CharType.PrefixOperation)]
        [TestCase('(', CharType.OpeningBracket)]
        [TestCase(')', CharType.ClosingBracket)]
        [TestCase('?', CharType.Error)]
        public void TestGetType(char c, CharType type)
        {
            var notation = new ReversePolishNotation();
            Assert.True(Equals(notation.GetType(c), type));
        }

        [TestCase(typeof(Exception), null)]
        [TestCase(typeof(Exception), "")]
        [TestCase(typeof(Exception), "a + B => C")]
        public void TestNull(Type expectedExceptionType, string str)
        {
            var notation = new ReversePolishNotation();
            var newString = str;
            Assert.Throws(expectedExceptionType,
                () => notation.GetNextChar(ref newString, out _));
        }
    }
}