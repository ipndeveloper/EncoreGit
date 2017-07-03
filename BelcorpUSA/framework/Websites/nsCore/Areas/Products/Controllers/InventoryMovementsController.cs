using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using System.Globalization;
using System.Collections.Generic;
using NetSteps.Data.Entities;

using NetSteps.Web.Mvc.Business.Controllers;
using System.Data.SqlTypes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Products.Controllers
{
    public class InventoryMovementsController : BaseProductsController
    {
        //
        // GET: /Products/InventoryMovements/
        [FunctionFilter("Products", "~/Accounts")]
        public ActionResult Index()
        {
            InventoryMovementsSearchParameters parameters = new InventoryMovementsSearchParameters();
            TempData["MovementTypes"] = InventoryMovements.DictionatySearchInventoryMovementTypes();

            return View(parameters);
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? warehouseId,
            int? productId,
            int? materialId,
            int? movementTypeId,
            DateTime? startDate,
            DateTime? endDate)
        {
            try
            {
                // Verify initial condition
                var warningMessage = "<tr><td colspan=\"12\">Please Complete Filter Information</td></tr>";
                var emptyResults = "<tr><td colspan=\"12\">There are no inventory movements</td></tr>";

                if (!warehouseId.HasValue)
                {
                    return Json(new { result = true, page = warningMessage });
                }
                else
                {
                    // Defaults if applicable
                    var startDateBL = startDate.HasValue ? startDate.Value : Convert.ToDateTime("01/01/2000", new CultureInfo("en-US"));
                    var endDateBL = endDate.HasValue ? endDate.Value : Convert.ToDateTime("12/31/2050", new CultureInfo("en-US"));

                    if (startDateBL != endDateBL && startDateBL > endDateBL)
                    {
                        warningMessage = "The end date (" + endDateBL.ToShortDateString() +
                                            ") may not be earlier than the start date (" + startDateBL.ToShortDateString() + ")";
                        return Json(new { result = false, message = warningMessage, page = emptyResults });
                    }

                    StringBuilder builder = new StringBuilder();
                    int count = 0;

                    var inventoryMovements = InventoryMovements.Search(new InventoryMovementsSearchParameters()
                    {
                        WarehouseID = warehouseId.Value,
                        ProductID = productId.HasValue ? productId.Value : 0,
                        MaterialID = materialId.HasValue ? materialId.Value : 0,
                        InventoryMovementTypeID = movementTypeId.HasValue ? movementTypeId.Value : 0,
                        StartDate = startDateBL,
                        EndDate = endDateBL,
                        PageIndex = page,
                        PageSize = pageSize,
                        OrderBy = orderBy,
                        OrderByDirection = orderByDirection
                    });

                    foreach (var inventoryMovement in inventoryMovements)
                    {
                        builder.Append("<tr>");

                        builder
                            .AppendLinkCell("~/Products/Materials/EditMaterial/" + inventoryMovement.MaterialID, inventoryMovement.MaterialSKU)
                            .AppendLinkCell("~/Products/Materials/EditMaterial/" + inventoryMovement.MaterialID, inventoryMovement.MaterialName)
                            .AppendCell(inventoryMovement.MovementDateUTC.ToString("g"))
                            .AppendCell(inventoryMovement.WarehouseName)
                            .AppendCell(inventoryMovement.InventoryMovementTypeName)
                            .AppendCell(inventoryMovement.QuantityOnHandBefore.ToString())
                            .AppendCell(inventoryMovement.QuantityMov.ToString())
                            .AppendCell(inventoryMovement.QuantityOnHandAfter.ToString())
                            .AppendCell(inventoryMovement.AverageCost.ToString("C",CoreContext.CurrentCultureInfo))
                            .AppendCell(inventoryMovement.OrderNumber)
                            .AppendCell(inventoryMovement.Description)
                            .AppendCell(inventoryMovement.UserName)
                            .Append("</tr>");
                        ++count;
                    }

                    return Json(new
                    {
                        result = true,
                        totalPages = inventoryMovements.TotalPages,
                        page = inventoryMovements.TotalCount == 0 ? emptyResults : builder.ToString()
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

       

        public virtual ActionResult ExportInventoryMovements(int? warehouseId, int? productId, int? materialId, string startDate, string endDate, int? movementTypeId)
        {
        
            try
            {
                DateTime startDateBL;
                if (!DateTime.TryParse(startDate, out startDateBL))
                    startDateBL =  Convert.ToDateTime("2001-01-01");// Convert.ToDateTime(SqlDateTime.MinValue);

                //var startDateBL = startDate.HasValue ? startDate.Value : Convert.ToDateTime("2001-01-01");
                //var endDateBL = endDate.HasValue ? endDate.Value : Convert.ToDateTime("2050-10-12");
                DateTime endDateBL;
                if (!DateTime.TryParse(endDate, out endDateBL))
                    endDateBL = Convert.ToDateTime("2050-10-12");// Convert.ToDateTime(SqlDateTime.MaxValue); 
                
               

                var lstResultado = InventoryMovements.Search(new InventoryMovementsSearchParameters()
                {
                    WarehouseID = warehouseId.Value,
                    ProductID = productId.HasValue ? productId.Value : 0,
                    MaterialID = materialId.HasValue ? materialId.Value : 0,
                    InventoryMovementTypeID = movementTypeId.HasValue ? movementTypeId.Value : 0,
                    StartDate = startDateBL,
                    EndDate = endDateBL
                });

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("Warehouse", "Ticket Search"));

                var columns = new Dictionary<string, string>
				{
                   
                    {"MaterialSKU", Translation.GetTerm("MaterialCode")} ,
                    {"MaterialName", Translation.GetTerm("MaterialName")} ,
                    {"MovementDateUTC", Translation.GetTerm("MovementDate")} ,
                    {"WarehouseName", Translation.GetTerm("Warehouse")} ,
                    {"InventoryMovementTypeName", Translation.GetTerm("MovementType")} ,
                    {"QuantityOnHandBefore", Translation.GetTerm("InitialOnHand")} ,
                    {"QuantityMov", Translation.GetTerm("QuantityMoved")} ,
                    {"QuantityOnHandAfter", Translation.GetTerm("FinalOnHand")} ,
                    {"AverageCost", Translation.GetTerm("AvgCost")} ,
                    {"OrderNumber", Translation.GetTerm("OrderNumber")} ,
                    {"Description", Translation.GetTerm("Description")}  ,
                    {"UserName", Translation.GetTerm("User")}  
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<WarehouseInventoryMovementsSearchData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }
    }
}
