using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionEvaluator
{
    public class MathExpression:ExpressionParser
    {
        private readonly Stack<Expression> _expressionStack = new Stack<Expression>();
        private readonly Stack<char> _operatorStack = new Stack<char>();
        private readonly List<string> _parameters = new List<string>();

        public Func<object, double> Compile(string expression)
        {
            var compiled = Parse(expression);
           
            Func<object, double> result = argument =>
            {
                var arguments = ParseArguments(argument);
                return Execute(compiled, arguments);
            };

            return result;
        }

        public double Evaluate(string expression, object argument = null)
        {
            var arguments = ParseArguments(argument);

            return Evaluate(expression, arguments);
        }
        private Func<double[], double> Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return s => 0;
            }

            var arrayParameter = Expression.Parameter(typeof(double[]), "args");

            _parameters.Clear();
            _operatorStack.Clear();
            _expressionStack.Clear();

            using (var reader = new StringReader(expression))
            {
                int peek;
                while ((peek = reader.Peek()) > -1)
                {
                    var next = (char)peek;

                    if (char.IsDigit(next))
                    {
                        _expressionStack.Push(ReadDoubleOperand(reader));
                        continue;
                    }

                    if (char.IsLetter(next))
                    {
                        _expressionStack.Push(ReadParameter(reader, arrayParameter));
                        continue;
                    }

                    if (ExpressionOperation.IsDefined(next))
                    {
                        var currentOperation = ReadOperation(reader);

                        EvaluateWhile(() => _operatorStack.Count > 0 && _operatorStack.Peek() != '(' &&
                            currentOperation.Precedence <= ((ExpressionOperation)_operatorStack.Peek()).Precedence);

                        _operatorStack.Push(next);
                        continue;
                    }

                    if (next == '(')
                    {
                        reader.Read();
                        _operatorStack.Push('(');
                        continue;
                    }

                    if (next == ')')
                    {
                        reader.Read();
                        EvaluateWhile(() => _operatorStack.Count > 0 && _operatorStack.Peek() != '(');
                        _operatorStack.Pop();
                        continue;
                    }

                    if (next == ' ')
                    {
                        reader.Read();
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Encountered invalid character {0}", next),
                            "expression");
                    }
                }
            }

            EvaluateWhile(() => _operatorStack.Count > 0);

            var lambda = Expression.Lambda<Func<double[], double>>(_expressionStack.Pop(), arrayParameter);
            var compiled = lambda.Compile();
            return compiled;
        }

        private Dictionary<string, double> ParseArguments(object argument)
        {
            if (argument == null)
            {
                return new Dictionary<string, double>();
            }

            var argumentType = argument.GetType();

            var properties = argumentType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && IsNumeric(p.PropertyType));

            var arguments = properties.ToDictionary(property => property.Name,
                property => Convert.ToDouble(property.GetValue(argument, null)));

            return arguments;
        }

        private double Evaluate(string expression, Dictionary<string, double> arguments)
        {
            var compiled = Parse(expression);

            return Execute(compiled, arguments);
        }
        private double Execute(Func<double[], double> compiled, Dictionary<string, double> arguments)
        {
            arguments = arguments ?? new Dictionary<string, double>();

            if (_parameters.Count != arguments.Count)
            {
                throw new ArgumentException(string.Format("Expression contains {0} parameters but got only {1}",
                    _parameters.Count, arguments.Count));
            }

            var missingParameters = _parameters.Where(p => !arguments.ContainsKey(p)).ToList();

            if (missingParameters.Any())
            {
                throw new ArgumentException("No values provided for parameters: " + string.Join(",", missingParameters));
            }

            var values = _parameters.Select(parameter => arguments[parameter]).ToArray();

            return compiled(values);
        }


        private void EvaluateWhile(Func<bool> condition)
        {
            while (condition())
            {
                var right = _expressionStack.Pop();
                var left = _expressionStack.Pop();

                _expressionStack.Push(((ExpressionOperation)_operatorStack.Pop()).Apply(left, right));
            }
        }

        private Expression ReadParameter(TextReader reader, Expression arrayParameter)
        {
            var parameter = string.Empty;

            int peek;

            while ((peek = reader.Peek()) > -1)
            {
                var next = (char)peek;

                if (char.IsLetter(next))
                {
                    reader.Read();
                    parameter += next;
                }
                else
                {
                    break;
                }
            }

            if (!_parameters.Contains(parameter))
            {
                _parameters.Add(parameter);
            }

            return Expression.ArrayIndex(arrayParameter, Expression.Constant(_parameters.IndexOf(parameter)));
        }

    }
}
