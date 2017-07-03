using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Common.Globalization;
using System.Text;
using NetSteps.Data.Entities.Repositories;

//@01 20150831 BR-IN-004 CSTI JMO: Added Excel Export

namespace nsCore.Areas.Products.Controllers
{
    public class InventoryAllocatedController : BaseController
    {
        //KLC - CSTI - BR-IN-004
        // GET: /Products/InventoryAllocated/

        public ActionResult Index()
        {
            InventoryAllocatedParameters parameters = new InventoryAllocatedParameters();


            return View(parameters);
           
        }

        public ActionResult InventoryAllocatedBrowse() {

            InventoryAllocatedParameters parameters = new InventoryAllocatedParameters();
            

            return View(parameters);
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? warehouseId,
            int? productId,
            int? materialId,
            DateTime? startDate,
            DateTime? endDate)
        {
            try
            {
                //Verify initial condition
                var warningMessage = "<tr><td colspan=\"12\">Please Complete Filter Information</td></tr>";
                var emptyResults = "<tr><td colspan=\"12\">There are no inventory movements</td></tr>";

                if (!warehouseId.HasValue)
                {
                    return Json(new { result = true, page = warningMessage });
                }
                else
                {
                    //    // Defaults if applicable
                    var startDateBL = startDate.HasValue ? startDate.Value : Convert.ToDateTime("2015-10-12");
                    var endDateBL = endDate.HasValue ? endDate.Value : Convert.ToDateTime("2015-10-12");

                    if (startDateBL != endDateBL && startDateBL > endDateBL)
                    {
                        warningMessage = "The end date (" + endDateBL.ToShortDateString() +
                                            ") may not be earlier than the start date (" + startDateBL.ToShortDateString() + ")";
                        return Json(new { result = false, message = warningMessage, page = emptyResults });
                    }

                    StringBuilder builder = new StringBuilder();
                    int count = 0;

                    var inventoryMovements = InventoryAllocated.Search(new InventoryAllocatedParameters()
                    {
                        WarehouseID = warehouseId.Value,
                        ProductID = productId.HasValue ? productId.Value : 0,
                        MaterialID = materialId.HasValue ? materialId.Value : 0,
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
                            .AppendCell(inventoryMovement.WarehouseName)
                            /*01 D01
                            .AppendCell(inventoryMovement.ProductSKU.ToString())
                            .AppendCell(inventoryMovement.ProductName.ToString())
                            */
                            .AppendCell(inventoryMovement.MaterialSKU.ToString())
                            .AppendCell(inventoryMovement.MaterialName.ToString())                            
                            .AppendCell(inventoryMovement.Quantity.ToString())
                            .AppendCell(inventoryMovement.PreOrderID.ToString())
                            .AppendCell(inventoryMovement.AllocationDateUTC.ToString("g"))
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

        #region [01 A1]
        /// <summary>
        ///  Excel Export
        /// </summary>
        /// <returns>Excel File</returns>
        public virtual ActionResult ExportInventoryAllocated(int? warehouseId, int? productId, int? materialId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var startDateBL = startDate.HasValue ? startDate.Value : Convert.ToDateTime("2015-10-12");
                var endDateBL = endDate.HasValue ? endDate.Value : Convert.ToDateTime("2015-10-12");

                var lstResultado = InventoryAllocated.Search(new InventoryAllocatedParameters()
                {
                    WarehouseID = warehouseId.Value,
                    ProductID = productId.HasValue ? productId.Value : 0,
                    MaterialID = materialId.HasValue ? materialId.Value : 0,
                    StartDate = startDateBL,
                    EndDate = endDateBL
                });

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("Warehouse", "Ticket Search"));

                var columns = new Dictionary<string, string>
				{
                    {"WarehouseName", Translation.GetTerm("Warehouse")} ,
                    {"MaterialSKU", Translation.GetTerm("MaterialCode")} ,
                    {"MaterialName", Translation.GetTerm("MaterialName")} ,
                    {"Quantity", Translation.GetTerm("Quantity")} ,
                    {"PreOrderID", Translation.GetTerm("Order")} ,
                    {"AllocationDateUTC", Translation.GetTerm("Date")}  
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<WarehouseInventoryAllocatedSearchData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }
        #endregion 

    }
}
