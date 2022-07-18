using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class RuleSet<T, TResult>
    {
        internal IList<Rule> Rules { get; } = new List<Rule>();

        public RuleSet<T, TResult> Add<TProperty, TResultProperty>(
            Expression<Func<T, TProperty>> sourceProperty,
            Expression<Func<TResult, TResultProperty>> destinationProperty,
            Expression<Func<TProperty, TResultProperty>> mappingExpression)
        {
            Rules.Add(new Rule
            {
                SourceParamName = (sourceProperty.Body as MemberExpression)?.Member.Name,
                MemberInfo = (destinationProperty.Body as MemberExpression).Member,
                MappingExpression = mappingExpression
            });

            return this;
        }
    }

    internal class Rule
    {
        public string SourceParamName { get; set; }
        public LambdaExpression MappingExpression { get; set; }
        public MemberInfo MemberInfo { get; set; }
    }
}
