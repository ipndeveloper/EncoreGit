namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class OrderItemReturnDto
    {
        /// <summary>
        /// Obtiene o establece OrderItermReturnID
        /// </summary>
        public int OrderItemReturnID { get; set; }

        /// <summary>
        /// Obtiene o establece OrderItemID
        /// </summary>
        public int OrderItemID { get; set; }

        /// <summary>
        /// Obtiene o establece ReturnReasonID
        /// </summary>
        public int ReturnReasonID { get; set; }

        /// <summary>
        /// Obtiene o establece IsDestroyed
        /// </summary>
        public bool IsDestroyed { get; set; }

        /// <summary>
        /// Obtiene o establece IsRestocked
        /// </summary>
        public bool IsRestocked { get; set; }

        /// <summary>
        /// Obtiene o establece HasBeenReceived
        /// </summary>
        public bool HasBeenReceived { get; set; }

        /// <summary>
        /// Obtiene o establece Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Obtiene o establece OriginalOrderItemID
        /// </summary>
        public int OriginalOrderItemID { get; set; }

        /// <summary>
        /// Obtiene o establece DateCreatedUTC
        /// </summary>
        public DateTime DateCreatedUTC { get; set; }

        /// <summary>
        /// Obtiene o establece DateLastModifiedUTC
        /// </summary>
        public DateTime DateLastModifiedUTC { get; set; }
    }
}