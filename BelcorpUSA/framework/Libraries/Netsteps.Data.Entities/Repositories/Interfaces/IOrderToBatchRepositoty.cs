using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Dto;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IOrderToBatchRepositoty
    {

        List<OrderToBatchDto> GetAll();
        //List<OrderToBatchDto> GetAllByFilters(
        //       int WarehouseID, int AccountID,
        //        int MaterialID, int ProductID,
        //        DateTime? StartDate, DateTime? EndDate,
        //        int PeriodID, int PeriodID2,
        //        int ShippingMethodID, int AccountTypeID,
        //        int OrderTypeID, int WarehousePrinterID,
        //        string OrderNumber, int LogisticProviderID,
        //        int RouteID, bool Reprocess,
        //        DataTable dtOrderCustomerIDs);
        PaginatedList<OrderToBatch> GetAllByFilters(GenerateBatchParameters searchParams);
        PaginatedList<OrderToBatch> GetAllOrdersByFilters(GenerateBatchParameters searchParams);

    }
}
