using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Métodos que son implementados del Req. BR-LG-004
    /// </summary>  
    
    public class ShippingCalculatorExtensions
    {
        /// <summary>
        /// Proceso Obtener medios de envío  Create By - FHP
        /// </summary>
        /// <param name="postalCode">Código Postal</param>
        /// <returns>Una lista con los GetShipping</returns>
        public static List<ShippingCalculatorSearchData.GetShipping> GetShippingResult(string postalCode)
        {
            if (String.IsNullOrEmpty(postalCode))
            {
                return new List<ShippingCalculatorSearchData.GetShipping>();
            }
            else
            {
                List<ShippingCalculatorSearchData.GetShipping> getShippingResult = DataAccess.ExecWithStoreProcedureListParam<ShippingCalculatorSearchData.GetShipping>("Core", "uspGetProcessShipping",
                 new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = postalCode }
               ).ToList();
                return getShippingResult;
            }
        }


        public static List<ShippingCalculatorSearchData.GetProductQuantity> GetProductQuantity(string OrderNumber)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ShippingCalculatorSearchData.GetProductQuantity>("Core", "uspGetProductQuantity",
                 new SqlParameter("OrderNumber ", SqlDbType.VarChar) { Value = OrderNumber }
               ).ToList();
        }

        public static int DispatchItemControlsDel(int AccountID, int OrderID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspDispatchItemControlsDel",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
               );
        }

       
        public static List<ShippingCalculatorSearchData.GetOrderDetails> GetPreOderDetail(int OrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ShippingCalculatorSearchData.GetOrderDetails>("Core", "uspGetPreOderDetail",
                 new SqlParameter("OrderID ", SqlDbType.Int) { Value = OrderID }
               ).ToList();
        }

        public static List<ShippingCalculatorSearchData.GetShippingRateGroup> GetShippingRateGroup(int ShippingOrderTypeID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ShippingCalculatorSearchData.GetShippingRateGroup>("Core", "uspShippingRateGroup",
                 new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = ShippingOrderTypeID }
               ).ToList();  
        }

        public static int ReplacementResult(ShippingCalculatorSearchParameters parameters)
        {
            return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspGetFreightValues",
                new SqlParameter("OrderValue", SqlDbType.Int) { Value = parameters.OrderValue },
                new SqlParameter("ShippingRateGroupID", SqlDbType.Int) { Value = parameters.ShippingRateGroupID }
               );
        }

        public static List<ShippingCalculatorSearchData.GetEstimatedDeliveryDate> GetEstimatedDeliveryDateResult(ShippingCalculatorSearchParameters parameters)
        {
            List<ShippingCalculatorSearchData.GetEstimatedDeliveryDate> getEstimatedDeliveryDateResult = DataAccess.ExecWithStoreProcedureListParam<ShippingCalculatorSearchData.GetEstimatedDeliveryDate>("Core", "uspGetEstimatedDeliveryDate",
                //new SqlParameter("ApprovalDate", SqlDbType.Int) { Value = parameters.ApprovalDate },
                new SqlParameter("LogisticsProviderID", SqlDbType.Int) { Value = parameters.LogisticsProviderID },
                new SqlParameter("ShippingRateGroupID", SqlDbType.Int) { Value = parameters.ShippingRateGroupID },
                new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = parameters.PostalCode }
               ).ToList();
            return getEstimatedDeliveryDateResult;
        }
        public static string GetDateDelivery(ShippingCalculatorSearchParameters parameters)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<string>("Core", "uspGetDateDelivery",
                new SqlParameter("LogisticsProviderID", SqlDbType.Int) { Value = parameters.LogisticsProviderID },
                new SqlParameter("ShippingRateGroupID", SqlDbType.Int) { Value = parameters.ShippingRateGroupID },
                new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = parameters.PostalCode },
                new SqlParameter("OrderTypeID", SqlDbType.Int) { Value = parameters.OrderTypeID },
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = parameters.ShippingOrderTypeID }
               );
        }
    


        public static int GetWareHouseIdByOrderID(int orderId)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetWareHouseIdByOrderID",
                new SqlParameter("OrderId", SqlDbType.Int) { Value = orderId } 
               );
        }
    }
}
