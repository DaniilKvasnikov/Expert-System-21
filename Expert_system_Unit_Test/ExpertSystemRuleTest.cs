using System;
using Expert_System_21.ExpertSystem;
using NUnit.Framework;

namespace Expert_system_Unit_Test
{
    [TestFixture]
    public class ExpertSystemRuleTest
    {
        [TestCase(typeof(Exception), "A => B => C")]
        public void ErrorLine(Type expectedExceptionType, string str)
        {
            Assert.Throws(expectedExceptionType, () => new ExpertSystemRule(str));
        }
    }
}