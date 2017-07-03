using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParcelPortIntegrationService
{
    [TestClass]
    public class ParcelPortIntegrationTest
    {
        private const string API_KEY = "d5wE-8u7une2rAFrud8frefepr?rebraP$_8e*#ye5para8ru8uqumutrejaqubr";

        [Ignore]
        [TestMethod]
        public void TestSendOrders()
        {
            ParcelPortIntegrationService x = new ParcelPortIntegrationService();
            x.SendOrders(ConfigurationManager.AppSettings["ParcelPortAPIKey"]);
        }


        #region ShipmentPush

        [TestMethod]
        public void ShipmentPush_InvalidApiKey_DoesNotCallSaveOrderShipment()
        {
            // Arrange
            XDocument doc = XDocument.Parse(ShippingFileWithTrackingURL("bad api key", null));

            var target = new ParcelPortIntegrationService();

            // Act
            var result = XElement.Parse(target.ShipmentPush(doc));

            // Assert
            var status = result.Element("status").Value;
            var error = result.Element("error").Value;

            Assert.AreEqual("NOK", status);
            Assert.AreEqual("API Key is not valid", error);
        }

        [TestMethod]
        public void ShipmentPush_ValidApiKeyAndOrderNumber_SaveOrderShipmentRecord()
        {
            // Arrange
            XDocument doc = XDocument.Parse(ShippingFileWithTrackingURL(API_KEY, "123456"));

            var target = new ParcelPortIntegrationService();

            // Act
            var result = XElement.Parse(target.ShipmentPush(doc));

            // Assert
            var status = result.Element("status").Value;
            var error = result.Element("error").Value;

            Assert.AreEqual("OK", status);
            Assert.AreEqual(string.Empty, error);
        }

        [TestMethod]
        public void ShipmentPush_NoTrackingUrlProvided_SavesValueAsNullWithoutAnyErrors()
        {
            // Arrange
            XDocument doc = XDocument.Parse(ShippingFileWithoutTrackingURL(API_KEY, "123456"));
            var target = new ParcelPortIntegrationService();

            // Act
            var result = XElement.Parse(target.ShipmentPush(doc));

            // Assert
            var status = (string)result.Element("status");
            var error = (string)result.Element("error");

            Assert.AreEqual("OK", status);
            Assert.AreEqual(string.Empty, error);

        }

        [TestMethod]
        public void ShipmentPush_ValidApiKeyNoOrderNumber_LogsError()
        {
            // Arrange
            XDocument doc = XDocument.Parse(ShippingFileWithTrackingURL(API_KEY, null));
            var target = new ParcelPortIntegrationService();

            // Act
            var result = XElement.Parse(target.ShipmentPush(doc));

            // Assert
            var status = result.Element("status").Value;
            var error = result.Element("error").Value;

            Assert.AreEqual("NOK", status);
            Assert.AreEqual("Reference number is not Valid", error);
        }

        [TestMethod]
        public void ConvertAmpersandToProperXML_IfNeeded()
        {
            const string trackingUrl = "http://www.fedex.com/Tracking?ascend_header=1&amp;clienttype=dotcom&cntry_code=us&language=english&tracknumbers=02931505918300558244";

            System.Text.RegularExpressions.Regex badAmpersand = new System.Text.RegularExpressions.Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");

            string regex = badAmpersand.Replace(trackingUrl, "&amp;");

            Assert.AreEqual(
                "http://www.fedex.com/Tracking?ascend_header=1&amp;clienttype=dotcom&amp;cntry_code=us&amp;language=english&amp;tracknumbers=02931505918300558244",
                regex);
        }




        public string ShippingFileWithTrackingURL(string apiKey, string orderNumber)
        {
            return
                @"
                    <Shipment>
                        <APIKey>" + apiKey + @"</APIKey>
                        <ReferenceNum>" + orderNumber + @"</ReferenceNum>
                        <TrackingNumber>1111111111</TrackingNumber>
                        <TrackingURL>http://www.fedex.com/Tracking?ascend_header=1&amp;clienttype=dotcom&amp;cntry_code=us&amp;language=english&amp;tracknumbers=02931505918300558244</TrackingURL>
                    </Shipment>
                ";
        }

        public string ShippingFileWithoutTrackingURL(string apiKey, string orderNumber)
        {
            return
                @"
                    <Shipment>
                        <APIKey>" + apiKey + @"</APIKey>
                        <ReferenceNum>" + orderNumber + @"</ReferenceNum>
                        <TrackingNumber>2222222222</TrackingNumber>
                    </Shipment>
                ";
        }

        #endregion

        #region SendOrders

        [TestMethod]
        public void SendOrders_WrongPassKey_DoesNothing()
        {
            // Arrange
            var target = new FakeParcelPortIntegrationService();

            // Act
            target.SendOrders("Wrong_API_Key");

            // Assert
            Assert.IsFalse(target.isGetOrdersToShipCalled);
        }

        [TestMethod]
        public void SendOrders_NoOrdersToSend_DoesNotSendOrdersToFullfillment()
        {
            // Arrange
            var target = new FakeParcelPortIntegrationService();

            // Act
            target.SendOrders(API_KEY);

            // Assert
            Assert.IsTrue(target.isGetOrdersToShipCalled);
            Assert.IsFalse(target.isSendOrderToParcelPortForFullfillmentCalled);
        }

        [TestMethod]
        public void SendOrders_OrdersToSendButNoOrderItems_ShouldNotSendOrdersToParcelPort()
        {
            // Arrange
            List<VwParcelPortOrder> ordersToSend = new List<VwParcelPortOrder>
                                                       {
                                                           new VwParcelPortOrder { OrderCustomerID = 123 }
                                                       };
            var target = new FakeParcelPortIntegrationService
                             {
                                 VwParcelPortOrders = ordersToSend
                             };

            // Act
            target.SendOrders(API_KEY);

            // Assert
            Assert.IsTrue(target.isGetOrdersToShipCalled);
            Assert.IsTrue(target.isGetOrderLinesForOrderCalled);
            Assert.IsFalse(target.isSendOrderToParcelPortForFullfillmentCalled);
        }

        [Ignore]
        [TestMethod]
        public void SendOrders_OrdersSendToParcelPortSuccessfull_MarksTheOrderAsPrinted()
        {
            // Arrange
            List<VwParcelPortOrder> ordersToSend = new List<VwParcelPortOrder> { new VwParcelPortOrder() };
            List<OrderItem> orderItems = new List<OrderItem> { new OrderItem() };

            var target = new FakeParcelPortIntegrationService
                             {
                                 VwParcelPortOrders = ordersToSend,
                                 OrderItems = orderItems
                             };

            // Act
            target.SendOrders(API_KEY);

            // Assert
            Assert.IsTrue(target.isGetOrdersToShipCalled);
            Assert.IsTrue(target.isGetOrderLinesForOrderCalled);
            Assert.IsTrue(target.isSendOrderToParcelPortForFullfillmentCalled);
            Assert.IsTrue(target.isMarkOrderAsPrintedCalled);
        }

        [Ignore]
        [TestCategory("Calls Real API")]
        [TestMethod]
        public void SendOrders_OrdersSendToParcelPortProperWithLifeTimeRankingValue_ShouldGetBackAccepttedResponse()
        {
            // Arrange
            List<VwParcelPortOrder> ordersToSend = new List<VwParcelPortOrder> { FakeVwParcelPortOrder("Ambasaddor Diamond") };

            var target = new ActualParcelPortIntegration
                             {
                                 VwParcelPortOrders = ordersToSend
                             };

            // Act
            target.SendOrders(API_KEY);


            // Assert
        }

        [Ignore]
        [TestCategory("Calls Real API")]
        [TestMethod]
        public void SendOrders_OrdersSendToParcelPortProperWithoutLifeTimeRankingValue_ShouldGetBackAccepttedResponse()
        {
            // Arrange
            List<VwParcelPortOrder> ordersToSend = new List<VwParcelPortOrder> { FakeVwParcelPortOrder(null) };

            var target = new ActualParcelPortIntegration
            {
                VwParcelPortOrders = ordersToSend
            };

            // Act
            target.SendOrders(API_KEY);


            // Assert
        }

        [TestMethod]
        public void GetOrderXML_OrderCustomerHasLifeTimeRanking_IncludesLifeTimeRankingNode()
        {
            // Arrange
            ParcelPortIntegrationService target = new ParcelPortIntegrationService();

            VwParcelPortOrder fakeOrder = FakeVwParcelPortOrder("Ambassador Diamond");
            List<OrderItem> orderItems = FakeOrderItems();


            // Act
            var xmlResult = target.GetOrderXML(fakeOrder, orderItems);

            // Assert
            XElement doc = xmlResult.Root;
            XElement header = doc.Element("OrderHeader");
            var result = (string)header.Element("CustomerPO");

            Assert.IsNotNull(result);
            Assert.AreEqual("Ambassador Diamond", result);
        }

        [TestMethod]
        public void GetOrderXML_OrderCustomerLifeTimeRankingIsNULL_ConvertsNullLifetimeToEmptyString()
        {
            // Arrange
            ParcelPortIntegrationService target = new ParcelPortIntegrationService();

            VwParcelPortOrder fakeOrder = FakeVwParcelPortOrder(null);
            List<OrderItem> orderItems = FakeOrderItems();


            // Act
            var xmlResult = target.GetOrderXML(fakeOrder, orderItems);

            // Assert
            XElement doc = xmlResult.Root;
            XElement header = doc.Element("OrderHeader");
            var result = (string)header.Element("CustomerPO");

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        public List<OrderItem> FakeOrderItems()
        {
            return new List<OrderItem>
                       {
                           new OrderItem
                               {
                                   SKU = "SKU123",
                                   Quantity = 5,
                                   ItemPrice = 29.99m,
                                   AdjustedPrice = 25.99m
                               }
                       };
        }

        public VwParcelPortOrder FakeVwParcelPortOrder(string lifeTimeRanking)
        {
            return new VwParcelPortOrder
                       {
                           OrderNumber = "62494",
                           OrderDate = "2012-07-06",
                           Carrier = "Parcel",
                           CustomerNum = 1192011,
                           OrderCustomerID = 1192011,
                           Payment = "12345",
                           OrderClass = 'A',
                           SubTotal = 128.99m,
                           Tax = 12.99m,
                           Shipping = 5.99m,
                           Total = 200m,
                           LifeTimeRanking = lifeTimeRanking,
                           BillToAttention = "Bill to attention",
                           BillToAddress1 = string.Empty,
                           BillToAddress2 = string.Empty,
                           BillToCity = "Salt Lake",
                           BillToState = "UT",
                           BillToPostalCode = "84111",
                           BillToCountry = "USA",
                           BillToPhone = "999-965-5425",
                           ShipToAttention = "Ship to attention",
                           ShipToAddress1 = string.Empty,
                           ShipToAddress2 = string.Empty,
                           ShipToCity = "Salt Lake",
                           ShipToState = "UT",
                           ShipToPostalCode = "84111",
                           ShipToCountry = "USA",
                           ShipToPhone = "123-456-7894"
                       };
        }
    }

    public class FakeParcelPortIntegrationService : ParcelPortIntegrationService
    {
        public bool isGetOrdersToShipCalled;
        public bool isGetOrderLinesForOrderCalled;
        public bool isSendOrderToParcelPortForFullfillmentCalled;
        public bool isMarkOrderAsPrintedCalled;

        public List<OrderItem> OrderItems { get; set; }
        public List<VwParcelPortOrder> VwParcelPortOrders { get; set; }
        public XElement XElement { get; set; }

        public override List<VwParcelPortOrder> GetOrdersToShip(ParcelPortDataContext context)
        {
            isGetOrdersToShipCalled = true;

            return VwParcelPortOrders ?? new List<VwParcelPortOrder>();
        }

        public override List<OrderItem> GetOrderLinesForOrder(ParcelPortDataContext context, int orderCustomerID)
        {
            isGetOrderLinesForOrderCalled = true;

            return OrderItems ?? new List<OrderItem>();
        }

        public override XElement SendOrderToParcelPortForFullfillment(ParcelPortCommunication pComm, XDocument orderXML)
        {
            isSendOrderToParcelPortForFullfillmentCalled = true;

            return XElement ?? XElement.Parse(ParcelPortResponse(null, "accepted"));
        }

        public override void MarkOrderAsPrinted(ParcelPortDataContext context, VwParcelPortOrder order)
        {
            isMarkOrderAsPrintedCalled = true;
        }

        public override XDocument GetOrderXML(VwParcelPortOrder order, List<OrderItem> orderlines)
        {
            return null;
        }

        public string ParcelPortResponse(string error, string status)
        {
            return @"<Response>
                        <Error>" + error + @"</Error>
                        <Status>" + status + @"</Status>
                    </Response>";

        }

    }

    public class ActualParcelPortIntegration : ParcelPortIntegrationService
    {
        public bool isGetOrdersToShipCalled;
        public bool isGetOrderLinesForOrderCalled;
        public List<OrderItem> OrderItems { get; set; }
        public List<VwParcelPortOrder> VwParcelPortOrders { get; set; }


        public override List<VwParcelPortOrder> GetOrdersToShip(ParcelPortDataContext context)
        {
            isGetOrdersToShipCalled = true;
            return VwParcelPortOrders ?? new List<VwParcelPortOrder>();
        }

    }
}
