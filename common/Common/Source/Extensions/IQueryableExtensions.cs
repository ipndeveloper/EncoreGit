using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetSteps.Common.Expressions;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: IQueryable Extensions to extend the functionality of LINQ to Entities
	/// Created: 06-04-2010
	/// </summary>
	public static class IQueryableExtensions
	{
		/// <summary>
		/// http://social.msdn.microsoft.com/Forums/en-US/adodotnetentityframework/thread/4a17b992-05ca-4e3b-9910-0018e7cc9c8c - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static string ToTraceString<T>(this IQueryable<T> t)
		{
			// try to cast to ObjectQuery<T>
			ObjectQuery<T> oqt = t as ObjectQuery<T>;
			if (oqt != null)
				return oqt.ToTraceString();
			return "";
		}


		public static ObjectQuery<T> ToObjectQuery<T>(this IQueryable<T> t)
		{
			ObjectQuery<T> oqt = t as ObjectQuery<T>;
			if (oqt != null)
				return oqt;
			return oqt;
		}

		// Taken from http://stackoverflow.com/questions/41244/dynamic-linq-orderby - JHE
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderBy");
		}
		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderByDescending");
		}
		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenBy");
		}
		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenByDescending");
		}
		public static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
		{
			//string[] props = property.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "x");
			Expression expr = arg;
			//foreach (string prop in props)
			//{
			//    // use reflection (not ComponentModel) to mirror LINQ 
			//    PropertyInfo pi = type.GetPropertyCached(prop);
			//    if (pi == null)
			//        throw new ArgumentException(string.Format("Property '{0}' does not exist on type '{1}'", property, type.Name), "property");
			//    expr = Expression.Property(expr, pi);
			//    type = pi.PropertyType;
			//}
			if (type.PropertyExists(property, (t, p) =>
			{
				expr = Expression.Property(expr, p);
				type = p.PropertyType;
			}))
			{
				//Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
				LambdaExpression lambda = Expression.Lambda(expr, arg);

				object result = typeof(Queryable).GetMethodsCached().Single(
						method => method.Name == methodName
								&& method.IsGenericMethodDefinition
								&& method.GetGenericArguments().Length == 2
								&& method.GetParameters().Length == 2)
						.MakeGenericMethod(typeof(T), type)
						.Invoke(null, new object[] { source, lambda });
				return (IOrderedQueryable<T>)result;
			}
			return null;
		}

		public static IOrderedQueryable<T> Order<T, TKey>(this IQueryable<T> source, NetSteps.Common.Constants.SortDirection orderDirection, Expression<Func<T, TKey>> selector)
		{
			return orderDirection == Constants.SortDirection.Ascending ? source.OrderBy(selector) : source.OrderByDescending(selector);
		}

		public static IOrderedQueryable<T> Then<T, TKey>(this IOrderedQueryable<T> source, NetSteps.Common.Constants.SortDirection orderDirection, Expression<Func<T, TKey>> selector)
		{
			return orderDirection == Constants.SortDirection.Ascending ? source.ThenBy(selector) : source.ThenByDescending(selector);
		}

		private static MethodInfo whereMethod;

		public static IQueryable<T> Where<T>(this IQueryable<T> list, string property, ComparisonType compType, object value)
		{
			//if (whereMethod == null)
			{
				Expression<Func<IQueryable<T>, IQueryable<T>>> lambda = l => l.Where(element => default(bool));
				whereMethod = (lambda.Body as MethodCallExpression).Method;
			}

			var results = whereMethod.Invoke(null, new object[] { list, ExpressionHelper.MakeExpression<T, bool>(property, compType, value) }) as IQueryable<T>;
			if (results != null)
				return results;
			return default(IQueryable<T>);
		}


		public static Expression<Func<T, bool>> GetExpression<T>(this IQueryable<T> list, Expression<Func<T, bool>> predicate)
		{
			return predicate;
		}
	}
}
