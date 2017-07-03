using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Data;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class GenerateBatchBusinessLogic
    {
        public static List<int> Getperiods()
        {
            try
            {
                return GenerateBatchRepository.GetperiodsInicio();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<int> GetperiodsFin()
        {
            try
            {
                return GenerateBatchRepository.GetperiodsFin();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static Dictionary<int, string> ListShippingMethods()
        {
            try
            {
                return GenerateBatchRepository.ListShippingMethods();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Dictionary<int, string> ListAccountTypes()
        {
            try
            {
                return GenerateBatchRepository.ListAccountTypes();
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }

        public static Dictionary<int, string> ListWarehousePrinters(int WarehouseID)
        {
          try
            {
                return GenerateBatchRepository.ListWarehousePrinters(WarehouseID);
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }
        public static Dictionary<int, string> ListRouteXlogisticsProvider(string query, int LogisticProviderID)
        {
            try
            {
                return GenerateBatchRepository.ListRouteXlogisticsProvider(query, LogisticProviderID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        static GenerateBatchBusinessLogic instance;
        static IOrderToBatchRepositoty _IOrderToBatchRepositoty;
        public static GenerateBatchBusinessLogic Instance
            {   
               
                get
                {
                    if (instance == null)
                    {
                        instance = new GenerateBatchBusinessLogic();
                         _IOrderToBatchRepositoty = new GenerateBatchRepository();
                    }
                    return instance;
                }
        }

        public static PaginatedList<OrderToBatch> GetAllByFilters(GenerateBatchParameters searchParams)
        {
            _IOrderToBatchRepositoty = new GenerateBatchRepository();
            return _IOrderToBatchRepositoty.GetAllByFilters(searchParams);
        }

        //public static IEnumerable<OrderToBatch> GetAllByFilters(
        //       int WarehouseID, int AccountID,
        //       int MaterialID, int ProductID,
        //       DateTime? StartDate, DateTime? EndDate,
        //       int PeriodID, int PeriodID2,
        //       int ShippingMethodID, int AccountTypeID,
        //       int OrderTypeID, int WarehousePrinterID,
        //       string OrderNumber, int LogisticProviderID,
        //       int RouteID, bool Reprocess,
        //     DataTable dtOrderCustomerIDs)
        //{
        //    _IOrderToBatchRepositoty = new GenerateBatchRepository();
        //    List<OrderToBatchDto> lstOrderToBatchDto = _IOrderToBatchRepositoty.GetAllByFilters(  
        //        WarehouseID,   AccountID,
        //        MaterialID,   ProductID,
        //        StartDate,   EndDate,
        //        PeriodID,   PeriodID2,
        //        ShippingMethodID,   AccountTypeID,
        //        OrderTypeID,   WarehousePrinterID,
        //        OrderNumber,   LogisticProviderID,
        //        RouteID, Reprocess, dtOrderCustomerIDs);
        //  for (int i = 0; i < lstOrderToBatchDto.Count(); i++)
        //  {
        //      yield return  (OrderToBatch)lstOrderToBatchDto[i];
        //  }
          

        //}


        #region GetAllOrdersByFilters

        public static PaginatedList<OrderToBatch> GetAllOrdersByFilters(GenerateBatchParameters searchParams)
        {
            _IOrderToBatchRepositoty = new GenerateBatchRepository();
            return _IOrderToBatchRepositoty.GetAllOrdersByFilters(searchParams);
        }

        #endregion

        public static int InsOrderInvoicesOrderInvoiceItems(DataTable dtOrderCustomerIDs, int UserID, Boolean Reprocessed, out int OrderInvoiceIDIniPOut,  out int OrderInvoiceIDFinPOut)
        {

            try
            {
                return GenerateBatchRepository.InsOrderInvoicesOrderInvoiceItems(dtOrderCustomerIDs, UserID, Reprocessed, out OrderInvoiceIDIniPOut, out  OrderInvoiceIDFinPOut);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region InsertOrderSeparationLote


        public static int InsertOrderSeparationLote(DataTable dtOrderCustomerIDs)
        {

            try
            {
                return GenerateBatchRepository.InsertOrderSeparationLote(dtOrderCustomerIDs);
            }
            catch (Exception ex)
            {

                throw;
            }
        } 

        #endregion

        #region GetOrdersValuesForE010


        public static DataTable GetOrdersValuesForE010(int separationLoteId)
        {

            try
            {
                return GenerateBatchRepository.GetOrdersValuesForE010(separationLoteId);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

    public static Dictionary<int, string> ListMaterials(string SkuOrName)
    {
        try
        {
            return GenerateBatchRepository.ListMaterials(SkuOrName);

        }
        catch (Exception ex)
        {
            throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        }
    }
    public static Dictionary<int, string> ListProducts(string SkuOrId)
    {
        try
        {
            return GenerateBatchRepository.ListProducts(SkuOrId);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

            }
    }

