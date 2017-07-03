using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for Calculation.
	/// </summary>
	[ContractClass(typeof(Contracts.CalculationContracts))]
	public interface ICalculation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this Calculation.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The Value for this Calculation.
		/// </summary>
		decimal Value { get; set; }
	
		/// <summary>
		/// The PeriodID for this Calculation.
		/// </summary>
		int PeriodID { get; set; }
	
		/// <summary>
		/// The CalculationTypeID for this Calculation.
		/// </summary>
		int CalculationTypeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this Calculation.
		/// </summary>
	    ICommissionsAccount Account { get; set; }
	
		/// <summary>
		/// The CalculationType for this Calculation.
		/// </summary>
	    ICalculationType CalculationType { get; set; }
	
		/// <summary>
		/// The Period for this Calculation.
		/// </summary>
	    IPeriod Period { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICalculation))]
		internal abstract class CalculationContracts : ICalculation
		{
		    #region Primitive properties
		
			int ICalculation.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal ICalculation.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICalculation.PeriodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICalculation.CalculationTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICommissionsAccount ICalculation.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ICalculationType ICalculation.CalculationType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IPeriod ICalculation.Period
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
