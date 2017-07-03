using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: ObjectQuery Extensions to extend the functionality of LINQ to Entities
    /// Created: 05-24-2010
    /// </summary>
    public static class ObjectQueryExtensions
    {
        /// <summary>
        /// Method to work around the lack of support for the Linq To Entities Linq method of Contains(Func). - JHE
        /// Taken from:
        /// http://stackoverflow.com/questions/374267/contains-workaround-using-linq-to-entities
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> WhereIn<TEntity, TValue>(this ObjectQuery<TEntity> query, Expression<Func<TEntity, TValue>> selector, IEnumerable<TValue> collection)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (collection == null) throw new ArgumentNullException("collection");
            ParameterExpression p = selector.Parameters.Single();

            if (!collection.Any()) return query;

            IEnumerable<Expression> equals = collection.Select(value =>
               (Expression)Expression.Equal(selector.Body,
                    Expression.Constant(value, typeof(TValue))));

            Expression body = equals.Aggregate((accumulate, equal) =>
                Expression.Or(accumulate, equal));

            return query.Where(Expression.Lambda<Func<TEntity, bool>>(body, p));
        }


        public static string ToSql<T>(this ObjectQuery<T> query)
        {
            string sql = query.ToTraceString();

            foreach (var item in query.Parameters)
                sql = sql.Replace("@" + item.Name, item.ParameterType == typeof(string) ? String.Format("'{0}'", item.Value) : item.Value.ToString());

            return sql;
        }
    }
}
