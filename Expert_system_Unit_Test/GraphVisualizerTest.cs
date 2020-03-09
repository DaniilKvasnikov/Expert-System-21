﻿using Expert_System_21;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class GraphVisualizerTest
    {
        [TestCase("tests/schoolTests/01")]
        public void TestGraph(string filePath)
        {
            var res = Program.CheckFileParser(filePath, graphVisualise: true);
            Assert.True(res);
        }
    }
}