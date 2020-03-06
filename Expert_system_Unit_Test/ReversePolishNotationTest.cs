﻿using System;
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
            string notationResult = notation.Convert(inputCopy);
            Assert.True(string.Equals(notationResult, expectedString));
        }

    }
}