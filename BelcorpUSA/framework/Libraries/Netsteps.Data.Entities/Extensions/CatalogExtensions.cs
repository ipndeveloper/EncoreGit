using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Extensions
{
    public class CatalogExtensions
    {
        /// <summary>
        /// Método que retorna los catálogos de envió Create by FHP 
        /// </summary>
        /// <param name="campaignID">Codigo del Perido seleccionado</param>
        /// <returns>Dictionary con el Id y Nombre de los diversos Catálogos</returns>
        public static List<UtilSearchData.Select> GetCatalogByCampaignID(int campaignID)
        {
            return DataAccess.ExecWithStoreProcedure<UtilSearchData.Select>(ConnectionStrings.BelcorpCore, "uspGetCatalogByCampaignID",
                 new SqlParameter("CampaignID", SqlDbType.Int) { Value = campaignID }).ToList();
        }

        /// <summary>
        /// Método que retorna los Temporal Matrix Create by FHP
        /// </summary>
        /// <returns>Retorna una lista de todos los Temporal Matrix</returns>
        public static List<TemporalMatrixSearchData> GetTemporalMatrix()
        {
            return DataAccess.ExecWithStoreProcedureLists<TemporalMatrixSearchData>(ConnectionStrings.BelcorpCore, "uspGetTemporalMatrix").ToList();
        }

        public static List<Catalog> GetActiveCatalogs(int StoreFrontID, int LanguageID)
        {
            List<Catalog> Result = new List<Catalog>();
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.GetActiveCatalogs";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter P1;
                    P1 = command.Parameters.AddWithValue("@StoreFrontID", StoreFrontID);

                    SqlParameter P2;
                    P2 = command.Parameters.AddWithValue("@LanguageID", LanguageID);

                    SqlDataReader dr;
                    dr = command.ExecuteReader();

                    //item.CatalogTypeID
                    while (dr.Read())
                    {
                        Catalog item = new Catalog();
                        item.CatalogID = Convert.ToInt32(dr["CatalogID"]);
                        item.Description = Convert.ToString(dr["Name"]);
                        item.CategoryID = Convert.ToInt32(dr["CategoryID"]);
                        item.CatalogTypeID = Convert.ToInt16(dr["CatalogTypeID"]);
                        Result.Add(item);
                    }
                }
            }
            return Result;
        }

    }
}
