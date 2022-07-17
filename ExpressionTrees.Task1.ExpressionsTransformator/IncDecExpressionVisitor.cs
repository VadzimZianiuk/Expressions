using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private readonly IReadOnlyDictionary<string, int> _parameters;

        public IncDecExpressionVisitor(IReadOnlyDictionary<string, int> parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameters.TryGetValue(node.Name, out var value))
            {
                return Expression.Constant(value);
            }

            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(
                Visit(node.Body), 
                node.Parameters.Where(x => !_parameters.ContainsKey(x.Name)));
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (IsIncrementOrDecrement(node.Right) && IsAddOrSubtract(node))
            {
                switch (node.Left)
                {
                    case ParameterExpression _:
                    case MemberExpression _:
                        return IncrementDecrement(node.Left);

                    case BinaryExpression left when IsAddOrSubtract(left):
                        var right = IncrementDecrement(left.Right);
                        return left.NodeType == ExpressionType.Add
                            ? Expression.Add(Visit(left.Left), right)
                            : Expression.Subtract(Visit(left.Left), right);
                    default:
                        break;
                }
            }

            return base.VisitBinary(node);

            UnaryExpression IncrementDecrement(Expression expression) =>
                node.NodeType == ExpressionType.Add
                    ? Expression.Increment(Visit(expression))
                    : Expression.Decrement(Visit(expression));
        }

        private static bool IsAddOrSubtract(BinaryExpression node) =>
            node.NodeType == ExpressionType.Add || node.NodeType == ExpressionType.Subtract;

        private static bool IsIncrementOrDecrement(Expression node) =>
            node is ConstantExpression exp 
            && byte.TryParse(exp.Value.ToString(), out var value) 
            && value == 1;
    }
}
