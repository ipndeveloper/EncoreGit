using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Globalization;
using System.Configuration;
using NetSteps.Data.Entities.EntityModels;
namespace NetSteps.Data.Entities.Extensions
{
    public class PaymentsMethodsExtensions
    {
        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-CC-008
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OrderTypeID"></param>
        /// <returns>Proceso GetPaymentsMethods : Retorna métodos de pago disponibles para el consultor</returns>
        /// 
        public static Dictionary<string, string> GetPaymentsMethods(int AccountID, int OrderTypeID)
        {
            List<ApplyPaymentSearchData.paymentSelect> paymentMethod = DataAccess.ExecWithStoreProcedure<ApplyPaymentSearchData.paymentSelect>("Core", "spGetPaymentMethods",
             new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
             new SqlParameter("OrderTypeID", SqlDbType.Int) { Value = OrderTypeID }
            ).ToList();
            Dictionary<string, string> ApplyPaymetReturn = new Dictionary<string, string>(); 

            ApplyPaymetReturn.Add("0", Translation.GetTerm("termPaymentType", "Select Payment Type"));
            foreach (var item in paymentMethod)
            {
                ApplyPaymetReturn.Add(Convert.ToString(item.Id), item.Name);
            } 
            return ApplyPaymetReturn;
        }

        public static Dictionary<string, string>  typedispatchDisplay()
        {
            List<typedispatchDisplay> paymentMethod = DataAccess.ExecWithStoreProcedure<typedispatchDisplay>("Core", "typedispatchDisplay",
             new SqlParameter("Active", SqlDbType.Int) { Value = 1 }
            ).ToList();

            Dictionary<string, string> ApplyPaymetReturn = new Dictionary<string, string>();

            ApplyPaymetReturn.Add("0", Translation.GetTerm("Select type Dipatch", "Select type Dipatch"));
            foreach (var item in paymentMethod)
            {
                ApplyPaymetReturn.Add(Convert.ToString(item.DispatchTypeID), item.Name);
            }
            return ApplyPaymetReturn;
        }

         

        public static Dictionary<string, string> GetReturnReasons()
        {
            List<ApplyPaymentSearchData.paymentSelect> returnReasons = DataAccess.ExecWithStoreProcedure<ApplyPaymentSearchData.paymentSelect>("Core", "GetReturnReasons").ToList();
            Dictionary<string, string> ApplyReturnReasonsReturn = new Dictionary<string, string>();
            foreach (var item in returnReasons)
            {
                ApplyReturnReasonsReturn.Add(Convert.ToString(item.Id), item.Name);
            }
            return ApplyReturnReasonsReturn;
        }

        //  paymentsTable
        public static List<PaymentsTable> GetPaymentsTable(PaymentsTable param)
        {

            List<PaymentsTable> paymentsTable = DataAccess.ExecWithStoreProcedureListParam<PaymentsTable>("Core", "usp_GetPaymentsTable",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = param.PreOrderID }).ToList();

            return paymentsTable;
        }

