using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects;

namespace NetSteps.Data.Entities.Repositories
{
    #region Modifications
    // @01 GYS - BR-CC-17 - Reporte de Estado Mensual de Boletos de Pago
    #endregion

    public partial class OrderPaymentRepository : IOrderPaymentRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<OrderPayment>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<OrderPayment>>(
                   (context) => from op in context.OrderPayments
                                               .Include("OrderPaymentResults")
                                select op);
            }
        }
        #endregion

         
        public List<OrderPaymentResult> LoadOrderPaymentResultsByOrderPaymentID(int orderPaymentID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = (from a in context.OrderPaymentResults
                                  where a.OrderPaymentID == orderPaymentID
                                  select a).ToList();
                    return result;
                }
            });
        }

        public bool HasOrderPaymentResults(int orderPaymentID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = (from a in context.OrderPaymentResults
                                  where a.OrderPaymentID == orderPaymentID
                                  select a).Count();
                    return result > 0;
                }
            });
        }

        public IEnumerable<IOrderPayment> LoadEftOrderPayments()
        {
            return (IEnumerable<IOrderPayment>)ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                {
                    using (NetStepsEntities context = CreateContext())
                    {
                        var result = (from a in context.OrderPayments
                                      where a.PaymentTypeID == (int)Constants.PaymentType.EFT 
                                      select a).ToList();
                        return result;
                    }
                });
        }

	    public IEnumerable<IOrderPayment> GetUnSubmittedOrderPaymentsByClassType(string classType)
	    {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.OrderPayments.Where(op => op.PaymentTypeID == (int) Constants.PaymentType.EFT && op.NachaClassType.Equals(classType, StringComparison.InvariantCultureIgnoreCase) && op.NachaSentDate == null).Cast<IOrderPayment>().ToList();
                }
            });
	    }

	    public IOrderPayment LoadEftOrderPaymentByOrderId(int orderId)
        {
            return (IOrderPayment)ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                {
                    using (NetStepsEntities context = CreateContext())
                    {
                        var result = (from a in context.OrderPayments
                                      where a.OrderID == orderId select a).FirstOrDefault();
                        return result;
                    }
                });
        }

        public IOrderPayment LoadOrderPaymentByOrderPaymentId(int orderPaymentId)
        {
            return (IOrderPayment)ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                {
                    using (NetStepsEntities context = CreateContext())
                    {
                        var result = (from a in context.OrderPayments
                                      where a.OrderPaymentID == orderPaymentId
                                      select a).FirstOrDefault();
                        return result;
                    }
                });
        }
        public IOrderPayment LoadOrderPaymentByOrderId(int orderId)
        {
            return (IOrderPayment)ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = (from a in context.OrderPayments
                                  where a.OrderID == orderId
                                  select a).FirstOrDefault();
                    return result;
                }
            });
        }
		/// <summary>
		/// This method was added to the interface, but the implementation seems to have been lost in a merge. -Lundy
		/// </summary>
		public virtual IEnumerable<OrderPayment> GetUnSubmittedOrderPaymentsByClassTypeAndCountryID(string classType, int countryID)
		{
			throw new NotImplementedException();
		}

        #region Modifications @1
        public List<DebtsPerAgeSearchData> GetTableDebtsPerAge(DebtsPerAgeSearchParameters parameters)
        {
            List<DebtsPerAgeSearchData> debtList = new List<DebtsPerAgeSearchData>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetTableDebtsPerAge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Parameters
                        cmd.Parameters.AddWithValue("@AccountId", parameters.AccountId.HasValue ? parameters.AccountId : 0);
                        cmd.Parameters.AddWithValue("@StartBirthDate", parameters.StartBirthDate);
                        cmd.Parameters.AddWithValue("@EndBirthDate", parameters.EndBirthDate);
                        cmd.Parameters.AddWithValue("@StartDueDate", parameters.StartDueDate);
                        cmd.Parameters.AddWithValue("@EndDueDate", parameters.EndDueDate);
                        cmd.Parameters.AddWithValue("@DaysOverdueStart", parameters.DaysOverdueStart);
                        cmd.Parameters.AddWithValue("@DaysOverdueEnd", parameters.DaysOverdueEnd);
                        cmd.Parameters.AddWithValue("@OrderNumber", parameters.OrderNumber);
                        cmd.Parameters.AddWithValue("@Forfeit", parameters.Forfeit);
                        cmd.Parameters.AddWithValue("@pPageNumber", parameters.PageNumber);
                        cmd.Parameters.AddWithValue("@pPageSize", parameters.PageSize);
                        #endregion

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DebtsPerAgeSearchData debt = null;
                            while (reader.Read())
                            {
                                debt = new DebtsPerAgeSearchData();

                                #region Assign Values
                                debt.AccountNumber = reader["AccountID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AccountID"]);
                                debt.FirstName = reader["FirstName"] == DBNull.Value ? null : reader["FirstName"].ToString();
                                debt.LastName = reader["LastName"] == DBNull.Value ? null : reader["LastName"].ToString();
                                debt.PaymentTicketNumber = reader["TicketNumber"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["TicketNumber"]);
                                debt.OrderNumber = reader["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderID"]);
                                debt.NfeNumber = reader["InvoiceNumber"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["InvoiceNumber"]);
                                debt.OrderDate = reader["CompleteDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CompleteDateUTC"]);
                                debt.ExpirationDate = reader["CurrentExpirationDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CurrentExpirationDateUTC"]);
                                debt.BalanceDate = reader["DateLastModifiedUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["DateLastModifiedUTC"]);
                                debt.OriginalBalance = reader["InitialAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["InitialAmount"]);
                                debt.CurrentBalance = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalAmount"]);
                                debt.OverdueDays = reader["OverdueDays"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OverdueDays"]);
                                debt.Forfeit = reader["Forfeit"] == DBNull.Value ? false : Convert.ToBoolean(reader["Forfeit"]);
                                debt.Period = reader["PeriodID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeriodID"]);
                                debt.DateOfBirth = reader["BirthdayUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["BirthdayUTC"]);
                                
                                debt.TotalRows = Convert.ToInt32(reader["TotalRows"]);
                                debt.TotalPages = Convert.ToInt32(reader["TotalPages"]);
                                #endregion

                                debtList.Add(debt);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                debtList = new List<DebtsPerAgeSearchData>();
            }

            return debtList;
        }

        public List<DebtsPerAgeSearchData> TableDebtsPerAgeExport(DebtsPerAgeSearchParameters parameters)
        {
            List<DebtsPerAgeSearchData> debtList = new List<DebtsPerAgeSearchData>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[TableDebtsPerAgeFull]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Parameters
                        cmd.Parameters.AddWithValue("@AccountId", parameters.AccountId.HasValue ? parameters.AccountId : 0);
                        cmd.Parameters.AddWithValue("@StartBirthDate", parameters.StartBirthDate);
                        cmd.Parameters.AddWithValue("@EndBirthDate", parameters.EndBirthDate);
                        cmd.Parameters.AddWithValue("@StartDueDate", parameters.StartDueDate);
                        cmd.Parameters.AddWithValue("@EndDueDate", parameters.EndDueDate);
                        cmd.Parameters.AddWithValue("@DaysOverdueStart", parameters.DaysOverdueStart);
                        cmd.Parameters.AddWithValue("@DaysOverdueEnd", parameters.DaysOverdueEnd);
                        cmd.Parameters.AddWithValue("@OrderNumber", parameters.OrderNumber);
                        cmd.Parameters.AddWithValue("@Forfeit", parameters.Forfeit);
                        #endregion

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DebtsPerAgeSearchData debt = null;
                            while (reader.Read())
                            {
                                debt = new DebtsPerAgeSearchData();

                                #region Assign Values
                                debt.AccountNumber = reader["AccountID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AccountID"]);
                                debt.FirstName = reader["FirstName"] == DBNull.Value ? null : reader["FirstName"].ToString();
                                debt.LastName = reader["LastName"] == DBNull.Value ? null : reader["LastName"].ToString();
                                debt.PaymentTicketNumber = reader["TicketNumber"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["TicketNumber"]);
                                debt.OrderNumber = reader["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderID"]);
                                debt.NfeNumber = reader["InvoiceNumber"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["InvoiceNumber"]);
                                debt.OrderDate = reader["CompleteDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CompleteDateUTC"]);
                                debt.ExpirationDate = reader["CurrentExpirationDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CurrentExpirationDateUTC"]);
                                debt.BalanceDate = reader["DateLastModifiedUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["DateLastModifiedUTC"]);
                                debt.OriginalBalance = reader["InitialAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["InitialAmount"]);
                                debt.CurrentBalance = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalAmount"]);
                                debt.OverdueDays = reader["OverdueDays"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OverdueDays"]);
                                debt.Forfeit = reader["Forfeit"] == DBNull.Value ? false : Convert.ToBoolean(reader["Forfeit"]);
                                debt.Period = reader["PeriodID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeriodID"]);
                                debt.DateOfBirth = reader["BirthdayUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["BirthdayUTC"]);
                                #endregion

                                debtList.Add(debt);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                debtList = new List<DebtsPerAgeSearchData>();
            }

            return debtList;
        }

        public List<TicketPaymentPerMonthSearchData> GetTableTicketPaymentsPerMonth(TicketPaymentPerMonthSearchParameters parameters)
        {
            List<TicketPaymentPerMonthSearchData> ticketList = new List<TicketPaymentPerMonthSearchData>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetTableTicketPaymentsPerMonth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Parameters
                        cmd.Parameters.AddWithValue("@TicketNumber", parameters.TicketNumber.HasValue ? parameters.TicketNumber.Value : 0);
                        cmd.Parameters.AddWithValue("@AccountId", parameters.AccountId.HasValue ? parameters.AccountId.Value : 0);
                        cmd.Parameters.AddWithValue("@StartIssueDate", parameters.StartIssueDate);
                        cmd.Parameters.AddWithValue("@EndIssueDate", parameters.EndIssueDate);
                        cmd.Parameters.AddWithValue("@StartDueDate", parameters.StartDueDate);
                        cmd.Parameters.AddWithValue("@EndDueDate", parameters.EndDueDate);
                        cmd.Parameters.AddWithValue("@OrderNumber", parameters.OrderNumber);
                        cmd.Parameters.AddWithValue("@StatusId", parameters.StatusId);
                        cmd.Parameters.AddWithValue("@Month", parameters.Month);
                        cmd.Parameters.AddWithValue("@pPageNumber", parameters.PageNumber);
                        cmd.Parameters.AddWithValue("@pPageSize", parameters.PageSize);
                        #endregion

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            TicketPaymentPerMonthSearchData ticket = null;
                            while (reader.Read())
                            {
                                ticket = new TicketPaymentPerMonthSearchData();

                                #region Assign Values
                                ticket.PaymentTicketNumber = reader["TicketNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TicketNumber"]);
                                ticket.OrderNumber = Convert.ToInt32(reader["OrderID"]);
                                ticket.NfeNumber = reader["InvoiceNumber"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["InvoiceNumber"]);
                                ticket.OrderDate = reader["CompleteDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CompleteDateUTC"]);
                                ticket.ExpirationDate = reader["CurrentExpirationDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CurrentExpirationDateUTC"]);
                                ticket.BalanceDate = reader["DateModifiedUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["DateModifiedUTC"]);
                                ticket.OriginalBalance = Convert.ToDecimal(reader["InitialAmount"]);
                                ticket.CurrentBalance = Convert.ToDecimal(reader["TotalAmount"]);
                                ticket.Status = Convert.ToInt32(reader["OrderPaymentStatusID"]);
                                ticket.OriginalExpirationDate = reader["OriginalExpirationDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["OriginalExpirationDate"]);
                                ticket.AccountNumber = Convert.ToInt32(reader["AccountID"]);
                                ticket.FirstName = reader["FirstName"].ToString();
                                ticket.LastName = reader["LastName"].ToString();
                                ticket.PhoneNumber = reader["PhoneNumber"].ToString();

                                ticket.TotalPages = Convert.ToInt32(reader["TotalPages"]);
                                ticket.TotalRows = Convert.ToInt32(reader["TotalRows"]);
                                #endregion

                                ticketList.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ticketList = new List<TicketPaymentPerMonthSearchData>();
            }

            return ticketList;
        }

        public List<TicketPaymentPerMonthSearchData> TableTicketPaymentsPerMonthExport(TicketPaymentPerMonthSearchParameters parameters)
        {
            List<TicketPaymentPerMonthSearchData> ticketList = new List<TicketPaymentPerMonthSearchData>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[TableTicketPaymentsPerMonthFull]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Parameters
                        cmd.Parameters.AddWithValue("@TicketNumber", parameters.TicketNumber.HasValue ? parameters.TicketNumber.Value : 0);
                        cmd.Parameters.AddWithValue("@AccountId", parameters.AccountId.HasValue ? parameters.AccountId.Value : 0);
                        cmd.Parameters.AddWithValue("@StartIssueDate", parameters.StartIssueDate);
                        cmd.Parameters.AddWithValue("@EndIssueDate", parameters.EndIssueDate);
                        cmd.Parameters.AddWithValue("@StartDueDate", parameters.StartDueDate);
                        cmd.Parameters.AddWithValue("@EndDueDate", parameters.EndDueDate);
                        cmd.Parameters.AddWithValue("@OrderNumber", parameters.OrderNumber);
                        cmd.Parameters.AddWithValue("@StatusId", parameters.StatusId);
                        cmd.Parameters.AddWithValue("@Month", parameters.Month);
                        #endregion

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            TicketPaymentPerMonthSearchData ticket = null;
                            while (reader.Read())
                            {
                                ticket = new TicketPaymentPerMonthSearchData();

                                #region Assign Values
                                ticket.PaymentTicketNumber = reader["TicketNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TicketNumber"]);
                                ticket.OrderNumber = Convert.ToInt32(reader["OrderID"]);
                                ticket.NfeNumber = reader["InvoiceNumber"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["InvoiceNumber"]);
                                ticket.OrderDate = reader["CompleteDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CompleteDateUTC"]);
                                ticket.ExpirationDate = reader["CurrentExpirationDateUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CurrentExpirationDateUTC"]);
                                ticket.BalanceDate = reader["DateModifiedUTC"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["DateModifiedUTC"]);
                                ticket.OriginalBalance = Convert.ToDecimal(reader["InitialAmount"]);
                                ticket.CurrentBalance = Convert.ToDecimal(reader["TotalAmount"]);
                                ticket.Status = Convert.ToInt32(reader["OrderPaymentStatusID"]);
                                ticket.OriginalExpirationDate = reader["OriginalExpirationDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["OriginalExpirationDate"]);
                                ticket.AccountNumber = Convert.ToInt32(reader["AccountID"]);
                                ticket.FirstName = reader["FirstName"].ToString();
                                ticket.LastName = reader["LastName"].ToString();
                                ticket.PhoneNumber = reader["PhoneNumber"].ToString();
                                #endregion

                                ticketList.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ticketList = new List<TicketPaymentPerMonthSearchData>();
            }

            return ticketList;
        }


        public List<OrderPaymentVirtualDesktop> TableOrderPaymentVirtualDesktop(int accountID)
        {
            List<OrderPaymentVirtualDesktop> OrderPaymentVirtualList = new List<OrderPaymentVirtualDesktop>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetOrderPaymentVirtualDesktop]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Parameters
                        cmd.Parameters.AddWithValue("@TicketNumber", accountID > 0 ? accountID : 0);
                        #endregion

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            OrderPaymentVirtualDesktop orderPayment = null;
                            while (reader.Read())
                            {
                                orderPayment = new OrderPaymentVirtualDesktop();

                                #region Assign Values
                                orderPayment.accountID = reader["accountID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["accountID"]);
                                orderPayment.accountID = reader["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderID"]);
                                orderPayment.CareerTitleTerm = reader["CareerTitleTerm"].ToString();
                                orderPayment.PaidAsCurrentMonth = reader["PaidAsCurrentMonth"].ToString();
                                orderPayment.EncerramentoDoCiclo = reader["EncerramentoDoCiclo"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["EncerramentoDoCiclo"]);
                                orderPayment.PQV = reader["PQV"] == DBNull.Value ? 0 :Convert.ToDecimal(reader["PQV"]);
                                orderPayment.DQV = reader["DQV"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DQV"]);
                                orderPayment.pendingAmount = reader["pendingAmount"] == DBNull.Value ? 0 : (float) Convert.ToDouble(reader["pendingAmount"]);
                                
                                #endregion

                                OrderPaymentVirtualList.Add(orderPayment);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                OrderPaymentVirtualList = new List<OrderPaymentVirtualDesktop>();
            }

            return OrderPaymentVirtualList;
        }


        public Dictionary<int, string> GetDropDownStatuses()
        {
            Dictionary<int, string> dropDown = new Dictionary<int, string>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[rptDropDownOrderPaymentStatuses]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dropDown.Add
                                    (
                                        Convert.ToInt32(reader["OrderPaymentStatusID"]),
                                        reader["Name"].ToString()
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                dropDown = new Dictionary<int, string>();
            }

            return dropDown;
        }

        public Dictionary<int, string> GetTicketNumberLookUp(string ticketNumberPrefix)
        {
            Dictionary<int, string> lookUp = new Dictionary<int, string>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[PaymentTicketNumberLookUp]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TicketNumberPrefix", ticketNumberPrefix);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lookUp.Add
                                    (
                                        Convert.ToInt32(reader["OrderPaymentID"]),
                                        reader["TicketNumber"].ToString()
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lookUp = new Dictionary<int, string>();
            }

            return lookUp;
        }
        #endregion

    }
}
