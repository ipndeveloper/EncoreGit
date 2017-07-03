using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Extensions
{
    public class TemporalMatrixEstensions
    {
        public static int InsTemporalMatrix(TemporalMatrixSearchParameters parameters)
        {
            return DataAccess.ExecWithStoreProcedureSave("Core", "uspInsTemporalMatrix",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = parameters.ProductID },
                 new SqlParameter("PeriodID", SqlDbType.Int) { Value = parameters.PeriodID },
                 new SqlParameter("ProductName", SqlDbType.VarChar) { Value = parameters.ProductName },
                 new SqlParameter("NumDeKit", SqlDbType.Int) { Value = parameters.NumDeKit },
                 new SqlParameter("IsKit", SqlDbType.Bit) { Value = parameters.IsKit },
                 new SqlParameter("ParticipationKit", SqlDbType.Int) { Value = parameters.ParticipationKit },
                 new SqlParameter("SKUSAP", SqlDbType.Int) { Value = parameters.SKUSAP },
                 new SqlParameter("TO", SqlDbType.Int) { Value = parameters.TO },
                 new SqlParameter("PrecioSinDto", SqlDbType.Decimal) { Value = parameters.PrecioSinDto },
                 new SqlParameter("PrecioMatriz", SqlDbType.Decimal) { Value = parameters.PrecioMatriz },
                 new SqlParameter("CodCatalog", SqlDbType.VarChar) { Value = parameters.CodCatalog },
                 new SqlParameter("Catalog", SqlDbType.VarChar) { Value = parameters.Catalog },
                 new SqlParameter("Page", SqlDbType.Int) { Value = parameters.Page },
                 new SqlParameter("Points", SqlDbType.Int) { Value = parameters.Points },
                 new SqlParameter("Type", SqlDbType.VarChar) { Value = parameters.Type });
        }

        public static void DeleteTemporalMatrix()
        {
            var result = DataAccess.ExecQueryDinamico("Core", "uspDelTemporalMatrix");
        }

        public static List<TemporalMatrixSearchData> GetTemporalMatrix()
        {
            return DataAccess.ExecWithStoreProcedureLists<TemporalMatrixSearchData>("Core", "uspGetTemporalMatrix").ToList();
        }

        public static void ProcesarMatriz(DataTable table, int PeriodID, int CatalogID, int UserID)
        {
            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Core"].ConnectionString;
            SqlConnection conexion = new SqlConnection(connectionString);
            conexion.Open();
            
            try
            {
                #region Cargar Matriz

                SqlTransaction tr = conexion.BeginTransaction();

                try
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conexion, SqlBulkCopyOptions.Default, tr))
                    {
                        bulkCopy.DestinationTableName = "BelcorpUSA.ProductsMatrix";
                        bulkCopy.BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BulkCopyBatchSize"]);
                        bulkCopy.BulkCopyTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BulkCopyTimeout"]);

                        #region ColumnMappings

                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CÓDIGO", "CUVCode"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CAMPAÑA", "PeriodID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CÓDIGO_1", "ExternalCode"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NOMBRE DE PRODUCTO", "ProductName"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FLAG KIT", "IsKit"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("# DE KIT", "KitID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("% PARTICIPACÓN EN EL KIT", "ParticipationPercentage"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SKU SAP", "SAPSKU"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FAC. REPETICON", "Quantity"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TIPO OFERTA", "OfferType"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PRECIO MATRIZ", "RetailPrice"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PUNTAJE", "QV"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TIPO", "ProductType"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ARCHIVO", "FilePath"));

                        #endregion

                        bulkCopy.WriteToServer(table);
                    }

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }

                #endregion

                #region Procesar Matriz

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PeriodID", PeriodID },
                                                                                           { "@CatalogID", CatalogID },
                                                                                           { "@UserID", UserID }};

                //conexion.Open();
                using (IDbCommand cmd = DataAccess.GetCommand("upsProcesarMatriz", parameters, "Core"))
                {
                    cmd.CommandTimeout = 0;
                    DataAccess.ExecuteNonQuery(cmd);
                }

                #endregion

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
            }
        }

        public static string GetSystemConfigValue(string ConfigCode)
        {
            string result = string.Empty;
            SqlDataReader reader = DataAccess.GetDataReaderF(string.Format("select dbo.sfnGetSystemConfig('{0}') 'Result'", ConfigCode), null, "Core");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToString(reader["Result"]);
                    break;
                }
            }
            return result;
        }
		
		public static string LkpSystemConfigValue(string ConfigCode)
        {
            string result = string.Empty;
            SqlDataReader reader = DataAccess.GetDataReaderF(string.Format("select dbo.sfnLkpSystemConfig('{0}') 'Result'", ConfigCode), null, "Commissions");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToString(reader["Result"]);
                    break;
                }
            }
            return result;
        }
    }
}
