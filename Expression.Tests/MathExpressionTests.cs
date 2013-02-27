using System;
using ExpressionEvaluator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expression.Tests
{
    [TestClass]
    public class MathExpressionTests
    {
        private readonly MathExpression _evaluator = new MathExpression();
        [TestMethod]
        public void TestBasicAddition()
        {
            Assert.AreEqual(_evaluator.Evaluate("5+3"), 5+3);
        }
        [TestMethod]
        public void TestBasicMultiplication()
        {
            Assert.AreEqual(_evaluator.Evaluate("5*3"), 5 * 3);
        }
        [TestMethod]
        public void TestBasicDivision()
        {
            Assert.AreEqual(_evaluator.Evaluate("21/3"), 21/3);
        }
        [TestMethod]
        public void TestBasicModulo()
        {
            Assert.AreEqual(_evaluator.Evaluate("10%2"), 10%2);
        }
        [TestMethod]
        public void TestParentheses()
        {
            Assert.AreEqual(_evaluator.Evaluate("(34+9)*8+7"), (34 + 9) * 8 + 7);
        }
        [TestMethod]
        public void TestParentheses2()
        {
            Assert.AreEqual(_evaluator.Evaluate("(34+9)*(8+7)"), (34 + 9) * (8 + 7));
        }
        [TestMethod]
        public void TestNested()
        {

            Assert.AreEqual(_evaluator.Evaluate("(((36-6/2)*2-9)/11-9-3)/(12+44/(77-55))"), (((36.0 - 6.0 / 2.0) * 2.0 - 9.0) / 11.0 - 9.0 - 3.0) / (12.0 + 44.0 / (77.0 - 55.0)));
        }
    }
}
