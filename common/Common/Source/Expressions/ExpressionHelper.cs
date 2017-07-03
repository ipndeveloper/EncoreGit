using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Expressions
{
    public enum ComparisonType
    {
        LessThan,
        LessThanOrEqual,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Like
    }

    /// <summary>
    /// Author: Daniel Stafford
    /// </summary>
    public class ExpressionHelper
    {
        public static Expression<Func<T, TResult>> MakeExpression<T, TResult>(string property, ComparisonType compType, object value)
        {
            Type tType = typeof(T);
            MemberInfo info = tType.GetPropertyCached(property);
            if (info == null)
                info = tType.GetField(property);
            if (info == null)
                throw new ArgumentException("property");
            var p = Expression.Parameter(tType, "x");

            var left = Expression.PropertyOrField(p, property);

            Expression right = null;
            if (compType == ComparisonType.Like)
            {
                if ((info.MemberType == MemberTypes.Property && (info as PropertyInfo).PropertyType == typeof(string))
                    || (info.MemberType == MemberTypes.Field && (info as FieldInfo).FieldType == typeof(string)))
                    right = Expression.Call(left, typeof(string).GetMethodCached("Contains"), Expression.Constant(value));
                else
                    compType = ComparisonType.Equal;
            }
            if (right == null)
                right = Expression.Constant(value);
            Expression filter = null;
            switch (compType)
            {
                case ComparisonType.LessThan:
                    filter = Expression.LessThan(left, right);
                    break;
                case ComparisonType.LessThanOrEqual:
                    filter = Expression.LessThanOrEqual(left, right);
                    break;
                case ComparisonType.Equal:
                    filter = Expression.Equal(left, right);
                    break;
                case ComparisonType.NotEqual:
                    filter = Expression.NotEqual(left, right);
                    break;
                case ComparisonType.GreaterThan:
                    filter = Expression.GreaterThan(left, right);
                    break;
                case ComparisonType.GreaterThanOrEqual:
                    filter = Expression.GreaterThanOrEqual(left, right);
                    break;
                case ComparisonType.Like:
                    filter = right;
                    break;
            }
            return Expression.Lambda(filter, p) as Expression<Func<T, TResult>>;
        }

        public static Expression<Func<T, IEnumerable<TValue>, bool>> MakeContainsExpression<T, TValue>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");
            var param = Expression.Parameter(typeof(T), "x");
            var valueParam = Expression.Parameter(typeof(IEnumerable<TValue>), "values");
            var prop = Expression.PropertyOrField(param, propertyName);
            var contains = Expression.Call(typeof(Enumerable).GetMethodsCached().First(m => m.Name == "Contains" && m.GetParameters().Length == 2).MakeGenericMethod(typeof(TValue)), valueParam, prop);

            return Expression.Lambda<Func<T, IEnumerable<TValue>, bool>>(contains, param, valueParam);
        }

        public static Expression<Func<T, bool>> MakeWhereInExpression<T, TValue>(Expression<Func<T, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            if (values == null)
                throw new ArgumentNullException("values");

            ParameterExpression p = valueSelector.Parameters.Single();

            var vals = values.ToList();

            if (!vals.Any())
            {
                return e => false;
            }

            var equals = vals.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate(Expression.Or);

            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        // NOTE: This method is causing some StackOverflow exceptions when the 'IEnumerable<TValue> values' parameter is large set of values like 2000 - JHE
        public static Expression<Func<T, bool>> MakeWhereNotInExpression<T, TValue>(Expression<Func<T, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            if (values == null)
                throw new ArgumentNullException("values");

            ParameterExpression p = valueSelector.Parameters.Single();

            var vals = values.ToList();

            if (!vals.Any())
            {
                return e => true;
            }

            var notEquals = vals.Select(value => (Expression)Expression.NotEqual(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = notEquals.Aggregate(Expression.And);

            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public delegate T ObjectActivator<T>(params object[] args);
        public delegate object ObjectActivator(params object[] args);

        private static readonly ConcurrentDictionary<ConstructorInfo, ObjectActivator> _genericActivators = new ConcurrentDictionary<ConstructorInfo, ObjectActivator>();
        private static readonly ConcurrentDictionary<ConstructorInfo, Delegate> _typedActivators = new ConcurrentDictionary<ConstructorInfo, Delegate>();

        public static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor)
        {
            return (ObjectActivator<T>) _typedActivators.GetOrAdd(ctor, key =>
            {
                //create a single param of type object[]
                ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

                NewExpression newExp = MakeNewExpression(key, param);

                //create a lambda with the NewExpression as body and our param object[] as arg
                LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

                //compile it
                var activator = (ObjectActivator<T>)lambda.Compile();

                return activator;
            });
        }

        public static ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            return _genericActivators.GetOrAdd(ctor, key =>
            {
                //create a single param of type object[]
                ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

                NewExpression newExp = MakeNewExpression(key, param);

                //create a lambda with the NewExpression as body and our param object[] as arg
                LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

                //compile it
                var activator = (ObjectActivator)lambda.Compile();

                return activator;
            });
        }

        public static NewExpression MakeNewExpression(ConstructorInfo ctor, ParameterExpression paramExpression)
        {
            ParameterInfo[] parameters = ctor.GetParameters();
            Expression[] argsExp = new Expression[parameters.Length];

            //pick each arg from the params array and create a typed expression of them
            for (int i = 0; i < parameters.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = parameters[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(paramExpression, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the ctor with the args we just created
            return Expression.New(ctor, argsExp);
        }
    }
}
