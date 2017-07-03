using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for OverrideType.
	/// </summary>
	[ContractClass(typeof(Contracts.OverrideTypeContracts))]
	public interface IOverrideType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OverrideTypeID for this OverrideType.
		/// </summary>
		int OverrideTypeID { get; set; }
	
		/// <summary>
		/// The OverrideCode for this OverrideType.
		/// </summary>
		string OverrideCode { get; set; }
	
		/// <summary>
		/// The Description for this OverrideType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Operator for this OverrideType.
		/// </summary>
		string Operator { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CalculationOverrides for this OverrideType.
		/// </summary>
		IEnumerable<ICalculationOverride> CalculationOverrides { get; }
	
		/// <summary>
		/// Adds an <see cref="ICalculationOverride"/> to the CalculationOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculationOverride"/> to add.</param>
		void AddCalculationOverride(ICalculationOverride item);
	
		/// <summary>
		/// Removes an <see cref="ICalculationOverride"/> from the CalculationOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculationOverride"/> to remove.</param>
		void RemoveCalculationOverride(ICalculationOverride item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOverrideType))]
		internal abstract class OverrideTypeContracts : IOverrideType
		{
		    #region Primitive properties
		
			int IOverrideType.OverrideTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOverrideType.OverrideCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOverrideType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOverrideType.Operator
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICalculationOverride> IOverrideType.CalculationOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOverrideType.AddCalculationOverride(ICalculationOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOverrideType.RemoveCalculationOverride(ICalculationOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
