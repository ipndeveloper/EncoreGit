using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace ParcelPortIntegrationService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ParcelPortIntegrationService : IParcelPortIntegrationService
    {
        #region Helpers
        public virtual List<VwParcelPortOrder> GetOrdersToShip(ParcelPortDataContext context)
        {
            return OrderActions.GetOrdersToShip(context);
        }

        public virtual List<OrderItem> GetOrderLinesForOrder(ParcelPortDataContext context, int orderCustomerID)
        {
            return OrderActions.GetOrderLinesForOrder(context, orderCustomerID);
        }

        public virtual XElement SendOrderToParcelPortForFullfillment(ParcelPortCommunication pComm, XDocument orderXML)
        {
            return pComm.SendOrderToParcelPort(orderXML);
        }

        public virtual void MarkOrderAsPrinted(ParcelPortDataContext context, VwParcelPortOrder order)
        {
            OrderActions.MarkOrderPrinted(context, order.OrderCustomerID);
        }

        public virtual XDocument GetOrderXML(VwParcelPortOrder order, List<OrderItem> orderlines)
        {
            return OrderActions.GetOrderXML(order, orderlines);
        }

        #endregion


        public void SendOrders(String PassKey)
        {
            if (PassKey.Equals(ConfigurationHelper.GetAPIKey()))
            {
                ParcelPortDataContext context = GetDataContext();

                List<VwParcelPortOrder> orders;
                ParcelPortCommunication pComm = new ParcelPortCommunication(ConfigurationHelper.GetParcelPortURL(), ConfigurationHelper.GetParcelPortPostAttributute());

                try
                {
                    orders = GetOrdersToShip(context);
                }
                catch (Exception e)
                {
                    ParcelPortLogging.LogCommunication(context,
                        String.Format("Failed to retrieve orders to ship for Parcel Port with message {0}.  Stack Trace: {1}{2}", e.Message, System.Environment.NewLine, e.StackTrace)
                        );
                    return;
                }

                if (orders.Count > 0)
                {
                    ParcelPortLogging.LogCommunication(context, String.Format("There are {0} orders to send to ParcelPort", orders.Count.ToString()));

                    int OrdersSentSuccessfully = 0;

                    foreach (VwParcelPortOrder order in orders)
                    {
                        try
                        {
                            List<OrderItem> orderlines = GetOrderLinesForOrder(context, order.OrderCustomerID);

                            if (orderlines != null && orderlines.Count > 0)
                            {
                                XDocument orderXML = GetOrderXML(order, orderlines); 
                                try
                                {
                                    XElement XResponse = SendOrderToParcelPortForFullfillment(pComm, orderXML);

                                    String XError = String.Empty;
                                    String XStatus = String.Empty;

                                    if (XResponse.Element("Error") != null)
                                        XError = XResponse.Element("Error").Value;

                                    if (XResponse.Element("Status") != null)
                                        XStatus = XResponse.Element("Status").Value;


                                    if (!XError.Trim().Equals(String.Empty) || !XStatus.ToUpper().Equals("ACCEPTED"))
                                    {
                                        throw new Exception(String.Format("Message sent to Parcel Port {0} : Message Received {1}", orderXML.ToString(), XResponse.ToString()));
                                    }

                                    MarkOrderAsPrinted(context, order);

                                    OrdersSentSuccessfully++;
                                }
                                catch (Exception e)
                                {
                                    ParcelPortLogging.LogCommunication(context,
                                        String.Format("Failed to send order with OrderCustomerID {0} to the Parcel Port API with message {1}.  Stack Trace: {2}{3}", order.OrderCustomerID.ToString(), e.Message, System.Environment.NewLine, e.StackTrace)
                                        );
                                }
                            }
                            else
                            {
                                ParcelPortLogging.LogCommunication(context,
                                    String.Format("No order lines were retrieved for OrderCustomerID {0}.  Order will not be sent to Parcel Port", order.OrderCustomerID.ToString())
                                    );
                            }
                        }
                        catch (Exception e)
                        {
                            ParcelPortLogging.LogCommunication(context,
                                String.Format("Failed to retrieve order lines for OrderCustomerID {0} to ship for Parcel Port with message {1}.  Stack Trace: {2}{3}", order.OrderCustomerID.ToString(), e.Message, System.Environment.NewLine, e.StackTrace)
                                );
                        }
                    }
                    ParcelPortLogging.LogCommunication(context, String.Format("{0} orders sent to Parcel Port successfully.", OrdersSentSuccessfully.ToString()));
                }
                else
                {
                    ParcelPortLogging.LogCommunication(context, "There were no orders to send to Parcel Port");
                }
            }
        }

        public string ShipmentPush(XDocument ShipmentPush)
        {
            XElement Shipment = ShipmentPush.Root;
            XElement Response = new XElement("response");
            XElement ResponseStatus = new XElement("status");
            XElement ResponseError = new XElement("error");

            String APIKey = String.Empty;
            String ReferenceNum = String.Empty;
            String TrackingNumber = String.Empty;

            String TrackingUrl = String.Empty;

            Int32 OrderCustomerID = 0;
            String OrderNumber = "";

            try
            {
                APIKey = Shipment.Element("APIKey").Value;
                ReferenceNum = Shipment.Element("ReferenceNum").Value;
                OrderNumber = ReferenceNum;
                TrackingNumber = Shipment.Element("TrackingNumber").Value;

                // Get Tracking URL here
                TrackingUrl = (string)Shipment.Element("TrackingURL");
            }
            catch (Exception ex)
            {
                ResponseStatus.Value = "NOK";
                ResponseError.Value = "XML does not contain all required elements";

                Response.Add(ResponseStatus);
                Response.Add(ResponseError);

                return Response.ToString();
            }

            if (string.Equals(APIKey, ConfigurationHelper.GetAPIKey()))
            {
                if (!String.IsNullOrEmpty(OrderNumber))
                {
                    try
                    {
                        if (OrderActions.SaveOrderShipment(GetDataContext(), OrderNumber, TrackingNumber, TrackingUrl))
                        {
                            ResponseStatus.Value = "OK";
                        }
                        else
                        {
                            ResponseStatus.Value = "NOK";
                            ResponseError.Value = "Error saving tracking number to database";
                        }
                    }
                    catch (Exception er)
                    {
                        ResponseStatus.Value = "NOK";
                        ResponseError.Value = er.Message;
                    }
                }
                else
                {
                    ResponseStatus.Value = "NOK";
                    ResponseError.Value = "Reference number is not Valid";
                }
            }
            else
            {
                ResponseStatus.Value = "NOK";
                ResponseError.Value = "API Key is not valid";
            }

            Response.Add(ResponseStatus);
            Response.Add(ResponseError);

            if (ResponseStatus.Value.Equals("NOK"))
            {
                try
                {
                    ParcelPortLogging.LogCommunication(GetDataContext(), Response.ToString());
                }
                catch (Exception e)
                {

                }
            }

            return Response.ToString();
        }

        private ParcelPortDataContext GetDataContext()
        {
            return new ParcelPortDataContext(ConfigurationHelper.GetParcelPortContextConnectionString());
        }
    }

}

