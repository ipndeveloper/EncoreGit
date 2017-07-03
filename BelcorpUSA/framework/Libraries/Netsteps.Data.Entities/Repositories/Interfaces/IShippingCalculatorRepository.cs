using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    /// <summary>
    /// Interface que implementa los métodos de que contendrán los ShippingCalculator Create By - FHP
    /// </summary>
    public interface IShippingCalculatorRepository
    {
        List<ShippingCalculatorSearchData.GetShipping> GetShippingResult(string postalCode);
        int ReplacementResult(ShippingCalculatorSearchParameters parameters);
        List<ShippingCalculatorSearchData.GetEstimatedDeliveryDate> GetEstimatedDeliveryDateResult(ShippingCalculatorSearchParameters parameters);
    }
}
