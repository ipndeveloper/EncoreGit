using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderFulfillment;

namespace TestOrderFulfillmentService
{
    class Program
    {
        public static void Main(string[] args)
        {
            OrderFulfillmentService serv = new OrderFulfillmentService();
            string s = serv.GetOrdersToFulfill("miche5779", "98jklrgeasg");
            string ss = CreateExampleEncoreOrderShippingInformationXml();
        }

        private static string CreateExampleEncoreOrderShippingInformationXml()
        {
            NetSteps.OrderFulfillment.OrderShipmentInformation.OrderCollection col = new NetSteps.OrderFulfillment.OrderShipmentInformation.OrderCollection();
            NetSteps.OrderFulfillment.OrderShipmentInformation.Order order = new NetSteps.OrderFulfillment.OrderShipmentInformation.Order();
            NetSteps.OrderFulfillment.OrderShipmentInformation.OrderCustomer cust = new NetSteps.OrderFulfillment.OrderShipmentInformation.OrderCustomer();
            cust.IDs.PrimaryID = 1;
            NetSteps.OrderFulfillment.OrderShipmentInformation.Package pack = new NetSteps.OrderFulfillment.OrderShipmentInformation.Package();
            pack.ActualShipCost = new NetSteps.OrderFulfillment.OrderShipmentInformation.Money
            {
                Currency = NetSteps.OrderFulfillment.OrderShipmentInformation.Currency.USD,
                Value = 456 };
            pack.DeliveryConfirmation = true;
            pack.ShippedDate = DateTime.Now;
            pack.TrackingNumber = "1Z54DF5D4F7E8R7E87R4SFS654F6";
            NetSteps.OrderFulfillment.OrderShipmentInformation.OrderItem item = new NetSteps.OrderFulfillment.OrderShipmentInformation.OrderItem();
            item.CommodityCode = "SFD545";
            item.IDs.PrimaryID = 454;
            item.ItemPrice = new NetSteps.OrderFulfillment.OrderShipmentInformation.Money
            {
                Currency = NetSteps.OrderFulfillment.OrderShipmentInformation.Currency.USD,
                Value = 4545 };
            item.Qty = 6;
            item.Sku = "DF545D";
            pack.OrderItem.Add(item);
            List<NetSteps.OrderFulfillment.OrderShipmentInformation.Package> packs = new List<NetSteps.OrderFulfillment.OrderShipmentInformation.Package>();
            packs.Add(pack);
            cust.Package = packs;
            order.OrderCustomer.Add(cust);
            col.Order.Add(order);
            return col.Serialize();
        }
    }
}
