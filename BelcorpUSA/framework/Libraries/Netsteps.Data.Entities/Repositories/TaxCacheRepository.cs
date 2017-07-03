using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Linq;
using NetSteps.Common;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Text;
using System.Data.OleDb;
using System.Transactions;
using System.IO;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class TaxCacheRepository
    {
        #region Members
        #endregion

        #region LoadByAddress Methods
        public List<TaxCache> LoadByAddress(string postalCode)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    postalCode = postalCode.ToCleanString();
                    return context.TaxCaches.Join(
                        context.TaxCaches
                            .Where(t2 => t2.PostalCode == postalCode)
                            .GroupBy(t2 => t2.TaxCategoryID)
                            .Select(t2 => new { TaxCategoryID = t2.Key, MaxExpirationDate = t2.Max(t2g => t2g.ExpirationDateUTC) }),
                        t1 => t1.TaxCategoryID,
                        t2 => t2.TaxCategoryID,
                        (t1, t2) => new { T1 = t1, T2 = t2 })
                        .Where(joined => joined.T1.PostalCode == postalCode && joined.T1.ExpirationDateUTC == joined.T2.MaxExpirationDate)// && joined.T1.ExpirationDateUTC > DateTime.UtcNow )
                        .Select(joined => joined.T1).ToList();
                }
            });
        }

        public List<TaxCache> LoadByAddress(int countryId, string postalCode)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    postalCode = postalCode.ToCleanString();
                    return context.TaxCaches.Join(
                        context.TaxCaches
                            .Where(t2 => t2.PostalCode == postalCode && t2.CountryID == countryId)
                            .GroupBy(t2 => t2.TaxCategoryID)
                            .Select(t2 => new { TaxCategoryID = t2.Key, MaxExpirationDate = t2.Max(t2g => t2g.ExpirationDateUTC) }),
                        t1 => t1.TaxCategoryID,
                        t2 => t2.TaxCategoryID,
                        (t1, t2) => new { T1 = t1, T2 = t2 })
                        .Where(joined => joined.T1.PostalCode == postalCode && joined.T1.CountryID == countryId)// && joined.T1.ExpirationDateUTC == joined.T2.MaxExpirationDate)
                        .Select(joined => joined.T1).ToList();
                }
            });
        }

        public virtual List<TaxCache> LoadByAddress(int countryId, string stateAbbr, string county, string city, string postalCode)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    stateAbbr = stateAbbr.ToCleanString();
                    county = county.ToCleanString();
                    city = city.ToCleanString();
                    postalCode = postalCode.ToCleanString();

                    // This modified query only pulls back the most recent records in each available TaxCategoryID - JHE
                    var results = from t in context.TaxCaches
                                  join SRMax in
                                      (from t2 in context.TaxCaches
                                       where t2.PostalCode == postalCode &&
                                                                t2.CountryID == countryId &&
                                                                (t2.StateAbbreviation == stateAbbr || stateAbbr == "") &&
                                                                (t2.County == county || county == "") &&
                                                                (t2.City == city || city == "")
                                       group t2 by t2.TaxCategoryID into g
                                       select new
                                       {
                                           TaxCategoryID = g.Key,
                                           MaxExpirationDateUTC = g.Max(v => v.ExpirationDateUTC)
                                       }) on t.TaxCategoryID equals SRMax.TaxCategoryID
                                  where t.PostalCode == postalCode &&
                                                            t.CountryID == countryId &&
                                                            (t.StateAbbreviation == stateAbbr || stateAbbr == "") &&
                                                            (t.County == county || county == "") &&
                                                            (t.City == city || city == "") &&
                                  t.ExpirationDateUTC == SRMax.MaxExpirationDateUTC //&& t.ExpirationDateUTC > DateTime.UtcNow
                                  select t;
                    return results.ToList();
                }
            });
        }

        public List<TaxCache> LoadByProvince(int countryId, string stateAbbr)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.TaxCaches.Join(
                        context.TaxCaches
                            .Where(t2 => t2.CountryID == countryId &&
                                (t2.StateAbbreviation == stateAbbr || stateAbbr == ""))
                            .GroupBy(t2 => t2.TaxCategoryID)
                            .Select(t2 => new { TaxCategoryID = t2.Key, MaxExpirationDate = t2.Max(t2g => t2g.ExpirationDateUTC) }),
                        t1 => t1.TaxCategoryID,
                        t2 => t2.TaxCategoryID,
                        (t1, t2) => new { T1 = t1, T2 = t2 })
                        .Where(joined => joined.T1.CountryID == countryId && (joined.T1.StateAbbreviation == stateAbbr || stateAbbr == ""))
                        .Select(joined => joined.T1).ToList();
                }
            });
        }

        /// <summary>
        /// Checks overrides table for any possible overrides on the passed collection of TaxCache objects.  Returns a modified list. 
        /// </summary>
        /// <param name="taxes">Collection of TaxCaches to check</param>
        /// <returns>Returns the original list of TaxCaches with any overrides applied.</returns>
        public virtual List<TaxCache> CheckForOverrides(IEnumerable<TaxCache> taxes)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    var countryIDs = taxes.Select(x => x.CountryID).Distinct();
                    var states = taxes.Select(x => x.State).Distinct();

                    var applicableOverridesBase = context.TaxCacheOverrides.Where(
                             tco => countryIDs.Contains(tco.CountryID)
                             && tco.EffectiveDateUTC < DateTime.UtcNow
                             && tco.ExpirationDateUTC > DateTime.UtcNow
                             && states.Contains(tco.State))
                             .ToList();

                    if (applicableOverridesBase.Count == 0)
                    {
                        return taxes.ToList();
                    }

                    foreach (var taxCache in taxes)
                    {
                        var applicableOverrides = applicableOverridesBase.Where(tco =>
                            //workaround for null == null comparisons in EF not working like C#
                             (tco.TaxCategoryID == null || tco.TaxCategoryID == taxCache.TaxCategoryID) && tco.State.EqualsIgnoreCase(taxCache.State)).ToList();

                        var stateMatch = applicableOverrides.FirstOrDefault(tco => tco.County == null && tco.City == null && tco.PostalCode == null);
                        var countyMatch = taxCache.County.IsNullOrWhiteSpace() ? null : applicableOverrides.FirstOrDefault(tco => taxCache.County.Equals(tco.County, StringComparison.OrdinalIgnoreCase) && tco.City == null && tco.PostalCode == null);
                        var cityMatch = taxCache.County.IsNullOrWhiteSpace() || taxCache.City.IsNullOrWhiteSpace() ? null : applicableOverrides.FirstOrDefault(tco => taxCache.County.Equals(tco.County, StringComparison.OrdinalIgnoreCase) && taxCache.City.Equals(tco.City, StringComparison.OrdinalIgnoreCase) && tco.PostalCode == null);
                        var postalMatch = taxCache.County.IsNullOrWhiteSpace() || taxCache.City.IsNullOrWhiteSpace() || taxCache.PostalCode.IsNullOrWhiteSpace() ? null : applicableOverrides.FirstOrDefault(tco => taxCache.County.Equals(tco.County, StringComparison.OrdinalIgnoreCase) && taxCache.City.Equals(tco.City, StringComparison.OrdinalIgnoreCase) && taxCache.PostalCode.StartsWith(tco.PostalCode, StringComparison.OrdinalIgnoreCase));

                        if (stateMatch != null)
                        {
                            stateMatch.CopyNonNullTaxValues(taxCache);
                        }
                        else if (countyMatch != null)
                        {
                            countyMatch.CopyNonNullTaxValues(taxCache);
                        }
                        else if (cityMatch != null)
                        {
                            cityMatch.CopyNonNullTaxValues(taxCache);
                        }
                        else if (postalMatch != null)
                        {
                            postalMatch.CopyNonNullTaxValues(taxCache);
                        }
                    }

                    return taxes.ToList();
                }
            });
        }

        public List<string> SearchCity(string city)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    city = city.ToCleanString();

                    var results = from t in context.TaxCaches
                                  where t.City.Contains(city)
                                  group t by t.City into g
                                  select g.Key;

                    return results.ToList();
                }
            });
        }

        public List<string> SearchPostalCode(string postalCode)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    postalCode = postalCode.ToCleanString();

                    var results = from t in context.TaxCaches
                                  where t.PostalCode.Contains(postalCode)
                                  group t by t.PostalCode into g
                                  select g.Key;

                    return results.ToList();
                }
            });
        }


        public List<PostalCodeData> Search(int countryId, string state, string stateAbbr, string county, string city, string postalCode)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<PostalCodeData> results = new List<PostalCodeData>();

                    state = state.ToCleanString();
                    stateAbbr = stateAbbr.ToCleanString();
                    county = county.ToCleanString();
                    city = city.ToCleanString();
                    postalCode = postalCode.ToCleanString();

                    IQueryable<TaxCache> matchingItems = context.TaxCaches;

                    if (!state.IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.State == state);
                    if (!stateAbbr.IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.StateAbbreviation == stateAbbr);
                    if (!county.IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.County == county);
                    if (!city.IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.City == city);
                    if (!postalCode.IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.PostalCode == postalCode);

                    var taxCacheData = matchingItems.Select(a => new
                    {
                        a.State,
                        a.StateAbbreviation,
                        a.County,
                        a.City,
                        a.PostalCode,
                        a.CountryID
                    });

                    foreach (var a in taxCacheData.ToList())
                        results.Add(new PostalCodeData()
                        {
                            State = a.State,
                            StateAbbreviation = a.StateAbbreviation,
                            County = a.County,
                            City = a.City,
                            PostalCode = a.PostalCode,
                            CountryID = a.CountryID.ToInt()
                        });

                    return results;
                }
            });
        }

        public List<PostalCodeData> Search(string location)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<PostalCodeData> results = new List<PostalCodeData>();

                    if (location.IsNullOrEmpty())
                        return results;

                    location = location.ToCleanString();

                    IQueryable<TaxCache> matchingItems = context.TaxCaches;

                    matchingItems = matchingItems.Where(a => a.State.Contains(location) || a.StateAbbreviation.Contains(location) || a.City.Contains(location) || a.PostalCode.Contains(location));

                    var taxCacheData = matchingItems.Select(a => new
                    {
                        a.State,
                        a.StateAbbreviation,
                        a.County,
                        a.City,
                        a.PostalCode,
                        a.CountryID
                    }).Distinct();

                    foreach (var a in taxCacheData.ToList())
                        results.Add(new PostalCodeData()
                        {
                            State = a.State,
                            StateAbbreviation = a.StateAbbreviation,
                            County = a.County,
                            City = a.City,
                            PostalCode = a.PostalCode,
                            CountryID = a.CountryID.ToInt()
                        });

                    return results;
                }
            });
        }
        #endregion

        /// <summary>
        /// This is a long running process that will remove older TaxCache records if new ones exist 
        /// unique on PostalCode, CountryID, StateAbbreviation, County, City, TaxCategoryID - JHE
        /// </summary>
        public void CleanOutOldTaxData()
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var distinctData = (context.TaxCaches.GroupBy(t => new { t.PostalCode, t.CountryID, t.StateAbbreviation, t.County, t.City, t.TaxCategoryID },
                             (key, group) => new { PostalCode = key.PostalCode, CountryID = key.CountryID, StateAbbreviation = key.StateAbbreviation, County = key.County, City = key.City, TaxCategoryID = key.TaxCategoryID, Count = group.Count() })).ToList();

                    foreach (var item2 in distinctData)
                    {
                        var toDelete = new List<int>();

                        var recentData = LoadByAddress(item2.CountryID.ToInt(), item2.StateAbbreviation, item2.County, item2.City, item2.PostalCode).Where(t => t.TaxCategoryID == item2.TaxCategoryID).ToList();
                        var allData = (from t in context.TaxCaches
                                       where t.PostalCode == item2.PostalCode && t.CountryID == item2.CountryID && t.StateAbbreviation == item2.StateAbbreviation && t.County == item2.County && t.City == item2.City
                                       select t).ToList();

                        allData = allData.Where(t => t.TaxCategoryID == item2.TaxCategoryID).ToList();

                        if (allData.Count > 1)
                        {
                            foreach (var item in allData)
                            {
                                var taxCache = recentData.FirstOrDefault(t => t.TaxCacheID == item.TaxCacheID);
                                if (taxCache == null)
                                    toDelete.Add(item.TaxCacheID);
                            }
                        }

                        if (toDelete.Count > 0)
                            DeleteBatch(toDelete);
                    }
                }
            });
        }

        public new virtual void DeleteBatch(IEnumerable<int> primaryKeys)
        {
            if (primaryKeys == null || !primaryKeys.Any())
            {
                return;
            }

            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var entityPrimaryKeyInfo = GetEntityPrimaryKeyInfo(context);
                    string entitySql = string.Format("DELETE FROM [{0}] WHERE [{1}] IN ({2})", "TaxCache", entityPrimaryKeyInfo.ColumnName, primaryKeys.Join(","));
                    context.ExecuteStoreCommand(entitySql);
                }
            });
        }

        /// <summary>
        /// This is a helper method used to generate fake data - JHE
        /// </summary>
        /// <returns></returns>
        public TaxCache GetRandomRecord(int countryID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    string sql = string.Format("SELECT TOP 1 {0} FROM {1} WHERE CountryID = {2} ORDER BY newid();", PrimaryKeyInfo.ColumnName, "TaxCache", countryID);
                    IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;
                    IDbCommand dbCommand = null;

                    try
                    {
                        dbCommand = DataAccess.SetCommand(sql, connectionString: conn.ConnectionString);
                        dbCommand.CommandType = CommandType.Text;

                        var result = DataAccess.GetValue(dbCommand).ToInt();
                        return TaxCache.Load(result);
                    }
                    finally
                    {
                        DataAccess.Close(dbCommand);
                    }
                }
            });
        }

        public static void MovementsTaxCache(TaxCacheSearchData oenMaterial)
        {

            var tbMaterial = new TaxCacheSearchDataType();
            oenMaterial.City = oenMaterial.City == null ? "" : oenMaterial.City;
            oenMaterial.County = oenMaterial.County == null ? "" : oenMaterial.County;
            tbMaterial.Add(oenMaterial);

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {

                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[sCOR_MovementsTaxCache]";
                SqlParameter param2 = cmd.Parameters.Add("@puTaxCache", SqlDbType.Structured);
                param2.Direction = ParameterDirection.Input;
                param2.TypeName = "uCOR_TaxCache";
                param2.Value = tbMaterial.Count == 0 ? null : tbMaterial;
                cmd.ExecuteNonQuery();
            }
        }


        public static List<TaxCacheSearchData> dllTaxCache(TaxCacheSearchData obj)
        {
            TaxCacheSearchData ObjTaxt;
            List<TaxCacheSearchData> lenTaxCache = null;
            var tipo = obj.CountyDefault == true ? 1 : 0;
            try
            {

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[sCOR_ListTaxCache]";
                    SqlParameter param2 = cmd.Parameters.Add("@piColumna", SqlDbType.Int);
                    param2.Direction = ParameterDirection.Input;
                    param2.Value = tipo;

                    SqlParameter param3 = cmd.Parameters.Add("@state", SqlDbType.VarChar);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = obj.State;


                    SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);
                    if (drd != null)
                    {
                        int pos_City = 0;
                        int pos_County = 0;
                        if (tipo == 1)
                        {
                            pos_City = drd.GetOrdinal("City");
                        }
                        else
                        {
                            pos_County = drd.GetOrdinal("County");
                        }


                        lenTaxCache = new List<TaxCacheSearchData>();
                        while (drd.Read())
                        {
                            ObjTaxt = new TaxCacheSearchData();
                            if (tipo == 1)
                            {
                                ObjTaxt.City = drd.GetString(pos_City);
                            }
                            else
                            {
                                ObjTaxt.County = drd.GetString(pos_County);
                            }
                            lenTaxCache.Add(ObjTaxt);


                        }
                        drd.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;

            }

            return lenTaxCache;

        }

        /// <summary>
        /// Method for insert, update data in the TaxCache table with BulkCopy
        /// </summary>
        /// <param name="uploadFileName">Name of file access .dba from file server</param>
        public static void BulkLoad(string uploadFileName, string path)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Inicia Metodo BulkLoad");
                }
            }


            SqlTransaction trans = null;
            try
            {
                DataTable dt = DataCeps(uploadFileName);

                //using (var scope = new TransactionScope())
                //{

                using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))//System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    trans = connection.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                   // cmd.CommandTimeout = 0;
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BulkCopyTimeout"]);

                    cmd.Transaction = trans;


                    //StringBuilder sb_ = new StringBuilder();
                    //sb_.Append("CREATE TABLE ##Code(");
                    //sb_.Append("[UFE_SG] [nvarchar](255) NULL,");
                    //sb_.Append("[LOC_NO] [nvarchar](255) NULL,");
                    //sb_.Append("[BAI_NO] [nvarchar](255) NULL,");
                    //sb_.Append("[LOG_NO] [nvarchar](255) NULL,");
                    //sb_.Append("[CEP] [nvarchar](255) NULL,");
                    //sb_.Append("[LOG_COMPLEMENTO] [nvarchar](255) NULL,");
                    //sb_.Append("[NOME] [nvarchar](255) NULL");
                    //sb_.Append(");");
                    //cmd.CommandText = sb_.ToString();
                    //cmd.ExecuteNonQuery();

                 
                    //// This text is always added, making the file longer over time 
                    //// if it is not deleted. 
                    //using (StreamWriter sw = File.AppendText(path))
                    //{
                    //    sw.WriteLine("Se creo la tabla  ##Code");
                    //}
                  
                    
                    //Bulk insert into temp table
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, trans))
                    //using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                    {
                        bulkcopy.BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BulkCopyBatchSize"]);// 2000;
                        bulkcopy.BulkCopyTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BulkCopyTimeout"]);// 3600; //1Hr.
                        bulkcopy.DestinationTableName = "TempCode";
                        bulkcopy.WriteToServer(dt);
                        bulkcopy.Close();
                    }


                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("Llego al bulkcopy, completado, cantidad : "+ dt.Rows.Count.ToString());
                    }
                  

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sCOR_CARGA";
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BulkCopyTimeout"]);
                    cmd.ExecuteNonQuery();
                    trans.Commit();

                    connection.Close();
                    dt.Dispose();

                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("Termino la carga");
                    }
                  
                    //scope.Complete();rssms
                }
                //}

            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(ex.Message);
                }
                  
                trans.Rollback();
                throw ex;
            }
            finally
            {
                System.IO.File.Delete(uploadFileName);
            }
        }


        public static string QueryMerge()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SET NOCOUNT ON;");
            sb.AppendLine("BEGIN TRANSACTION");
            sb.AppendLine("BEGIN TRY");
            sb.AppendLine("MERGE [dbo].[TaxCache_Copy] AS TARGET");
            sb.AppendLine("USING ##Code AS SOURCE");
            sb.AppendLine("ON (TARGET.[PostalCode]=SOURCE.[CEP])");
            sb.AppendLine("WHEN MATCHED  And TARGET.StateAbbreviation!=SOURCE.UFE_SG THEN");
            sb.AppendLine("UPDATE SET TARGET.StateAbbreviation= (CASE WHEN TARGET.StateAbbreviation!=SOURCE.UFE_SG THEN SOURCE.UFE_SG ELSE TARGET.StateAbbreviation END),");
            sb.AppendLine("TARGET.City= (CASE WHEN TARGET.City!=SOURCE.LOC_NO THEN SOURCE.LOC_NO ELSE TARGET.City END),");
            sb.AppendLine("TARGET.County=  (CASE WHEN TARGET.County!=SOURCE.BAI_NO THEN SOURCE.BAI_NO ELSE TARGET.County END),");
            sb.AppendLine("TARGET.Street=   (CASE WHEN TARGET.Street!=SOURCE.LOG_NO  THEN SOURCE.LOG_NO ELSE TARGET.Street END)");
            sb.AppendLine("WHEN NOT MATCHED BY TARGET   THEN");
            sb.AppendLine("INSERT");
            sb.AppendLine("(");
            sb.AppendLine("TaxCategoryID ,");
            sb.AppendLine("PostalCode,");
            sb.AppendLine("StateAbbreviation,");
            sb.AppendLine("[State],");
            sb.AppendLine("City,");
            sb.AppendLine("County,");
            sb.AppendLine("Street,");
            sb.AppendLine("CountryID,");
            sb.AppendLine("CountyFIPS,");
            sb.AppendLine("CitySalesTax,");
            sb.AppendLine("CityUseTax,");
            sb.AppendLine("CityLocalSales,");
            sb.AppendLine("CityLocalUse,");
            sb.AppendLine("CountySalesTax,");
            sb.AppendLine("CombinedSalesTax,");
            sb.AppendLine("CombinedUseTax,");
            sb.AppendLine("CountyDefault,");
            sb.AppendLine("GeneralDefault,");
            sb.AppendLine("IncityLimits,");
            sb.AppendLine("ChargeTaxOnShipping,");
            sb.AppendLine("DateCreatedUTC,");
            sb.AppendLine("DateCachedUTC,");
            sb.AppendLine("Latitude,");
            sb.AppendLine("Longitude,");
            sb.AppendLine("SpecialTax,");
            sb.AppendLine("MiscTax,");
            sb.AppendLine("TaxPercentage,");
            sb.AppendLine("active");
            sb.AppendLine(")");
            sb.AppendLine("VALUES(  null,");
            sb.AppendLine("SOURCE.[CEP],");
            sb.AppendLine("SOURCE.[UFE_SG],");
            sb.AppendLine("(Select TOP 1 name from [dbo].[StateProvinces] where StateAbbreviation = SOURCE.UFE_SG),");
            sb.AppendLine("SOURCE.[LOC_NO],");
            sb.AppendLine("SOURCE.[BAI_NO],");
            sb.AppendLine("SOURCE.[LOG_NO],");
            sb.AppendLine("73 ,");
            sb.AppendLine("null,");
            sb.AppendLine("0,");
            sb.AppendLine("0,");
            sb.AppendLine("0	,");
            sb.AppendLine("0	,");
            sb.AppendLine("0	,");
            sb.AppendLine("0	,");
            sb.AppendLine("0	,");
            sb.AppendLine("null	,");
            sb.AppendLine("null	,");
            sb.AppendLine("null	,");
            sb.AppendLine("0	,");
            sb.AppendLine("getdate(),");
            sb.AppendLine("getdate()	,");
            sb.AppendLine("null	,");
            sb.AppendLine("null	,");
            sb.AppendLine("null	,");
            sb.AppendLine("null	,");
            sb.AppendLine("null	,");
            sb.AppendLine("0");

            sb.AppendLine(")");
            sb.AppendLine("WHEN NOT MATCHED BY SOURCE THEN");
            sb.AppendLine("UPDATE SET TARGET.Active =0;");
            sb.AppendLine("END TRY");
            sb.AppendLine("BEGIN CATCH");

            // para versiones inferiores a sql server 2012 usar raiserror ('usp_my_procedure_name: %d: %s', 16, 1, @error, @message)  en lugar de THROW;
            sb.AppendLine("ROLLBACK TRANSACTION");
            sb.AppendLine("THROW;");
            sb.AppendLine("END CATCH");

            return sb.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadFileName">Name of file access .dba from file server</param>
        /// <returns>Returns DataTable object with data from file access</returns>
        public static DataTable DataCeps(string uploadFileName)
        {
            //*******************************************************************************************************************
            //   Conexión a la BD acces y llenar datatable con resultado de consulta
            //**********************************************************************************************************************
            DataTable dt = new DataTable();

            string sConAcces = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + uploadFileName;

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  LOG_LOGRADOURO.UFE_SG,  LOG_LOCALIDADE.LOC_NO,  LOG_BAIRRO.BAI_NO, LOG_LOGRADOURO.TLO_TX+\" \" + LOG_LOGRADOURO.LOG_NO AS LOG_NO, LOG_LOGRADOURO.CEP,  LOG_LOGRADOURO.LOG_COMPLEMENTO,\"\" AS NOME ");
            sb.Append(" FROM LOG_LOGRADOURO, LOG_LOCALIDADE, LOG_BAIRRO ");
            sb.Append(" WHERE log_logradouro.loc_nu= LOG_LOCALIDADE.loc_nu  AND LOG_LOGRADOURO.bai_nu_INI=LOG_BAIRRO.BAI_NU AND LOG_LOGRADOURO.LOG_STA_TLO =\"S\" ");
            sb.Append(" Union ");
            sb.Append(" SELECT LOG_LOGRADOURO.UFE_SG,  LOG_LOCALIDADE.LOC_NO,  LOG_BAIRRO.BAI_NO,  LOG_LOGRADOURO.LOG_NO AS LOG_NO, LOG_LOGRADOURO.CEP,  LOG_LOGRADOURO.LOG_COMPLEMENTO,\"\" AS NOME ");
            sb.Append(" FROM LOG_LOGRADOURO, LOG_LOCALIDADE, LOG_BAIRRO ");
            sb.Append(" WHERE log_logradouro.loc_nu= LOG_LOCALIDADE.loc_nu  AND LOG_LOGRADOURO.bai_nu_INI=LOG_BAIRRO.BAI_NU  AND LOG_LOGRADOURO.LOG_STA_TLO =\"N\" ");
            sb.Append(" Union ");
            sb.Append(" SELECT LOG_LOCALIDADE.UFE_SG, LOG_LOCALIDADE.LOC_NO,\"\" AS BAI_NO,\"\" AS LOG_NO, LOG_LOCALIDADE.CEP, \"\" AS LOG_COMPLEMENTO,\"\" AS NOME ");
            sb.Append(" FROM LOG_LOCALIDADE ");
            sb.Append(" WHERE LOG_LOCALIDADE.CEP IS NOT NULL ");
            sb.Append(" UNION SELECT LOG_CPC.UFE_SG, LOG_LOCALIDADE.LOC_NO,\"\" AS  BAI_NO, LOG_CPC.CPC_ENDERECO AS LOG_NO, LOG_CPC.CEP,\"\" AS LOG_COMPLEMENTO,CPC_NO AS NOME ");
            sb.Append(" FROM LOG_CPC,  LOG_LOCALIDADE ");
            sb.Append(" WHERE  LOG_CPC.LOC_NU=LOG_LOCALIDADE.LOC_NU ");
            sb.Append(" UNION SELECT  LOG_GRANDE_USUARIO.UFE_SG, LOG_LOCALIDADE.LOC_NO, LOG_BAIRRO.BAI_NO AS  BAI_NO,  LOG_GRANDE_USUARIO.GRU_ENDERECO AS LOG_NO,  LOG_GRANDE_USUARIO.CEP,\"\" AS LOG_COMPLEMENTO,GRU_NO AS NOME ");
            sb.Append(" FROM LOG_GRANDE_USUARIO,  LOG_LOCALIDADE, LOG_BAIRRO ");
            sb.Append(" WHERE  LOG_GRANDE_USUARIO.LOC_NU=LOG_LOCALIDADE.LOC_NU AND  LOG_GRANDE_USUARIO.BAI_NU = LOG_BAIRRO.BAI_NU ");
            sb.Append(" UNION SELECT  LOG_UNID_OPER.UFE_SG, LOG_LOCALIDADE.LOC_NO, LOG_BAIRRO.BAI_NO AS  BAI_NO,  LOG_UNID_OPER.UOP_ENDERECO AS LOG_NO, LOG_UNID_OPER.CEP,\"\" AS LOG_COMPLEMENTO, UOP_NO AS NOME ");
            sb.Append(" FROM LOG_UNID_OPER,  LOG_LOCALIDADE, LOG_BAIRRO ");
            sb.Append(" WHERE  LOG_UNID_OPER.LOC_NU=LOG_LOCALIDADE.LOC_NU AND  LOG_UNID_OPER.BAI_NU = LOG_BAIRRO.BAI_NU; ");


            using (OleDbConnection Conn = new OleDbConnection(sConAcces))
            {
                Conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = Conn;
                cmd.CommandText = sb.ToString();
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);

                Conn.Close();

            }

            return dt;
        }


        public static List<TaxCache> GetPostalCodes(int countryID, string postalCode)
        {
            //using (NetStepsEntities context = CreateContext())
            //{
            //    return context.TaxCaches.Where(x => x.CountryID == countryID && x.PostalCode == postalCode.Replace("-", "")).ToList();
            //}

            TaxCache taxCache;
            List<TaxCache> ListTaxCache = null;
            //var tipo = obj.CountyDefault == true ? 1 : 0;
            try
            {

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
//                    string query = string.Format(@"
//                                                Select 
//                                                    T.TaxCacheID, 
//                                                    T.PostalCode, 
//                                                    T.City, 
//                                                    T.State, 
//                                                    T.StateAbbreviation, 
//                                                    T.County, 
//                                                    T.CountryID, 
//                                                    T.Street 
//                                                From TaxCache T 
//                                                where T.CountryID ={0} and T.PostalCode= '{1}'", countryID, postalCode);

                    string query = string.Format(@"
                                                Select 
                                                    T.TaxCacheID, 
                                                    T.PostalCode, 
                                                    T.City, 
                                                    T.State, 
                                                    T.StateAbbreviation, 
                                                    T.County, 
                                                    T.CountryID, 
                                                    T.Street,
                                                    P.StateProvinceID
                                                From TaxCache T 
                                                Join StateProvinces P On T.StateAbbreviation = P.StateAbbreviation
                                                where T.CountryID ={0} and T.PostalCode= '{1}'", countryID, postalCode);

                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text; //CommandType.StoredProcedure;
                    cmd.CommandText = query;// "[dbo].[sCOR_ListTaxCache]";
                    //SqlParameter param2 = cmd.Parameters.Add("@piColumna", SqlDbType.Int);
                    //param2.Direction = ParameterDirection.Input;
                    //param2.Value = tipo;
                    SqlDataReader drd = cmd.ExecuteReader();
                    if (drd != null)
                    {

                        int iTaxCacheID = drd.GetOrdinal("TaxCacheID");
                        int iPostalCode = drd.GetOrdinal("PostalCode");
                        int iCity = drd.GetOrdinal("City");
                        int iState = drd.GetOrdinal("State");
                        int iStateAbbreviation = drd.GetOrdinal("StateAbbreviation");
                        int iCounty = drd.GetOrdinal("County");
                        int iCountryID = drd.GetOrdinal("CountryID");
                        int iStreet = drd.GetOrdinal("Street");
                        //A01
                        int iStateProvinceID = drd.GetOrdinal("StateProvinceID");

                        ListTaxCache = new List<TaxCache>();

                        while (drd.Read())
                        {
                            taxCache = new TaxCache();
                            taxCache.TaxCacheID = drd.GetInt32(iTaxCacheID);
                            taxCache.PostalCode = drd.GetString(iPostalCode);
                            taxCache.City = drd.GetString(iCity);
                            taxCache.State = drd.GetString(iState);
                            taxCache.StateAbbreviation = drd.GetString(iStateAbbreviation);
                            taxCache.County = drd.GetString(iCounty);
                            taxCache.CountryID = drd.GetInt32(iCountryID);
                            taxCache.Street = drd.GetString(iStreet);
                            //A01
                            taxCache.StateProvinceID = drd.GetInt32(iStateProvinceID);

                            ListTaxCache.Add(taxCache);
                        }

                        drd.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;

            }

            return ListTaxCache;
        }


        //@01 20150717 BR-CC-003 G&S LIB: Se agregaron los metodos SearchCityFromState y SearchCountyFromCity
        public List<string> SearchCityFromState(string state)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    state = state.ToCleanString();

                    var results = (from t in context.TaxCaches
                                  where t.State.Equals(state)
                                  orderby t.City ascending
                                  group t by t.City into g                                  
                                  select g.Key).ToList().OrderBy(s => s);

                    return results.ToList();
                }
            });
        }

        public List<string> SearchCountyFromCity(string state,string city)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    city = city.ToCleanString();

                    var results = (from t in context.TaxCaches
                                  where t.City.Equals(city) && t.State.Equals(state)
                                  && !t.County.Equals("")
                                  orderby t.County ascending
                                  group t by t.County into g
                                  select g.Key).ToList().OrderBy(s => s);

                    return results.ToList();
                }
            });
        }

    }
}
