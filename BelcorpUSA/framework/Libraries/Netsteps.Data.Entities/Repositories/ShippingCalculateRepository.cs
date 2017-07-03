using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Clase Repository que implenta el acceso a Datos Create By - FHP
    /// </summary>
    public class ShippingCalculateRepository : IShippingCalculatorRepository
    {
        /// <summary>
        /// Método que obtiene los medios de envío  Create By - FHP
        /// </summary>
        /// <param name="postalCode">Código Postal</param>
        /// <returns>Una lista con los GetShipping</returns>
        public List<ShippingCalculatorSearchData.GetShipping> GetShippingResult(string postalCode)
        {
            List<ShippingCalculatorSearchData.GetShipping> getShippingResult = DataAccess.ExecWithStoreProcedure<ShippingCalculatorSearchData.GetShipping>(ConnectionStrings.BelcorpCore, "uspGetProcessShipping",
                 new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = postalCode }
               ).ToList();
            return getShippingResult;
        }

        /// <summary>
        /// Método que obtiene todos los Replacement Create By - FHP
        /// </summary>
        /// <param name="parameters">Objeto SippingCalculator</param>
        /// <returns>Todos los Replacement</returns>
        public int ReplacementResult(ShippingCalculatorSearchParameters parameters)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspGetFreightValues",
                new SqlParameter("OrderValue", SqlDbType.Int) { Value = parameters.OrderValue },
                new SqlParameter("ShippingRateGroupID", SqlDbType.Int) { Value = parameters.ShippingRateGroupID }
               );
        }

        /// <summary>
        /// Método que obtiene EstimatedDeliveryDateResult Create By - FHP
        /// </summary>
        /// <param name="parameters">Objeto SippingCalculator</param>
        /// <returns>Todos EstimatedDeliveryDateResult</returns>
        public List<ShippingCalculatorSearchData.GetEstimatedDeliveryDate> GetEstimatedDeliveryDateResult(Business.HelperObjects.SearchParameters.ShippingCalculatorSearchParameters parameters)
        {
            return DataAccess.ExecWithStoreProcedure<ShippingCalculatorSearchData.GetEstimatedDeliveryDate>(ConnectionStrings.BelcorpCore, "uspGetEstimatedDeliveryDate",
               new SqlParameter("ApprovalDate", SqlDbType.Int) { Value = parameters.ApprovalDate },
               new SqlParameter("LogisticsProviderID", SqlDbType.Int) { Value = parameters.LogisticsProviderID },
               new SqlParameter("ShippingRateGroupID", SqlDbType.Int) { Value = parameters.ShippingRateGroupID },
               new SqlParameter("PostalCode", SqlDbType.Int) { Value = parameters.PostalCode }
              ).ToList();
        }
    }
}
