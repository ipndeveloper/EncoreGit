using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Payments.Common.Models
{
	/// <summary>
	/// Common interface for GiftCard.
	/// </summary>
	[ContractClass(typeof(Contracts.GiftCardContracts))]
	public interface IGiftCard
	{
	    #region Primitive properties
	
		/// <summary>
		/// The GiftCardID for this GiftCard.
		/// </summary>
		int GiftCardID { get; set; }
	
		/// <summary>
		/// The Code for this GiftCard.
		/// </summary>
		string Code { get; set; }
	
		/// <summary>
		/// The InitialAmount for this GiftCard.
		/// </summary>
		Nullable<decimal> InitialAmount { get; set; }
	
		/// <summary>
		/// The Balance for this GiftCard.
		/// </summary>
		Nullable<decimal> Balance { get; set; }
	
		/// <summary>
		/// The ExpirationDate for this GiftCard.
		/// </summary>
		Nullable<System.DateTime> ExpirationDate { get; set; }
	
		/// <summary>
		/// The CurrencyID for this GiftCard.
		/// </summary>
		Nullable<int> CurrencyID { get; set; }
	
		/// <summary>
		/// The OriginOrderItemID for this GiftCard.
		/// </summary>
		Nullable<int> OriginOrderItemID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IGiftCard))]
		internal abstract class GiftCardContracts : IGiftCard
		{
		    #region Primitive properties
		
			int IGiftCard.GiftCardID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IGiftCard.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IGiftCard.InitialAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IGiftCard.Balance
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IGiftCard.ExpirationDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IGiftCard.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IGiftCard.OriginOrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
