using System;
using ExpressionEvaluator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expression.Tests
{
    [TestClass]
    public class ConditionalExpressionTests
    {
        private readonly ConditionExpression _evaluator = new ConditionExpression();
        
        [TestMethod]
        public void TestBasicStringComparison()
        {
            var name = "scott";
            Assert.AreEqual(_evaluator.Evaluate("scott == scott"),name.Equals("scott") );
        }
        [TestMethod]
        public void TestBasicNumberComparison()
        {
            Assert.AreEqual(_evaluator.Evaluate("2==3"), 2==3);
        }
        [TestMethod]
        public void TestGreaterThanComparison()
        {
            Assert.AreEqual(_evaluator.Evaluate("2>3"), 2 > 3);
        }
        [TestMethod]
        public void TestGreaterThanEqualComparison()
        {
            Assert.AreEqual(_evaluator.Evaluate("2>=3"), 2 >= 3);
        }
        [TestMethod]
        public void TestLessThanComparison()
        {
            Assert.AreEqual(_evaluator.Evaluate("2<3"), 2 < 3);
        }
        [TestMethod]
        public void TestLessThanEqualComparison()
        {
            Assert.AreEqual(_evaluator.Evaluate("2<=3"), 2 <= 3);
        }
         [TestMethod]
        public void TestParanthesesComparison()
        {
            Assert.AreEqual(_evaluator.Evaluate("((2<=3)&&(3>=2))"), ((2<=3)&&(3>=2)));
        }
         [TestMethod]
         public void TestParanthesesComparisonNotEqual()
         {
             Assert.AreNotEqual(_evaluator.Evaluate("(2<=3)&&(3<=2)"),(2 <= 3) && (3 >= 2));
         }
         [TestMethod]
         public void TestMultipleParanthesesComparison()
         {
             Assert.AreEqual(_evaluator.Evaluate("((1>2||4>3)&&(scott==scott))"), ((1 > 2 || 4 > 3) && ("scott" == "scott")));
         }
    }
}
