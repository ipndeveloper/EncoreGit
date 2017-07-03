namespace NetSteps.Data.Entities.Extensions
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Utility;

    /// <summary>
    /// Extiende funcionalidad a order return confirm
    /// </summary>
    public class OrderItemReturnConfirmExtension
    {
        /// <summary>
        /// Inserta un nuevo item return confirm
        /// </summary>
        /// <param name="orderItemReturnId">Id de order item return</param>
        /// <param name="quantity">Cantidad</param>
        public static void InsItemReturnConfirm(int orderItemReturnId, int quantity)
        {
            object[] parameters = { new SqlParameter("@OrderItemReturnId", SqlDbType.Int) { Value = orderItemReturnId },
                                    new SqlParameter("@Quantity", SqlDbType.Int) { Value = quantity } };

            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = context.Database.SqlQuery<int>(DataBaseHelper.GenerateQueryString(SPRegisterOrderItemReturnConfirm, parameters), parameters).First();
            }
        }

        /// <summary>
        /// Actualiza el estado de una orden
        /// </summary>
        /// <param name="orderId">Id de la orden</param>
        /// <param name="statusId">Id del estado</param>
        /// <param name="idSupportTicket">Id tickt de soporte</param>
        /// <param name="idNationalMail">Id mail nacional</param>
        public static void UpdOrderStatus(int orderId, int statusId, int idSupportTicket, string idNationalMail)
        {
            object[] parameters = { new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId },
                                    new SqlParameter("@StatusId", SqlDbType.Int) { Value = statusId },
                                    new SqlParameter("@IdSupportTicket", SqlDbType.Int) { Value = idSupportTicket },
                                    new SqlParameter("@IDNationalMail", SqlDbType.NVarChar) { Value = idNationalMail } };

            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = context.Database.SqlQuery<int>(DataBaseHelper.GenerateQueryString(SPUpdateOrderStatus, parameters), parameters).First();
            }
        }

        private const string SPRegisterOrderItemReturnConfirm = "RegistrarOrderItemReturnConfim";

        private const string SPUpdateOrderStatus = "ActualizaOrderStatusSupportMail";
    }
}
