using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Business
{
    public class ShippingCalculator
    {
        /// <summary>
        /// Proceso Obtener medios de envío Create By - FHP
        /// </summary>
        /// <param name="postalCode">Código Postal</param>
        /// <returns>Una lista con los GetShipping</returns>
        public static List<ShippingCalculatorSearchData.GetShipping> GetShippingResult(string parameters)
        {
            try
            {
                return ShippingCalculatorExtensions.GetShippingResult(parameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }
        
    }
}
