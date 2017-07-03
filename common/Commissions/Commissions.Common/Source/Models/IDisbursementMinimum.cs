using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementMinimum.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementMinimumContracts))]
	public interface IDisbursementMinimum
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CountryID for this DisbursementMinimum.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The MinimumAmount for this DisbursementMinimum.
		/// </summary>
		decimal MinimumAmount { get; set; }
	
		/// <summary>
		/// The CurrencyTypeID for this DisbursementMinimum.
		/// </summary>
		int CurrencyTypeID { get; set; }
	
		/// <summary>
		/// The DisbursementTypeID for this DisbursementMinimum.
		/// </summary>
		int DisbursementTypeID { get; set; }
	
		/// <summary>
		/// The DisbursementMinimumID for this DisbursementMinimum.
		/// </summary>
		int DisbursementMinimumID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DisbursementType for this DisbursementMinimum.
		/// </summary>
	    IDisbursementType DisbursementType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementMinimum))]
		internal abstract class DisbursementMinimumContracts : IDisbursementMinimum
		{
		    #region Primitive properties
		
			int IDisbursementMinimum.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IDisbursementMinimum.MinimumAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementMinimum.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementMinimum.DisbursementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementMinimum.DisbursementMinimumID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDisbursementType IDisbursementMinimum.DisbursementType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
