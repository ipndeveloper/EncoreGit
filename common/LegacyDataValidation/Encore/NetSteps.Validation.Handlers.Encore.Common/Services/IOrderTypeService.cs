
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Common.Services
{
    public interface IOrderTypeService
    {
        IOrderType GetOrderType(int orderTypeId);

        /// <summary>
        /// Gets the Order type from the Order type name.
        /// </summary>
        /// <param name="orderTypeName">Name of the order type.</param>
        /// <returns></returns>
        IOrderType GetOrderType(string orderTypeName);

        /// <summary>
        /// Gets all order types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOrderType> GetAllOrderTypes();
    }
}
