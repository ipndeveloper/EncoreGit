using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Logging.Common.Models
{
	/// <summary>
	/// Common interface for StatisticType.
	/// </summary>
	[ContractClass(typeof(Contracts.StatisticTypeContracts))]
	public interface IStatisticType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StatisticTypeID for this StatisticType.
		/// </summary>
		short StatisticTypeID { get; set; }
	
		/// <summary>
		/// The Name for this StatisticType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this StatisticType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this StatisticType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this StatisticType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Statistics for this StatisticType.
		/// </summary>
		IEnumerable<IStatistic> Statistics { get; }
	
		/// <summary>
		/// Adds an <see cref="IStatistic"/> to the Statistics collection.
		/// </summary>
		/// <param name="item">The <see cref="IStatistic"/> to add.</param>
		void AddStatistic(IStatistic item);
	
		/// <summary>
		/// Removes an <see cref="IStatistic"/> from the Statistics collection.
		/// </summary>
		/// <param name="item">The <see cref="IStatistic"/> to remove.</param>
		void RemoveStatistic(IStatistic item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IStatisticType))]
		internal abstract class StatisticTypeContracts : IStatisticType
		{
		    #region Primitive properties
		
			short IStatisticType.StatisticTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IStatisticType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IStatistic> IStatisticType.Statistics
			{
				get { throw new NotImplementedException(); }
			}
		
			void IStatisticType.AddStatistic(IStatistic item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IStatisticType.RemoveStatistic(IStatistic item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
