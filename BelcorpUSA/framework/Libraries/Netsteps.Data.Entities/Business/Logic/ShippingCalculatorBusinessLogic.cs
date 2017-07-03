using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Business.Logic
{
    /// <summary>
    /// Clase Businnes Logic de ShippingCalculato
    /// </summary>
    public class ShippingCalculatorBusinessLogic
    {
        /// <summary>
        /// Constructor que inicializa la clase ShippingCalculatorBusinessLogic
        /// </summary>
        public ShippingCalculatorBusinessLogic()
        { 
        }

        /// <summary>
        /// Instancia de la clase ShippingCalculatorBusinessLogic
        /// </summary>
        private static ShippingCalculatorBusinessLogic instance;
        
        /// <summary>
        /// Repositorio de la interface  IShippingCalculatorRepository
        /// </summary>
        private static IShippingCalculatorRepository repositoryShippingCalculator ;
        
        /// <summary>
        /// Instancia de la clase ShippingCalculatorBusinessLogic
        /// </summary>
        public static ShippingCalculatorBusinessLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ShippingCalculatorBusinessLogic();
                    repositoryShippingCalculator = new ShippingCalculateRepository();
                }
                return instance;
            }        
        }

        /// <summary>
        /// Método que obtiene los medios de envío  Create By - FHP
        /// </summary>
        /// <param name="postalCode">Código Postal</param>
        /// <returns>Una lista con los GetShipping</returns>
        public List<ShippingCalculatorSearchData.GetShipping> GetShippingResult(string postalCode)
        {
            try
            {
                return repositoryShippingCalculator.GetShippingResult(postalCode);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que obtiene todos los Replacement Create By - FHP
        /// </summary>
        /// <param name="parameters">Objeto SippingCalculator</param>
        /// <returns>Todos los Replacement</returns>
        public int ReplacementResult(ShippingCalculatorSearchParameters parameters)
        {
            try
            {
                return repositoryShippingCalculator.ReplacementResult(parameters);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que obtiene EstimatedDeliveryDateResult Create By - FHP
        /// </summary>
        /// <param name="parameters">Objeto SippingCalculator</param>
        /// <returns>Todos EstimatedDeliveryDateResult</returns>
        public List<ShippingCalculatorSearchData.GetEstimatedDeliveryDate> GetEstimatedDeliveryDateResult(Business.HelperObjects.SearchParameters.ShippingCalculatorSearchParameters parameters)
        {
            try
            {
                return repositoryShippingCalculator.GetEstimatedDeliveryDateResult(parameters);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

    }
}
