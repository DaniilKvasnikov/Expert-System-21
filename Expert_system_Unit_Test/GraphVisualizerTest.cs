using Expert_System_21;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class GraphVisualizerTest
    {
        [TestCase("tests/_examples/good_files/and.txt")]
        public void TestGraph(string filePath)
        {
            var res = Program.CheckFileParser(filePath, true, true);
            Assert.True(res);
        }
    }
}