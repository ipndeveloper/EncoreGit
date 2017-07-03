using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using NetstepsDataAccess.DataEntities;
using log4net;


namespace NetSteps.Integrations.Service
{
    public static class OrderUpdateRepository
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(OrderUpdateRepository));

        private static OrderUpdate.OrderStatus GetOrderStatusByName(string name)
        {
            switch (name)
            {
                case "Pending": return OrderUpdate.OrderStatus.Pending;
                case "Pending Error": return OrderUpdate.OrderStatus.PendingError;
                case "Paid": return OrderUpdate.OrderStatus.Paid;
                case "Cancelled": return OrderUpdate.OrderStatus.Cancelled;
                case "Partially Paid": return OrderUpdate.OrderStatus.PartiallyPaid;
                case "Printed": return OrderUpdate.OrderStatus.Printed;
                case "Shipped": return OrderUpdate.OrderStatus.Shipped;
                case "Credit Card Declined": return OrderUpdate.OrderStatus.CreditCardDeclined;
                case "Credit Card Declined Retry": return OrderUpdate.OrderStatus.CreditCardDeclinedRetry;
                default: return OrderUpdate.OrderStatus.Pending;
            }
        }

        internal static string UpdateOrderAcknowledgement(string data, NetStepsEntities db)
        {
            var orderUpdateSucceded = new ObjectParameter("Succeeded", typeof(bool));
            var orderUpdateStatusMessage = new ObjectParameter("ErrorMessage", typeof(string));
            // parse the data coming in and apply it to the OrderUpdate.OrderCollection classes
            // using the database to perform the parsing. 
            List<uspIntegrationsParseIDAndStatusResult> parseResult = null;
            try
            {
                parseResult = db.uspIntegrationsParseIDAndStatus(data, "Orders", orderUpdateSucceded, orderUpdateStatusMessage).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return "An Error was Encountered in parsing the Order Acknowledgment data, please ensure that all Order IDs passed in exist";
            }

            OrderUpdate.OrderCollection orderCollection = new OrderUpdate.OrderCollection();
            foreach (var result in parseResult)
            {
                OrderUpdate.Order order = new OrderUpdate.Order();
                order.OrderNumber = result.ID.ToString();
                order.Status = GetOrderStatusByName(result.StatusName);
                orderCollection.Order.Add(order);
            }
            /*** Parsing Complete ***/

            OrderUpdateResponse.OrderCollection xmlUpdateOrderCollection = new OrderUpdateResponse.OrderCollection();
            foreach (NetSteps.Integrations.Service.OrderUpdate.Order o in orderCollection.Order)
            {
                // output parameters of the stored procedure
                string statusName = Enum.GetName(o.Status.GetType(), o.Status);

                var orderStatusID = (from s in db.OrderStatuses
                                     where s.Name == statusName
                                     select s.OrderStatusID).Single();

                // call stored procedure
                db.uspIntegrationsOrderFulfillmentAcknowledgement(Convert.ToInt32(o.OrderNumber), orderStatusID, orderUpdateSucceded, orderUpdateStatusMessage);

                if (!(Convert.ToBoolean(orderUpdateSucceded.Value)))
                {
                    logger.Error(orderUpdateStatusMessage.ToString());
                    db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "UpdateOrderAcknowledgement", orderUpdateStatusMessage.ToString());
                }

                // update XML with result
                OrderUpdateResponse.Order xmlUpdateOrder = new OrderUpdateResponse.Order();
                xmlUpdateOrder.OrderNumber = o.OrderNumber;
                xmlUpdateOrder.UpdateSucceeded = Convert.ToBoolean(orderUpdateSucceded.Value);
                xmlUpdateOrder.Message = orderUpdateStatusMessage.Value.ToString();
                
                // add the XML order to the collection
                xmlUpdateOrderCollection.Order.Add(xmlUpdateOrder);
            }
            // return serialized XML
            return xmlUpdateOrderCollection.Serialize();
        }

        internal static string OrderShipmentsUpdate(string data)
        {
            string[] splitShipping = new string[5000];
            splitShipping = data.Split('|');
            DataTable baseTable = new DataTable();
            baseTable.Columns.Add("OrderID", typeof(Int32));
            baseTable.Columns.Add("OrderShipmentID", typeof(Int32));
            baseTable.Columns.Add("OrderItemID", typeof(Int32));
            baseTable.Columns.Add("QuantityShipped", typeof(Int32));
            baseTable.Columns.Add("DateShipped", typeof(DateTime));
            baseTable.Columns.Add("TrackingNumber", typeof(string));
            baseTable.Columns.Add("ShippingMethodID", typeof(Int32));

            foreach (string ship in splitShipping)
            {
                string[] sArray = new string[6];
                sArray = ship.Split(',');
                DataRow row = baseTable.NewRow();
                row[0] = Convert.ToInt32(sArray[0]);
                row[1] = String.IsNullOrEmpty(sArray[1]) ? 0 : (int?)Convert.ToInt32(sArray[1]);
                row[2] = String.IsNullOrEmpty(sArray[2]) ? 0 : (int?)Convert.ToInt32(sArray[2]);
                row[3] = String.IsNullOrEmpty(sArray[3]) ? 0 : (int?)Convert.ToInt32(sArray[3]);
                row[4] = String.IsNullOrEmpty(sArray[4]) ? DateTime.Now : (DateTime?)Convert.ToDateTime(sArray[4]);
                row[5] = sArray[5];
                row[6] = String.IsNullOrEmpty(sArray[6]) ? 0 : (int?)Convert.ToInt32(sArray[6]);
                baseTable.Rows.Add(row);
            }

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["dbconn"]))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("uspIntegrationsOrderShipmentUpdate", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter();
                param1 = cmd.Parameters.Add(new SqlParameter("@BaseTable", System.Data.SqlDbType.Structured));
                param1.Direction = ParameterDirection.Input;
                param1.Value = baseTable;

                SqlParameter param2 = new SqlParameter();
                param2 = cmd.Parameters.Add(new SqlParameter("@ModifiedByUserID", System.Data.SqlDbType.Int));
                param2.Direction = ParameterDirection.Input;
                param2.Value = 186;

                SqlParameter param3 = new SqlParameter();
                param3 = cmd.Parameters.Add(new SqlParameter("@Succeeded", System.Data.SqlDbType.Bit));
                param3.Direction = ParameterDirection.Output;

                SqlParameter param4 = new SqlParameter();
                param4 = cmd.Parameters.Add(new SqlParameter("@ErrorMessage", System.Data.SqlDbType.VarChar, 8000));
                param4.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (!(bool)param3.Value)
                {
                    logger.Error(param4.Value.ToString());
                    return param4.Value.ToString();
                }
            }

            // else return success message
            return "Successfully updated Order(s)";
        }

        //internal static string UpdateOrderShipments(NetStepsEntities db, string data)
        //{
        //    var orderUpdateSucceded = new ObjectParameter("Succeeded", typeof(bool));
        //    var orderUpdateStatusMessage = new ObjectParameter("ErrorMessage", typeof(string));

        //    // update the database with new order shipment information.
        //    db.uspIntegrationsUpdateOrderShipments(Convert.ToInt32(ConfigurationManager.AppSettings["modifiedByUserID"]),
        //        data, orderUpdateSucceded, orderUpdateStatusMessage);
        //    if (!Convert.ToBoolean(orderUpdateSucceded.Value))
        //        return orderUpdateStatusMessage.Value.ToString();
        //    // else return success message
        //    return "Successfully updated Order(s)";
        //}

        //    internal static string UpdateOrderShipments(NetSteps.OrderFulfillment.OrderShipmentInformation.OrderCollection col, NetStepsEntities db)
        //    {
        //        OrderUpdateResponse.OrderCollection xmlUpdateOrderCollection = new OrderUpdateResponse.OrderCollection();
        //        int count = col.Order.Count();
        //        foreach (NetSteps.OrderFulfillment.OrderShipmentInformation.Order o in col.Order)
        //        {
        //            OrderUpdateResponse.Order xmlUpdateOrder = new OrderUpdateResponse.Order();
        //            // output parameters of the stored procedure
        //            var orderUpdateSucceded = new ObjectParameter("Succeeded", typeof(bool));
        //            var orderUpdateStatusMessage = new ObjectParameter("ErrorMessage", typeof(string));

        //            string valuesForProc = "";
        //            if (o.OrderCustomer != null && o.OrderCustomer.Any(p => p.Package != null))
        //            {
        //                foreach (NetSteps.OrderFulfillment.OrderShipmentInformation.OrderCustomer cust in o.OrderCustomer)
        //                {
        //                    int numberOfShipments = cust.Package.Count();
        //                    int orderShipmentIndex = 1;
        //                    string delimiter = ",";
        //                    string pipe = "|";
        //                    foreach (Package pack in cust.Package)
        //                    {
        //                        if (numberOfShipments == orderShipmentIndex)
        //                            pipe = String.Empty;
        //                        foreach (NetSteps.OrderFulfillment.OrderShipmentInformation.OrderItem i in pack.OrderItem)
        //                        {
        //                            valuesForProc += orderShipmentIndex.ToString()
        //                                + delimiter
        //                                + i.IDs.PrimaryID
        //                                + delimiter
        //                                + i.Qty
        //                                + delimiter
        //                                + pack.ShippedDate
        //                                + delimiter
        //                                + pack.TrackingNumber
        //                                + pipe;
        //                        }
        //                        orderShipmentIndex++;
        //                    }
        //                }
        //            }
        //            // update the database with new order shipment information.
        //            db.UspUpdateOrderShipments(Convert.ToInt32(ConfigurationManager.AppSettings["modifiedByUserID"]),
        //                Convert.ToInt32(o.IDs.PrimaryID), valuesForProc, orderUpdateSucceded, orderUpdateStatusMessage);
        //            // update XML with result
        //            xmlUpdateOrder.OrderNumber = o.IDs.PrimaryID.ToString();
        //            xmlUpdateOrder.UpdateSucceeded = Convert.ToBoolean(orderUpdateSucceded.Value);
        //            xmlUpdateOrder.Message = orderUpdateStatusMessage.Value.ToString();
        //            // add the XML order to the collection
        //            xmlUpdateOrderCollection.Order.Add(xmlUpdateOrder);
        //        }
        //        // return serialized XML
        //        return xmlUpdateOrderCollection.Serialize();
        //    }
    }
}
