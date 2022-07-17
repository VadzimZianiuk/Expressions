/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            var expressions = new List<Expression>
            {
                (Expression<Func<int, int>>)((int x) => x + 1 + x - 1 * x / x - 1),
                (Expression<Func<int, int, int>>)((int x, int y) => x + 1 * y - 1 + x - y),
                (Expression<Func<int, int, int, int>>)((int x, int y, int z) => x + 1 * y - 1 + z + 1),
            };

            var incDecVisitor = new IncDecExpressionVisitor(GetParams());
            foreach (var expression in expressions)
            {
                var expressionMod = incDecVisitor.Visit(expression);
                Log(expression, expressionMod);
            }

            Console.ReadLine();
        }

        private static Dictionary<string, int> GetParams()
        {
            Console.WriteLine("Parameters");
            var parameters = new Dictionary<string, int>
            {
                { "a", 99 },
                { "x", 5 },
                { "y", 10 },
            };
            foreach (var item in parameters)
            {
                Console.WriteLine(" {0}: {1}", item.Key, item.Value);
            }

            return parameters;
        }

        static void Log(Expression expression, Expression expressionMod)
        {
            Console.WriteLine();
            Console.WriteLine("Source: {0}", expression);
            Console.WriteLine("   Mod: {0}", expressionMod);
        }
    }
}
