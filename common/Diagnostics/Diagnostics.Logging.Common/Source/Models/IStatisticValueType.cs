using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Logging.Common.Models
{
	/// <summary>
	/// Common interface for StatisticValueType.
	/// </summary>
	[ContractClass(typeof(Contracts.StatisticValueTypeContracts))]
	public interface IStatisticValueType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StatisticValueTypeID for this StatisticValueType.
		/// </summary>
		short StatisticValueTypeID { get; set; }
	
		/// <summary>
		/// The Name for this StatisticValueType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this StatisticValueType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this StatisticValueType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this StatisticValueType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The StatisticValues for this StatisticValueType.
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
		[ContractClassFor(typeof(IStatisticValueType))]
		internal abstract class StatisticValueTypeContracts : IStatisticValueType
		{
		    #region Primitive properties
		
			short IStatisticValueType.StatisticValueTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticValueType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticValueType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStatisticValueType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IStatisticValueType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IStatisticValue> IStatisticValueType.StatisticValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IStatisticValueType.AddStatisticValue(IStatisticValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IStatisticValueType.RemoveStatisticValue(IStatisticValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
