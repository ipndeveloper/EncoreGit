using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;


namespace NetSteps.Data.Entities.Repositories
{
    public class SupportLevelRepository
    {
        public static List<SupportLevelSearchData> Search(int? ID)
        {
            return SupportLevelDataAccess.Search().FindAll(x => x.ParentSupportLevelID == (ID)).OrderBy(y=> y.SortIndex).ToList();
        }

        public static SupportLevelSearchData Get(int? ID)
        {
            return SupportLevelDataAccess.Search().FindAll(x => x.SupportLevelID == (ID)).ToList().FirstOrDefault();
        }

        public static void Save(SupportLevelSearchData level)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("SaveLevelTypesTran");

                try
                {
                    // Save restriction
                    var levelID = SupportLevelDataAccess.SaveSupportLevel(level, connection, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static void Update(SupportLevelSearchData level)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("UpdateLevelTypesTran");

                try
                {
                    // Save restriction
                    SupportLevelDataAccess.UpdateSupportLevel(level, connection, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static string Delete(int supportLevelID)
        {
            string mensaje = null;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("DeleteLevelTypesTran");

                try
                {
                    // Save restriction
                    mensaje = SupportLevelDataAccess.DeleteSupportLevel(supportLevelID, connection, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
            return mensaje;
        }

        public static Dictionary<string, string> GetLevels()
        {
            var List = SupportLevelDataAccess.Search().FindAll(x => x.Active == true && x.ParentSupportLevelID == 0).OrderBy(y => y.Name).ToList();

            Dictionary<string, string> listFin = new Dictionary<string, string>();

            listFin.Add("-1" , "---- Select ----");

            foreach (var item in List)
            {
                listFin.Add(item.SupportLevelID.ToString(), item.Name);
            }
            return listFin;
        }

        public static List<SupportLevelSearchData> GetItemsLevelMotives(string ID,string secc)
        {
            return SupportLevelDataAccess.GetItemsLevelMotives(ID,secc);
        }

        public static List<SupportLevelSearchData> ListarJerarquiaSupporLevel(int SupportLevelID)
        {
            return SupportLevelDataAccess.ListarJerarquiaSupporLevel(SupportLevelID);
        }
    }

    public class SupportLevelDataAccess
    {

        public static List<SupportLevelSearchData> Search()
        {
            List<SupportLevelSearchData> result = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("usp_levels_get_tree", null, "Core"))
                {

                    if (reader.HasRows)
                    {
                        result = new List<SupportLevelSearchData>();
                        while (reader.Read())
                        {
                            SupportLevelSearchData level = new SupportLevelSearchData();
                            level.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);
                            level.Name = Convert.ToString(reader["Name"]);
                            level.Description = Convert.ToString(reader["Description"]);
                            level.ParentSupportLevelID = Convert.ToInt32(reader["ParentSupportLevelID"]);
                            level.Active = Convert.ToBoolean(reader["Active"]);
                            level.IsVisibleToWorkStation = Convert.ToBoolean(reader["IsVisibleToWorkStation"]);
                            level.SortIndex = Convert.ToInt32(reader["SortIndex"]);
                            result.Add(level);
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

        public static List<SupportLevelSearchData> TraerJeraquiaSupportLevel( int SupportLevelID)
        {
            List<SupportLevelSearchData> result = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspTraerJerarquiaSupporLevel", new Dictionary<string, object>() { { "@SupportLevelID", SupportLevelID } }, "Core"))
                {
 
                    if (reader.HasRows)
                    {
                        result = new List<SupportLevelSearchData>();
                        while (reader.Read())
                        {
                            SupportLevelSearchData level = new SupportLevelSearchData();
                            level.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);
                            level.Name = Convert.ToString(reader["Name"]);
                            level.Description = Convert.ToString(reader["Description"]);
                            level.ParentSupportLevelID = Convert.ToInt32(reader["ParentSupportLevelID"]);
                            level.Active = Convert.ToBoolean(reader["Active"]);
                            level.IsVisibleToWorkStation = Convert.ToBoolean(reader["IsVisibleToWorkStation"]);
                            level.SortIndex = Convert.ToInt32(reader["SortIndex"]);
                            level.HasChild = Convert.ToBoolean(reader["HasChild"]);
                            
                            result.Add(level);
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


        public static int SaveSupportLevel(SupportLevelSearchData motive, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@Name", motive.Name },
                                                                                            { "@Description", motive.Description },
                                                                                            { "@IsVisibleToWorkStation", motive.IsVisibleToWorkStation},
                                                                                            { "@ParentSupportLevelID", motive.ParentSupportLevelID },
                                                                                            { "@Active", motive.Active},
                                                                                            { "@SortIndex", motive.SortIndex}       
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("upsInsSupportLevel", parameters, connection, transaction) as SqlCommand;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void UpdateSupportLevel(SupportLevelSearchData motive, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SupportLevelID", motive.SupportLevelID },
                                                                                            { "@Name", motive.Name },
                                                                                            { "@Description", motive.Description },
                                                                                            { "@IsVisibleToWorkStation", motive.IsVisibleToWorkStation},
                                                                                            { "@ParentSupportLevelID", motive.ParentSupportLevelID },
                                                                                            { "@Active", motive.Active},
                                                                                            { "@SortIndex", motive.SortIndex}       
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("upsUpdSupportLevel", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteScalar();





            }
            catch (Exception ex)
            {
                
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string DeleteSupportLevel(int supportLevelID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SupportLevelID",supportLevelID} };

                SqlCommand cmd = DataAccess.GetCommand("upsDelSupportLevel", parameters, connection, transaction) as SqlCommand;
                return cmd.ExecuteScalar().ToString();
                //return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<SupportLevelSearchData> GetItemsLevelMotives(string ID, string secc)
        {
            List<SupportLevelSearchData> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", ID }, { "@Seccion", secc } };

                using (SqlDataReader reader = DataAccess.GetDataReader("upsGetItemsLevelMotives", parameters, "Core"))
                {

                    if (reader.HasRows)
                    {
                        result = new List<SupportLevelSearchData>();
                        while (reader.Read())
                        {
                            SupportLevelSearchData level = new SupportLevelSearchData();
                            level.Description = Convert.ToString(reader["Cod"]);
                            level.Name = Convert.ToString(reader["Name"]);
                            result.Add(level);
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




        public static List<SupportLevelSearchData> ListarJerarquiaSupporLevel(int SupportLevelID)
        {
            SupportLevelSearchData SupportLevel = null;
            List<SupportLevelSearchData> result = new List<SupportLevelSearchData>();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("SpListarJerarquiaSupportLevel", new Dictionary<string, object>() { { "@SupportLevelID", SupportLevelID } }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            SupportLevel = new SupportLevelSearchData();
                            SupportLevel.ParentSupportLevelID = Convert.ToInt32(reader["ParentSupportLevelID"]);
                            SupportLevel.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);

                            result.Add(SupportLevel);
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
