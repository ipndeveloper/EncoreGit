using NetSteps.Foundation.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Handlers.Common.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Services
{
    public class OrderCommissionService : IOrderCommissionService
    {
        private const string _name = "Order Commission Service";

        public OrderCommissionService()
        {
            useCachedData = false;
            _orderCommissionTotals = new Dictionary<int, decimal>();
        }

        private bool useCachedData;
        private Dictionary<int, decimal> _orderCommissionTotals;

        public bool GetStoredOrderCommission(int orderID, out decimal commissionTotal)       
        {
            if (useCachedData)
            {
                if (_orderCommissionTotals.ContainsKey(orderID))
                {
                    commissionTotal = Math.Round(_orderCommissionTotals[orderID],2);
                    return true;
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
                {
                    SqlCommand command =
                        new SqlCommand(
                            String.Format(String.Format(
@"SELECT
    o.OrderID as OrderID,
	ISNULL(SUM(oc.Value), 0) as CommissionAmount	
FROM 
	Sites_Orders o
		LEFT JOIN 
		[dbo].[OrderCalculations] oc on o.OrderID = oc.OrderID and oc.CalculationTypeID = 1
WHERE 
	op.OrderID = {0}
GROUP BY
	o.OrderID", orderID)),
                            connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        commissionTotal = Math.Round((decimal)reader["CommissionAmount"], 2);
                        return true;
                    }
                }
            }
            commissionTotal = 0M;
            return false;
        }

        public IRecordQuery QueryBase { get; set; }

        public void Initialize()
        {
            LoadData();
            useCachedData = true;
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Commissions].ConnectionString))
            {
                SqlCommand command = new SqlCommand(String.Format(
@"SELECT
    o.OrderID as OrderID,
	ISNULL(SUM(oc.Value), 0) as CommissionAmount	
FROM 
	Sites_Orders o
		LEFT JOIN 
		[dbo].[OrderCalculations] oc on o.OrderID = oc.OrderID and oc.CalculationTypeID = 1
WHERE 
	{0}
GROUP BY
	o.OrderID", QueryBase.GetWhereClauseString("o")), connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    _orderCommissionTotals.Add((int)reader["OrderID"], Math.Round(((decimal)reader["CommissionAmount"]), 2));
                }
            }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
