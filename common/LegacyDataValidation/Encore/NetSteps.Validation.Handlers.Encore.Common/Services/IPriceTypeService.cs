using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;

namespace NetSteps.Validation.Handlers.Encore.Common.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPriceTypeService
    {
        /// <summary>
        /// Gets the price type from the price type id.
        /// </summary>
        /// <param name="productPriceTypeId">The product price type id.</param>
        /// <returns></returns>
        IProductPriceType GetPriceType(int productPriceTypeId);

        /// <summary>
        /// Gets the price type from the price type name.
        /// </summary>
        /// <param name="priceTypeName">Name of the price type.</param>
        /// <returns></returns>
        IProductPriceType GetPriceType(string priceTypeName);

        /// <summary>
        /// Gets the currency price types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProductPriceType> GetCurrencyPriceTypes();

        /// <summary>
        /// Gets the volume price types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProductPriceType> GetVolumePriceTypes();

        /// <summary>
        /// Determines whether [is currency price type] [the specified price type ID].
        /// </summary>
        /// <param name="priceTypeID">The price type ID.</param>
        /// <returns>
        ///   <c>true</c> if [is currency price type] [the specified price type ID]; otherwise, <c>false</c>.
        /// </returns>
        bool IsCurrencyPriceType(int priceTypeID);

        /// <summary>
        /// Gets the primary volume price type.
        /// </summary>
        /// <returns></returns>
        IProductPriceType GetPrimaryVolumeType();

        /// <summary>
        /// Gets all price types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProductPriceType> GetAllPriceTypes();
    }
}
