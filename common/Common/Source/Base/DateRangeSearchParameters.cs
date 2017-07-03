using System;
using System.Runtime.Serialization;
using NetSteps.Encore.Core;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to allow date range filtering on a PaginatedList.
    /// Created: 08-18-2010
    /// </summary>
    [DataContract]
    [Serializable]
    public class DateRangeSearchParameters : PaginatedListParameters
    {
        static readonly int CHashCodeSeed = typeof(PaginatedListParameters).GetKeyForType().GetHashCode();
        
        protected Nullable<System.DateTime> _startDate;
        protected Nullable<System.DateTime> _endDate;

        [DataMember]
        public Nullable<System.DateTime> StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (value.HasValue && value.Value.Year < 1900)
                    value = null;

                _startDate = value;
            }
        }
        [DataMember]
        public Nullable<System.DateTime> EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (value.HasValue && value.Value.Year < 1900)
                    value = null;

                _endDate = value;
            }
        }

        /// <summary>
        /// Determines if the instance is equal to another.
        /// </summary>
        /// <param name="other">the other instance</param>
        /// <returns>true if equal; otherwise false.</returns>
        public bool Equals(DateRangeSearchParameters other)
        {
            return other != null
                && base.Equals(other)
                && _startDate == other._startDate
                && _endDate == other._endDate;
        }

        /// <summary>
        /// Determines if the instance is equal to another object.
        /// </summary>
        /// <param name="obj">the other object</param>
        /// <returns>true if equal; otherwise false</returns>
        public override bool Equals(object obj)
        {
            return obj is DateRangeSearchParameters
                && Equals((DateRangeSearchParameters)obj);
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

            if (_startDate.HasValue && _startDate.Value > DateTime.MinValue)
                result ^= _startDate.GetHashCode() * prime;

            if (_endDate.HasValue && _endDate.Value > DateTime.MinValue)
                result ^= _endDate.GetHashCode() * prime;

            return result;
        }
    }
}
