using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Logging.Common.Models
{
	/// <summary>
	/// Common interface for StatisticValue.
	/// </summary>
	[ContractClass(typeof(Contracts.StatisticValueContracts))]
	public interface IStatisticValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StatisticValueID for this StatisticValue.
		/// </summary>
		long StatisticValueID { get; set; }
	
		/// <summary>
		/// The StatisticID for this StatisticValue.
		/// </summary>
		long StatisticID { get; set; }
	
		/// <summary>
		/// The StatisticValueTypeID for this StatisticValue.
		/// </summary>
		short StatisticValueTypeID { get; set; }
	
		/// <summary>
		/// The Value for this StatisticValue.
		/// </summary>
		string Value { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Statistic for this StatisticValue.
		/// </summary>
	    IStatistic Statistic { get; set; }
	
		/// <summary>
		/// The StatisticValueType for this StatisticValue.
		/// </summary>
	    IStatisticValueType StatisticValueType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IStatisticValue))]
		internal abstract class StatisticValueContracts : IStatisticValue
		{
		    #region Primitive properties
		
			long IStatisticValue.StatisticValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			long IStatisticValue.StatisticID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IStatisticValue.StatisticValueTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IStatistic IStatisticValue.Statistic
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IStatisticValueType IStatisticValue.StatisticValueType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
