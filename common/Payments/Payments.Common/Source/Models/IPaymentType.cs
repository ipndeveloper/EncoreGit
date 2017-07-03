using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Payments.Common.Models
{
	/// <summary>
	/// Common interface for PaymentType.
	/// </summary>
	[ContractClass(typeof(Contracts.PaymentTypeContracts))]
	public interface IPaymentType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PaymentTypeID for this PaymentType.
		/// </summary>
		int PaymentTypeID { get; set; }
	
		/// <summary>
		/// The Name for this PaymentType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this PaymentType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this PaymentType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The PaymentTypeCode for this PaymentType.
		/// </summary>
		string PaymentTypeCode { get; set; }
	
		/// <summary>
		/// The IsCreditCard for this PaymentType.
		/// </summary>
		bool IsCreditCard { get; set; }
	
		/// <summary>
		/// The IsCash for this PaymentType.
		/// </summary>
		bool IsCash { get; set; }
	
		/// <summary>
		/// The IsCheck for this PaymentType.
		/// </summary>
		bool IsCheck { get; set; }
	
		/// <summary>
		/// The IsEFT for this PaymentType.
		/// </summary>
		bool IsEFT { get; set; }
	
		/// <summary>
		/// The IsGiftCard for this PaymentType.
		/// </summary>
		bool IsGiftCard { get; set; }
	
		/// <summary>
		/// The Active for this PaymentType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The FunctionName for this PaymentType.
		/// </summary>
		string FunctionName { get; set; }
	
		/// <summary>
		/// The CanPayForShippingAndHandling for this PaymentType.
		/// </summary>
		bool CanPayForShippingAndHandling { get; set; }
	
		/// <summary>
		/// The CanPayForTax for this PaymentType.
		/// </summary>
		bool CanPayForTax { get; set; }
	
		/// <summary>
		/// The IsCommissionable for this PaymentType.
		/// </summary>
		bool IsCommissionable { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPaymentType))]
		internal abstract class PaymentTypeContracts : IPaymentType
		{
		    #region Primitive properties
		
			int IPaymentType.PaymentTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentType.PaymentTypeCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.IsCreditCard
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.IsCash
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.IsCheck
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.IsEFT
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.IsGiftCard
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPaymentType.FunctionName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.CanPayForShippingAndHandling
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.CanPayForTax
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPaymentType.IsCommissionable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
