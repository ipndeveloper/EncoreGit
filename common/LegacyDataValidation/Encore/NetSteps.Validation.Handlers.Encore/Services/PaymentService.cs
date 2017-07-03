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
    public class PaymentService : IPaymentService
    {
        private const string _name = "Payment Service";

        public PaymentService()
        {
            useCachedData = false;
            _orderOrderPaymentTotals = new Dictionary<int, decimal>();
        }

        private bool useCachedData;
        private Dictionary<int, decimal> _orderOrderPaymentTotals;

        public bool GetPaymentTotal(int orderID, out decimal paymentTotal)        
        {
            if (useCachedData)
            {
                if (_orderOrderPaymentTotals.ContainsKey(orderID))
                {
                    paymentTotal = Math.Round(_orderOrderPaymentTotals[orderID],2);
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
	SUM(op.Amount) as Amount
FROM 
	[dbo].[OrderPayments] op
	JOIN 
		[dbo].[Orders] o ON o.OrderID = op.OrderID
WHERE 
	op.OrderID in ({0}) and 
	op.OrderPaymentStatusID = 2
GROUP BY
	o.OrderID", orderID)),
                            connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        paymentTotal = Math.Round((decimal)reader["Price"],2);
                        return true;
                    }
                }
            }
            paymentTotal = 0M;
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
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
            {
                SqlCommand command = new SqlCommand(String.Format(
@"SELECT
    o.OrderID as OrderID,      
	SUM(op.Amount) as Amount
FROM 
	[dbo].[OrderPayments] op
	JOIN 
		[dbo].[Orders] o ON o.OrderID = op.OrderID
WHERE 
	{0} and 
	op.OrderPaymentStatusID = 2
GROUP BY
	o.OrderID", QueryBase.GetWhereClauseString("o")), connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    _orderOrderPaymentTotals.Add((int)reader["OrderID"], (decimal)reader["Amount"]);
                }
            }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
