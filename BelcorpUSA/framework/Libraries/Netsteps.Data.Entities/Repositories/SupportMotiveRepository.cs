using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.Logic;
using System.Data;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Repositories
{
    public class SupportMotiveRepository
    {

        public static PaginatedList<SupportMotiveSearchData> Search(SupportMotiveSearchParameters searchParams)
        {
            // Apply filters
            var periods = SupportMotiveDataAccess.Search().FindAll(
                    x => x.Active == (searchParams.Active.HasValue ? searchParams.Active : x.Active) &&
                    (x.SupportMotiveID == (searchParams.SupportMotiveID > 0 ? searchParams.SupportMotiveID : x.SupportMotiveID)) &&
                    (x.Name == (searchParams.Name != "" ? searchParams.Name : x.Name)
                      
                    )
                                                                                                                            );

            // Apply pagination
            IQueryable<SupportMotiveSearchData> matchingItems = periods.AsQueryable<SupportMotiveSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<SupportMotiveSearchData>(searchParams, resultTotalCount);
        }

        public static PaginatedList<SupportMotiveSearchData> SearchFilter(SupportMotiveSearchParameters searchParams)
        {
            // Apply filters
            var periods = SupportMotiveDataAccess.SearchFilter(searchParams.dtSupportLevelIDs).FindAll(
                    x => x.Active == (searchParams.Active.HasValue ? searchParams.Active : x.Active) &&
                    (x.SupportMotiveID == (searchParams.SupportMotiveID > 0 ? searchParams.SupportMotiveID : x.SupportMotiveID)) &&
                    (x.Name.Contains( (searchParams.Name != "" ? searchParams.Name : x.Name))                        
                    )
                                                                                                                            );

            // Apply pagination
            IQueryable<SupportMotiveSearchData> matchingItems = periods.AsQueryable<SupportMotiveSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<SupportMotiveSearchData>(searchParams, resultTotalCount);
        }
        public static Dictionary<string, string> SearchAllSupportMotiveLevel(out Dictionary<string, Dictionary<string, string>> dcAdicionales)
        {
            return SupportMotiveDataAccess.SearchAllSupportMotiveLevel( out dcAdicionales);
        }
        public static List<SupportMotiveSearchData> Search()
        {
            return SupportMotiveDataAccess.Search();
        }
        public static List<SupportMotiveSearchData> SearchByMotive(string name)
        {
            return SupportMotiveDataAccess.Search().FindAll(x => x.Name.Contains(name));
        }
        public static List<MarketSearchData> GetMarketsbySupportMotive(string SupportMotiveID)
        {
            return SupportMotiveDataAccess.GetMarketsbySupportMotive(SupportMotiveID);
        }

        public static List<SupportMotivePropertyDinamic> GetSupportMotivePropertyDinamic()
        {
            return SupportMotiveDataAccess.GetSupportMotivePropertyDinamic();
        }

        public static List<int> GetMotiveLevelBySupportMotive(int SupportMotiveID)
        {
            return SupportMotiveDataAccess.GetMotiveLevelBySupportMotive(SupportMotiveID);
        }


        public static int Save(SupportMotiveSearchData motive, string[] SupportLevelIDs, string[] MarketIDs, out byte resultadoinsercionSupportLevelMotive, out byte resultadoEliminarSupportMotiveLevel)
        {
             
            int supportMotiveID = motive.SupportMotiveID;
            if (motive.SupportMotiveID == 0)
            {
                supportMotiveID = SupportMotiveDataAccess.InsertSupportMotive(motive);

            }
            else
            {
                SupportMotiveDataAccess.Update(motive);
            }

            resultadoEliminarSupportMotiveLevel = SaveSupportLevelMotiveIDs(supportMotiveID, SupportLevelIDs, out resultadoinsercionSupportLevelMotive);
            SaveSupportMotiveMarketIDs(supportMotiveID, MarketIDs);
            return supportMotiveID;
        }

        public static byte SaveSupportLevelMotiveIDs(int supportMotiveID, string[] supportLevelIDs, out  byte resultadoinsertarSupportMotiveLevel)
        {
            byte resultado = 0;
              resultadoinsertarSupportMotiveLevel = 0;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("LevelMotiveIDsTransaction");

                try
                {
                    if (supportLevelIDs.Length!=0)
                    {
                        resultado=  SupportMotiveDataAccess.DeleteSupportLevelMotiveIDs(supportMotiveID, connection, transaction);
                    }
                    // Save restriction per titles
                    if (!supportLevelIDs.IsNull())
                    {
                        foreach (var titleID in supportLevelIDs)
                        {
                            resultadoinsertarSupportMotiveLevel=    SupportMotiveDataAccess.SaveSupportMotivePerTitles(supportMotiveID, titleID, 1, connection, transaction);
                        break;
                        }
                    }

                    
                    transaction.Commit();
                    return resultado;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static void SaveSupportMotiveMarketIDs(int supportMotiveID, string[] MarketIDs)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("MotiveMarketTransaction");

                try
                {
                    SupportMotiveDataAccess.DeleteSupportMotiveMarketIDs(supportMotiveID, connection, transaction);

                    // Save restriction per titles
                    if (!MarketIDs.IsNull())
                    {
                        foreach (var titleID in MarketIDs)
                        {
                            SupportMotiveDataAccess.SaveSupportMotivePerTitles(supportMotiveID, titleID, 2, connection, transaction);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static void SaveMotiveProperty(SupportMotivePropertyTypeSearchData property)
        {
            SupportMotiveDataAccess.InsertSupportMotiveProperty(property);
        }

        //public static void Save(SupportMotiveSearchData motive, string[] SupportLevelIDs, string[] MarketIDs)
        //{
        //    using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
        //    {
        //        connection.Open();
        //        SqlTransaction transaction = connection.BeginTransaction("SaveRestAccountTypesTran");

        //        try
        //        {
        //            // Save restriction
        //            var motivoID = SupportMotiveDataAccess.SaveSupportMotive(motive, connection, transaction);


        //            // Save restriction per titles
        //            if (!SupportLevelIDs.IsNull())
        //            {
        //                foreach (var titleID in SupportLevelIDs)
        //                {
        //                    SupportMotiveDataAccess.SaveSupportMotivePerTitles(motivoID, titleID, 1, connection, transaction);
        //                }
        //            }
        //            transaction.Commit();

        //            transaction = connection.BeginTransaction("SaveRestAccountTypesTran");
        //            if (!MarketIDs.IsNull())
        //            {
        //                foreach (var titleID in MarketIDs)
        //                {
        //                    SupportMotiveDataAccess.SaveSupportMotivePerTitles(motivoID, titleID, 2, connection, transaction);
        //                }
        //            }


        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        //        }
        //    }
        //}

        public static PaginatedList<SupportMotivePropertyTypeSearchData> SearchMotiveProperty(SupportMotiveSearchParameters searchParams)
        {
            // Apply filters
            var data = SupportMotiveDataAccess.SearchMotiveProperty().FindAll(x => x.SupportMotiveID == (searchParams.SupportMotiveID.HasValue ? searchParams.SupportMotiveID : x.SupportMotiveID));

            // Apply pagination
            IQueryable<SupportMotivePropertyTypeSearchData> matchingItems = data.AsQueryable<SupportMotivePropertyTypeSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<SupportMotivePropertyTypeSearchData>(searchParams, resultTotalCount);
        }


        public static List<SupportMotivePropertyTypeSearchData> GetPropertyTypesBySupportMotive(int SupportMotiveID)
        {
            return SupportMotiveDataAccess.SearchMotiveProperty().FindAll(x => x.SupportMotiveID == SupportMotiveID);
        }

        public static void SaveMotiveTask(SupportMotiveTaskSearchData property)
        {
            SupportMotiveDataAccess.InsertSupportMotiveTask(property);
        }

        public static PaginatedList<SupportMotiveTaskSearchData> SearchMotiveTask(SupportMotiveSearchParameters searchParams)
        {
            // Apply filters
            var data = SupportMotiveDataAccess.SearchMotiveTask().FindAll(x => x.SupportMotiveID == (searchParams.SupportMotiveID.HasValue ? searchParams.SupportMotiveID : x.SupportMotiveID));

            // Apply pagination
            IQueryable<SupportMotiveTaskSearchData> matchingItems = data.AsQueryable<SupportMotiveTaskSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<SupportMotiveTaskSearchData>(searchParams, resultTotalCount);
        }

        public static void DeleteSupportMotivePropertyIDs(string[] propertyIDs)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("MotivePropertyIDsTransaction");
                try
                {

                    // Save restriction per titles
                    if (!propertyIDs.IsNull())
                    {
                        foreach (var titleID in propertyIDs)
                        {
                            SupportMotiveDataAccess.DeleteSupportMotiveProperty(int.Parse(titleID), connection, transaction);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static void DeleteSupportTaskIDs(string[] taskIDs)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("MotiveTaskIDsTransaction");
                try
                {

                    // Save restriction per titles
                    if (!taskIDs.IsNull())
                    {
                        foreach (var titleID in taskIDs)
                        {
                            SupportMotiveDataAccess.DeleteSupportMotiveTask(int.Parse(titleID), connection, transaction);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static PaginatedList<SupportMotivePropertyValuesSearchData> SearchPropertyValues(SupportMotiveSearchParameters searchParams)
        {
            // Apply filters
            var data = SupportMotiveDataAccess.SearchPropertyValues().FindAll(x => x.SupportMotivePropertyTypeID == searchParams.SupportMotiveID);

            // Apply pagination
            IQueryable<SupportMotivePropertyValuesSearchData> matchingItems = data.AsQueryable<SupportMotivePropertyValuesSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<SupportMotivePropertyValuesSearchData>(searchParams, resultTotalCount);
        }

        public static void SavePropertyValue(SupportMotivePropertyValuesSearchData property)
        {
            SupportMotiveDataAccess.InsertPropertyValue(property);
        }

        public static void DeletePropertyValueIDs(string[] valueIDs)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("PropertyValueIDsTransaction");
                try
                {

                    // Save restriction per titles
                    if (!valueIDs.IsNull())
                    {
                        foreach (var titleID in valueIDs)
                        {
                            SupportMotiveDataAccess.DeletePropertyValue(int.Parse(titleID), connection, transaction);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }


    }

    public class SupportMotiveDataAccess
    {
        public static Dictionary<string, string> SearchAllSupportMotiveLevel(out Dictionary<string,    Dictionary<string, string>> dcAdicionales)
        {
            Dictionary<string, string> dcSupportMotiveLevel = new Dictionary<string, string>();
            Dictionary<string, string> dcKeys = null;
            Dictionary<string, Dictionary<string, string>> _dcAdicionales = new Dictionary<string, Dictionary<string, string>>();
            dcAdicionales = _dcAdicionales;
            
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetSupportLevelMotives", null, "Core"))
                {

                    if (reader.HasRows)
                    {                    
                        while (reader.Read())
                        {
                            dcKeys = new Dictionary<string, string>();
                            //SupportMotiveSearchData supportMotive = new SupportMotiveSearchData();
                            //supportMotive.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]); 
                            //supportMotive.LevelName = 
                            dcKeys[reader["SupportMotiveID"].ToString()] = reader["SupportLevelID"].ToString();
                            _dcAdicionales[reader["Row"].ToString()] = dcKeys;
                            dcSupportMotiveLevel[reader["Row"].ToString()] = Convert.ToString(reader["LevelName"]);
                        }
                       
                    }
                    return dcSupportMotiveLevel;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
           
        }


        public static List<SupportMotiveSearchData> SearchFilter(DataTable dtSupportLevelID)
        {
            SqlParameter[] LstParameter = new SqlParameter[] 
                {
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Structured,Value=dtSupportLevelID,ParameterName="@UtdSupportLevelIDs"} 

                };
            List<SupportMotiveSearchData> result = new List<SupportMotiveSearchData>();
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = connection;
                        ocom.CommandText = "upsGetSupportMotives_temp";
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.Parameters.AddRange(LstParameter);

                        using (IDataReader reader = ocom.ExecuteReader())
                        {
                                result = new List<SupportMotiveSearchData>();
                                while (reader.Read())
                                {
                                    SupportMotiveSearchData supportMotive = new SupportMotiveSearchData();
                                    supportMotive.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]);
                                    supportMotive.Name = Convert.ToString(reader["Name"]);
                                    supportMotive.Description = Convert.ToString(reader["Description"]);
                                    supportMotive.Active = Convert.ToBoolean(reader["Active"]);
                                    supportMotive.LevelName = Convert.ToString(reader["LevelName"]);
                                    supportMotive.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);


                                    supportMotive.IsVisibleToWorkStation = Convert.ToBoolean(reader["IsVisibleToWorkStation"]);
                                    supportMotive.HasConfirmation = Convert.ToBoolean(reader["HasConfirmation"]);
                                    if (Convert.ToString(reader["DateCreatedUTC"]) != "")
                                        supportMotive.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);
                                    if (Convert.ToString(reader["DateLastModifiedUTC"]) != "")
                                        supportMotive.DateLastModifiedUTC = Convert.ToDateTime(reader["DateLastModifiedUTC"]);

                                    supportMotive.Edit = Convert.ToBoolean(reader["Edit"]);

                                    result.Add(supportMotive);
                                }
                             
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static List<MarketSearchData> GetMarketsbySupportMotive(string SupportMotiveID)
        {
            List<MarketSearchData> result = new List<MarketSearchData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SupportMotiveID", SupportMotiveID } };
                SqlDataReader reader = DataAccess.GetDataReader("upsGetMarketsbySupportMotive  ", parameters, "Core");

                using (reader)
                {
                    if (reader.HasRows)
                    {
                        result = new List<MarketSearchData>();
                        while (reader.Read())
                        {
                            MarketSearchData Markets = new MarketSearchData();
                            Markets.MarketID = Convert.ToInt32(reader["MarketID"]);
                            Markets.Name = Convert.ToString(reader["Name"]);
                            Markets.Relacionado = Convert.ToInt32(reader["Relacionado"]);
                            result.Add(Markets);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }



        public static List<int> GetMotiveLevelBySupportMotive(int SupportMotiveID)
        {
            List<int> lstSupportMotivesLevel = new List<int>();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SupportMotiveID", SupportMotiveID } };
                SqlDataReader reader = DataAccess.GetDataReader("UspGetSupportLevelMotiveBySupportMotive  ", parameters, "Core");

                using (reader)
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            lstSupportMotivesLevel.Add(Convert.ToInt32(reader["SupportLevelID"]));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportMotivesLevel;
        }

        
        public static  Dictionary< int ,string> GetSuportLevelConcats(System.Data.DataTable dtSupportLevel)
        {

            Dictionary<int, string> dcResultados = new  Dictionary<int, string>();
            try
            {
                SqlParameter[] LstParameter = new SqlParameter[] 
                {
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Structured,Value=dtSupportLevel,ParameterName="@UtdSupportLevelIDs"} 

                };

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                   
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = connection;

                        ocom.CommandText = "UspListarSupportLevelConcatenados";
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.Parameters.AddRange(LstParameter);
                        
                        using(IDataReader reader= ocom.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dcResultados[Convert.ToInt32(reader["SupportLevelID"])] = reader["LevelName"].ToString();
                            }
                        }
                    }
                }
                return dcResultados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            
        }



        public static string  ConcatenarSupportLevel(int SupportLevelID)
        {
            String LevelName = "";

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SupportLevelID", SupportLevelID } };
                SqlDataReader reader = DataAccess.GetDataReader("UspConcatSupportLevel  ", parameters, "Core");

                using (reader)
                {
                    if (reader.HasRows)
                    {

                         if (reader.Read())
                        {
                            LevelName=  reader["LevelName"].ToString ();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return LevelName;
        }

        public static int InsertSupportMotive(SupportMotiveSearchData motive)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@Name", motive.Name },
                                                                                            { "@Description", motive.Description },
                                                                                            { "@MotiveSLA", motive.MotiveSLA},
                                                                                            { "@Active", motive.Active },
                                                                                            { "@IsVisibleToWorkStation", motive.IsVisibleToWorkStation},
                                                                                            { "@HasConfirmation", motive.HasConfirmation}
                                                                                         };

                using (SqlCommand cmd = DataAccess.GetCommand("upsInsSupportMotive", parameters, "Core") as SqlCommand)
                {
                    using (SqlConnection ocon = cmd.Connection)
                    {
                        ocon.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void Update(SupportMotiveSearchData motive)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotiveID", motive.SupportMotiveID },
                                                                                            { "@Name", motive.Name },
                                                                                            { "@Description", motive.Description },
                                                                                            { "@MotiveSLA", motive.MotiveSLA},
                                                                                            { "@Active", motive.Active },
                                                                                            { "@IsVisibleToWorkStation", motive.IsVisibleToWorkStation},
                                                                                            { "@HasConfirmation", motive.HasConfirmation}        
                                                                                         };

                using (SqlCommand cmd = DataAccess.GetCommand("upsUpdSupportMotive", parameters, "Core") as SqlCommand)
                {
                    using (SqlConnection con = cmd.Connection)
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static byte SaveSupportMotivePerTitles(int supportMotiveID, string titleID, int titleTypeID, SqlConnection connection, SqlTransaction transaction)
        {
            byte resultado = 0;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotiveID", supportMotiveID },
                                                                                            { "@TitleID", titleID },
                                                                                            { "@TitleTypeID", titleTypeID }};

                using (SqlCommand cmd = DataAccess.GetCommand("upsInsSupportMotivePerTitle", parameters, connection, transaction) as SqlCommand)
                {
                    resultado= Convert.ToByte(  cmd.ExecuteScalar());
                    return resultado;
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleteSupportMotiveMarketIDs(int supportMotiveID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SupportMotiveID", supportMotiveID } };

                using (SqlCommand cmd = DataAccess.GetCommand("upsDelSupportMotiveMarket", parameters, connection, transaction) as SqlCommand)
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static byte  DeleteSupportLevelMotiveIDs(int supportMotiveID, SqlConnection connection, SqlTransaction transaction)
        {
            byte resultado = 0;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SupportMotiveID", supportMotiveID } };

                using (SqlCommand cmd = DataAccess.GetCommand("upsDelSupportLevelMotive", parameters, connection, transaction) as SqlCommand)
                {
                    resultado =     Convert.ToByte( cmd.ExecuteScalar());
                    return resultado;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int InsertSupportMotiveProperty(SupportMotivePropertyTypeSearchData property)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotiveID", property.SupportMotiveID },
                                                                                            { "@Name", property.Name },
                                                                                            { "@DataType", property.DataType },
                                                                                            { "@Required", property.Required},
                                                                                            { "@Editable", property.Editable },
                                                                                            { "@IsVisibleToWorkStation", property.IsVisibleToWorkStation},
                                                                                            { "@FieldSolution", property.FieldSolution},
                                                                                            { "@SupportMotivePropertyTypeID", property.SupportMotivePropertyTypeID},
                                                                                            { "@SupportMotivePropertyDinamicID", property.SupportMotivePropertyDinamicID}

                                                                                         };

                using (SqlCommand cmd = DataAccess.GetCommand("upsInsSupportMotivePropertyType", parameters, "Core") as SqlCommand)
                {
                    using (SqlConnection ocon = cmd.Connection)
                    {
                        ocon.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Obtiene los tipos de busqueda
        /// </summary>
        /// <returns></returns>
        public static List<SupportMotivePropertyDinamic> GetSupportMotivePropertyDinamic()
        {
            List<SupportMotivePropertyDinamic> result = new List<SupportMotivePropertyDinamic>();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetSupportMotivePropertyDinamic", null, "Core"))
                {

                    if (reader.HasRows)
                    {
                        result = new List<SupportMotivePropertyDinamic>();
                        while (reader.Read())
                        {
                            SupportMotivePropertyDinamic supportMotive = new SupportMotivePropertyDinamic();
                            supportMotive.SupportMotivePropertyDinamicID = Convert.ToInt32(reader["SupportMotivePropertyDinamicID"]);
                            supportMotive.Name = Convert.ToString(reader["Name"]);
                            supportMotive.TermName = Convert.ToString(reader["TermName"]);
                            supportMotive.Active = Convert.ToBoolean(reader["Active"].ToString());
                          
                            result.Add(supportMotive);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }


        public static List<SupportMotivePropertyTypeSearchData> SearchMotiveProperty()
        {
            List<SupportMotivePropertyTypeSearchData> result = new List<SupportMotivePropertyTypeSearchData>();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetSupportMotivePropertyType", null, "Core"))
                {

                    if (reader.HasRows)
                    {
                        result = new List<SupportMotivePropertyTypeSearchData>();
                        while (reader.Read())
                        {
                            SupportMotivePropertyTypeSearchData supportMotive = new SupportMotivePropertyTypeSearchData();
                            supportMotive.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]);
                            supportMotive.Name = Convert.ToString(reader["Name"]);
                            supportMotive.DataType = Convert.ToString(reader["DataType"]);
                            supportMotive.Required = Convert.ToBoolean(reader["Required"]);
                            supportMotive.SortIndex = Convert.ToInt32(reader["SortIndex"]);
                            supportMotive.IsVisibleToWorkStation = Convert.ToBoolean(reader["IsVisibleToWorkStation"]);
                            supportMotive.Editable = Convert.ToBoolean(reader["Editable"]);
                            supportMotive.SupportMotivePropertyTypeID = Convert.ToInt32(reader["SupportMotivePropertyTypeID"]);

                            supportMotive.FieldSolution = Convert.ToBoolean(reader["FieldSolution"]);
                            result.Add(supportMotive);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static int InsertSupportMotiveTask(SupportMotiveTaskSearchData task)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotiveID", task.SupportMotiveID },
                                                                                            { "@Name", task.Name },
                                                                                            { "@Link", task.Link },
                                                                                            { "@SupportMotivePropertyTypeID", task.SupportMotivePropertyTypeID},
                                                                                            { "@SupportMotiveTaskID", task.SupportMotiveTaskID}

                                                                                            
                                                                                         };

                using (SqlCommand cmd = DataAccess.GetCommand("upsInsSupportMotiveTask", parameters, "Core") as SqlCommand)
                {
                    using (SqlConnection con = cmd.Connection)
                    {
                        con.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<SupportMotiveTaskSearchData> SearchMotiveTask()
        {
            List<SupportMotiveTaskSearchData> result = new List<SupportMotiveTaskSearchData>();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetSupportMotiveTask", null, "Core"))
                {
                    if (reader.HasRows)
                    {
                        result = new List<SupportMotiveTaskSearchData>();
                        while (reader.Read())
                        {
                            SupportMotiveTaskSearchData supportMotive = new SupportMotiveTaskSearchData();
                            supportMotive.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]);
                            supportMotive.Name = Convert.ToString(reader["Name"]);
                            supportMotive.Link = Convert.ToString(reader["Link"]);
                            supportMotive.NameProperty = Convert.ToString(reader["NameProperty"]);
                            supportMotive.SortIndex = Convert.ToInt32(reader["SortIndex"]);
                            supportMotive.SupportMotiveTaskID = Convert.ToInt32(reader["SupportMotiveTaskID"]);
                            result.Add(supportMotive);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static void DeleteSupportMotiveProperty(int SupportMotivePropertyTypeID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotivePropertyTypeID", SupportMotivePropertyTypeID }
                                                                                           
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("upsDelSupportMotivePropertyType", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleteSupportMotiveTask(int SupportMotiveTaskID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotiveTaskID", SupportMotiveTaskID }
                                                                                           
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("upsDelSupportMotiveTask", parameters, connection, transaction) as SqlCommand;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<SupportMotivePropertyValuesSearchData> SearchPropertyValues()
        {
            List<SupportMotivePropertyValuesSearchData> result = new List<SupportMotivePropertyValuesSearchData>();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetSupportMotivePropertyValues", null, "Core")) 
                {

                    if (reader.HasRows)
                    {
                        result = new List<SupportMotivePropertyValuesSearchData>();
                        while (reader.Read())
                        {
                            SupportMotivePropertyValuesSearchData supportMotive = new SupportMotivePropertyValuesSearchData();
                            supportMotive.SupportMotivePropertyValueID = Convert.ToInt32(reader["SupportMotivePropertyValueID"]);
                            supportMotive.SupportMotivePropertyTypeID = Convert.ToInt32(reader["SupportMotivePropertyTypeID"]);
                            supportMotive.Value = Convert.ToString(reader["Value"]);
                            supportMotive.SortIndex = Convert.ToInt32(reader["SortIndex"]);
                            result.Add(supportMotive);
                        }
                    }
               }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static int InsertPropertyValue(SupportMotivePropertyValuesSearchData motive)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@Value", motive.Value },
                                                                                            { "@SortIndex", motive.SortIndex },
                                                                                            { "@SupportMotivePropertyTypeID", motive.SupportMotivePropertyTypeID}
                                                                                         };

                using (SqlCommand cmd = DataAccess.GetCommand("upsInsSupportMotivePropertyValue", parameters, "Core") as SqlCommand)
                {
                    using (SqlConnection ocon = cmd.Connection)
                    {
                        ocon.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeletePropertyValue(int SupportMotivePropertyValueID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportMotivePropertyValueID", SupportMotivePropertyValueID }
                                                                                           
                                                                                         };

                using (SqlCommand cmd = DataAccess.GetCommand("upsDelSupportMotivePropertyValue", parameters, connection, transaction) as SqlCommand)
                {
                     
                             cmd.ExecuteNonQuery();
                      
                }

                
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<SupportMotiveSearchData> Search()
        {
            List<SupportMotiveSearchData> result = new List<SupportMotiveSearchData>();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetSupportMotives", null, "Core"))
                {

                    if (reader.HasRows)
                    {
                        result = new List<SupportMotiveSearchData>();
                        while (reader.Read())
                        {
                            SupportMotiveSearchData supportMotive = new SupportMotiveSearchData();
                            supportMotive.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]);
                            supportMotive.Name = Convert.ToString(reader["Name"]);
                            supportMotive.Description = Convert.ToString(reader["Description"]);
                            supportMotive.Active = Convert.ToBoolean(reader["Active"]);
                            supportMotive.LevelName = Convert.ToString(reader["LevelName"]);
                            supportMotive.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);


                            supportMotive.IsVisibleToWorkStation = Convert.ToBoolean(reader["IsVisibleToWorkStation"]);
                            supportMotive.HasConfirmation = Convert.ToBoolean(reader["HasConfirmation"]);
                            if (Convert.ToString(reader["DateCreatedUTC"]) != "")
                                supportMotive.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);
                            if (Convert.ToString(reader["DateLastModifiedUTC"]) != "")
                                supportMotive.DateLastModifiedUTC = Convert.ToDateTime(reader["DateLastModifiedUTC"]);

                            supportMotive.Edit = Convert.ToBoolean(reader["Edit"]);

                            result.Add(supportMotive);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
    }
}
