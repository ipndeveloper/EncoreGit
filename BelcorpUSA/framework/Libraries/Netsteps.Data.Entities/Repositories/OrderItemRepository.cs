using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Business;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    #region Modifications
    // @01  BR-AT-008 GYS EFP: Envío de Productos Por Reclamo
    #endregion

    public partial class OrderItemRepository
    {
		public IOrderRepository OrderRepository { get { return Create.New<IOrderRepository>(); } }
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

		public IEnumerable<OrderItem> GetOrderItemsNotInStoreFront(Order order, int storeFrontId)
		{
			return order.OrderCustomers
				.SelectMany(oc => oc.ParentOrderItems
					.Where(oi =>
						oi.ProductID.HasValue
						&& !Inventory.IsProductInStoreFront(oi.ProductID.Value, storeFrontId)
					)
				);
		}

        public TrackableCollection<OrderItemProperty> GetOrderItemProperties(int orderItemID)
        {
            Contract.Requires<ArgumentNullException>(orderItemID != 0);

            using (var context = CreateContext())
            {
                return
                    context.OrderItemProperties.Where(x => x.OrderItemID == orderItemID) as
                    TrackableCollection<OrderItemProperty>;
	}
        }




        #region Modifications @01

        public int GetSupportTicketID(string orderNumber)
        {
            int supportTicket = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[GetSupportTicketByOrderNumber]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pOrderNumber", orderNumber);

                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                            {
                                supportTicket = Convert.ToInt32(sdr["SupportTicketID"]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return supportTicket;
        }

        public List<ClaimedOrderItem> LoadItemsToClaim(string orderNumber)
        {
            List<ClaimedOrderItem> itemsToClaim = new List<ClaimedOrderItem>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[GetOrderItemsToClaimByOrderNumber]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pOrderNumber", orderNumber);

                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            ClaimedOrderItem item = null;
                            while (sdr.Read())
                            {
                                item = new ClaimedOrderItem();

                                #region Assign Values
                                item.OrderID = Convert.ToInt32(sdr["OrderID"]);
                                item.OrderItemID = Convert.ToInt32(sdr["OrderItemID"]);
                                item.OrderCustomerID = Convert.ToInt32(sdr["OrderCustomerID"]);

                                item.CultureInfo = sdr["CultureInfo"].ToString();
                                item.CurrencyCode = sdr["CurrencyCode"].ToString();

                                item.SKU = sdr["SKU"].ToString();
                                item.ProductName = sdr["ProductName"].ToString();
                                item.ProductType = sdr["ProductType"].ToString();
                                item.OrderQuantity = Convert.ToInt32(sdr["Quantity"]);
                                item.PricePerItem = Convert.ToDecimal(sdr["ItemPrice"]);
                                #endregion

                                itemsToClaim.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                itemsToClaim = new List<ClaimedOrderItem>();
            }

            return itemsToClaim;
        }

        public bool ClaimOrderItems(Dictionary<int, int> listToClaim, string orderNumber, string ticketSupport)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[InsertItemClaimed]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pOrderNumber", orderNumber);
                        cmd.Parameters.AddWithValue("@pTicketSupport", ticketSupport);
                        cmd.Parameters.Add("@pListToClaim", SqlDbType.Structured).Value = BuildTableFromDictionary(listToClaim);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                rpta = true;
            }
            catch (Exception)
            {

            }

            return rpta;
        }

        private DataTable BuildTableFromDictionary(Dictionary<int, int> dictionary)
        {
            DataTable dt = new DataTable("Estructura");

            dt.Columns.Add("NombreColumna");
            dt.Columns.Add("ValorColumna");

            foreach (KeyValuePair<int, int> pair in dictionary)
            {
                dt.Rows.Add(pair.Key, pair.Value);
            }

            return dt;
        }

        #endregion

	}
}