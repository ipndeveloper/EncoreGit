using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementDetail.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementDetailContracts))]
	public interface IDisbursementDetail
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementDetailID for this DisbursementDetail.
		/// </summary>
		int DisbursementDetailID { get; set; }
	
		/// <summary>
		/// The DisbursementID for this DisbursementDetail.
		/// </summary>
		int DisbursementID { get; set; }
	
		/// <summary>
		/// The DisbursementTypeID for this DisbursementDetail.
		/// </summary>
		int DisbursementTypeID { get; set; }
	
		/// <summary>
		/// The Percentage for this DisbursementDetail.
		/// </summary>
		Nullable<decimal> Percentage { get; set; }
	
		/// <summary>
		/// The DisbursementProfileID for this DisbursementDetail.
		/// </summary>
		Nullable<int> DisbursementProfileID { get; set; }
	
		/// <summary>
		/// The DisbursementStatusID for this DisbursementDetail.
		/// </summary>
		int DisbursementStatusID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Disbursement for this DisbursementDetail.
		/// </summary>
	    IDisbursement Disbursement { get; set; }
	
		/// <summary>
		/// The DisbursementProfile for this DisbursementDetail.
		/// </summary>
	    IDisbursementProfile DisbursementProfile { get; set; }
	
		/// <summary>
		/// The DisbursementStatus for this DisbursementDetail.
		/// </summary>
	    IDisbursementStatus DisbursementStatus { get; set; }
	
		/// <summary>
		/// The DisbursementType for this DisbursementDetail.
		/// </summary>
	    IDisbursementType DisbursementType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementDetail))]
		internal abstract class DisbursementDetailContracts : IDisbursementDetail
		{
		    #region Primitive properties
		
			int IDisbursementDetail.DisbursementDetailID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementDetail.DisbursementID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementDetail.DisbursementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IDisbursementDetail.Percentage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursementDetail.DisbursementProfileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementDetail.DisbursementStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDisbursement IDisbursementDetail.Disbursement
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursementProfile IDisbursementDetail.DisbursementProfile
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursementStatus IDisbursementDetail.DisbursementStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursementType IDisbursementDetail.DisbursementType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
