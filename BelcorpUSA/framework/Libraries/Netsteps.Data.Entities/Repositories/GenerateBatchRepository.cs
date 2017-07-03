using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Dto;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class GenerateBatchRepository : IOrderToBatchRepositoty
    {
        public static Dictionary<int, string> ShippingRulesLookUp(string input)
        {
            return LogisticShippingRulesDataAccess.ShippingRulesLookUp(input);
        }

        public static List<int> GetperiodsInicio()
        {
            List<int> lstPeriods = new List<int>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("UspSelPeriods", null, "Core");

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        lstPeriods.Add(Convert.ToInt32(reader["PeriodID"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstPeriods;
        }

        public static List<int> GetperiodsFin()
        {
            List<int> lstPeriods = new List<int>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("UspSelOrderPeriodsEnd", null, "Core");

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        lstPeriods.Add(Convert.ToInt32(reader["PeriodID"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstPeriods;
        }

        public static Dictionary<int, string> ListShippingMethods()
        {
            Dictionary<int, string> dcShippingMethods = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                SqlParameter[] lista = new SqlParameter[] { };
                connection.Open();
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[UspSelShippingMethods]", connection, lista);

                while (dataReader.Read())
                {
                    dcShippingMethods[Convert.ToInt32(dataReader["ShippingMethodID"])] = Convert.ToString(dataReader["Name"]);
                }
                return dcShippingMethods;
            }
        }

        public static Dictionary<int, string> ListAccountTypes()
        {
            SqlParameter[] lista = new SqlParameter[] { };
            Dictionary<int, string> dcAccountTypes = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {

                connection.Open();
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[UspSelAccountTypesAsc]", connection, lista);

                while (dataReader.Read())
                {
                    dcAccountTypes[Convert.ToInt32(dataReader["AccountTypeID"])] = Convert.ToString(dataReader["Name"]);
                }
                return dcAccountTypes;
            }
        }

        public static Dictionary<int, string> ListWarehousePrinters(int WarehouseID)
        {
            SqlParameter[] LstParameter = new SqlParameter[] { new SqlParameter() { SqlDbType = System.Data.SqlDbType.Int, Value = WarehouseID, ParameterName = "@WarehouseID" } };
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[uspGetWarehousePrinters]", connection, LstParameter);
                while (dataReader.Read())
                {
                    result[Convert.ToInt32(dataReader["WarehousePrinterID"])] = Convert.ToString(dataReader["Description"]);
                }
                return result;
            }
        }

        public static Dictionary<int, string> ListRouteXlogisticsProvider(string query, int LogisticProviderID)
        {
            Dictionary<int, string> dcRouteXlogisticsProvider = new Dictionary<int, string>();

            SqlParameter[] LstParameter = new SqlParameter[] 
                { 
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.VarChar,Value=query,ParameterName="@Name"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=LogisticProviderID,ParameterName="@LogisticProviderID"},
                
                };
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {

                connection.Open();
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[SpRoutesLogisticProvider]", connection, LstParameter);

                while (dataReader.Read())
                {
                    dcRouteXlogisticsProvider[Convert.ToInt32(dataReader["RouteID"])] = Convert.ToString(dataReader["Name"]);
                }
                return dcRouteXlogisticsProvider;
            }
        }

        public List<OrderToBatchDto> GetAll()
        {
            throw new NotImplementedException();
        }

        #region GetAllByFilters

        //public List<OrderToBatchDto> GetAllByFilters(
        //        int WarehouseID, int AccountID,
        //        int MaterialID, int ProductID,
        //        DateTime? StartDate, DateTime? EndDate,
        //        int PeriodID, int PeriodID2,
        //        int ShippingMethodID, int AccountTypeID,
        //        int OrderTypeID, int WarehousePrinterID,
        //        string OrderNumber, int LogisticProviderID,
        //        int RouteID, bool Reprocess,
        //        DataTable dtOrderCustomerIDs
        //     )
        public PaginatedList<OrderToBatch> GetAllByFilters(GenerateBatchParameters searchParams)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString);

            try
            {
                #region Parameters

                SqlParameter sqlpStartDate = null;
                SqlParameter sqlpEndDate = null;
                if (searchParams.StartDate == null)
                {
                    sqlpStartDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = DBNull.Value, ParameterName = "@StartDate" };
                }
                else
                {
                    sqlpStartDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = searchParams.StartDate, ParameterName = "@StartDate" };
                }
                if (searchParams.EndDate == null)
                {
                    sqlpEndDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = DBNull.Value, ParameterName = "@EndDate" };
                }
                else
                {
                    sqlpEndDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = searchParams.EndDate, ParameterName = "@EndDate" };
                }

                List<OrderToBatchDto> lstOrderToBatchDto = new List<OrderToBatchDto>();
                SqlParameter[] LstParameter = new SqlParameter[] 
                { 
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.WarehouseID,ParameterName="@WarehouseID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.AccountID,ParameterName="@AccountID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.MaterialID,ParameterName="@MaterialID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.ProductID,ParameterName="@ProductID"},
                    sqlpStartDate,
                    sqlpEndDate,
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.PeriodID,ParameterName="@PeriodID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.PeriodID2,ParameterName="@PeriodID2"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.ShippingMethodID,ParameterName="@ShippingMethodID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.AccountTypeID,ParameterName="@AccountTypeID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.OrderTypeID,ParameterName="@OrderTypeID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.WarehousePrinterID,ParameterName="@WarehousePrinterID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.VarChar,Value=searchParams.OrderNumber,ParameterName="@OrderNumber"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.LogisticProviderID,ParameterName="@LogisticProviderID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.RouteID,ParameterName="@RouteID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Bit,Value=searchParams.Reprocess,ParameterName="@Reprocess"} ,
               /*CS.19AG2016.Inicio.Nuevo Filtro*/
               new SqlParameter(){SqlDbType=System.Data.SqlDbType.TinyInt,Value=searchParams.ShowGenerated,ParameterName="@ShowGenerated"} ,
               /*CS.19AG2016.Fin.Nuevo Filtro*/
                   new SqlParameter(){SqlDbType=System.Data.SqlDbType.Structured,Value=searchParams.dtOrderCustomerIDs,ParameterName="@OrderCustomerIDs"} 
                };

                #endregion

                #region GetData

                connection.Open();
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[SpSearchOrdersToGenerateBilling]", connection, LstParameter);

                while (dataReader.Read())
                {
                    OrderToBatchDto objOrderToBatchDto = new OrderToBatchDto();

                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.OrderID.ToString()]))
                    {
                        objOrderToBatchDto.OrderID = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.OrderID.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Amount.ToString()]))
                    {
                        objOrderToBatchDto.Amount = Convert.ToDecimal(dataReader[CloumnOrderToBatchSearch.Amount.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.State.ToString()]))
                    {
                        objOrderToBatchDto.CityState = Convert.ToString(dataReader[CloumnOrderToBatchSearch.State.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.CompleteDate.ToString()]))
                    {
                        objOrderToBatchDto.CompleteDate = Convert.ToDateTime(dataReader[CloumnOrderToBatchSearch.CompleteDate.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Consultant.ToString()]))
                    {
                        objOrderToBatchDto.Consultant = Convert.ToString(dataReader[CloumnOrderToBatchSearch.Consultant.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Period.ToString()]))
                    {
                        objOrderToBatchDto.Period = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.Period.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Quantity.ToString()]))
                    {
                        objOrderToBatchDto.Quantity = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.Quantity.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Route.ToString()]))
                    {
                        objOrderToBatchDto.Route = Convert.ToString(dataReader[CloumnOrderToBatchSearch.Route.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.ShipmentMethod.ToString()]))
                    {
                        objOrderToBatchDto.ShipmentMethod = Convert.ToString(dataReader[CloumnOrderToBatchSearch.ShipmentMethod.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Transporter.ToString()]))
                    {
                        objOrderToBatchDto.Transporter = Convert.ToString(dataReader[CloumnOrderToBatchSearch.Transporter.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.OrderCustomerID.ToString()]))
                    {
                        objOrderToBatchDto.OrderCustomerID = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.OrderCustomerID.ToString()]);
                    }

                    /*CS.19AG2016.Inicio.Nueva columna de retorno*/
                    objOrderToBatchDto.BatchGenerated = Convert.ToBoolean(dataReader[CloumnOrderToBatchSearch.BatchGenerated.ToString()]);
                    /*CS.19AG2016.Fin.Nueva columna de retorno*/
                    lstOrderToBatchDto.Add(objOrderToBatchDto);
                }

                #endregion

                #region Convert to

                List<OrderToBatch> result = new List<OrderToBatch>();

                for (int i = 0; i < lstOrderToBatchDto.Count(); i++)
                {
                    result.Add((OrderToBatch)lstOrderToBatchDto[i]);
                }

                #endregion

                #region Pagination

                IQueryable<OrderToBatch> matchingItems = result.AsQueryable<OrderToBatch>();
                var resultTotalCount = matchingItems.Count();
                matchingItems = matchingItems.ApplyPagination(searchParams);
                return matchingItems.ToPaginatedList<OrderToBatch>(searchParams, resultTotalCount);

                #endregion

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region GetAllOrdersByFilters

        public PaginatedList<OrderToBatch> GetAllOrdersByFilters(GenerateBatchParameters searchParams)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString);

            try
            {
                #region Parameters

                SqlParameter sqlpStartDate = null;
                SqlParameter sqlpEndDate = null;
                if (searchParams.StartDate == null)
                {
                    sqlpStartDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = DBNull.Value, ParameterName = "@StartDate" };
                }
                else
                {
                    sqlpStartDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = searchParams.StartDate, ParameterName = "@StartDate" };
                }

                if (searchParams.EndDate == null)
                {
                    sqlpEndDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = DBNull.Value, ParameterName = "@EndDate" };
                }
                else
                {
                    sqlpEndDate = new SqlParameter() { SqlDbType = System.Data.SqlDbType.DateTime, Value = searchParams.EndDate, ParameterName = "@EndDate" };
                }

                List<OrderToBatchDto> lstOrderToBatchDto = new List<OrderToBatchDto>();

                //SqlParameter pTotalPages = new SqlParameter();
                //pTotalPages.ParameterName = "@TotalPages";
                //pTotalPages.DbType = DbType.Int32;
                //pTotalPages.Direction = ParameterDirection.Output;

                SqlParameter[] LstParameter = new SqlParameter[] 
                { 
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.WarehouseID,ParameterName="@WarehouseID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.AccountID,ParameterName="@AccountID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.MaterialID,ParameterName="@MaterialID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.ProductID,ParameterName="@ProductID"},
                    sqlpStartDate,
                    sqlpEndDate,
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.PeriodID,ParameterName="@PeriodID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.PeriodID2,ParameterName="@PeriodID2"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.ShippingMethodID,ParameterName="@ShippingMethodID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.AccountTypeID,ParameterName="@AccountTypeID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.OrderTypeID,ParameterName="@OrderTypeID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.WarehousePrinterID,ParameterName="@WarehousePrinterID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.VarChar,Value=searchParams.OrderNumber,ParameterName="@OrderNumber"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.LogisticProviderID,ParameterName="@LogisticProviderID"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=searchParams.RouteID,ParameterName="@RouteID"},
               
                   new SqlParameter(){SqlDbType=System.Data.SqlDbType.Structured,Value=searchParams.dtOrderCustomerIDs,ParameterName="@OrderCustomerIDs"},

                   //new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=page,ParameterName="@Page"},
                   //new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=pageSize,ParameterName="@PageSize"}
                   //pTotalPages
                };

                #endregion

                #region GetData

                //using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                //{

                connection.Open();
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[SpSearchOrdersToGenerateBatch]", connection, LstParameter);
                //TotalPages = 1; //test 
                while (dataReader.Read())
                {
                    OrderToBatchDto objOrderToBatchDto = new OrderToBatchDto();

                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.OrderID.ToString()]))
                    {
                        objOrderToBatchDto.OrderID = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.OrderID.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Amount.ToString()]))
                    {
                        objOrderToBatchDto.Amount = Convert.ToDecimal(dataReader[CloumnOrderToBatchSearch.Amount.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.State.ToString()]))
                    {
                        objOrderToBatchDto.CityState = Convert.ToString(dataReader[CloumnOrderToBatchSearch.State.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.CompleteDate.ToString()]))
                    {
                        objOrderToBatchDto.CompleteDate = Convert.ToDateTime(dataReader[CloumnOrderToBatchSearch.CompleteDate.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Consultant.ToString()]))
                    {
                        objOrderToBatchDto.Consultant = Convert.ToString(dataReader[CloumnOrderToBatchSearch.Consultant.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Period.ToString()]))
                    {
                        objOrderToBatchDto.Period = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.Period.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Quantity.ToString()]))
                    {
                        objOrderToBatchDto.Quantity = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.Quantity.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Route.ToString()]))
                    {
                        objOrderToBatchDto.Route = Convert.ToString(dataReader[CloumnOrderToBatchSearch.Route.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.ShipmentMethod.ToString()]))
                    {
                        objOrderToBatchDto.ShipmentMethod = Convert.ToString(dataReader[CloumnOrderToBatchSearch.ShipmentMethod.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.Transporter.ToString()]))
                    {
                        objOrderToBatchDto.Transporter = Convert.ToString(dataReader[CloumnOrderToBatchSearch.Transporter.ToString()]);
                    }
                    if (!Convert.IsDBNull(dataReader[CloumnOrderToBatchSearch.OrderCustomerID.ToString()]))
                    {
                        objOrderToBatchDto.OrderCustomerID = Convert.ToInt32(dataReader[CloumnOrderToBatchSearch.OrderCustomerID.ToString()]);
                    }

                    lstOrderToBatchDto.Add(objOrderToBatchDto);
                }

                #endregion

                #region Convert to

                List<OrderToBatch> result = new List<OrderToBatch>();

                for (int i = 0; i < lstOrderToBatchDto.Count(); i++)
                {
                    result.Add((OrderToBatch)lstOrderToBatchDto[i]);
                }

                #endregion

                #region Pagination

                IQueryable<OrderToBatch> matchingItems = result.AsQueryable<OrderToBatch>();
                var resultTotalCount = matchingItems.Count();
                matchingItems = matchingItems.ApplyPagination(searchParams);
                return matchingItems.ToPaginatedList<OrderToBatch>(searchParams, resultTotalCount);

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        /// <summary>
        /// listar los materiales
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ListMaterials(string SkuOrName)
        {
            Dictionary<int, string> dcMaterials = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {

                connection.Open();
                var parameters = new SqlParameter[] {
                                    new SqlParameter("@Query", SkuOrName ),
                                    
                };

                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[UspSelmaterials]", connection, parameters);

                while (dataReader.Read())
                {
                    dcMaterials[Convert.ToInt32(dataReader["MaterialID"])] = Convert.ToString(dataReader["Name"]);
                }
                return dcMaterials;
            }
        }

        /// <summary>
        /// listar los materiales
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ListProducts(string SkuOrId)
        {
            Dictionary<int, string> dcProducts = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {

                connection.Open();
                var parameters = new SqlParameter[] {
                                    new SqlParameter("@Query", SkuOrId ),
                                    
                };

                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[UspSelProduct]", connection, parameters);

                while (dataReader.Read())
                {
                    dcProducts[Convert.ToInt32(dataReader["ProductID"])] = Convert.ToString(dataReader["SKU"]);
                }
                return dcProducts;
            }
        }

        public static int InsOrderInvoicesOrderInvoiceItems(DataTable dtOrderCustomerIDs, int UserID, Boolean Reprocessed, out int OrderInvoiceIDIniPOut, out int OrderInvoiceIDFinPOut)
        {
            SqlTransaction otr = null;
            int resultado = 0;
            try
            {
                SqlParameter[] LstParameter = new SqlParameter[] 
                {
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Structured,Value=dtOrderCustomerIDs,ParameterName="@OrderCustomerIDs"},
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=UserID,ParameterName="@UserID"} ,
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Bit,Value=Reprocessed,ParameterName="@Reprocessed"} ,
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,ParameterName="@OrderInvoiceIDIniPOut",Direction=ParameterDirection.Output} ,
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,ParameterName="@OrderInvoiceIDFinPOut",Direction=ParameterDirection.Output} 
                    
                    
                };

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    otr = connection.BeginTransaction();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = connection;
                        ocom.Transaction = otr;
                        ocom.CommandText = "UspInsOrderInvoicesOrderInvoiceItems";
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.CommandTimeout = 0;
                        ocom.Parameters.AddRange(LstParameter);
                        resultado = (int)ocom.ExecuteScalar();

                        OrderInvoiceIDIniPOut = (int)ocom.Parameters["@OrderInvoiceIDIniPOut"].Value;
                        OrderInvoiceIDFinPOut = (int)ocom.Parameters["@OrderInvoiceIDFinPOut"].Value;

                        if (resultado == 0)
                        {
                            otr.Rollback();
                        }
                        else
                        {
                            otr.Commit();
                        }
                        return resultado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return resultado;
        }

        #region InsertOrderSeparationLote


        public static int InsertOrderSeparationLote(DataTable dtOrderCustomerIDs)
        {
            SqlTransaction otr = null;
            try
            {
                SqlParameter[] LstParameter = new SqlParameter[] 
                {
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.Structured,Value=dtOrderCustomerIDs,ParameterName="@OrderIDs"}
                    
                    
                };

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    otr = connection.BeginTransaction();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = connection;
                        ocom.Transaction = otr;
                        ocom.CommandText = "SPInsertOrderSeparationLote";
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.Parameters.AddRange(LstParameter);
                        int resultado = (int)ocom.ExecuteScalar();

                        if (resultado == 0)
                        {
                            otr.Rollback();
                        }
                        else
                        {
                            otr.Commit();
                        }
                        return resultado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        #endregion

        #region GetOrdersValuesForE010


        public static DataTable GetOrdersValuesForE010(int separationLoteId)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlParameter[] LstParameter = new SqlParameter[] 
                    {
                        new SqlParameter(){SqlDbType=System.Data.SqlDbType.Int,Value=separationLoteId,ParameterName="@SeparationLoteId"}
                    };
                using (SqlConnection ocon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    ocon.Open();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = ocon;
                        ocom.CommandTimeout = 3600;
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.CommandText = "SPgetOrdersValuesForE010";
                        ocom.Parameters.AddRange(LstParameter);

                        using (SqlDataAdapter odta = new SqlDataAdapter())
                        {
                            odta.SelectCommand = ocom;
                            odta.Fill(ds);
                            dt = ds.Tables[0];

                            ocon.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }


}
