using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ParcelPortIntegrationService
{

    static class OrderActions
    {
        public static List<VwParcelPortOrder> GetOrdersToShip(ParcelPortDataContext context)
        {
            int defaultCommandTimeout = context.CommandTimeout;

            IncreaseCommandTimeout(context);

            var result = context.uspLogisticsGetOrdersParcelPort().ToList();

            ResetCommandTimeoutToDefault(context, defaultCommandTimeout);

            return result;
        }

        private static void IncreaseCommandTimeout(ParcelPortDataContext context)
        {
            context.CommandTimeout = ConfigurationHelper.CommandTimeoutPeriod();
        }

        private static void ResetCommandTimeoutToDefault(ParcelPortDataContext context, int defaultTimeoutDuration)
        {
            context.CommandTimeout = defaultTimeoutDuration;
        }

        public static List<OrderItem> GetOrderLinesForOrder(ParcelPortDataContext context, Int32 OrderCustomerID)
        {
            return context.uspLogisticsGetOrderItemsParcelPort(OrderCustomerID).ToList();
        }

        public static XDocument GetOrderXML(VwParcelPortOrder order, List<OrderItem> orderItems)
        {
            XElement OrderAcceptanceRequest = new XElement("OrderAcceptanceRequest");

            var OrderHeader = new XElement("OrderHeader",
                new XElement("Owner", ConfigurationHelper.GetParcelPortOwner()),
                new XElement("ReferenceNum", order.OrderNumber),
                new XElement("OrderDate", order.OrderDate),
                new XElement("Carrier", order.Carrier),
                new XElement("CustomerNum", order.CustomerNum.ToString()),
                new XElement("Payment", order.Payment),
                new XElement("OrderClass", order.OrderClass),
                new XElement("SubTotal", order.SubTotal.Value.ToString("0.00")),
                new XElement("Tax", order.Tax.Value.ToString("0.00")),
                new XElement("CustomerPO", order.LifeTimeRanking.ToValidString()),
                new XElement("Shipping", order.Shipping.Value.ToString("0.00")),
                new XElement("Total", order.Total.Value.ToString("0.00")));

            var BillToAddress = new XElement("Bill-toAddress",
                new XElement("Company"),
                new XElement("Attention", order.BillToAttention),
                new XElement("Address1", order.BillToAddress1),
                new XElement("Address2", order.BillToAddress2),
                new XElement("City", order.BillToCity),
                new XElement("State", order.BillToState),
                new XElement("PostalCode", order.BillToPostalCode),
                new XElement("Country", order.BillToCountry),
                new XElement("Phone", order.BillToPhone),
                new XElement("Fax"),
                new XElement("Email"));

            var ShipToAddress = new XElement("Ship-toAddress",
                new XElement("Company"),
                new XElement("Attention", order.ShipToAttention),
                new XElement("Address1", order.ShipToAddress1),
                new XElement("Address2", order.ShipToAddress2),
                new XElement("City", order.ShipToCity),
                new XElement("State", order.ShipToState),
                new XElement("PostalCode", order.ShipToPostalCode),
                new XElement("Country", order.ShipToCountry),
                new XElement("Phone", order.ShipToPhone),
                new XElement("Fax"),
                new XElement("Email"));

            var OrderLines = new XElement("OrderLines");
            foreach (OrderItem item in orderItems)
            {
                OrderLines.Add(new XElement("OrderLine",
                    new XElement("Item", ConfigurationHelper.GetItemPrefix() + item.SKU),
                    new XElement("Quantity", item.Quantity.ToString()),
                    new XElement("UOM", "EA"),
                    new XElement("Price", item.ItemPrice.ToString("0.00")),
                    new XElement("ExtendedPrice", item.AdjustedPrice.Value.ToString("0.00"))));
            }

            OrderAcceptanceRequest.Add(OrderHeader);
            OrderAcceptanceRequest.Add(BillToAddress);
            OrderAcceptanceRequest.Add(ShipToAddress);
            OrderAcceptanceRequest.Add(OrderLines);

            XDocument xdoc = new XDocument(OrderAcceptanceRequest);

            return xdoc;
        }


        public static bool SaveOrderShipment(ParcelPortDataContext dc, String OrderNumber, String TrackingNumber, String TrackingUrl)
        {
            if (TrackingNumber.Length > 50)
                TrackingNumber = TrackingNumber.Substring(0, 50);

            ParcelPortShipment pps = new ParcelPortShipment
                                         {
                                             OrderNumber = OrderNumber,
                                             TrackingNumber = TrackingNumber,
                                             DateCreatedUTC = DateTime.Now.ToUniversalTime()
                                         };

            dc.ParcelPortShipments.InsertOnSubmit(pps);

            try
            {
                dc.SubmitChanges();

                dc.uspLogisticsRecordTrackingNumberByOrderNumber(OrderNumber, TrackingNumber, TrackingUrl);
                return true;
            }
            catch (Exception e)
            {
                ParcelPortLogging.LogCommunication(dc, String.Format("Error saving order shipment with error message: {0}.  Stack Trace: {1}", e.Message, e.StackTrace));
            }

            return false;
        }

        public static void MarkOrderPrinted(ParcelPortDataContext dc, Int32 OrderCustomerID)
        {
            try
            {
                dc.uspLogisticsMarkOrderPrintedByOrderCustomerID(OrderCustomerID);
            }
            catch (Exception e)
            {
                ParcelPortLogging.LogCommunication(dc, String.Format("Error when marking order as printed: {)}", e.Message));
            }

        }
    }
}
