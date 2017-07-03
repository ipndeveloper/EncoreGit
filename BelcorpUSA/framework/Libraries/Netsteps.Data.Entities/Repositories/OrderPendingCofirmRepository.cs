namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    public class OrderPendingCofirmRepository : IOrderPendingConfirmRepository
    {
        private static string _reportConnectionString = string.Empty;

        private static string GetReportConnectionString()
        {

            _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();

            return _reportConnectionString;
        }

        public IEnumerable<OrderDto> OrderSearch(DateTime? DateStart, DateTime? DateEnd, string OrderNumber)
        {
            List<OrderDto> lstOrderPending = new List<OrderDto>();
            OrderDto orderPending = null;
            SqlParameter op = null;
            IDataReader iReader = null;
            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "SeleccionarOrderPending";

                    op = new SqlParameter();
                    op.DbType = DbType.DateTime;
                    op.ParameterName = "@DateStart";
                    if (!DateStart.HasValue)
                    {
                        op.Value = DBNull.Value;
                    }
                    else
                    {
                        op.Value = DateStart;
                    }
                    ocom.Parameters.Add(op);


                    op = new SqlParameter();
                    op.DbType = DbType.DateTime;
                    op.ParameterName = "@DateEnd";
                    if (!DateEnd.HasValue)
                    {
                        op.Value = DBNull.Value;
                    }
                    else
                    {
                        op.Value = DateEnd;
                    }
                    ocom.Parameters.Add(op);

                    op = new SqlParameter();
                    op.DbType = DbType.String;
                    op.ParameterName = "@OrderNumber";
                    op.Value = OrderNumber;
                    ocom.Parameters.Add(op);


                    using (iReader = ocom.ExecuteReader())
                    {
                        #region cargar data
                        while (iReader.Read())
                        {
                            orderPending = new OrderDto();
                            if (!Convert.IsDBNull(iReader["OrderNumber"]))
                            {
                                orderPending.OrderNumber = Convert.ToString(iReader["OrderNumber"]);
                            }
                            if (!Convert.IsDBNull(iReader["AccountNumber"]))
                            {
                                orderPending.AccountNumber = Convert.ToString(iReader["AccountNumber"]);
                            }
                            if (!Convert.IsDBNull(iReader["IDSupportTicket"]))
                            {
                                orderPending.IDSupportTicket = Convert.ToInt32(iReader["IDSupportTicket"]);
                            }
                            if (!Convert.IsDBNull(iReader["IDNationalMail"]))
                            {
                                orderPending.IDNationalMail = Convert.ToString(iReader["IDNationalMail"]);
                            }
                            if (!Convert.IsDBNull(iReader["DateCreatedUTC"]))
                            {
                                orderPending.DateCreatedUTC = Convert.ToDateTime(iReader["DateCreatedUTC"]);
                            }
                            if (!Convert.IsDBNull(iReader["FirstName"]))
                            {
                                orderPending.CustomerFirstName = Convert.ToString(iReader["FirstName"]);
                            }
                            if (!Convert.IsDBNull(iReader["LastName"]))
                            {
                                orderPending.CustomerLastName = Convert.ToString(iReader["LastName"]);
                            }
                            if (!Convert.IsDBNull(iReader["OrderID"]))
                            {
                                orderPending.OrderID = Convert.ToInt32(iReader["OrderID"]);
                            }

                            lstOrderPending.Add(orderPending);
                        }
                        return lstOrderPending;
                        #endregion
                    }

                }
            }
        }

        public IEnumerable<OrderItemReturnConfirmDto> GetDetaillOrderItemReturnConfirm(string OrderNumber)
        {
            List<OrderItemReturnConfirmDto> lstOrderItemReturnConfirm = new List<OrderItemReturnConfirmDto>();
            OrderItemReturnConfirmDto orderItemReturnConfirm = null;
            SqlParameter op = null;
            IDataReader iReader = null;
            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "ObtieneOrderItemReturnConfirm";

                    op = new SqlParameter();
                    op.DbType = DbType.String;
                    op.ParameterName = "@OrderNumber";
                    op.Value = OrderNumber;
                    ocom.Parameters.Add(op);
                    #region cargar data
                    try
                    {
                        using (iReader = ocom.ExecuteReader())
                        {
                            while (iReader.Read())
                            {
                                orderItemReturnConfirm = new OrderItemReturnConfirmDto();
                                if (!Convert.IsDBNull(iReader["CUV"]))
                                {
                                    orderItemReturnConfirm.CUV = Convert.ToString(iReader["CUV"]);
                                }
                                if (!Convert.IsDBNull(iReader["Note"]))
                                {
                                    orderItemReturnConfirm.Note = Convert.ToString(iReader["Note"]);
                                }
                                if (!Convert.IsDBNull(iReader["Diferencia"]))
                                {
                                    orderItemReturnConfirm.Diferencia = Convert.ToInt32(iReader["Diferencia"]);
                                }
                                if (!Convert.IsDBNull(iReader["OrderItemConfirmID"]))
                                {
                                    orderItemReturnConfirm.OrderItemConfirmID = Convert.ToInt32(iReader["OrderItemConfirmID"]);
                                }
                                if (!Convert.IsDBNull(iReader["ProductName"]))
                                {
                                    orderItemReturnConfirm.ProductName = Convert.ToString(iReader["ProductName"]);
                                }
                                if (!Convert.IsDBNull(iReader["QuantityOrderItem"]))
                                {
                                    orderItemReturnConfirm.QuantityOrderItem = Convert.ToInt32(iReader["QuantityOrderItem"]);
                                }
                                if (!Convert.IsDBNull(iReader["QuantityOrderItemReturn"]))
                                {
                                    orderItemReturnConfirm.QuantityOrderItemReturn = Convert.ToInt32(iReader["QuantityOrderItemReturn"]);
                                }
                                if (!Convert.IsDBNull(iReader["QuantityOrderItemReturnConfirm"]))
                                {
                                    orderItemReturnConfirm.QuantityOrderItemReturnConfirm = Convert.ToInt32(iReader["QuantityOrderItemReturnConfirm"]);
                                }
                                if (!Convert.IsDBNull(iReader["OrderItemReturnID"]))
                                {
                                    orderItemReturnConfirm.OrderItemReturnID = Convert.ToInt32(iReader["OrderItemReturnID"]);
                                }
                                if (!Convert.IsDBNull(iReader["ItemPrice"]))
                                {
                                    orderItemReturnConfirm.ItemPrice = Convert.ToDecimal(iReader["ItemPrice"]);
                                }

                                lstOrderItemReturnConfirm.Add(orderItemReturnConfirm);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return lstOrderItemReturnConfirm;
                    #endregion

                }
            }
        }

        public int InsertOrderPendingConfirm(IEnumerable<OrderItemReturnConfirmDto> lstOrderItemReturnConfirm, int orderId, string orderNote, int noteType)
        {
            List<OrderItemReturnConfirmDto> orderItemReturnConfirms = lstOrderItemReturnConfirm.ToList();
            int resultado = 0;
            SqlParameter parameter = null;
            SqlTransaction transaction = null;
            string connectionString = GetReportConnectionString();
            int count = orderItemReturnConfirms.Count();
            try
            {
                using (SqlConnection ocon = new SqlConnection(connectionString))
                {
                    ocon.Open();
                    transaction = ocon.BeginTransaction();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = ocon;
                        command.Transaction = transaction;

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertaOActualizaOrdenNota";

                        parameter = new SqlParameter();
                        parameter.DbType = DbType.Int32;
                        parameter.ParameterName = "@OrderId";
                        parameter.Value = orderId;
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter();
                        parameter.DbType = DbType.String;
                        parameter.ParameterName = "@OrderNote";
                        parameter.Value = orderNote;
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter();
                        parameter.DbType = DbType.Int32;
                        parameter.ParameterName = "@NoteTypeID";
                        parameter.Value = noteType;
                        command.Parameters.Add(parameter);

                        resultado = Convert.ToInt32(command.ExecuteScalar());
                    }

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = ocon;
                        command.Transaction = transaction;

                        command.CommandTimeout = 0;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertarOrderItemReturnConfirm";
                        for (int index = 0; index < count; index++)
                        {
                            command.Parameters.Clear();

                            parameter = new SqlParameter();
                            parameter.DbType = DbType.Int32;
                            parameter.ParameterName = "@OrderItemConfirmID";
                            parameter.Value = orderItemReturnConfirms[index].OrderItemConfirmID;
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter();
                            parameter.DbType = DbType.Int32;
                            parameter.ParameterName = "@OrderItemReturnID";
                            parameter.Value = orderItemReturnConfirms[index].OrderItemReturnID;
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter();
                            parameter.DbType = DbType.Int32;
                            parameter.ParameterName = "@Quantity";
                            parameter.Value = orderItemReturnConfirms[index].Quantity;
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter();
                            parameter.DbType = DbType.String;
                            parameter.ParameterName = "@Note";

                            if (orderItemReturnConfirms[index].Note == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = orderItemReturnConfirms[index].Note;
                            }

                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter();
                            parameter.DbType = DbType.Int32;
                            parameter.ParameterName = "@ModifiedByUserID";

                            if (!orderItemReturnConfirms[index].ModifiedByUserID.HasValue)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = orderItemReturnConfirms[index].ModifiedByUserID;
                            }

                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter();
                            parameter.DbType = DbType.DateTime;
                            parameter.ParameterName = "@DateLastModifiedUTC";
                            if (!orderItemReturnConfirms[index].DateLastModifiedUTC.HasValue)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = orderItemReturnConfirms[index].DateLastModifiedUTC;
                            }

                            command.Parameters.Add(parameter);

                            resultado = Convert.ToInt32(command.ExecuteScalar());
                        }

                        transaction.Commit();
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }
}