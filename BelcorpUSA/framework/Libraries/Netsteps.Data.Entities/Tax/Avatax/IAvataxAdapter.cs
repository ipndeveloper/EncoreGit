using System;
using System.Collections.Generic;
using System.Data;

using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.TaxService;

using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities.Tax.Avatax;

namespace NetSteps.Data.Entities.AvataxAPI
{
	public interface IAvataxAdapter : IDisposable
	{
		string PingResultCode(PingResult pingResult);

		/// <summary>
		/// 
		/// </summary>
		string PingResultMessage(PingResult pingResult);

		bool IsGetTaxRequestSuccess(GetTaxResult getTaxResult);

		string GetTaxResultCode(GetTaxResult getTaxResult);

		string GetTaxResultMessage(GetTaxResult getTaxResult);

		bool IsPostTaxRequestSuccess(PostTaxResult postTaxResult);

		string PostTaxResultCode(PostTaxResult postTaxResult);

		string PostTaxResultMessage(PostTaxResult postTaxResult);

		bool IsCommitTaxRequestSuccess(CommitTaxResult commitTaxResult);

		string CommitTaxResultCode(CommitTaxResult commitTaxResult);

		string CommitTaxResultMessage(CommitTaxResult commitTaxResult);

		bool IsAdjustTaxRequestSuccess(AdjustTaxResult adjustTaxResult);

		string AdjustTaxResultCode(AdjustTaxResult adjustTaxResult);

		string AdjustTaxResultMessage(AdjustTaxResult adjustTaxResult);

		/// <summary>
		/// Default Origin address saved in config file
		/// </summary>
		Avalara.AvaTax.Adapter.AddressService.Address OriginAddressDefault { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="detailLevel">Summary = 0, Document = 1, Line = 2, Tax = 3, Diagnostic = 4</param>
		/// <param name="pingBeforeRequest"></param>
		/// <param name="taxRequestValues"></param>
		GetTaxResult GetQuote(OrderCustomer orderCustomer, int detailLevel, bool pingBeforeRequest, Dictionary<string, string> taxRequestValues, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU, AvataxCalculationInfo avataxCalculationInfo);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="detailLevel">Summary = 0, Document = 1, Line = 2, Tax = 3, Diagnostic = 4</param>
		/// <param name="commit"></param>
		/// <param name="pingBeforeRequest"></param>
		/// <param name="taxRequestValues"></param>
		GetTaxResult SaveInvoice(OrderCustomer orderCustomer, int detailLevel, bool commit, bool pingBeforeRequest, Dictionary<string, string> taxRequestValues, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU);

		/// <summary>
		/// Setup GetTaxRequest with data to be sent to Avatax
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="documentType"></param>
		/// <param name="detailLevel"></param>
		/// <param name="commit"></param>
		GetTaxRequest SetUpTaxRequestData(OrderCustomer orderCustomer, OrderShipment shipment, string orderId, DocumentType documentType, DetailLevel detailLevel, bool commit, Dictionary<string, string> taxRequestValues, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU, Dictionary<string, string> _itemCodeIndexes, DataTable _data, IAddress _originAddress);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		GetTaxResult SendGetTaxRequest(GetTaxRequest getTaxRequest);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		GetTaxResult SendGetTaxRequestNoPing(GetTaxRequest getTaxRequest);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool PingSuccess(PingResult pingResult);

		PingResult Ping();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Avalara.AvaTax.Adapter.AddressService.Address GetOriginAddressDefault();

		/// <summary>
		/// PostTax [to succeed, the document must be in a state of saved.]
		/// </summary>
		/// <param name="docCode">orderId</param>
		/// <param name="totalAmount">total taxable amount</param>
		/// <param name="totalTax">total tax</param>
		/// <param name="newDocCode">to change the docCode</param>
		/// <param name="commit"></param>
		PostTaxResult PostTax(string docCode, decimal totalAmount, decimal totalTax, string newDocCode, bool commit);

		/// <summary>
		/// send postTax request
		/// </summary>
		/// <param name="postTaxRequest"></param>
		PostTaxResult SendPostTaxRequest(PostTaxRequest postTaxRequest);

		/// <summary>
		/// Commiting tax document [to succeed, the document must be in a state of posted]
		/// </summary>
		/// <param name="docCode"></param>
		/// <param name="newDocCode"></param>
		CommitTaxResult CommitTax(string docCode, string newDocCode);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="commitTaxRequest"></param>
		CommitTaxResult SendCommitTaxRequest(CommitTaxRequest commitTaxRequest);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docCode"></param>
		/// <param name="cancelCode"></param>
		CancelTaxResult CancelTax(string docCode, int cancelCode, int documentType);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancelTaxRequest"></param>
		CancelTaxResult SendCancelTaxRequest(CancelTaxRequest cancelTaxRequest);

		/// <summary>
		/// Handling Return Invoices
		/// Assumptions:
		/// The document in question has already been committed and tax remitted to the tax jurisdictions.
		/// There may be multiple lines in the document.
		/// A complete or partial refund is the expected outcome.
		/// </summary>
		GetTaxResult ReturnInvoice(Order order, Order parentOrder, OrderCustomer orderCustomer, Dictionary<string, string> taxRequestValues, bool commit);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancelTaxRequest"></param>
		AdjustTaxResult SendAdjustTaxRequest(AdjustTaxRequest adjustTaxRequest);
	}
}