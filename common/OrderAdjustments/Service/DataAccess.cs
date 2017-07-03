using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Threading;
using NetSteps.Common.Configuration;
namespace NetSteps.OrderAdjustments.Service
{
    public class DataAccess
    {
        SqlConnection connection;
        public DataAccess()
        {
            connection = new OrderAdjustmentsConnection().Cnx;
        }

        public class InventoryCheck
        {
            public int MaterialID { get; set; }
            public int Available { get; set; }
        }
        public bool Exist(int productId, int quantity, int wareHouseID)
        {
            List<InventoryCheck> inventoryCheck = InventoryCheckResult(productId, wareHouseID);
            foreach (var item in inventoryCheck)
            {
                if (quantity > item.Available)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public List<InventoryCheck> InventoryCheckResult(int productoID, int wareHouseID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("upsAvailabilityValidate", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoSolicitadoID", productoID);
                cmd.Parameters.AddWithValue("@WarehouseID", wareHouseID);
                var returnInventoryCheck = new List<InventoryCheck>();
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returnInventoryCheck.Add(
                        new InventoryCheck
                        {
                            Available = reader.GetInt32(reader.GetOrdinal("Available")),
                            MaterialID = reader.GetInt32(reader.GetOrdinal("MaterialID")),
                        }
                        );
                }
                connection.Close();
                return returnInventoryCheck;

            }
            catch (Exception)
            {

                throw;
            }
        }
         
    }
}
