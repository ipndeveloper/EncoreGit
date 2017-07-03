using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Xml.Linq;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Repositories.Interfaces;

namespace NetSteps.Data.Entities.Repositories
{

    #region Modifications
    // @01 BR-CB-002 GYS: Implementando metodos de la interfaz IOrderPaymentRepository
    #endregion

    public class OverduePaymentsRepository : IOverduePaymentsRepository
    {

        #region Modifications @01
        public OverduePaymentReport RegularProcess(int fileSequentialCode, ref string errorMessage)
        {
            OverduePaymentReport report = new OverduePaymentReport();
            string reportDate = String.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    con.Open();
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@pFileCode", fileSequentialCode)
                    };

                    SqlDataReader reader = DataAccess.queryDatabase("[dbo].[uspFillOverduePayments]", con, parameters);
                    if (reader.Read())
                    {
                        errorMessage = reader.GetString(0);
                        reportDate = reader.GetDateTime(1).ToString("yyyyddMM");
                    }

                    if (String.IsNullOrEmpty(errorMessage.Trim()))
                        report = GetOverduePaymentsReport(con, fileSequentialCode, reportDate);

                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
            }
            catch (Exception e)
            {
                report = new OverduePaymentReport();
                errorMessage = e.Message;
            }

            return report;
        }

        public OverduePaymentReport AlternativeProcess(int fileSequentialCode, ref string errorMessage)
        {
            OverduePaymentReport report = new OverduePaymentReport();
            string reportDate = String.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspCrossOverdueErrors]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                            reportDate = reader.GetDateTime(0).ToString("yyyyddMM");
                    }

                    report = GetOverduePaymentsReport(con, fileSequentialCode, reportDate);

                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
            }
            catch (Exception e)
            {
                report = new OverduePaymentReport();
                errorMessage = e.Message;
            }

            return report;
        }

        public bool LoadOverdueErrors(List<int> AccountNumbers)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    con.Open();

                    foreach (int number in AccountNumbers)
                    {
                        using (SqlCommand cmd = new SqlCommand("[dbo].[uspInsertLoadingErrors]", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pAccountNumber", number);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    rpta = true;
                }
            }
            catch (Exception)
            {
                rpta = false;
            }

            return rpta;
        }
        #endregion


        #region Private Methods

        private OverduePaymentReport GetOverduePaymentsReport(SqlConnection con, int fileSequentialCode, string reportDate)
        {

            OverduePaymentReport report = new OverduePaymentReport();
            report.OverduePaymentDetails = new List<OverduePaymentDetail>();

            #region ReportHeader
            report.TypeA = ConfigurationManager.AppSettings["TypeA"].Trim();
            report.Sequential = fileSequentialCode.ToString("00000");
            report.Date = reportDate;
            report.CompanyCode = ConfigurationManager.AppSettings["CompanyCodeR"].Trim();
            report.Destination = ConfigurationManager.AppSettings["Destination"].Trim();
            report.RemainingA = new String(' ', 314);
            #endregion

            if (con.State != ConnectionState.Open)
                con.Open();

            using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetOverdueAccounts]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlDataReader reader = DataAccess.queryDatabase("[dbo].[uspGetOverdueAccounts]", con);
                SqlDataReader reader = cmd.ExecuteReader();

                OverduePaymentDetail detail = null;
                while (reader.Read())
                {
                    detail = new OverduePaymentDetail();

                    #region StaticValues

                    detail.TypeB = ConfigurationManager.AppSettings["TypeB"].Trim();
                    detail.SequentialB = (report.OverduePaymentDetails.Count() + 1).ToString("00000");
                    detail.OperationTypeB = ConfigurationManager.AppSettings["OperationTypeB"].Trim();
                    detail.Identity = new String(' ', 13);
                    detail.StateEmiting = new String(' ', 2);
                    detail.Nationality = ConfigurationManager.AppSettings["NationalityB"].Trim();
                    detail.Naturality = new String(' ', 20);
                    detail.NaturalityState = new String(' ', 2);
                    detail.MotherName = new String(' ', 40);
                    detail.FatherName = new String(' ', 40);
                    detail.MaritalStatus = new String(' ', 1);
                    detail.Spouse = new String(' ', 40);
                    detail.SpouseBirthday = new String(' ', 8);
                    #endregion

                    detail.AccountID = Convert.ToInt32(reader["AccountID"]);
                    detail.AccountName = FormatLimitString(reader["AccountName"].ToString(), 60);
                    detail.Birthday = Convert.ToDateTime(reader["Birthday"]).ToString("yyyyddMM");
                    detail.CPF = FormatLimitString(reader["CPF"].ToString(), 11, '0');
                    detail.Address1 = FormatLimitString(reader["Address1"].ToString(), 40);
                    detail.County = FormatLimitString(reader["County"].ToString(), 45);
                    detail.PostalCode = FormatLimitString(reader["PostalCode"].ToString(), 8);
                    detail.City = FormatLimitString(reader["City"].ToString(), 20);
                    detail.State = FormatLimitString(reader["State"].ToString(), 2);
                    detail.Gender = FormatLimitString(reader["Gender"].ToString(), 1);

                    report.OverduePaymentDetails.Add(detail);
                }
            }

            foreach (var opd in report.OverduePaymentDetails)
            {
                opd.OverduePaymentOcurrences = GetOcurrencePaymentsByAccount(con, Int32.Parse(opd.SequentialB), opd.AccountID);
            }


            #region ReportFooter
            report.TypeZ = ConfigurationManager.AppSettings["TypeZ"].Trim();
            report.Zeros = new String('0', 35);
            report.SUMI = new String(' ', 7);
            report.SUMJ = new String(' ', 7);
            report.SUMK = new String(' ', 7);
            report.RemainingZ = new String(' ', 268);
            #endregion

            return report;
        }

        private List<OverduePaymentOccurrence> GetOcurrencePaymentsByAccount(SqlConnection con, int sequentialCode, int accountId)
        {
            List<OverduePaymentOccurrence> occurrences = new List<OverduePaymentOccurrence>();

            if (con.State != ConnectionState.Open)
                con.Open();

            using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetOverduesByAccountId]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pAccountId", accountId);
                //SqlDataReader reader = DataAccess.queryDatabase("[dbo].[uspGetOverduesByAccountId]", con, parameters);
                SqlDataReader reader = cmd.ExecuteReader();

                OverduePaymentOccurrence occurrence = null;
                while (reader.Read())
                {
                    occurrence = new OverduePaymentOccurrence();

                    #region Static Values
                    occurrence.TypeC = ConfigurationManager.AppSettings["TypeC"].Trim();
                    occurrence.SequentialC1 = sequentialCode.ToString("00000");
                    occurrence.SequentialC2 = (occurrences.Count() + 1).ToString("000");
                    occurrence.DebtType = ConfigurationManager.AppSettings["DebtType"].Trim();
                    occurrence.Blanks = new String(' ', 30);
                    occurrence.OcurrenceCode = ConfigurationManager.AppSettings["OcurrenceCode"].Trim();
                    occurrence.SPCCode = ConfigurationManager.AppSettings["SPCCode"].Trim();
                    occurrence.OperationTypeC = ConfigurationManager.AppSettings["OperationTypeC"].Trim();
                    occurrence.RemainingC = new String(' ', 248);
                    #endregion

                    occurrence.OverduePaymentID = Convert.ToInt32(reader["OverduePaymentID"]);
                    occurrence.AccountID = Convert.ToInt32(reader["AccountID"]);
                    occurrence.ExpirationDate = reader["ExpirationDate"].ToString();
                    occurrence.OrderDate = Convert.ToDateTime(reader["OrderDate"]).ToString("yyyyddMM");
                    occurrence.Title = Convert.ToInt32(reader["TicketNumber"]).ToString("000000000000000");
                    occurrence.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);

                    occurrences.Add(occurrence);
                }

                return occurrences;
            }
        }

        private string FormatLimitString(string text, int limit, char helperToFill = ' ')
        {
            if (text.Length < limit)
                text = text.PadLeft(limit, helperToFill);
            else if (text.Length > limit)
                text = text.Substring(0, limit);

            return text;
        }

        #endregion

    }
}
