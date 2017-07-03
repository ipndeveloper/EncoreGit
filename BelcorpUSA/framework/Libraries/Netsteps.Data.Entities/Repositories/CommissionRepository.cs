using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class CommissionRepository : ICommissionRepository
    {

        #region Instance
        private static ICommissionRepository _instance;

        public static ICommissionRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CommissionRepository();
                return _instance;
            }
        }
        #endregion

        public Dictionary<int, string> GetPeriods()
        {
            Dictionary<int, string> periods = new Dictionary<int, string>();
            periods.Add(0, "-- Seleccione --");

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetPeriods]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                periods.Add(Convert.ToInt32(reader["PeriodID"]), reader["PeriodID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                periods = new Dictionary<int, string>();
                periods.Add(0, "-- Seleccione --");
            }

            return periods;
        }

        public Dictionary<string, string> GetCommissionTypes()
        {
            Dictionary<string, string> commissionTypes = new Dictionary<string, string>();
            commissionTypes.Add("0", "-- Seleccione --");

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetCommissionTypes]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                commissionTypes.Add(reader["BonusCode"].ToString(), reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                commissionTypes = new Dictionary<string, string>();
                commissionTypes.Add("0", "-- Seleccione --");
            }

            return commissionTypes;
        }

        public List<CommissionSearchData> GetTotalCommissions(CommissionSearchParameters searchParameters)
        {
            List<CommissionSearchData> commissions = new List<CommissionSearchData>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetTotalCommissions]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pPeriodID", searchParameters.PeriodID);
                        cmd.Parameters.AddWithValue("@pBonusCode", searchParameters.CommissionType);
                        cmd.Parameters.AddWithValue("@pAccountID", searchParameters.AccountID);
                        cmd.Parameters.AddWithValue("@pPageNumber", searchParameters.PageNumber);
                        cmd.Parameters.AddWithValue("@pPageSize", searchParameters.PageSize);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            CommissionSearchData commission = null;
                            while (reader.Read())
                            {
                                commission = new CommissionSearchData();

                                #region Assign Values
                                commission.PeriodID = Convert.ToInt32(reader["PeriodID"]);
                                commission.AccountNumber = Convert.ToInt32(reader["AccountNumber"]);
                                commission.AccountName = reader["AccountName"].ToString();
                                commission.CommissionType = reader["CommissionType"] != DBNull.Value ? reader["CommissionType"].ToString() : null;
                                commission.CommissionName = reader["CommissionName"] != DBNull.Value ? reader["CommissionName"].ToString() : null;
                                commission.CommissionAmount = reader["CommissionAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CommissionAmount"]) : 0;
                                commission.TotalRows = Convert.ToInt32(reader["TotalRows"]);
                                commission.TotalPages = Convert.ToInt32(reader["TotalPages"]);
                                #endregion

                                commissions.Add(commission);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                commissions = new List<CommissionSearchData>();
            }

            return commissions;
        }

        public List<CommissionDetailSearchData> GetDetailCommissions(CommissionDetailSearchParameters searchParameters)
        {
            List<CommissionDetailSearchData> detailCommissions = new List<CommissionDetailSearchData>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetCommDetailByAccountPeriod]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pPeriodID", searchParameters.PeriodID);
                        cmd.Parameters.AddWithValue("@pBonusCode", searchParameters.CommissionType);
                        cmd.Parameters.AddWithValue("@pAccountID", searchParameters.AccountID);
                        cmd.Parameters.AddWithValue("@pPageNumber", searchParameters.PageNumber);
                        cmd.Parameters.AddWithValue("@pPageSize", searchParameters.PageSize);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            CommissionDetailSearchData detail = null;
                            while (reader.Read())
                            {
                                detail = new CommissionDetailSearchData();

                                #region Assign Values
                                detail.SponsorID = Convert.ToInt32(reader["SponsorNumber"]);
                                detail.SponsorName = reader["SponsorName"].ToString();
                                detail.AccountNumber = Convert.ToInt32(reader["AccountNumber"]);
                                detail.AccountName = reader["AccountName"].ToString();
                                detail.CommissionType = reader["CommissionType"] != DBNull.Value ? reader["CommissionType"].ToString() : null;
                                detail.CommissionName = reader["CommissionName"] != DBNull.Value ? reader["CommissionName"].ToString() : null;
                                detail.OrderNumber = reader["OrderNumber"] != DBNull.Value ? Convert.ToInt32(reader["OrderNumber"]) : 0;
                                detail.CommissionableValue = reader["CommissionableValue"] != DBNull.Value ? Convert.ToDecimal(reader["CommissionableValue"]) : 0;
                                detail.Percentage = reader["Percentage"] != DBNull.Value ? Convert.ToDecimal(reader["Percentage"]) : 0;
                                detail.PayoutAmount = reader["PayoutAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PayoutAmount"]) : 0;
                                detail.PeriodID = Convert.ToInt32(reader["PeriodID"]);
                                detail.TotalRows = Convert.ToInt32(reader["TotalRows"]);
                                detail.TotalPages = Convert.ToInt32(reader["TotalPages"]);
                                #endregion

                                detailCommissions.Add(detail);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                detailCommissions = new List<CommissionDetailSearchData>();
            }

            return detailCommissions;
        }

    }
}
