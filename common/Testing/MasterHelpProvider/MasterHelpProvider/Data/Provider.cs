using System.Data.SqlClient;

namespace TestMasterHelpProvider
{
   public class Provider
    {
        #region --SQL executions--
        public static int FindPCTestAccount(string userName, string userLastName)
        {
            int accountId = 0;
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_account_get_WatiNtestAccount");
            DataAccess.AddInputParameter("userName", userName, sqlCommand);
            DataAccess.AddInputParameter("UserLastName", userLastName, sqlCommand);
            DataAccess.AddInputOutputParameter("AccountID", accountId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            accountId = DataAccess.GetInt32ReturnValue("AccountId", sqlCommand);
            DataAccess.Close(sqlCommand);

            return accountId;
        }

        public  static int FindConsultantTestAccount(string ConsultantFirstName, string ConsultantLastName)
        {
            int accountId = 0;
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_account_get_WatiNtestAccount");
            DataAccess.AddInputParameter("userName", ConsultantFirstName, sqlCommand);
            DataAccess.AddInputParameter("UserLastName", ConsultantLastName, sqlCommand);
            DataAccess.AddInputOutputParameter("AccountID", accountId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            accountId = DataAccess.GetInt32ReturnValue("AccountId", sqlCommand);
            DataAccess.Close(sqlCommand);

            return accountId;
        }

        public static int FindRetailCustomerAccount(string RetailFirstName, string RetailLastName)
        {
            int accountId = 0;
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_account_get_WatiNtestAccount");
            DataAccess.AddInputParameter("userName", RetailFirstName, sqlCommand);
            DataAccess.AddInputParameter("UserLastName", RetailLastName, sqlCommand);
            DataAccess.AddInputOutputParameter("AccountID", accountId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            accountId = DataAccess.GetInt32ReturnValue("AccountId", sqlCommand);
            DataAccess.Close(sqlCommand);

            return accountId;
        }

        /// <summary>
        /// Delete a test order by accountid
        /// </summary>
        /// <param name="accountId"></param>
        public static void DeleteTestCustomerOrder(int accountId)
        {
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_orders_delete_WatiNtestRecords");
            DataAccess.AddInputParameter("Accountid", accountId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            DataAccess.Close(sqlCommand);
        }

        public static int GetOrderStatusSubmitted()
        {
            int orderId = 0;
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_Order_select_status_submitted");
            DataAccess.AddInputOutputParameter("OrderId", orderId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            orderId = DataAccess.GetInt32ReturnValue("OrderId",sqlCommand);
            DataAccess.Close(sqlCommand);
            return orderId;
        }

        public static int GetOrderStatusPending()
        {
            int OrderId = 0;
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_Order_select_status_Pending");
            DataAccess.AddInputOutputParameter("OrderId", OrderId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            OrderId = DataAccess.GetInt32ReturnValue("OrderId", sqlCommand);
            DataAccess.Close(sqlCommand);
            return OrderId;
        }

        public static int GetOrderWithInactiveAccount()
        {
            int OrderId = 0;
            SqlCommand sqlCommand = DataAccess.SetCommand("usp_Order_select_account_inactive");
            DataAccess.AddInputOutputParameter("OrderId", OrderId, sqlCommand);
            DataAccess.ExecuteNonQuery(sqlCommand);
            OrderId = DataAccess.GetInt32ReturnValue("OrderId", sqlCommand);
            DataAccess.Close(sqlCommand);
            return OrderId;
        }

        #endregion
    }
}
