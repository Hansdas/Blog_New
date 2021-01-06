using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Repository.Where
{
   public class ParseCondition<T>
    {
        private Expression _expression;
        private ParameterExpression _parameter;
        private Type _targetType;
        public ParseCondition(Type targetType)
        {
            _targetType = targetType;
            _parameter = Expression.Parameter(targetType, "m");
        }
        public  Expression BuildExpression(string name,string value, ConditionOperation operation)
        {
            var property = _targetType.GetProperty(name);
            if (property == null)
                return null;
            Type realType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            Expression<Func<object>> valueLamba =()=>Convert.ChangeType(value, realType);
            Func<Expression, Expression, Expression> Append = (exp1, exp2) =>
            {
                if (exp1 == null)
                    return exp2;
                return Expression.AndAlso(exp1, exp2);
            };
            switch (operation)
            {
                case ConditionOperation.Equal:
                    {
                        _expression = Append(_expression, Expression.Equal(Expression.Property(_parameter, name),
                            Expression.Convert(valueLamba.Body, property.PropertyType)));
                        break;
                    }
                case ConditionOperation.GreaterThan:
                    {
                        _expression = Append(_expression, Expression.GreaterThan(Expression.Property(_parameter, name),
                            Expression.Convert(valueLamba.Body, property.PropertyType)));
                        break;
                    }
                case ConditionOperation.GreaterThanOrEqual:
                    {
                        _expression = Append(_expression, Expression.GreaterThanOrEqual(Expression.Property(_parameter, name),
                            Expression.Convert(valueLamba.Body, property.PropertyType)));
                        break;
                    }
                case ConditionOperation.LessThan:
                    {
                        _expression = Append(_expression, Expression.LessThan(Expression.Property(_parameter, name),
                            Expression.Convert(valueLamba.Body, property.PropertyType)));
                        break;
                    }
                case ConditionOperation.LessThanOrEqual:
                    {
                        _expression = Append(_expression, Expression.LessThanOrEqual(Expression.Property(_parameter, name),
                            Expression.Convert(valueLamba.Body, property.PropertyType)));
                        break;
                    }
                case ConditionOperation.Contains:
                    {
                        var nullCheck = Expression.Not(Expression.Call(typeof(string), "IsNullOrEmpty", null, Expression.Property(_parameter, name)));
                        var contains = Expression.Call(Expression.Property(_expression, name), "Contains", null,
                            Expression.Convert(valueLamba.Body, property.PropertyType));
                        _expression = Append(_expression, Expression.AndAlso(nullCheck, contains));
                        break;
                    }
            }

            if (_expression == null)
            {
                return null;
            }
            return _expression;
        }
        public  Expression<Func<T, bool>> BuildWhereExpression()
        {
            return ((Expression<Func<T, bool>>)Expression.Lambda(_expression, _parameter));
        }
    }
}
