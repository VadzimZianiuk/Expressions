using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<T, TResult> Generate<T, TResult>(RuleSet<T, TResult> ruleSet)
        {
            var sourceParam = Expression.Parameter(typeof(T));

            var bindings = new List<MemberAssignment>();
            foreach (var rule in ruleSet.Rules)
            {
                Expression value;
                if (rule.SourceParamName == null)
                {
                    value = Expression.Invoke(rule.MappingExpression, sourceParam);
                }
                else
                {
                    var field = Expression.PropertyOrField(sourceParam, rule.SourceParamName);
                    value = Expression.Invoke(rule.MappingExpression, field);
                }

                var bindExpression = Expression.Bind(rule.MemberInfo, value);
                bindings.Add(bindExpression);
            }

            var newExpression = Expression.New(typeof(TResult));
            var body = Expression.MemberInit(newExpression, bindings);
            var mapFunction = Expression.Lambda<Func<T, TResult>>(body, sourceParam);

            return new Mapper<T, TResult>(mapFunction.Compile());
        }
    }
}
