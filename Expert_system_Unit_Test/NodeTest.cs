using System;
using Expert_System_21.Nodes;
using Expert_System_21.Type;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class NodeTest
    {
        [Test]
        public void AddOperandException()
        {
            var rootNode = new ConnectorNode(ConnectorType.IMPLY);
            rootNode.AddOperand(new AtomNode('A'));
            Assert.Throws(typeof(Exception), () => rootNode.AddOperand(new AtomNode('B')));
        }

        [Test]
        public void NegativeNodeNull()
        {
            Assert.Throws(typeof(Exception), () => new NegativeNode(null));
        }
    }
}