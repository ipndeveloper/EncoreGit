using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace UPSQuery.Common
{
	[DTO]
	public interface IUPSQueryProcessorResult
	{
		string CustomerName { get; set; }
		string Address1 { get; set; }
		string Address2 { get; set; }
		string Address3 { get; set; }
		string Country { get; set; }
		string PostalCode { get; set; }
		string City { get; set; }
		string State { get; set; }
		string Telephone { get; set; }
	}

	public interface IUPSQueryProcessor
	{
		IUPSQueryProcessorResult GetUPSOrderShipmentInfo(int orderNumber);
		void SetUPSOrderShippingTrackingNumber(int shipmentPackageID, string orderTrackingNumber);
		List<int> GetOrderShipmentIDsToUpdate();
	}
}
