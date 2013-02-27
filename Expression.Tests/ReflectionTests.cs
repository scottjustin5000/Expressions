using System;
using Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expression.Tests
{
    [TestClass]
    public class ReflectionTests
    {
        private readonly TestClass _instance = new TestClass();
        [TestMethod]
        public void TestCreateObject()
        {
            var invoker = new ObjectInvoker<TestClass>(typeof(TestClass));
            var test = invoker.GetObject();
            Assert.IsNotNull(test);
        }
        [TestMethod]
        public void TestCreateObjectWithArgs()
        {
            var invoker = new ObjectInvoker<TestClass>(typeof(TestClass));
            var test = invoker.GetObject("Scott");
           
            Assert.AreEqual(test.Name,"Scott");
        }
        [TestMethod]
        public void TestCreateTypeInferface()
        {
            string typeName = "Expression.Tests.TestClass";
            Type t = Type.GetType(typeName);
            var invoker = new ObjectInvoker<ITestClass>(t);
            var test = invoker.GetObject();
            Assert.IsInstanceOfType(test, typeof(TestClass));
        }
        [TestMethod]
        public void TestPrivatePropertySet()
        {
            var reflector = new TypeReflector(typeof(TestClass));
            var property = reflector.GetProperty("SocialNumber");
            var val = PropertyCache.GetInvoker<TestClass, string>(_instance, property);
            val.SetValue("123456789");
            var result = val.GetValue();
            Assert.AreEqual(result, "123456789");


        }
        [TestMethod]
        public void TestPrivatePropertyGet()
        {
            var reflector = new TypeReflector(typeof(TestClass));
            var property = reflector.GetProperty("BirthDate");
            var val = PropertyCache.GetInvoker<TestClass, DateTime>(_instance, property);
            var result = val.GetValue();
            Assert.AreEqual(result, DateTime.MinValue);
        }
        [TestMethod]
        public void TestPrivateMethodInvoke()
        {
            _instance.Name = "Scott";
            var reflector = new TypeReflector(typeof(TestClass));
            var method = reflector.GetMethod("HelloPerson");
            var val = MethodInvokerCache.GetInvoker<TestClass, string>(_instance, method);
            var result = val.Invoke(null);
            Assert.AreEqual(result, "Hello Scott");
        }
    }
}
