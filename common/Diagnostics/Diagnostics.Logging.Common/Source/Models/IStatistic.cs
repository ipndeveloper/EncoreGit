using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Logging.Common.Models
{
	/// <summary>
	/// Common interface for Statistic.
	/// </summary>
	[ContractClass(typeof(Contracts.StatisticContracts))]
	public interface IStatistic
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StatisticID for this Statistic.
		/// </summary>
		long StatisticID { get; set; }
	
		/// <summary>
		/// The OccuredDateTimeUTC for this Statistic.
		/// </summary>
		System.DateTime OccuredDateTimeUTC { get; set; }
	
		/// <summary>
		/// The StatisticTypeID for this Statistic.
		/// </summary>
		short StatisticTypeID { get; set; }
	
		/// <summary>
		/// The HasBeenReported for this Statistic.
		/// </summary>
		bool HasBeenReported { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The StatisticType for this Statistic.
		/// </summary>
	    IStatisticType StatisticType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The StatisticValues for this Statistic.
		/// </summary>
		IEnumerable<IStatisticValue> StatisticValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IStatisticValue"/> to the StatisticValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IStatisticValue"/> to add.</param>
		void AddStatisticValue(IStatisticValue item);
	
		/// <summary>
		/// Removes an <see cref="IStatisticValue"/> from the StatisticValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IStatisticValue"/> to remove.</param>
		void RemoveStatisticValue(IStatisticValue item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IStatistic))]
		internal abstract class StatisticContracts : IStatistic
		{
		    #region Primitive properties
		
			long IStatistic.StatisticID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IStatistic.OccuredDateTimeUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IStatistic.StatisticTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IStatistic.HasBeenReported
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IStatisticType IStatistic.StatisticType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IStatisticValue> IStatistic.StatisticValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IStatistic.AddStatisticValue(IStatisticValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IStatistic.RemoveStatisticValue(IStatisticValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