        public static int UpdPaymentsTable(PaymentsTable param)
        {
            try{
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "usp_UpdatePaymentsTable",
                new SqlParameter("ubic", SqlDbType.Int) { Value = param.ubic },
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = param.PreOrderID },
                new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value  = (object)param.PaymentConfigurationID  ?? DBNull.Value },
                new SqlParameter("AppliedAmount", SqlDbType.VarChar) { Value = (object)param.AppliedAmount.ToString() ?? DBNull.Value  },
               new SqlParameter("PaymentStatusID", SqlDbType.Int) { Value = (object)param.PaymentStatusID ?? DBNull.Value  },
               new SqlParameter("ExpirationDate", SqlDbType.DateTime) { Value = (object)param.ExpirationDate ?? DBNull.Value   },
                new SqlParameter("NumberCuota", SqlDbType.Int) { Value = (object)param.NumberCuota ?? DBNull.Value   },
                new SqlParameter("AutorizationNumber", SqlDbType.VarChar) { Value = (object)param.AutorizationNumber  ?? DBNull.Value  },
                new SqlParameter("OrderPaymentId", SqlDbType.Int) { Value = (object)param.OrderPaymentId ?? DBNull.Value   },
            new SqlParameter("PaymentType", SqlDbType.Int) { Value = (object)param.PaymentType ?? DBNull.Value   }
               );
            return result;
             }
            catch (Exception e)
            {
                return 0;
            }

        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-CC-008
        /// </summary>
        /// <param name="NumDocument"></param>
        /// <returns>Proceso ApplyPayment : retorna orderStatusID</returns>
        public static List<ApplyPaymentSearchData.paymentYpe> ApplyPayment(ApplyPaymentSearchData param)
        {

            List<ApplyPaymentSearchData.paymentYpe> orderStatusID = DataAccess.ExecWithStoreProcedureListParam<ApplyPaymentSearchData.paymentYpe>("Core", "ApplyPayment",
                new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value = param.PaymentConfigurationID }).ToList();
            
            return orderStatusID;
        }

        public static List<ApplyPaymentSearchData.OrderShipment> GetOrderShipment(int OrderCustomerID)
        {

            return DataAccess.ExecWithStoreProcedureListParam<ApplyPaymentSearchData.OrderShipment>("Core", "uspGetOrderShipment",
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID }).ToList(); 
        } 
 
        public static int GetApplyPayment(ApplyPaymentSearchData param)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspApplyPaymentGet",
               new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value = param.PaymentConfigurationID } 
               );
            return result; 
        }

        public static int GetApplyPaymentType(int PaymentConfigurationID)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspGetPaymentTypeId",
               new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value = PaymentConfigurationID } 
               );
            return result; 
        }

        public static int GetApplyPaymentType(bool Active, int DispatchID)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUpdateDispatchStatuses",
               new SqlParameter("Active", SqlDbType.Bit) { Value = Active },
               new SqlParameter("DispatchID", SqlDbType.Int) { Value = DispatchID } 
               );
            return result; 
        }

        


        public static int UPDOrderbyID(int OrderID, int TypeSave, int WarehouseID, int SelectedPeriod, int AccountTypeID)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDOrderbyIDForAccountTypeTesting",
               new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderID },
               new SqlParameter("TypeSave", SqlDbType.Int) { Value = TypeSave },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID },
                new SqlParameter("SelectedPeriod", SqlDbType.Int) { Value = SelectedPeriod },
                new SqlParameter("AccountTypeID", SqlDbType.Int) { Value = AccountTypeID }
               );
            return result;
        }

        public static int UPDOrderbyID(int OrderID, int TypeSave, int WarehouseID)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDOrderbyID",
               new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderID },
               new SqlParameter("TypeSave", SqlDbType.Int) { Value = TypeSave },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID }
               );
            return result;
        }

        public static int UPDOrderShipments(int OrderShipmentID, string PostalCode, int WareHouseID, int OrderID, string DateEstimated)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDOrderShipments",
               new SqlParameter("OrderShipmentID", SqlDbType.Int) { Value = OrderShipmentID },
               new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = PostalCode },
               new SqlParameter("WareHouseID", SqlDbType.Int) { Value = WareHouseID },
               new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
               new SqlParameter("DateEstimated", SqlDbType.VarChar) { Value = DateEstimated }
               );
            return result;
        }


        public static int UPDOrderItemClaimsProduct(int OrderItemID, int OrderId)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDOrderItemProductClaims",
               new SqlParameter("OrderItemID", SqlDbType.Int) { Value = OrderItemID },
               new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderId }
               );
            return result;
        }

        public static int UPDOrderItemProduct(int OrderItemID, int OrderId)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDOrderItemProduct",
               new SqlParameter("OrderItemID", SqlDbType.Int) { Value = OrderItemID } ,
               new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderId } 
               );
            return result;
        }

        public static int UPDPaymentConfigurations(int OrderPaymentID, int OrderID, int PaymentConfigurationID, int? Cuota, OrderPaymentsParameters p, string GeneralParameterVal)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDPaymentConfigurations",
               new SqlParameter("OrderPaymentID", SqlDbType.Int) { Value = OrderPaymentID },
               new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
               new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value = PaymentConfigurationID },
               new SqlParameter("Cuota", SqlDbType.Int) { Value = (object)Cuota ?? DBNull.Value },
               new SqlParameter("InitialAmount", SqlDbType.Decimal) { Value = p.InitialAmount },
               new SqlParameter("ProcessOnDateUTC", SqlDbType.DateTime) { Value = (object)p.ProcessOnDateUTC ?? DBNull.Value },
               new SqlParameter("ModifiedByUserID", SqlDbType.Int) { Value = p.ModifiedByUserID },
               new SqlParameter("GeneralParameterVal", SqlDbType.VarChar) { Value = GeneralParameterVal },
               new SqlParameter("ProductCredit", SqlDbType.Decimal) { Value = p.ProductCredit }

               );
            return result;
        }

        public static int UPDAccountCredit(int AccountID, decimal ValorComparacion)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspUPDAccountCredit",
              new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
              new SqlParameter("ValorComparacion", SqlDbType.Decimal) { Value = ValorComparacion }
              );
            return result;
        }
        public static bool UPDOrderPayments(int OrderId)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspUPDOrderPayment]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OrderId", OrderId);
                        
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                rpta = Convert.ToInt32(reader["Answer"]) > 0;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                var msg = e.Message.ToString();
            }

            return rpta;
        }

        public static int ManagementKit(string PostalCode, int WareHouseID, int OrderID, string DateEstimated)
        {
            int result = DataAccess.ExecWithStoreProcedureScalar("Core", "uspManagementKit_2",
               new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderID },
               new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = PostalCode },
               new SqlParameter("WareHouseID", SqlDbType.Int) { Value = WareHouseID },
               new SqlParameter("DateEstimated", SqlDbType.VarChar) { Value = DateEstimated }               
               );
            return result;
        }
    }
}
