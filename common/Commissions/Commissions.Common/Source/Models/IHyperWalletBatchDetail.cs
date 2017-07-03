using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for HyperWalletBatchDetail.
	/// </summary>
	[ContractClass(typeof(Contracts.HyperWalletBatchDetailContracts))]
	public interface IHyperWalletBatchDetail
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HyperWalletBatchDetailID for this HyperWalletBatchDetail.
		/// </summary>
		int HyperWalletBatchDetailID { get; set; }
	
		/// <summary>
		/// The HyperWalletBatchID for this HyperWalletBatchDetail.
		/// </summary>
		Nullable<int> HyperWalletBatchID { get; set; }
	
		/// <summary>
		/// The HyperWalletBatchDetailStatusID for this HyperWalletBatchDetail.
		/// </summary>
		int HyperWalletBatchDetailStatusID { get; set; }
	
		/// <summary>
		/// The HyperWalletProfileID for this HyperWalletBatchDetail.
		/// </summary>
		int HyperWalletProfileID { get; set; }
	
		/// <summary>
		/// The OperationCode for this HyperWalletBatchDetail.
		/// </summary>
		int OperationCode { get; set; }
	
		/// <summary>
		/// The ConsultantId for this HyperWalletBatchDetail.
		/// </summary>
		string ConsultantId { get; set; }
	
		/// <summary>
		/// The FirstName for this HyperWalletBatchDetail.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this HyperWalletBatchDetail.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The Birthday for this HyperWalletBatchDetail.
		/// </summary>
		System.DateTime Birthday { get; set; }
	
		/// <summary>
		/// The Street for this HyperWalletBatchDetail.
		/// </summary>
		string Street { get; set; }
	
		/// <summary>
		/// The City for this HyperWalletBatchDetail.
		/// </summary>
		string City { get; set; }
	
		/// <summary>
		/// The StateProvince for this HyperWalletBatchDetail.
		/// </summary>
		string StateProvince { get; set; }
	
		/// <summary>
		/// The PostalCode for this HyperWalletBatchDetail.
		/// </summary>
		string PostalCode { get; set; }
	
		/// <summary>
		/// The CountryCode3 for this HyperWalletBatchDetail.
		/// </summary>
		string CountryCode3 { get; set; }
	
		/// <summary>
		/// The DoAllowWebAccess for this HyperWalletBatchDetail.
		/// </summary>
		bool DoAllowWebAccess { get; set; }
	
		/// <summary>
		/// The DoAttachCard for this HyperWalletBatchDetail.
		/// </summary>
		bool DoAttachCard { get; set; }
	
		/// <summary>
		/// The EmailAddress for this HyperWalletBatchDetail.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The PhoneMain for this HyperWalletBatchDetail.
		/// </summary>
		string PhoneMain { get; set; }
	
		/// <summary>
		/// The PhoneCell for this HyperWalletBatchDetail.
		/// </summary>
		string PhoneCell { get; set; }
	
		/// <summary>
		/// The PhoneFax for this HyperWalletBatchDetail.
		/// </summary>
		string PhoneFax { get; set; }
	
		/// <summary>
		/// The TaxNumber for this HyperWalletBatchDetail.
		/// </summary>
		string TaxNumber { get; set; }
	
		/// <summary>
		/// The CurrencyCode3 for this HyperWalletBatchDetail.
		/// </summary>
		string CurrencyCode3 { get; set; }
	
		/// <summary>
		/// The PaymentAmount for this HyperWalletBatchDetail.
		/// </summary>
		Nullable<decimal> PaymentAmount { get; set; }
	
		/// <summary>
		/// The PaymentTitle for this HyperWalletBatchDetail.
		/// </summary>
		string PaymentTitle { get; set; }
	
		/// <summary>
		/// The PaymentDescription for this HyperWalletBatchDetail.
		/// </summary>
		string PaymentDescription { get; set; }
	
		/// <summary>
		/// The PrivateNotes for this HyperWalletBatchDetail.
		/// </summary>
		string PrivateNotes { get; set; }
	
		/// <summary>
		/// The ResponseCode for this HyperWalletBatchDetail.
		/// </summary>
		string ResponseCode { get; set; }
	
		/// <summary>
		/// The ResponseTransaction for this HyperWalletBatchDetail.
		/// </summary>
		Nullable<int> ResponseTransaction { get; set; }
	
		/// <summary>
		/// The ResponseText for this HyperWalletBatchDetail.
		/// </summary>
		string ResponseText { get; set; }
	
		/// <summary>
		/// The DataVersion for this HyperWalletBatchDetail.
		/// </summary>
		byte[] DataVersion { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHyperWalletBatchDetail))]
		internal abstract class HyperWalletBatchDetailContracts : IHyperWalletBatchDetail
		{
		    #region Primitive properties
		
			int IHyperWalletBatchDetail.HyperWalletBatchDetailID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHyperWalletBatchDetail.HyperWalletBatchID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHyperWalletBatchDetail.HyperWalletBatchDetailStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHyperWalletBatchDetail.HyperWalletProfileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHyperWalletBatchDetail.OperationCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.ConsultantId
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IHyperWalletBatchDetail.Birthday
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.Street
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.City
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.StateProvince
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PostalCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.CountryCode3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHyperWalletBatchDetail.DoAllowWebAccess
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHyperWalletBatchDetail.DoAttachCard
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PhoneMain
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PhoneCell
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PhoneFax
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.TaxNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.CurrencyCode3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IHyperWalletBatchDetail.PaymentAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PaymentTitle
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PaymentDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.PrivateNotes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.ResponseCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHyperWalletBatchDetail.ResponseTransaction
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHyperWalletBatchDetail.ResponseText
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IHyperWalletBatchDetail.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
