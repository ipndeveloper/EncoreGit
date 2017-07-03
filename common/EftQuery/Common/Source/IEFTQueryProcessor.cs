using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace EFTQuery.Common
{
    [DTO]
    public interface IEFTQueryProcessorResult
    {
        string RecordTypeCode { get; set; }
        string TransactionCode { get; set; }
        string RoutingNumber { get; set; }
        string AccountNumber { get; set; }
        string TransactionDate { get; set; }
        string OrderId { get; set; }
        string OrderPaymentId { get; set; }
        string CountryCode { get; set; }
        string Amount { get; set; }
        string IndividualName { get; set; }
        string PaymentTypeCode { get; set; }
        string TransactionNumber { get; set; }
        string NachaClassType { get; set; }
        string BankAccountType { get; set; }
    }
	
	[DTO]
	public interface IOrderPaymentUpdateResult
	{
		int OrderPaymentId { get; set; }
		bool Success { get; set; }
		string Message { get; set; }
	}

    public interface IEFTQueryProcessor
    {
        IEnumerable<IEFTQueryProcessorResult> GetTransfersByClassType(string classType);
        IEnumerable<IEFTQueryProcessorResult> GetTransfersByClassTypeAndCountryID(string classType, int countryID);
        IEFTQueryProcessorResult GetTransferByOrderId(int orderId);
		IEnumerable<IOrderPaymentUpdateResult> UpdateNachaQueryProcessorResults(List<int> orderPaymentIds);
    }
}
