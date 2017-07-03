using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for CheckHold.
	/// </summary>
	[ContractClass(typeof(Contracts.CheckHoldContracts))]
	public interface ICheckHold
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CheckHoldID for this CheckHold.
		/// </summary>
		int CheckHoldID { get; set; }
	
		/// <summary>
		/// The AccountID for this CheckHold.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The HoldUntil for this CheckHold.
		/// </summary>
		Nullable<System.DateTime> HoldUntil { get; set; }
	
		/// <summary>
		/// The CreatedDate for this CheckHold.
		/// </summary>
		System.DateTime CreatedDate { get; set; }
	
		/// <summary>
		/// The UserID for this CheckHold.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The ReasonID for this CheckHold.
		/// </summary>
		int ReasonID { get; set; }
	
		/// <summary>
		/// The StartDate for this CheckHold.
		/// </summary>
		System.DateTime StartDate { get; set; }
	
		/// <summary>
		/// The Notes for this CheckHold.
		/// </summary>
		string Notes { get; set; }
	
		/// <summary>
		/// The ApplicationSourceID for this CheckHold.
		/// </summary>
		Nullable<int> ApplicationSourceID { get; set; }
	
		/// <summary>
		/// The UpdatedDate for this CheckHold.
		/// </summary>
		System.DateTime UpdatedDate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this CheckHold.
		/// </summary>
	    ICommissionsAccount Account { get; set; }
	
		/// <summary>
		/// The OverrideReason for this CheckHold.
		/// </summary>
	    IOverrideReason OverrideReason { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICheckHold))]
		internal abstract class CheckHoldContracts : ICheckHold
		{
		    #region Primitive properties
		
			int ICheckHold.CheckHoldID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICheckHold.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICheckHold.HoldUntil
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICheckHold.CreatedDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICheckHold.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICheckHold.ReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICheckHold.StartDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICheckHold.Notes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICheckHold.ApplicationSourceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICheckHold.UpdatedDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICommissionsAccount ICheckHold.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOverrideReason ICheckHold.OverrideReason
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
