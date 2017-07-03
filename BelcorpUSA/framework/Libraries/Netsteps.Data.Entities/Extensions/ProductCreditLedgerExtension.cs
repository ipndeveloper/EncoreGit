using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using System.Configuration;

namespace NetSteps.Data.Entities.Extensions
{
    public class ProductCreditLedgerExtension
    {
        #region ProductCreditLedger (KTC-CSTI)

        public static void CreateProductCreditLedger(ProductCreditLedgerParameters productCreditLedgerModel)
        {
            SqlParameter paramOrderPaymentID = new SqlParameter();
            paramOrderPaymentID.SqlDbType = SqlDbType.Decimal;
            paramOrderPaymentID.ParameterName = "OrderPaymentID";

            if (productCreditLedgerModel.OrderPaymentID.HasValue)
            {
                paramOrderPaymentID.Value =productCreditLedgerModel.OrderPaymentID.Value;
            }
            else
            {
                paramOrderPaymentID.Value = DBNull.Value;
            }
            
            int preOrderId = DataAccess.ExecWithStoreProcedureSave("Commissions", "upsInsProductCreditLedger",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = productCreditLedgerModel.AccountID },
                new SqlParameter("EntryReasonID", SqlDbType.Int) { Value = productCreditLedgerModel.EntryReasonID },
                new SqlParameter("EntryOriginID", SqlDbType.Int) { Value = productCreditLedgerModel.EntryOriginID },
                new SqlParameter("EntryTypeID", SqlDbType.Int) { Value = productCreditLedgerModel.EntryTypeID },
                new SqlParameter("UserID", SqlDbType.Int) { Value = productCreditLedgerModel.UserID },
                new SqlParameter("EntryAmount", SqlDbType.Decimal) { Value = productCreditLedgerModel.EntryAmount },
                new SqlParameter("CurrencyTypeID", SqlDbType.Int) { Value = productCreditLedgerModel.CurrencyTypeID },
                new SqlParameter("OrderID", SqlDbType.Int) { Value = productCreditLedgerModel.OrderID },
                paramOrderPaymentID
             );          
        }
       
        #endregion

        public static void ProcedimientoCuentaCorriente(int OrderID, string TipoMovimiento, decimal MontoParcial, int UserID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;

                    transaction = connection.BeginTransaction();

                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = "SPUPDRETURNSCC";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                        command.Parameters.Add(new SqlParameter("@TipoMovimiento", TipoMovimiento));
                        command.Parameters.Add(new SqlParameter("@MontoParcial", MontoParcial));
                        command.Parameters.Add(new SqlParameter("@UserId", UserID));

                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
