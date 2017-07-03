using System;
using NetSteps.Encore.Core;

namespace NetSteps.Common.Base
{
    public interface IPaginatedListParameters
    {
        int PageIndex { get; }
        int? PageSize { get; }
        string OrderBy { get; }
        Constants.SortDirection OrderByDirection { get; }
        string OrderByString { get; }        
        int LanguageID { get; }
    }

	/// <summary>
	/// Author: John Egbert
	/// Description: Parameter class to pass into methods for paged and ordered results.
	/// Created: 05-07-2010
	/// </summary>
	[Serializable]
	public class PaginatedListParameters : IPaginatedListParameters
	{
        static readonly int CHashCodeSeed = typeof(PaginatedListParameters).AssemblyQualifiedName.GetHashCode();

		public int PageIndex { get; set; }
		public int? PageSize { get; set; }
		public string OrderBy { get; set; }
		public Constants.SortDirection OrderByDirection { get; set; }
        public int LanguageID { get; set; }

		public PaginatedListParameters()
		{
			PageIndex = 0;
			PageSize = 20;
			OrderBy = string.Empty;
			OrderByDirection = Constants.SortDirection.Ascending;
            LanguageID = ApplicationContextCommon.Instance.CurrentLanguageID;
		}

        /// <summary>
        /// An "OrderBy" string that includes sort direction to allow the use of DynamicQueryable.OrderBy().
        /// </summary>
        public string OrderByString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OrderBy))
                    return string.Empty;

                return OrderByDirection == Constants.SortDirection.Ascending
                    ? OrderBy
                    : OrderBy + " DESC";
            }
        }

        /// <summary>
		/// Determines if the instance is equal to another.
		/// </summary>
		/// <param name="other">the other instance</param>
		/// <returns>true if equal; otherwise false.</returns>
        public bool Equals(PaginatedListParameters other)
        {
            return other != null
                && PageIndex == other.PageIndex
                && PageSize == other.PageSize
                && OrderBy == other.OrderBy
                && OrderByDirection == other.OrderByDirection
                && LanguageID == other.LanguageID;
        }

        /// <summary>
        /// Determines if the instance is equal to another object.
        /// </summary>
        /// <param name="obj">the other object</param>
        /// <returns>true if equal; otherwise false</returns>
        public override bool Equals(object obj)
        {
            return obj is PaginatedListParameters
                && Equals((PaginatedListParameters)obj);
        }

        /// <summary>
		/// Gets the instance's hashcode.
		/// </summary>
		/// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            int prime = 999067; // a random prime
            int result = CHashCodeSeed * prime;

            result ^= PageIndex * prime;
            
            if (PageSize.HasValue)
                result ^= PageSize.Value * prime;

            if (OrderBy != null && OrderBy.Length > 0)
                result ^= OrderBy.GetHashCode() * prime;

            result ^= OrderByDirection.GetHashCode() * prime;

            result ^= LanguageID * prime;

            return result;
        }
	}
}
