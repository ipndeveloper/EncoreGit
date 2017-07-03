using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace NetSteps.Data.Entities.Repositories
{
    public class TitleEstimateRepository : ITitleEstimateRepository
    {

        #region Instance
        private static ITitleEstimateRepository _instance;

        public static ITitleEstimateRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TitleEstimateRepository();
                return _instance;
            }
        }
        #endregion


        public bool CalculateCareerTitle(int? PlanID)
        {

            
            bool rpta = false;
            SqlParameter opPlanID = null;
            if (!PlanID.HasValue)
            {
               opPlanID= new SqlParameter() { ParameterName = "@PlanID", Value = DBNull.Value, SqlDbType = SqlDbType.Int };
            }
            else {
               opPlanID= new SqlParameter() { ParameterName = "@PlanID", Value = PlanID.Value, SqlDbType = SqlDbType.Int };
            }
            
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[CalculateCareerTitle]", con))
                    {
                        cmd.Parameters.Add(opPlanID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                rpta = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return rpta;
        }

        public bool CalculatePaidAsTitle(int PlanID)
        {
            bool rpta = false;

            try
            {
                SqlParameter pPlanID = null;
                pPlanID = new SqlParameter() { ParameterName = "@PlanID", Value = PlanID, SqlDbType = SqlDbType.Int };
            
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[CalculoTitulosPago]", con))
                    {
                        cmd.Parameters.Add(pPlanID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                rpta = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return rpta;
        }

    }
}
