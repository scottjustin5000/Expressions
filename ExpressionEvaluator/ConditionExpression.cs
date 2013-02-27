using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    public class ConditionExpression :ExpressionParser
    {
        private readonly Stack<Expression> _expressionStack = new Stack<Expression>();
        private readonly Stack<Expression> _compoundStack = new Stack<Expression>();
        private readonly Stack<char> _operatorStack = new Stack<char>();
        private bool _fullExpression;
        private string _appending;

        public Func<object, bool> Compile(string expression)
        {
            throw new NotImplementedException();
        }

        public bool Evaluate(string expression, object argument = null)
        {
            expression = ReplaceTokens(expression);
            var exp = Parse(expression);
         
            var res = exp.Compile();
            return res();

        }
        private Expression<Func<bool>> Parse(string expression)
        {
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
                        _expressionStack.Push(ReadDigitOperand(reader));
                        continue;
                    }

                    if (char.IsLetter(next))
                    {
                        _expressionStack.Push(ReadStringOperand(reader));
                        continue;
                    }

                    if (ExpressionOperation.IsDefined(next))
                    {
                        var currentOperation = ReadOperation(reader);
                        if (next == '&' || next == '|')
                        {
                            reader.Read();
                            _appending = next == '&' ? "And" : "Or";
                        }
            
                        if (next == '=')
                        {
                            reader.Read();
                        }

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

            EvaluateWhile(() => _operatorStack.Count > 0 && _operatorStack.Peek() != '(');

            if (_expressionStack.Any())
            {
                if (!string.IsNullOrEmpty(_appending))
                {
                    ExpressionType binary;
                    if (ExpressionType.TryParse(_appending, out binary))
                    {
                        if (_compoundStack.Any())
                        {
                            var compoundLeft = _compoundStack.Pop();
                            var newRight = _expressionStack.Pop();
                            var combined = Expression.MakeBinary(binary, compoundLeft, newRight);
                            _compoundStack.Push(combined);
                        }
                    }
                }
                else
                {
                    _compoundStack.Push(_expressionStack.Pop());
                }
            }

            var lambda = Expression.Lambda<Func<bool>>(_compoundStack.Pop());
            return lambda;
        }

        
        private void EvaluateWhile(Func<bool> condition)
        {
            if (!string.IsNullOrEmpty(_appending) && _fullExpression)
            {
                ExpressionType binary;
                if (ExpressionType.TryParse(_appending, out binary))
                {
                    if (_compoundStack.Any())
                    {
                        var compoundLeft = _compoundStack.Pop();
                        var newRight = _expressionStack.Pop();
                        var combined = Expression.MakeBinary(binary, compoundLeft, newRight);
                        _compoundStack.Push(combined);
                        _appending = string.Empty;
                    }
                    else
                    {
                        if (_expressionStack.Count > 1)
                        {

                            var right = _expressionStack.Pop();
                            var left = _expressionStack.Pop();
                            if (right.Type == typeof(bool) && left.Type == typeof(bool))
                            {
                                var exp = Expression.MakeBinary(binary, left, right);
                                _compoundStack.Push(exp);
                                _appending = string.Empty;
                            }
                            else
                            {
                                _expressionStack.Push(left);
                                _expressionStack.Push(right);

                            }

                        }
                        _fullExpression = false;
                    }
                }
            }
            else
            {
                while (condition())
                {
                    if (_expressionStack.Count > 1)
                    {
                        var right = _expressionStack.Pop();
                        var left = _expressionStack.Pop();

                        _expressionStack.Push(((ExpressionOperation)_operatorStack.Pop()).Apply(left, right));
                        if (!string.IsNullOrEmpty(_appending))
                        {
                            _fullExpression = true;

                        }
                    }
                }
            }
        }

        private const string PATTERN = ">=|<=";
        private const string GTE= ">=";
        private const string LTE = "<=";
        private string ReplaceTokens(string expression)
        {
            var regex = new Regex(PATTERN);
            var m = regex.Match(expression);
            if (m.Success)
            {
                var lte = new Regex(LTE);
                var lteMatch = lte.Match(expression);
                if (lteMatch.Success)
                {
                    expression = Regex.Replace(expression, LTE, "`");
                }
                var gte = new Regex(GTE);
                var gteMatch = gte.Match(expression);
                if (gteMatch.Success)
                {
                    expression = Regex.Replace(expression, GTE, "~");
                }
            }
            return expression;
        }
     

    }
}
