using System;
using System.Linq.Expressions;

namespace NetSteps.Common.Base
{
	public class FilterPaginatedListParameters<T> : PaginatedListParameters
	{
		/// <summary>
		/// Can be used as a normal lambda function, i.e. WhereClause = x => x.MyProperty;
		/// </summary>
		public Expression<Func<T, bool>> WhereClause
		{
			get;
			set;
		}
	}
}
