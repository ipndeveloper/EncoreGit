using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Payments.Common.Models
{
	/// <summary>
	/// Common interface for PaymentGateway.
	/// </summary>
	[ContractClass(typeof(Contracts.PaymentGatewayContracts))]
	public interface IPaymentGateway
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PaymentGatewayID for this PaymentGateway.
		/// </summary>
		short PaymentGatewayID { get; set; }
	
		/// <summary>
		/// The Name for this PaymentGateway.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this PaymentGateway.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this PaymentGateway.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Namespace for this PaymentGateway.
		/// </summary>
		string Namespace { get; set; }
	
		/// <summary>
		/// The Active for this PaymentGateway.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPaymentGateway))]
		internal abstract class PaymentGatewayContracts : IPaymentGateway
		{
		    #region Primitive properties
		
			short IPaymentGateway.PaymentGatewayID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentGateway.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentGateway.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentGateway.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentGateway.Namespace
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentGateway.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
