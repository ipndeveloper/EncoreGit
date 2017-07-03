using System;
using System.Linq.Expressions;
using NetSteps.Encore.Core;

namespace NetSteps.Common.Base
{
	[Serializable]
	public class FilterDateRangePaginatedListParameters<T> : DateRangeSearchParameters
	{
        static readonly int CHashCodeSeed = typeof(PaginatedListParameters).GetKeyForType().GetHashCode();
        
        [NonSerialized]
		private Expression<Func<T, bool>> _whereClause = null;
		/// <summary>
		/// Can be used as a normal lambda function, i.e. WhereClause = x => x.MyProperty;
		/// </summary>
		public Expression<Func<T, bool>> WhereClause
		{
			get
			{
				return _whereClause;
			}
			set
			{
				_whereClause = value;
			}
		}

        /// <summary>
        /// Determines if the instance is equal to another.
        /// </summary>
        /// <param name="other">the other instance</param>
        /// <returns>true if equal; otherwise false.</returns>
        public bool Equals(FilterDateRangePaginatedListParameters<T> other)
        {
            return other != null
                && base.Equals(other)
                && _whereClause == other._whereClause;
        }

        /// <summary>
        /// Determines if the instance is equal to another object.
        /// </summary>
        /// <param name="obj">the other object</param>
        /// <returns>true if equal; otherwise false</returns>
        public override bool Equals(object obj)
        {
            return obj is FilterDateRangePaginatedListParameters<T>
                && Equals((FilterDateRangePaginatedListParameters<T>)obj);
        }

        /// <summary>
        /// Gets the instance's hashcode.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            int prime = 999067; // a random prime
            int result = CHashCodeSeed * prime;

            result ^= base.GetHashCode() * prime;

            if (_whereClause != null)
                result ^= _whereClause.GetHashCode() * prime;

            return result;
        }
	}
}
