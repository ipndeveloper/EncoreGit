using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Common.Services.ServiceModels
{
    public interface IProductPriceType
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the price type ID.
        /// </summary>
        /// <value>
        /// The price type ID.
        /// </value>
        int PriceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        PriceTypeCategory Category { get; set; }
    }
}
