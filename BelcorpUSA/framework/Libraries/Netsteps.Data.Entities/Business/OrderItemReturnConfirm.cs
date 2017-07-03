
namespace NetSteps.Data.Entities.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OrderItemReturnConfirm
    {
        /// <summary>
        /// Obtiene o establece OrderItemConfirmID
        /// </summary>
        public int OrderItemConfirmID { get; set; }

        /// <summary>
        /// Obtiene o establece OrderItemReturnID
        /// </summary>
        public int OrderItemReturnID { get; set; }

        /// <summary>
        /// Obtiene o establece Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Obtiene o establece Note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Obtiene o establece ModifiedByUserID
        /// </summary>
        public int? ModifiedByUserID { get; set; }

        /// <summary>
        /// Obtiene o establece DateLastModifiedUTC
        /// </summary>
        public DateTime? DateLastModifiedUTC { get; set; }

        #region External Properties
        /// <summary>
        /// Gets or sets CUV
        /// </summary>
        public string CUV { get; set; }

        /// <summary>
        /// Gets or sets CUV
        /// </summary>
        public int Diferencia { get; set; }

        /// <summary>
        /// Gets or sets Product Name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets Item Price
        /// </summary>
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// Gets or sets Quantity Order Item Return
        /// </summary>
        public int QuantityOrderItemReturn { get; set; }

        /// <summary>
        /// Gets or sets QuantityOrderItem
        /// </summary>
        public int QuantityOrderItem { get; set; }

        /// <summary>
        /// Gets or sets Quantity Order Item ReturnConfirm
        /// </summary>
        public int QuantityOrderItemReturnConfirm { get; set; }
        #endregion
    }
}
