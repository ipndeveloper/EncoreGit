using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Common.Expressions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace NetSteps.Data.Entities.Repositories
{

    // Creador de la Clase : IPN
    partial class MaterialRepository
    {


        //************************************************************************************************
        //**   Metodo : Paginación de la lado del Servidor
        //************************************************************************************************
        // Create by Ivan Pinedo Nava - IPN
        public PaginatedList<MaterialsSearchData> Search(MaterialsSearchParameters searchParams)
        {

            int Columna = 0;

            var results = new PaginatedList<MaterialsSearchData>(searchParams);
            var sort = searchParams.OrderByDirection;
        
            string orderBy = searchParams.OrderBy;
            string direction = searchParams.OrderByDirection ==0 ? "ASC" : "DESC";


            try
            {



                // Default sort
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = "MaterialID";
                }
                switch (orderBy)
                {
                    case "MaterialID":
                        Columna = 0;
                        break;
                    case "Name":
                        Columna = 1;
                        break;
                    case "SKU":
                        Columna = 2;
                        break;
                    case "EANCode":
                        Columna = 4;
                        break;
                    case "BPCSCode":
                        Columna = 5;
                        break;
                    case "UnityType":
                        Columna = 6;
                        break;
                    case "Weight":
                        Columna = 7;
                        break;
                    case "Volume":
                        Columna = 8;
                        break;
                    case "NCM":
                        Columna = 9;
                        break;
                    case "Origin":
                        Columna = 10;
                        break;
                    case "OriginCountry":
                        Columna = 11;
                        break;
                    case "Brand":
                        Columna = 12;
                        break;
                    case "Group":
                        Columna = 13;
                        break;
                    case "Active":
                        Columna = 14;
                        break;
                }
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    var parameters = new SqlParameter[] {
                                    new SqlParameter("@piMaterialID", searchParams.MaterialID ),
                                    new SqlParameter("@piActive", searchParams.Active ),
                                    new SqlParameter("@pivBPCSCode", searchParams.BPCSCode ),
                                    new SqlParameter("@pivBrand", searchParams.Brand ),
                                    new SqlParameter("@piOrdernarPor", Columna ),
                                    new SqlParameter("@pvOrderPorDireccion", direction ),
                                    new SqlParameter("@piiRegistroActual", searchParams.PageIndex ),
                                    new SqlParameter("@piiNumeroRegistros", searchParams.PageSize )
                };



                    SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[sCOR_ListMaterials]", connection, parameters);
                    results.TotalCount = 1000;

                    var TotalFilas = 0;
                    while (dataReader.Read())
                    {
                        TotalFilas = dataReader.GetInt32(0);

                        results.Add(new MaterialsSearchData()
                        {
                            MaterialID = dataReader.IsDBNull(2) ? (int)0 : dataReader.GetInt32(2),
                            Name = dataReader.IsDBNull(3) ? "" : dataReader.GetString(3),
                            SKU = dataReader.IsDBNull(4) ? "" : dataReader.GetString(4),
                            Active = dataReader.IsDBNull(5) ? false : dataReader.GetBoolean(5),
                            EANCode = dataReader.IsDBNull(6) ? (decimal)0 : dataReader.GetDecimal(6),
                            BPCSCode = dataReader.IsDBNull(7) ? "" : dataReader.GetString(7),
                            UnityType = dataReader.IsDBNull(8) ? "" : dataReader.GetString(8),
                            Weight = dataReader.IsDBNull(9) ? (decimal)0 : dataReader.GetDecimal(9),
                            Volume = dataReader.IsDBNull(10) ? (decimal)0 : dataReader.GetDecimal(10),
                            //NCM = dataReader.IsDBNull(11) ? "" : dataReader.GetString(11),
                            //Origin = dataReader.IsDBNull(12) ? (decimal)0 : dataReader.GetDecimal(12),
                            OriginCountry = dataReader.IsDBNull(11) ? (decimal)0 : dataReader.GetDecimal(11),
                            Brand = dataReader.IsDBNull(12) ? "" : dataReader.GetString(12),
                            Group = dataReader.IsDBNull(13) ? "" : dataReader.GetString(13),

                        });

                        //var t = dataReader["PeriodID"];

                        //Console.WriteLine(String.Format("{0} {1}", dataReader["CompanyName"], dataReader["ContactName"]));
                    }

                    results.TotalCount = TotalFilas;
                    dataReader.Close();
                }

                return results;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static System.Collections.Generic.Dictionary<int, string> SearchMaterialsByText(string text)
        {
            string firstValue = "";
            List<MaterialsSearchData> lenMaterial = new List<MaterialsSearchData>();
            try
            {
                firstValue = text;
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    var parameters = new SqlParameter[] {
                                    new SqlParameter("@firstValue", firstValue )
                                    
                };

                    SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[usp_FilterMaterials]", connection, parameters);

                    while (dataReader.Read())
                    {

                        lenMaterial.Add(new MaterialsSearchData()
                        {
                            MaterialID = dataReader.IsDBNull(0) ? (int)0 : dataReader.GetInt32(0),
                            Name = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1)

                        });

                    }


                    dataReader.Close();

                    var result = lenMaterial;
                    return result.ToDictionary(a => a.MaterialID, a => a.Name);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        private static string FormatForFullTextSearch(string fullTextParameter)
        {
            if (string.IsNullOrEmpty(fullTextParameter))
            {
                return null;
            }

            return string.Format("\"{0}*\"", fullTextParameter);
        }

        //************************************************************************************************
        //**   Metodo : Register Material
        //************************************************************************************************
        // Create by Ivan Pinedo Nava - IPN
        // Modified by Frank Segura Vilca - GYS

        public void MovementsMaterial(MaterialsSearchData oenMaterial)
        {
            var tbMaterial = new MaterialsSearchDataType();
            tbMaterial.Add(oenMaterial);

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[sCOR_RegisterMaterial]";
                SqlParameter param2 = cmd.Parameters.Add("@piTypeMaterial", SqlDbType.Structured);
                param2.Direction = ParameterDirection.Input;
                param2.TypeName = "uCOR_Material";
                param2.Value = tbMaterial.Count == 0 ? null : tbMaterial;
                cmd.ExecuteNonQuery();
            }
        }

        public static MaterialsSearchData MaterialData(int Id)
        {

            MaterialsSearchData obj_material = null;

            try
            {


                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    var parameters = new SqlParameter[] {
                                    new SqlParameter("@piMaterialID", Id ),
                                    
                };

                    SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[sCOR_DataMaterials]", connection, parameters);

                    while (dataReader.Read())
                    {
                        obj_material = new MaterialsSearchData();
                        obj_material.MaterialID = dataReader.IsDBNull(0) ? (int)0 : dataReader.GetInt32(0);
                        obj_material.Name = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1);
                        obj_material.SKU = dataReader.IsDBNull(2) ? "" : dataReader.GetString(2);
                        obj_material.Active = dataReader.IsDBNull(3) ? false : dataReader.GetBoolean(3);
                        obj_material.EANCode = dataReader.IsDBNull(4) ? (decimal)0 : dataReader.GetDecimal(4);
                        obj_material.BPCSCode = dataReader.IsDBNull(5) ? "" : dataReader.GetString(5);
                        obj_material.UnityType = dataReader.IsDBNull(6) ? "" : dataReader.GetString(6);
                        obj_material.Weight = dataReader.IsDBNull(7) ? (decimal)0 : dataReader.GetDecimal(7);
                        obj_material.Volume = dataReader.IsDBNull(8) ? (decimal)0 : dataReader.GetDecimal(8);
                        //obj_material.NCM = dataReader.IsDBNull(9) ? "" : dataReader.GetString(9);
                        //obj_material.Origin = dataReader.IsDBNull(10) ? (decimal)0 : dataReader.GetDecimal(10);
                        obj_material.OriginCountry = dataReader.IsDBNull(11) ? (decimal)0 : dataReader.GetDecimal(11);
                        //obj_material.Brand = dataReader.IsDBNull(12) ? "" : dataReader.GetString(12);
                        obj_material.BrandID = dataReader.IsDBNull(12) ? (int)0 : dataReader.GetInt32(12);
                        obj_material.Group = dataReader.IsDBNull(13) ? "" : dataReader.GetString(13);
                        //obj_material.MarketID = dataReader.IsDBNull(14) ? (int)0 : dataReader.GetInt32(14);
                    }
                    dataReader.Close();

                    return obj_material;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static void ChangeState(int id)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[sCORE_ChangeStateMaterial]";

                    SqlParameter param3 = cmd.Parameters.Add("@piIdMaterial", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Input;
                    param3.Value = id;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MaterialsSearchData ValidateEAN(MaterialsSearchData ObjMaterial)
        {
            MaterialsSearchData obj = null;
            var tbMaterial = new MaterialsSearchDataType();


            try
            {

                tbMaterial.Add(ObjMaterial);

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[sCOR_ValideEANCode]";
                    SqlParameter param2 = cmd.Parameters.Add("@piTypeMaterial", SqlDbType.Structured);
                    param2.Direction = ParameterDirection.Input;
                    param2.TypeName = "uCOR_Material";
                    param2.Value = tbMaterial.Count == 0 ? null : tbMaterial;
                    SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);


                    while (drd.Read())
                    {
                        obj = new MaterialsSearchData();
                        obj.MarketID = drd.IsDBNull(0) ? (int)0 : drd.GetInt32(0);
                    }

                    drd.Close();

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        public static MaterialsSearchData ValidateEAN2(MaterialsSearchData ObjMaterial)
        {
            MaterialsSearchData obj = null;
            var tbMaterial = new MaterialsSearchDataType();


            try
            {

                tbMaterial.Add(ObjMaterial);

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[sCOR_ValideEANCode2]";
                    SqlParameter param2 = cmd.Parameters.Add("@piTypeMaterial", SqlDbType.Structured);
                    param2.Direction = ParameterDirection.Input;
                    param2.TypeName = "uCOR_Material";
                    param2.Value = tbMaterial.Count == 0 ? null : tbMaterial;
                    SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);


                    while (drd.Read())
                    {
                        obj = new MaterialsSearchData();
                        obj.MarketID = drd.IsDBNull(0) ? (int)0 : drd.GetInt32(0);
                    }

                    drd.Close();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        public static string ActiveDeactive(List<int> ListMaterials, bool Active)
        {
            string result = string.Empty;
            try
            {
                DataTable dtItemList = new DataTable();
                dtItemList.Columns.Add("ItemIndex");
                dtItemList.Columns.Add("ItemID");

                for (int i = 0; i < ListMaterials.Count(); i++)
                {
                    dtItemList.Rows.Add(i + 1, ListMaterials[i]);
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[ActivateDeactivateMaterials]";
                    cmd.Parameters.AddWithValue("@ItemList", dtItemList).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Active", Active);
                    cmd.Parameters.Add("@UnableToUpdate", SqlDbType.VarChar, -1).Value = result;

                    cmd.Parameters["@UnableToUpdate"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["@UnableToUpdate"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        //Obtención de datos de Brand 
        //Desarrollado por Frank Segura. - GYS
        public static List<BrandSP> GetBrand(int languageID)
        {
            List<BrandSP> result = new List<BrandSP>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@languageID", languageID } };

                SqlDataReader reader = DataAccess.GetDataReader("uspGetBrands", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandSP BrandSP = new BrandSP();
                        BrandSP.BrandID = Convert.ToInt32(reader["BrandID"]);
                        BrandSP.Name = Convert.ToString(reader["ShortDescription"]);

                        result.Add(BrandSP);
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
