using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    public class ExpressionOperation
    {
        private readonly int _precedence;
        // private readonly string name;
        private readonly Func<Expression, Expression, Expression> _operation;

        public static readonly ExpressionOperation Addition = new ExpressionOperation(1, Expression.Add);
        public static readonly ExpressionOperation Subtraction = new ExpressionOperation(1, Expression.Subtract);
        public static readonly ExpressionOperation Multiplication = new ExpressionOperation(2, Expression.Multiply);
        public static readonly ExpressionOperation Division = new ExpressionOperation(2, Expression.Divide);
        public static readonly ExpressionOperation Modulo = new ExpressionOperation(2, Expression.Modulo);
        public static readonly ExpressionOperation PowerOf = new ExpressionOperation(2, Expression.Power);
        public static readonly ExpressionOperation EqualTo = new ExpressionOperation(1, Expression.Equal);
        public static readonly ExpressionOperation NotEqualTo = new ExpressionOperation(1, Expression.NotEqual);
        public static readonly ExpressionOperation GreaterThan = new ExpressionOperation(1, Expression.GreaterThan);
        public static readonly ExpressionOperation GreaterThanEqual = new ExpressionOperation(1, Expression.GreaterThanOrEqual);
        public static readonly ExpressionOperation LessThan = new ExpressionOperation(1, Expression.LessThan);
        public static readonly ExpressionOperation LessThanEqual = new ExpressionOperation(1, Expression.LessThanOrEqual);
        public static readonly ExpressionOperation Or = new ExpressionOperation(1, Expression.Or);
        public static readonly ExpressionOperation And = new ExpressionOperation(1, Expression.And);
        private static readonly Dictionary<char, ExpressionOperation> Operations = new Dictionary<char, ExpressionOperation>
        {
            { '+', Addition },
            { '-', Subtraction },
            { '*', Multiplication},
            { '/', Division },
            {'%', Modulo},
            {'^', PowerOf},
            {'>', GreaterThan},
            {'~', GreaterThanEqual},
            {'<', LessThan},
            {'`', LessThan},
            {'=', EqualTo},
            {'&', And},
            {'|', Or}
        };

        private ExpressionOperation(int precedence, Func<Expression, Expression, Expression> operation)
        {
            _precedence = precedence;
            _operation = operation;
        }

        public int Precedence
        {
            get { return _precedence; }
        }

        public static explicit operator ExpressionOperation(char operation)
        {
            ExpressionOperation result;

            if (Operations.TryGetValue(operation, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public Expression Apply(Expression left, Expression right)
        {
            return _operation(left, right);
        }

        public static bool IsDefined(char operation)
        {
            return Operations.ContainsKey(operation);
        }
    }
}
