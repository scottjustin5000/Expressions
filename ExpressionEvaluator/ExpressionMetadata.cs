using System;

namespace ExpressionEvaluator
{
    public class ExpressionMetadata
    {
        public string Property { get; set; }
        public string LSide { get; set; }
        public string Operator { get; set; }
        public Type ObjectType { get; set; }
        public string RSide { get; set; }
        public bool UsesProperty { get; set; }
        public bool IsExcutable { get; set; }

        public void AddParameter(string value, Type type)
        {
            if (ObjectType != null)
            {
                ObjectType = type;
            }
            else if (ObjectType != type)
            {
                throw new Exception("Expression Mismatch");
            }
            if (!string.IsNullOrEmpty(LSide))
            {
                LSide = value;
            }
            else
            {
                RSide = value;
            }
        }

    }
}
