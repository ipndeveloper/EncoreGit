using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Diagnostics.Utilities;
using NetstepsDataAccess.DataEntities;


namespace NetSteps.Integrations.Service
{
	public static class OrderUpdateRepository
	{
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

		internal static BasicResponse UpdateOrderAcknowledgement(string data, NetStepsEntities db)
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
                ex.TraceException(ex);
				return new BasicResponse { Success = false, Message = "An Error was Encountered in parsing the Order Acknowledgment data, please ensure that all Order IDs passed in exist" };
			}

			var orderCollection = new OrderUpdate.OrderCollection();
			foreach (var result in parseResult)
			{
				var order = new OrderUpdate.Order();
				order.OrderNumber = result.ID.ToString();
				order.Status = GetOrderStatusByName(result.StatusName);
				orderCollection.Order.Add(order);
			}
			/*** Parsing Complete ***/

			var xmlUpdateOrderCollection = new OrderUpdateResponse.OrderCollection();
			foreach (var o in orderCollection.Order)
			{
				// output parameters of the stored procedure
				var statusName = Enum.GetName(o.Status.GetType(), o.Status);

				var orderStatusID = (from s in db.OrderStatuses
									 where s.Name == statusName
									 select s.OrderStatusID).Single();

				// call stored procedure
				db.uspIntegrationsOrderFulfillmentAcknowledgement(Convert.ToInt32(o.OrderNumber), orderStatusID, orderUpdateSucceded, orderUpdateStatusMessage);

				if (!(Convert.ToBoolean(orderUpdateSucceded.Value)))
				{
                    (new object()).TraceError(orderUpdateStatusMessage.ToString());
					db.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", "UpdateOrderAcknowledgement", orderUpdateStatusMessage.ToString());
				}

				// update XML with result
				var xmlUpdateOrder = new OrderUpdateResponse.Order();
				xmlUpdateOrder.OrderNumber = o.OrderNumber;
				xmlUpdateOrder.UpdateSucceeded = Convert.ToBoolean(orderUpdateSucceded.Value);
				xmlUpdateOrder.Message = orderUpdateStatusMessage.Value.ToString();

				// add the XML order to the collection
				xmlUpdateOrderCollection.Order.Add(xmlUpdateOrder);
			}
			// return serialized XML
			return new BasicResponse { Success = true, Message = xmlUpdateOrderCollection.Serialize() };
		}

		internal static BasicResponse OrderShipmentsUpdate(string data)
		{
			var splitShipping = data.Split('|');
			var baseTable = new DataTable();
			baseTable.Columns.Add("OrderID", typeof(Int32));
			baseTable.Columns.Add("OrderShipmentID", typeof(Int32));
			baseTable.Columns.Add("OrderItemID", typeof(Int32));
			baseTable.Columns.Add("QuantityShipped", typeof(Int32));
			baseTable.Columns.Add("DateShipped", typeof(DateTime));
			baseTable.Columns.Add("TrackingNumber", typeof(string));
			baseTable.Columns.Add("ShippingMethodID", typeof(Int32));

			foreach (string ship in splitShipping)
			{
				var sArray = ship.Split(',');
				var row = baseTable.NewRow();
				row[0] = Convert.ToInt32(sArray[0]);
				row[1] = String.IsNullOrEmpty(sArray[1]) ? 0 : (int?)Convert.ToInt32(sArray[1]);
				row[2] = String.IsNullOrEmpty(sArray[2]) ? 0 : (int?)Convert.ToInt32(sArray[2]);
				row[3] = String.IsNullOrEmpty(sArray[3]) ? 0 : (int?)Convert.ToInt32(sArray[3]);
				row[4] = String.IsNullOrEmpty(sArray[4]) ? DateTime.Now : (DateTime?)Convert.ToDateTime(sArray[4]);
				row[5] = sArray[5];
				row[6] = String.IsNullOrEmpty(sArray[6]) ? 0 : (int?)Convert.ToInt32(sArray[6]);
				baseTable.Rows.Add(row);
			}

			using (var sqlConn = new SqlConnection(ConfigurationManager.AppSettings["dbconn"]))
			{
				sqlConn.Open();
			    var cmd = sqlConn.CreateCommand();
                cmd.CommandText = "uspIntegrationsOrderShipmentUpdate";
				cmd.CommandType = CommandType.StoredProcedure;

				var param1 = cmd.Parameters.Add(new SqlParameter("@BaseTable", System.Data.SqlDbType.Structured));
				param1.Direction = ParameterDirection.Input;
				param1.Value = baseTable;

				var param2 = cmd.Parameters.Add(new SqlParameter("@ModifiedByUserID", System.Data.SqlDbType.Int));
				param2.Direction = ParameterDirection.Input;
				param2.Value = 186;

				var param3 = cmd.Parameters.Add(new SqlParameter("@Succeeded", System.Data.SqlDbType.Bit));
				param3.Direction = ParameterDirection.Output;

				var param4 = cmd.Parameters.Add(new SqlParameter("@ErrorMessage", System.Data.SqlDbType.VarChar, 8000));
				param4.Direction = ParameterDirection.Output;

				cmd.ExecuteNonQuery();

				if (!(bool)param3.Value)
				{
                    (new object()).TraceError(param4.Value.ToString());
					return new BasicResponse { Success = false, Message = param4.Value.ToString() };
				}
			}

			// else return success message
			return new BasicResponse { Success = true, Message = "Successfully updated Order(s)" };
		}
	}
}
