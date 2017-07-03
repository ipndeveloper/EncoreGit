using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Payments.Common.Models
{
	/// <summary>
	/// Common interface for PaymentOrderType.
	/// </summary>
	[ContractClass(typeof(Contracts.PaymentOrderTypeContracts))]
	public interface IPaymentOrderType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PaymentOrderTypeID for this PaymentOrderType.
		/// </summary>
		int PaymentOrderTypeID { get; set; }
	
		/// <summary>
		/// The OrderTypeID for this PaymentOrderType.
		/// </summary>
		short OrderTypeID { get; set; }
	
		/// <summary>
		/// The PaymentTypeID for this PaymentOrderType.
		/// </summary>
		int PaymentTypeID { get; set; }
	
		/// <summary>
		/// The CountryID for this PaymentOrderType.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The PaymentGatewayID for this PaymentOrderType.
		/// </summary>
		short PaymentGatewayID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPaymentOrderType))]
		internal abstract class PaymentOrderTypeContracts : IPaymentOrderType
		{
		    #region Primitive properties
		
			int IPaymentOrderType.PaymentOrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IPaymentOrderType.OrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPaymentOrderType.PaymentTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPaymentOrderType.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IPaymentOrderType.PaymentGatewayID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
