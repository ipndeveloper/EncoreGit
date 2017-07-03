using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
   public  class PaymentTicktesRepository
    {

       private enum AccountColumn
       {
            AccountID,
            Name 
       }
       private enum OrderPaymentStatusesColumn
       {
           OrderPaymentStatusID,
           Name 
       }
        private enum  CountryColumn
       {
           CountryID,
           Name 
       }
          private enum BanksColumn
          {
              BankID,
              BankName
         
          }


          private enum NegotiationLevelColumn
          {
              NegotiationLevelID,
              Name
          }
          private enum ExpirationStatusesColumn
          {
              ExpirationStatusID,
              Name
          }
        private static string _reportConnectionString = string.Empty;
        private static string GetReportConnectionString()
        {

            _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();

            return _reportConnectionString;
        }
        #region cargar autocomplete 
        public static Dictionary<int, string> AccountSearchAuto(string AccountID)
        {
            Dictionary<int, string > dcAccounts = new Dictionary<int, string >();
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
                    ocom.CommandText = "rptDropDownAccountsById";
                    op = new SqlParameter();
                    op.DbType = DbType.String;
                    op.ParameterName = "@AccountID";
                    op.Value = AccountID;
                    ocom.Parameters.Add(op);

                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcAccounts[Convert.ToInt32(odata[AccountColumn.AccountID.ToString()])] = Convert.ToString(odata[AccountColumn.Name.ToString()]);
                        
                        }
                        return dcAccounts;
                    }
                }
            }


        }
        public static Dictionary<int, string> OrderPaymentStatuDrop()
        {
            Dictionary<int, string> dcAccounts = new Dictionary<int, string>();
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
                    ocom.CommandText = "rptDropDownOrderPaymentStatus";
                   

                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcAccounts[Convert.ToInt32(odata[OrderPaymentStatusesColumn.OrderPaymentStatusID.ToString()])] = Convert.ToString(odata[OrderPaymentStatusesColumn.Name.ToString()]);
                        }
                        return dcAccounts;
                    }
                }
            }


        }
        public static Dictionary<int, string> CountriesActiveDrop()
        {
            Dictionary<int, string> dcCountries = new Dictionary<int, string>();
             
            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "rptDropDownCountriesActive";
                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcCountries[Convert.ToInt32(odata[CountryColumn.CountryID.ToString()])] = Convert.ToString(odata[CountryColumn.Name.ToString()]);
                        }
                        return dcCountries;
                    }
                }
            }

        }
        public static Dictionary<int, string> ExpirationStatusesDrop()
        {
            Dictionary<int, string> dcExpirationStatuses = new Dictionary<int, string>();

            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "rptDropDownExpirationStatuses";
                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcExpirationStatuses[Convert.ToInt32(odata[ExpirationStatusesColumn.ExpirationStatusID.ToString()])] = Convert.ToString(odata[ExpirationStatusesColumn.Name.ToString()]);
                        }
                        return dcExpirationStatuses;
                    }
                }
            }

        }

        public static Dictionary<int, string> BanksActiveDrop()
        {
            Dictionary<int, string> dcBanks = new Dictionary<int, string>();
          
            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "rptDropDownBanksActive";
                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcBanks[Convert.ToInt32(odata[BanksColumn.BankID.ToString()])] = Convert.ToString(odata[BanksColumn.BankName.ToString()]);
                        }
                        return dcBanks;
                    }
                }
            }

        }

        public static Dictionary<int, string> NegotiationLevelsActiveDrop()
        {
            Dictionary<int, string> dcNegotiationLevels = new Dictionary<int, string>();

            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "rptDropDownNegotiationLevelsActive";
                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcNegotiationLevels[Convert.ToInt32(odata[NegotiationLevelColumn.NegotiationLevelID.ToString()])] = Convert.ToString(odata[NegotiationLevelColumn.Name.ToString()]);
                        }
                        return dcNegotiationLevels;
                    }
                }
            }

        }

        public static Dictionary<int, string> BankSearchAuto(string Query)
        {
            Dictionary<int, string> dcBank = new Dictionary<int, string>();
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
                    ocom.CommandText = "SpSearchBanks";
                    op = new SqlParameter();
                    op.DbType = DbType.String;
                    op.ParameterName = "@Query";
                    op.Value = Query;
                    ocom.Parameters.Add(op);

                    using (IDataReader odata = ocom.ExecuteReader())
                    {
                        while (odata.Read())
                        {
                            dcBank[Convert.ToInt32(odata["BankID"])] = Convert.ToString(odata["BankName"]);
                        }
                        return dcBank;
                    }
                }
            }


        }
        #endregion 
    }
}
