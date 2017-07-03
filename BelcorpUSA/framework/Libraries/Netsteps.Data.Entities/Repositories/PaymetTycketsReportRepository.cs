using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Web.UI.WebControls;

//

namespace NetSteps.Data.Entities.Repositories
{
    public class PaymetTycketsReportRepository
    {
            private static string _reportConnectionString = string.Empty;
            private static string GetReportConnectionString()
        {

            _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();

            return _reportConnectionString;
        }
            
            public static List<InformacionFacturacion> ObtenerInformacionFacturacion(DataTable dtOrderPaymentIDs)
            {
                try
                {
                     SqlParameter op;
                    string cadena = GetReportConnectionString();
                    List<InformacionFacturacion> lstInformacionFacturacion = new List<InformacionFacturacion>();
                    InformacionFacturacion objInformacionFacturacion = null;
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "SpObtenerInformacionFacturacion";

                            op = new SqlParameter();
                            op.SqlDbType= SqlDbType.Structured;
                            op.ParameterName = "@OrderPaymentIDs";
                            op.Value = dtOrderPaymentIDs;
                            ocom.Parameters.Add(op);
                             using(IDataReader ir=ocom.ExecuteReader())
                             {
                                while(ir.Read())
                                {
                                    objInformacionFacturacion = new InformacionFacturacion();
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.AccountID.ToString()]))
                                    {
                                        objInformacionFacturacion.AccountID = Convert.ToInt32(ir[ColumnasInformacionFacturacion.AccountID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.AccountName.ToString()]))
                                    {
                                        objInformacionFacturacion.AccountName = Convert.ToString(ir[ColumnasInformacionFacturacion.AccountName.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Barrio.ToString()]))
                                    {
                                        objInformacionFacturacion.Barrio = Convert.ToString(ir[ColumnasInformacionFacturacion.Barrio.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Cep.ToString()]))
                                    {
                                        objInformacionFacturacion.Cep = Convert.ToString(ir[ColumnasInformacionFacturacion.Cep.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Ciudad.ToString()]))
                                    {
                                        objInformacionFacturacion.Ciudad = Convert.ToString(ir[ColumnasInformacionFacturacion.Ciudad.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.CurrentExpirationDateUTC.ToString()]))
                                    {
                                        objInformacionFacturacion.CurrentExpirationDateUTC = Convert.ToDateTime(ir[ColumnasInformacionFacturacion.CurrentExpirationDateUTC.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Estado.ToString()]))
                                    {
                                        objInformacionFacturacion.Estado = Convert.ToString(ir[ColumnasInformacionFacturacion.Estado.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.ExpirationId.ToString()]))
                                    {
                                        objInformacionFacturacion.ExpirationId = Convert.ToInt32(ir[ColumnasInformacionFacturacion.ExpirationId.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Numero.ToString()]))
                                    {
                                        objInformacionFacturacion.Numero = Convert.ToString(ir[ColumnasInformacionFacturacion.Numero.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.OrderId.ToString()]))
                                    {
                                        objInformacionFacturacion.OrderId = Convert.ToInt32(ir[ColumnasInformacionFacturacion.OrderId.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.OrderPaymentID.ToString()]))
                                    {
                                        objInformacionFacturacion.OrderPaymentID = Convert.ToInt32(ir[ColumnasInformacionFacturacion.OrderPaymentID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Pais.ToString()]))
                                    {
                                        objInformacionFacturacion.Pais = Convert.ToString(ir[ColumnasInformacionFacturacion.Pais.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.PaisId.ToString()]))
                                    {
                                        objInformacionFacturacion.PaisId = Convert.ToInt32(ir[ColumnasInformacionFacturacion.PaisId.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.PaymentStatus.ToString()]))
                                    {
                                        objInformacionFacturacion.PaymentStatus = Convert.ToString(ir[ColumnasInformacionFacturacion.PaymentStatus.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Referencia.ToString()]))
                                    {
                                        objInformacionFacturacion.Referencia = Convert.ToString(ir[ColumnasInformacionFacturacion.Referencia.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.Rua.ToString()]))
                                    {
                                        objInformacionFacturacion.Rua = Convert.ToString(ir[ColumnasInformacionFacturacion.Rua.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnasInformacionFacturacion.TransationId.ToString()]))
                                    {
                                        objInformacionFacturacion.TransationId = Convert.ToString(ir[ColumnasInformacionFacturacion.TransationId.ToString()]);
                                    }
                                    lstInformacionFacturacion.Add(objInformacionFacturacion);
                                }
                                return lstInformacionFacturacion;
                             }

                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            #region Reportes 
            public static DataSet GenerateTicketBB(int TicketNumber)
            {
                string cadena = GetReportConnectionString();
                DataSet dstTicketBB = new DataSet();
                SqlParameter op=null;
                try
                {
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "SpGenerateTicketBB";

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@TicketNumber";
                            op.Value = TicketNumber;
                            ocom.Parameters.Add(op);

                             using (SqlDataAdapter odta = new SqlDataAdapter())
                             {
                                 odta.SelectCommand = ocom;
                                 odta.Fill(dstTicketBB);
                                 return dstTicketBB;
                             }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            
            }
            public static DataSet GenerateTicketCaixa(int TicketNumber)
            {
                string cadena = GetReportConnectionString();
                DataSet dstTicketCaixa = new DataSet();
                SqlParameter op = null;
                try
                {
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "SpGenerateTicketCaixa";

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@TicketNumber";
                            op.Value = TicketNumber;
                            ocom.Parameters.Add(op);

                            using (SqlDataAdapter odta = new SqlDataAdapter())
                            {
                                odta.SelectCommand = ocom;
                                odta.Fill(dstTicketCaixa);
                                return dstTicketCaixa;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            public static DataSet GenerateTicketItau(int TicketNumber)
            {
                string cadena = GetReportConnectionString();
                DataSet dstTicketItau = new DataSet();
                SqlParameter op = null;
                try
                {
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "SpGenerateTicketItau";

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@TicketNumber";
                            op.Value = TicketNumber;
                            ocom.Parameters.Add(op);

                            using (SqlDataAdapter odta = new SqlDataAdapter())
                            {
                                odta.SelectCommand = ocom;
                                odta.Fill(dstTicketItau);
                                return dstTicketItau;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            public static List<PaymentInfoBancoOrden> GetInformacionBanco(int TicketNumber)
            {
                try
                {
                    SqlParameter op;
                    string cadena = GetReportConnectionString();
                    List<PaymentInfoBancoOrden> lstPaymentInfoBancoOrden = new List<PaymentInfoBancoOrden>();
                    PaymentInfoBancoOrden objPaymentInfoBancoOrden = null;
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "SpInformacionBancoOrdeNPayments";

                            //op = new SqlParameter();
                            //op.SqlDbType = SqlDbType.Structured;
                            //op.ParameterName = "@TicketNumbers";
                            //op.Value = TicketNumbers;
                            //ocom.Parameters.Add(op);
                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@TicketNumber";
                            op.Value = TicketNumber;
                            ocom.Parameters.Add(op);
                            using (IDataReader ir = ocom.ExecuteReader())
                            {
                                while (ir.Read())
                                {
                                    objPaymentInfoBancoOrden = new PaymentInfoBancoOrden();
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.BankID.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.BankID = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.BankID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.BankName.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.BankName = Convert.ToString(ir[ColumnPaymentInfoBancoOrden.BankName.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.IsCreditCard.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.IsCreditCard = Convert.ToBoolean(ir[ColumnPaymentInfoBancoOrden.IsCreditCard.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.OrderPaymentID.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.OrderPaymentID = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.OrderPaymentID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.PaymentTypeID.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.PaymentTypeID = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.PaymentTypeID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.BankCode.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.BankCode = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.BankCode.ToString()]);
                                    }
                                    lstPaymentInfoBancoOrden.Add(objPaymentInfoBancoOrden);
                                }
                                return lstPaymentInfoBancoOrden;
                            }
                        }
                    }
                }
                catch( Exception ex)
                {
                    throw ex;
                }
            }

            public static List<PaymentInfoBancoOrden> GetInformacionByOrder(int OrderID)
            {
                try
                {
                    SqlParameter op;
                    string cadena = GetReportConnectionString();
                    List<PaymentInfoBancoOrden> lstPaymentInfoBancoOrden = new List<PaymentInfoBancoOrden>();
                    PaymentInfoBancoOrden objPaymentInfoBancoOrden = null;
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "SpInformacionBancoByOrderID";

                            //op = new SqlParameter();
                            //op.SqlDbType = SqlDbType.Structured;
                            //op.ParameterName = "@TicketNumbers";
                            //op.Value = TicketNumbers;
                            //ocom.Parameters.Add(op);
                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@OrderID";
                            op.Value = OrderID;
                            ocom.Parameters.Add(op);
                            using (IDataReader ir = ocom.ExecuteReader())
                            {
                                while (ir.Read())
                                {
                                    objPaymentInfoBancoOrden = new PaymentInfoBancoOrden();
                                   
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.OrderPaymentID.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.OrderPaymentID = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.OrderPaymentID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.PaymentTypeID.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.PaymentTypeID = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.PaymentTypeID.ToString()]);
                                    }
                                    if (!Convert.IsDBNull(ir[ColumnPaymentInfoBancoOrden.BankCode.ToString()]))
                                    {
                                        objPaymentInfoBancoOrden.BankCode = Convert.ToInt32(ir[ColumnPaymentInfoBancoOrden.BankCode.ToString()]);
                                    }
                                    lstPaymentInfoBancoOrden.Add(objPaymentInfoBancoOrden);
                                }
                                return lstPaymentInfoBancoOrden;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion

            public static EmailTemplateTranslation GetEmailTemplate(int TicketNumbers, string UbicacionPDFGenerado) {
                return DataAccess.ExecWithStoreProcedureListParam<EmailTemplateTranslation>("Core", "SpConfPaymentTicketBank",
                  new SqlParameter("TicketNumber", SqlDbType.Int) { Value = TicketNumbers },
                  new SqlParameter("UbicacionPDFGenerado", SqlDbType.VarChar) { Value = UbicacionPDFGenerado }
                  ).ToList().First();
            }

            public static PaginatedList<PaymetTycketsReportSearchData> SearchTickets(PaymentTicketsSearchParameters searchParameter)
            {
                // No  sirve lo comentado porque funciona con todos los filtros menos con TicketNumber....  extraño
                //List<PaymetTycketsReportSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<PaymetTycketsReportSearchData>("Core", "sp_prueba",
                //     new SqlParameter("Ticket", SqlDbType.Int) { Value = (object)searchParameter.TicketNumber ?? DBNull.Value },
                //    new SqlParameter("NegotiationLevelID", SqlDbType.Int) { Value = (object)searchParameter.NegotiationLevelID ?? DBNull.Value },
                //    new SqlParameter("ExpirationStatusID", SqlDbType.Int) { Value = (object)searchParameter.ExpirationStatusID ?? DBNull.Value },
                //    new SqlParameter("Country", SqlDbType.Int) { Value = (object)searchParameter.Country ?? DBNull.Value },
                //    new SqlParameter("BankID", SqlDbType.Int) { Value = (object)searchParameter.BankID ?? DBNull.Value },
                //    new SqlParameter("Forefit", SqlDbType.Int) { Value = (object)searchParameter.Forefit ?? DBNull.Value },
                //    new SqlParameter("AccountID", SqlDbType.Int) { Value = (object)searchParameter._AccountID ?? DBNull.Value },
                //    new SqlParameter("ExpiriedDateFrom", SqlDbType.VarChar) { Value = (object)searchParameter.ExpiriedDateFrom ?? DBNull.Value },
                //    new SqlParameter("ExpiriedDateTo", SqlDbType.VarChar) { Value = (object)searchParameter.ExpiriedDateTo ?? DBNull.Value },
                //    new SqlParameter("LiquidationDateFrom", SqlDbType.VarChar) { Value = (object)searchParameter.LiquidationDateFrom ?? DBNull.Value },
                //    new SqlParameter("LiquidationDateTo", SqlDbType.VarChar) { Value = (object)searchParameter.LiquidationDateTo ?? DBNull.Value },
                   
                //    new SqlParameter("OrderID", SqlDbType.Int) { Value = (object)searchParameter.OrderId ?? DBNull.Value },
                //    new SqlParameter("FiscalNote", SqlDbType.VarChar) { Value = (object)searchParameter.FiscalNote ?? DBNull.Value }
                    
                //    ).ToList();

                List<PaymetTycketsReportSearchData> paginatedResult = new List<PaymetTycketsReportSearchData>();
                PaymetTycketsReportSearchData objPaymetTycketsReport = null;
                SqlParameter op = null;
                string cadena = GetReportConnectionString();
                using (SqlConnection ocon = new SqlConnection(cadena))
                {

                    ocon.Open();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = ocon;
                        ocom.CommandTimeout = 0;
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.CommandText = "sp_SearchTickets";

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@TicketNumber";
                        op.Value = (object)searchParameter.TicketNumber ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@NegotiationLevelID";
                        op.Value = (object)searchParameter.NegotiationLevelID ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@ExpirationStatusID";
                        op.Value = (object)searchParameter.ExpirationStatusID ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@Country";
                        op.Value = (object)searchParameter.Country ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@BankID";
                        op.Value = (object)searchParameter.BankID ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@Forefit";
                        op.Value = (object)searchParameter.Forefit ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@AccountID";
                        op.Value = (object)searchParameter._AccountID ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.String;
                        op.ParameterName = "@ExpiriedDateFrom";
                        op.Value = (object)searchParameter.ExpiriedDateFrom ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.String;
                        op.ParameterName = "@ExpiriedDateTo";
                        op.Value = (object)searchParameter.ExpiriedDateTo ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.String;
                        op.ParameterName = "@LiquidationDateFrom";
                        op.Value = (object)searchParameter.LiquidationDateFrom ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.String;
                        op.ParameterName = "@LiquidationDateTo";
                        op.Value = (object)searchParameter.LiquidationDateTo ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@OrderID";
                        op.Value = (object)searchParameter.OrderId ?? DBNull.Value;
                        ocom.Parameters.Add(op);


                        op = new SqlParameter();
                        op.DbType = DbType.String;
                        op.ParameterName = "@FiscalNote";
                        op.Value = (object)searchParameter.FiscalNote ?? DBNull.Value;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@OrderPaymentStatusId";
                        op.Value = (object)searchParameter.OrderPaymentStatusId ?? DBNull.Value;
                        ocom.Parameters.Add(op);
                        

                        using (IDataReader iReader = ocom.ExecuteReader())
                        {
                            while (iReader.Read())
                            {
                                objPaymetTycketsReport = new PaymetTycketsReportSearchData();


                                objPaymetTycketsReport.AccountName = Convert.ToString(iReader[PaymetTycketsReportColumn.AccountName.ToString()]);

                                objPaymetTycketsReport.AccountNumber = Convert.ToString(iReader[PaymetTycketsReportColumn.AccountNumber.ToString()]);

                                objPaymetTycketsReport.CurrentExpirationDateUTC = Convert.ToString(iReader[PaymetTycketsReportColumn.CurrentExpirationDateUTC.ToString()]);

                                objPaymetTycketsReport.FinancialAmount = Convert.ToDecimal(iReader[PaymetTycketsReportColumn.FinancialAmount.ToString()]);

                                objPaymetTycketsReport.DiscountedAmount = Convert.ToDecimal(iReader[PaymetTycketsReportColumn.DiscountedAmount.ToString()]);

                                objPaymetTycketsReport.InitialAmount = Convert.ToDecimal(iReader[PaymetTycketsReportColumn.InitialAmount.ToString()]);

                                objPaymetTycketsReport.Period = Convert.ToString(iReader[PaymetTycketsReportColumn.Period.ToString()]);

                                objPaymetTycketsReport.StatusPaymentName = Convert.ToString(iReader[PaymetTycketsReportColumn.StatusPaymentName.ToString()]);

                                objPaymetTycketsReport.TicketNumber = Convert.ToInt32(iReader[PaymetTycketsReportColumn.TicketNumber.ToString()]);

                                objPaymetTycketsReport.TotalAmount = Convert.ToDecimal(iReader[PaymetTycketsReportColumn.TotalAmount.ToString()]);                              

                                objPaymetTycketsReport.OrderID = Convert.ToInt32(iReader[PaymetTycketsReportColumn.OrderID.ToString()]);

                                //objPaymetTycketsReport.OrderPaymentID = Convert.ToInt32(iReader[PaymetTycketsReportColumn.OrderPaymentID.ToString()]);

                               
                                objPaymetTycketsReport.DateValidity = Convert.ToString(iReader[PaymetTycketsReportColumn.DateValidity.ToString()]);
                                

                                objPaymetTycketsReport.PaymentTypeID = Convert.ToString(iReader[PaymetTycketsReportColumn.PaymentTypeID.ToString()]);

                                objPaymetTycketsReport.DescPayConf = Convert.ToString(iReader[PaymetTycketsReportColumn.DescPayConf.ToString()]);

                                objPaymetTycketsReport.NameExpiration = Convert.ToString(iReader[PaymetTycketsReportColumn.NameExpiration.ToString()]);

                                objPaymetTycketsReport.ExpirationDays = Convert.ToInt32(iReader[PaymetTycketsReportColumn.ExpirationDays.ToString()]);

                                objPaymetTycketsReport.Forefit = Convert.ToInt32(iReader[PaymetTycketsReportColumn.Forefit.ToString()]);

                                objPaymetTycketsReport.ExpirationStatusID = Convert.ToInt32(iReader[PaymetTycketsReportColumn.ExpirationStatusID.ToString()]);

                                objPaymetTycketsReport.DateCreatedUTC = Convert.ToString(iReader[PaymetTycketsReportColumn.DateCreatedUTC.ToString()]);

                                objPaymetTycketsReport.NegotiationLevelName = Convert.ToString(iReader[PaymetTycketsReportColumn.NegotiationLevelName.ToString()]);

                                objPaymetTycketsReport.ViewTicket = "ViewTicket";

                                paginatedResult.Add(objPaymetTycketsReport);
                            }
                         
                        }


                    }
                }

                IQueryable<PaymetTycketsReportSearchData> matchingItems = paginatedResult.AsQueryable<PaymetTycketsReportSearchData>();

                var resultTotalCount = matchingItems.Count();
                matchingItems = matchingItems.ApplyPagination(searchParameter);

                return matchingItems.ToPaginatedList<PaymetTycketsReportSearchData>(searchParameter, resultTotalCount);
            }

            public static DataTable SearchTicketsReport(
                      PaymentTicketsSearchParameters searchParameter
                      )
            {
                try
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                  
                    SqlParameter op = null;
                    string cadena = GetReportConnectionString();
                    using (SqlConnection ocon = new SqlConnection(cadena))
                    {

                        ocon.Open();
                        using (SqlCommand ocom = new SqlCommand())
                        {
                            ocom.Connection = ocon;
                            ocom.CommandTimeout = 0;
                            ocom.CommandType = CommandType.StoredProcedure;
                            ocom.CommandText = "sp_SearchTickets";

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@TicketNumber";
                            op.Value = (object)searchParameter.TicketNumber ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@NegotiationLevelID";
                            op.Value = (object)searchParameter.NegotiationLevelID ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@ExpirationStatusID";
                            op.Value = (object)searchParameter.ExpirationStatusID ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@Country";
                            op.Value = (object)searchParameter.Country ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@BankID";
                            op.Value = (object)searchParameter.BankID ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@Forefit";
                            op.Value = (object)searchParameter.Forefit ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@AccountID";
                            op.Value = (object)searchParameter._AccountID ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.String;
                            op.ParameterName = "@ExpiriedDateFrom";
                            op.Value = (object)searchParameter.ExpiriedDateFrom ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.String;
                            op.ParameterName = "@ExpiriedDateTo";
                            op.Value = (object)searchParameter.ExpiriedDateTo ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.String;
                            op.ParameterName = "@LiquidationDateFrom";
                            op.Value = (object)searchParameter.LiquidationDateFrom ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.String;
                            op.ParameterName = "@LiquidationDateTo";
                            op.Value = (object)searchParameter.LiquidationDateTo ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@OrderID";
                            op.Value = (object)searchParameter.OrderId ?? DBNull.Value;
                            ocom.Parameters.Add(op);


                            op = new SqlParameter();
                            op.DbType = DbType.String;
                            op.ParameterName = "@FiscalNote";
                            op.Value = (object)searchParameter.FiscalNote ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            op = new SqlParameter();
                            op.DbType = DbType.Int32;
                            op.ParameterName = "@OrderPaymentStatusId";
                            op.Value = (object)searchParameter.OrderPaymentStatusId ?? DBNull.Value;
                            ocom.Parameters.Add(op);

                            using (SqlDataAdapter odta = new SqlDataAdapter())
                            {
                                odta.SelectCommand = ocom;
                                odta.Fill(ds);
                                dt = ds.Tables[0];
                                
                                ocon.Close();
                                return dt;
                            }

                          


                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

     }


   }
     
 
