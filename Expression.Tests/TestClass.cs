using System;

namespace Expression.Tests
{
    public interface  ITestClass
    {
        string Name { get; set; }
        int Age { get; set; }
    }
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            BirthDate = DateTime.MinValue;
        }
        public TestClass(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public int Age { get; set; }

        private DateTime BirthDate { get; set; }
        private string SocialNumber { get; set; }

        private string HelloPerson()
        {
            return string.Format("Hello {0}", Name);
        }
    }
}
