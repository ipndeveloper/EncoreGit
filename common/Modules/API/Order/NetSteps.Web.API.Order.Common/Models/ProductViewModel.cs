using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Order.Common.Models
{
    /// <summary>
    /// Default implementation of ProductViewModel
    /// </summary>
    public class ProductViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public ProductViewModel() { }

        #endregion

        #region Properties

        /// <summary>
        /// ProductID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        public string Sku { get; set; }

        #endregion
    }
}
