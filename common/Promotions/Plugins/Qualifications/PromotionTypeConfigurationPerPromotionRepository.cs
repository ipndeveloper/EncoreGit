using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NetSteps.Common.Configuration;
using System.Data;
using System.IO;
using System.Configuration;
using NetSteps.Encore.Core;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Plugins.EntityModel;
using System.Data.EntityClient;
using NetSteps.Foundation.Common;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class PromotionTypeConfigurationPerPromotionRepository
    {
        EncorePromotionsPluginsEntities db = new EncorePromotionsPluginsEntities();

        public bool ExistePromotionType(int PromotionID)
        {
            bool resultado = false;

            try
            {
                SqlParameter sqlPromotionID = null;
                sqlPromotionID = new SqlParameter() { ParameterName = "@PromotionID", Value = PromotionID, SqlDbType = SqlDbType.Int };

                EncorePromotionsPluginsEntities ctx = new EncorePromotionsPluginsEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspEsPromotionAcumulativo]", con))
                    {
                        cmd.Parameters.Add(sqlPromotionID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        resultado = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        internal static string GetConnectionString()
        {
            EncorePromotionsPluginsEntities ctx = new EncorePromotionsPluginsEntities();
            return ((EntityConnection)ctx.Connection).StoreConnection.ConnectionString;
        }

        public Decimal ObtenerValorAcumuladoAccountKPIs(int ProductPriceTypeID, int AccountID)
        {
            Decimal resultado = 0;
            try
            {
                SqlParameter sqlProductPriceTypeID = null;
                SqlParameter sqlAccountID = null;
                sqlProductPriceTypeID = new SqlParameter() { ParameterName = "@ProductPriceTypeID", Value = ProductPriceTypeID, SqlDbType = SqlDbType.Int };
                sqlAccountID = new SqlParameter() { ParameterName = "@AccountID", Value = AccountID, SqlDbType = SqlDbType.Int };
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspObtenerValorAcumuladoAccountKPIs]", con))
                    {
                        cmd.Parameters.Add(sqlProductPriceTypeID);
                        cmd.Parameters.Add(sqlAccountID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        resultado = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        public Decimal GetVolumeByProductListPriceType(int OrderID,int PromotionID, int ProductPriceTypeID)
        {
            Decimal resultado = 0;
            try
            {
                SqlParameter sqlOrderID = null;
                SqlParameter sqlPromotionID = null;
                SqlParameter sqlProductPriceTypeID = null;

                sqlOrderID = new SqlParameter() { ParameterName = "@OrderID", Value = OrderID, SqlDbType = SqlDbType.Int };
                sqlPromotionID = new SqlParameter() { ParameterName = "@PromotionID", Value = PromotionID, SqlDbType = SqlDbType.Int };
                sqlProductPriceTypeID = new SqlParameter() { ParameterName = "@ProductPriceTypeID", Value = ProductPriceTypeID, SqlDbType = SqlDbType.Int };

                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetVolumeByProductListPriceType]", con))
                    {
                        cmd.Parameters.Add(sqlOrderID);
                        cmd.Parameters.Add(sqlPromotionID);
                        cmd.Parameters.Add(sqlProductPriceTypeID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        resultado = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        public bool ExistsOrCondition(int PromotionID)
        {
            bool resultado = false;

            try
            {
                SqlParameter sqlPromotionID = null;
                sqlPromotionID = new SqlParameter() { ParameterName = "@PromotionID", Value = PromotionID, SqlDbType = SqlDbType.Int };

                EncorePromotionsPluginsEntities ctx = new EncorePromotionsPluginsEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspExistsOrCondition]", con))
                    {
                        cmd.Parameters.Add(sqlPromotionID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        resultado = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        public Dictionary<bool, Dictionary<decimal, decimal>> ExistsAndConditionQVTotal(int promotionID)
        {
            Dictionary<bool, Dictionary<decimal, decimal>> result = new Dictionary<bool, Dictionary<decimal, decimal>>();
            Dictionary<decimal, decimal> qvRange = new Dictionary<decimal, decimal>();

            SqlParameter sqlPromotionID = null;
            sqlPromotionID = new SqlParameter() { ParameterName = "@PromotionID", Value = promotionID, SqlDbType = SqlDbType.Int };

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[uspExistsAndConditionQVTotalRanges]", con);
                cmd.Parameters.Add(sqlPromotionID);
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                //DataAccess.ExecuteReader(DataAccess.GetCommand("uspExistsAndConditionQVTotalRanges", new Dictionary<string, object>() { { "PromotionID", promotionID } }, ConnectionStrings.BelcorpCore));
                IDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    qvRange.Add(Convert.ToDecimal(dr["QvMin"]), Convert.ToDecimal(dr["QvMax"]));
                    result.Add(Convert.ToBoolean(dr["RetVal"]), qvRange);
                    break;
                }
                dr.Close();
            }
            return result;
        }

        public Dictionary<int, int> GetPromotionQualifications(int PromotionID)
        {
            Dictionary<int, int> resultado = new Dictionary<int, int>();

            try
            {
                SqlParameter sqlPromotionID = null;
                sqlPromotionID = new SqlParameter() { ParameterName = "@PromotionID", Value = PromotionID, SqlDbType = SqlDbType.Int };

                EncorePromotionsPluginsEntities ctx = new EncorePromotionsPluginsEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspGetPromotionQualifications]", con))
                    {
                        cmd.Parameters.Add(sqlPromotionID);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        //resultado = Convert.ToBoolean(cmd.ExecuteScalar());
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read()) resultado.Add(Convert.ToInt32(dr["ProductID"]), Convert.ToInt32(dr["Quantity"]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }
    }
}
