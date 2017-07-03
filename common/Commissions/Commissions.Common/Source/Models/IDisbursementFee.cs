using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementFee.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementFeeContracts))]
	public interface IDisbursementFee
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementFeeTypeID for this DisbursementFee.
		/// </summary>
		int DisbursementFeeTypeID { get; set; }
	
		/// <summary>
		/// The CountryID for this DisbursementFee.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The Amount for this DisbursementFee.
		/// </summary>
		double Amount { get; set; }
	
		/// <summary>
		/// The CurrencyTypeID for this DisbursementFee.
		/// </summary>
		Nullable<int> CurrencyTypeID { get; set; }
	
		/// <summary>
		/// The DisbursementFeeID for this DisbursementFee.
		/// </summary>
		int DisbursementFeeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DisbursementFeeType for this DisbursementFee.
		/// </summary>
	    IDisbursementFeeType DisbursementFeeType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementFee))]
		internal abstract class DisbursementFeeContracts : IDisbursementFee
		{
		    #region Primitive properties
		
			int IDisbursementFee.DisbursementFeeTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementFee.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			double IDisbursementFee.Amount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursementFee.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementFee.DisbursementFeeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDisbursementFeeType IDisbursementFee.DisbursementFeeType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
