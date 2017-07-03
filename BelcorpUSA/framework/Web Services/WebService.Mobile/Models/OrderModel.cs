using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;

namespace NetSteps.WebService.Mobile.Models
{
	public class OrderModel
	{
		public string date;
		public int id;
		public string number;
		public string name;
		public int period;
		public string pv;
		public string total;
		public string type;
		public string status;
		public string completedate;
		public string commissiondate;
		public string partydate;
		public string partyenddate;
		public string hostess;
		public PartyDetailModel partydetail;
		public List<string> trackingnumbers;
		public List<string> trackingurls;

		/*public static implicit operator OrderModel(Order order)
		{
			string date = string.Empty, completeDate = string.Empty, commissionDate = string.Empty, shipdate = string.Empty;
			if (order.CompleteDate > DateTime.MinValue)
				date = completeDate = order.CompleteDate.ToShortDateString();
			else
				completeDate = "N/A";
			if (order.CommissionDate > DateTime.MinValue)
				commissionDate = order.CommissionDate.ToShortDateString();
			else
				commissionDate = "N/A";
			if (date.IsNullOrEmpty())
				date = order.DateCreated.ToShortDateString();

			string name = "N/A";
			if (order.OrderCustomers.Count > 0)
				name = order.OrderCustomers[0].FullName;

			var trackingNumbers = new List<string>();
			var trackingUrls = new List<string>();
			if (order.OrderStatusID == (int)Constants.OrderStatus.Shipped)
			{
				order.OrderShipments.Each(os =>
				{
					os.OrderShipmentPackages.DistinctBy(osp => osp.TrackingNumber).Each(osp =>
					{
						trackingNumbers.Add(osp.TrackingNumber);
                        string url = osp.ShippingMethodID.HasValue ? string.Format(SmallCollectionCache.Instance.ShippingMethods.GetById(osp.ShippingMethodID.Value).TrackingNumberBaseUrl ?? string.Empty, osp.TrackingNumber) : osp.TrackingNumber;
						trackingUrls.Add(url);
					});
				});
			//    order.OrderShipments.Each(os =>
			//    {
			//        os.TrackingNumbers.Each(tn =>
			//        {
			//            trackingNumbers.Add(tn);
			//            trackingUrls.Add(OrderShipment.GetTrackingURL(tn));
			//        });
			//    });
			}

			var prv = order.CommissionableTotal.HasValue ? order.CommissionableTotal.Value : 0m;
			var total = order.GrandTotal.HasValue ? order.GrandTotal.Value : 0m;
			var status = ((Constants.OrderStatus)order.OrderStatusID).PascalToSpaced();
			var type = ((Constants.OrderType)order.OrderTypeID).PascalToSpaced();
			
			var model = new OrderModel
			{
				date = date,
				id = order.OrderID,
				number = order.OrderNumber,
				name = name,
				pv = prv.ToString("0.00"),
				total = total.ToString("0.00"),
				commissiondate = commissionDate,
				completedate = completeDate,
				status = status,
				trackingnumbers = trackingNumbers,
				trackingurls = trackingUrls,
				type = type
			};

			//var type = (Constants.OrderType)order.OrderTypeId;
			//model.type = type.ToString().PascalToSpaced();

			return model;
		}*/

		public static implicit operator OrderModel(NetSteps.Data.Entities.Business.OrderSearchData orderData)
		{
			string date = string.Empty, completeDate = string.Empty, commissionDate = string.Empty, shipdate = string.Empty;
			if (orderData.CompleteDate > DateTime.MinValue)
				date = completeDate = orderData.CompleteDate.ToShortDateString();
			else
				completeDate = "N/A";
			if (orderData.CommissionDate > DateTime.MinValue)
				commissionDate = orderData.CommissionDate.ToShortDateString();
			else
				commissionDate = "N/A";
			if (date.IsNullOrEmpty())
				date = orderData.DateCreated.ToShortDateString();

			string name = string.Join(" ", orderData.FirstName, orderData.LastName);
			if (name.Length < 2)
				name = "N/A";
			
			var model = new OrderModel
			{
				date = date,
				id = orderData.OrderID,
				number = orderData.OrderNumber,
				name = name,
                pv = orderData.CommissionableTotal.ToString("0.00"),
				total = orderData.GrandTotal.ToString("0.00"),
				commissiondate = commissionDate,
				completedate = completeDate,
				status = orderData.OrderStatus,
				type = orderData.OrderType,
                trackingnumbers = new List<string>(),
			    trackingurls = new List<string>()
			};

            if (!orderData.TrackingNumber.IsNullOrWhiteSpace())
                model.trackingnumbers.Add(orderData.TrackingNumber);
            if (!orderData.TrackingUrl.IsNullOrWhiteSpace())
                model.trackingurls.Add(orderData.TrackingUrl);

			return model;
		}

		//public static implicit operator OrderModel(PendingOrder order)
		//{
		//    string date = string.Empty, partyDate = string.Empty, partyEndDate = string.Empty;
		//    if (order.PartyDate.HasValue)
		//        date = partyDate = order.PartyDate.ToShortDateString();
		//    else
		//        partyDate = "N/A";
		//    partyEndDate = order.PartyEndDate.ToShortDateString();
		//    if (date.IsNullOrEmpty())
		//        date = order.DateCreated.ToShortDateString();

		//    var type = (Constants.OrderType)order.OrderTypeID;
			
		//    var model = new OrderModel
		//    {
		//        date = date,
		//        name = order.Name,
		//        type = type.ToString().PascalToSpaced(),
		//        partydate = partyDate,
		//        partyenddate = partyEndDate,
		//        hostess = order.Hostess
		//    };

		//    return model;
		//}
	}
}