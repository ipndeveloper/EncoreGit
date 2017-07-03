using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for CalculationType.
	/// </summary>
	[ContractClass(typeof(Contracts.CalculationTypeContracts))]
	public interface ICalculationType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CalculationTypeID for this CalculationType.
		/// </summary>
		int CalculationTypeID { get; set; }
	
		/// <summary>
		/// The Code for this CalculationType.
		/// </summary>
		string Code { get; set; }
	
		/// <summary>
		/// The Name for this CalculationType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The UserOverridable for this CalculationType.
		/// </summary>
		bool UserOverridable { get; set; }
	
		/// <summary>
		/// The RealTime for this CalculationType.
		/// </summary>
		bool RealTime { get; set; }
	
		/// <summary>
		/// The TermName for this CalculationType.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CalculationOverrides for this CalculationType.
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
	
		/// <summary>
		/// The Calculations for this CalculationType.
		/// </summary>
		IEnumerable<ICalculation> Calculations { get; }
	
		/// <summary>
		/// Adds an <see cref="ICalculation"/> to the Calculations collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculation"/> to add.</param>
		void AddCalculation(ICalculation item);
	
		/// <summary>
		/// Removes an <see cref="ICalculation"/> from the Calculations collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculation"/> to remove.</param>
		void RemoveCalculation(ICalculation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICalculationType))]
		internal abstract class CalculationTypeContracts : ICalculationType
		{
		    #region Primitive properties
		
			int ICalculationType.CalculationTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICalculationType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICalculationType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICalculationType.UserOverridable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICalculationType.RealTime
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICalculationType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICalculationOverride> ICalculationType.CalculationOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICalculationType.AddCalculationOverride(ICalculationOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICalculationType.RemoveCalculationOverride(ICalculationOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICalculation> ICalculationType.Calculations
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICalculationType.AddCalculation(ICalculation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICalculationType.RemoveCalculation(ICalculation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
