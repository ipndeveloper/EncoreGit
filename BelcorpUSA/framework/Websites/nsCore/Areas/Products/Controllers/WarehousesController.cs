using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace nsCore.Areas.Products.Controllers
{
    public class WarehousesController : BaseProductsController
    {

        public string dataBase = "Core";       
         protected virtual List<ProductSlimSearchData> AllProducts
        {
            get { return Session["AllSlimProducts"] as List<ProductSlimSearchData>; }
            set { Session["AllSlimProducts"] = value; }
        } 

         private ProductBaseSearchParameters currentReport
         {
             get;
             set;
         }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index()
        {
            Dictionary<string, string> dcWarehouse = new Dictionary<string, string>();
            try
            {
                AllProducts = Product.LoadAllSlim(new NetSteps.Common.Base.FilterPaginatedListParameters<Product>()
                {
                    WhereClause = p => p.ProductBase.IsShippable && !(p.ProductBase.Products.Count > 1 && !p.IsVariantTemplate),
                    OrderBy = "SKU",
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    PageIndex = 0,
                    PageSize = null
                });
                TempData["InventoryMovementTypes"] = from x in InventoryMovementTypes.ListInventoryMovementTypesDictionary()
                                                     orderby x.Value
                                                     select new SelectListItem()
                                                     {
                                                         Text = x.Key,
                                                         Value = x.Value
                                                     };



                TempData["WareHouse"] = Warehouse.GetWareHouse(); 

                return View();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
            finally {
                dcWarehouse = null;
            }
        } 
        

        private StringBuilder GetWarehouseInventoryCells(Warehouse warehouse, WarehouseProduct wp, ProductSlimSearchData product)
        {
            bool productExists = wp != default(WarehouseProduct) && wp.IsAvailable;
            StringBuilder builder = new StringBuilder();
            builder.Append("<td class=\"warehouseProduct\"><div class=\"warehouse").Append(warehouse.WarehouseID)
                .Append("\"><input type=\"hidden\" class=\"changed\" value=\"false\" />")
                .Append("<input type=\"hidden\" class=\"warehouseProductId\" value=\"").Append(wp == null ? 0 : wp.WarehouseProductID).Append("\" />")
                .Append("<input type=\"hidden\" class=\"warehouseId\" value=\"").Append(warehouse.WarehouseID).Append("\" />")
                .Append("<input type=\"hidden\" class=\"productId\" value=\"").Append(product.ProductID).Append("\" />")
                .Append("<input type=\"checkbox\"").Append(productExists ? " checked=\"checked\"" : "").Append(" class=\"IsAvailable warehouseEnabler\" />")
                .Append("<input type=\"text\" class=\"QuantityOnHand numeric\" value=\"").Append(wp != null ? wp.QuantityOnHand : 0).Append("\"").Append(productExists ? "" : " style=\"color: #cfcfcf;\"").Append(productExists ? "" : " disabled=\"disabled\"").Append(" />")
                .Append("<input type=\"text\" class=\"QuantityBuffer numeric\" value=\"").Append(wp != null ? wp.QuantityBuffer : 0).Append("\"").Append(productExists ? "" : " style=\"color: #cfcfcf;\"").Append(productExists ? "" : " disabled=\"disabled\"").Append(" />")
                .Append("<input type=\"text\" class=\"ReorderLevel numeric\" value=\"").Append(wp != null ? wp.ReorderLevel : 0).Append("\"").Append(productExists ? "" : " style=\"color: #cfcfcf;\"").Append(productExists ? "" : " disabled=\"disabled\"").Append(" />")
                .Append("<span class=\"QuantityAllocated\">").Append(wp == null ? 0 : wp.QuantityAllocated).Append("</span>")
                .Append("<span ").Append("title=\"Not shipping\" style=\"display: ").Append(productExists ? "none" : "inline-block").Append(";\" class=\"icon-cancelled NoShip").Append(product.ProductID).Append("\" ></span>")
            
                .Append("</div></td>");
            return builder;
        }

      
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult SaveInventory(List<WarehouseProduct> wp)
        {
            try
            {
                if (wp != null)
                {
                    foreach (WarehouseProduct warehouseProduct in wp)
                    {
                        var product = new WarehouseProduct();
                        if (warehouseProduct.WarehouseProductID > 0)
                            product = WarehouseProduct.LoadFull(warehouseProduct.WarehouseProductID);
                        if (product == null)
                            product = new WarehouseProduct();
                        product.StartEntityTracking();
                        product.WarehouseID = warehouseProduct.WarehouseID;
                        product.ProductID = warehouseProduct.ProductID;
                        product.QuantityBuffer = warehouseProduct.QuantityBuffer;
                        product.QuantityOnHand = warehouseProduct.QuantityOnHand;
                        product.ReorderLevel = warehouseProduct.ReorderLevel;
                        product.IsAvailable = warehouseProduct.IsAvailable;
                        product.Save();
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
         
        #region Transfer
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Transfer()
        {
            return View();
        }

        public virtual ActionResult GetAmountAvailable(int productId, int fromWarehouseId, int? toWarehouseId)
        {
            try
            {
                var fromWarehouseProduct = WarehouseProduct.GetWarehouseProduct(fromWarehouseId, productId);
                var toWarehouseProduct = toWarehouseId.HasValue && toWarehouseId.Value > 0 ? WarehouseProduct.GetWarehouseProduct(toWarehouseId.Value, productId) : null;
                return Json(new
                {
                    result = true,
                    amountAvailableFrom = fromWarehouseProduct == null ? 0 : fromWarehouseProduct.QuantityOnHand - fromWarehouseProduct.QuantityAllocated,
                    amountAvailableTo = toWarehouseProduct == null ? 0 : toWarehouseProduct.QuantityOnHand - toWarehouseProduct.QuantityAllocated,
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult PerformTransfer(int productId, int fromWarehouse, int toWarehouse, int amount)
        {
            try
            {
                WarehouseProduct from = WarehouseProduct.GetWarehouseProduct(fromWarehouse, productId);
                from.StartEntityTracking();

                WarehouseProduct to = WarehouseProduct.GetWarehouseProduct(toWarehouse, productId);

                if (to == null)
                {
                    to = new WarehouseProduct()
                    {
                        WarehouseID = toWarehouse,
                        ProductID = productId,
                        QuantityBuffer = 0,
                        QuantityAllocated = 0,
                        IsAvailable = true
                    };
                }
                to.StartEntityTracking();

                if (fromWarehouse != toWarehouse)
                {
                    from.QuantityOnHand -= amount;
                    to.QuantityOnHand += amount;
                }

                from.Save();
                to.Save();

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region CSTI -FHP 
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(string warehouse, int? productoId, int? materialId, 
                                        int page, int pageSize,string orderBy, string orderByDirection)
        {
            try
            {
                
                if (string.IsNullOrEmpty(orderBy)){
                    orderBy = string.Empty;
                }

                if (orderByDirection == "Descending")
                {
                    orderByDirection = "desc";
                }
                else
                {
                    orderByDirection = "asc";
                }
                StringBuilder builder = new StringBuilder();
                int count = 0;
                var wareHouseMaterial = WareHouseMaterials.Search(new WareHouseMaterialSearchParameters()
                {
                    WareHouseID = Convert.ToInt32(warehouse),//csti-mescobar-EB-478-23/02/2016-inicio-fin
                    MaterialID = materialId,
                    ProductID = productoId,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Order = orderByDirection
                }); 
                foreach (var wareHouseMaterials in wareHouseMaterial)
                {
                    builder.Append("<tr>");
                    builder
                        //csti-mescobar-EB-478-23/02/2016-inicio
                       .AppendCell(wareHouseMaterials.Warehouse.ToString())
                       //.AppendCell(wareHouseMaterials.ProductCode.ToString())
                       .AppendCell(wareHouseMaterials.ProductCode)
                        //csti-mescobar-EB-478-23/02/2016-fin
                       .AppendLinkCell("~/Products/Materials/EditMaterial/" + wareHouseMaterials.MaterialID, wareHouseMaterials.SKU)
                       .AppendLinkCell("~/Products/Materials/EditMaterial/" + wareHouseMaterials.MaterialID, wareHouseMaterials.MaterialName)
                       .AppendCell(wareHouseMaterials.CostAvarage.ToString())
                       .AppendCell(wareHouseMaterials.QuantityOnHand.ToString())
                       .AppendCell(wareHouseMaterials.QuantityBuffer.ToString())
                       .AppendCell(wareHouseMaterials.ReorderLevel.ToString())
                       .AppendCell(wareHouseMaterials.QuantityAllocated.ToString());
                    builder.AppendLinkCell("javascript:void(0);", Translation.GetTerm("AddManualMovement", "Add Manual Movement"), linkCssClass: "btnViewStats", linkID: wareHouseMaterials.WarehouseMaterialID.ToString());
                    //csti-mescobar-EB-478-08/03/2016-inicio
                    builder.AppendLinkCell("javascript:void(0);", Translation.GetTerm("ViewReemplacements", "View Reemplacements"), linkCssClass: "btnViewReemplacements", linkID: wareHouseMaterials.ProductWarehouseMaterialID.ToString());
                    //csti-mescobar-EB-478-08/03/2016-fin
                    builder.Append("</tr>");
                    ++count;
                }
                return Json(new { result = true, totalPages = wareHouseMaterial.TotalPages, page = wareHouseMaterial.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no WareHouseInventory</td></tr>" : builder.ToString() });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ItemsWareHouseInvetoryExport(int wareHouse, int productCod, int materialCod)
        {
            try
            {
                //Validar los parametros
                WareHouseMaterialSearchParameters objP = new WareHouseMaterialSearchParameters();
                objP.PageIndex = 0;
                objP.PageSize = 300;
                objP.OrderBy = "SKU";
                objP.Order = "ASC";
                objP.WareHouseID = wareHouse;
                if (productCod ==0)
                {
                    objP.ProductID = null;
                }
                else
                {
                    objP.ProductID = productCod;              
                }
                if (materialCod == 0)
                {
                    objP.ProductID = null;
                }
                else
                {
                    objP.MaterialID = materialCod;
                }
                //Genera la lista que sera la data
                PaginatedList<WareHouseMaterialSearchData> objList = new PaginatedList<WareHouseMaterialSearchData>();
                objList = WareHouseMaterials.Search(objP, true);
                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("WareHouses", "Ware Houses"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                 
                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
                    {"Warehouse", Translation.GetTerm("Warehouse", "Warehouse")},
                    {"ProductCode", Translation.GetTerm("ProductCode", "Product Code")},

					{"SKU", Translation.GetTerm("MaterialCode", "Material Code")},
                    {"MaterialName", Translation.GetTerm("MaterialName","Material Name")},
					{"CostAvarage", Translation.GetTerm("AvgCost","Avg. Cost")},
					{"QuantityOnHand", Translation.GetTerm("OnHand","On Hand")},
					{"QuantityBuffer", Translation.GetTerm("Buffer", "Buffer")},
                    {"ReorderLevel", Translation.GetTerm("ReorderLevel", "Reorder Level")},
                    {"QuantityAllocated", Translation.GetTerm("Allocated","Allocated")},
					{"WarehouseMaterialID", Translation.GetTerm("WarehouseMaterialID", "WarehouseMaterialID")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<WareHouseMaterialSearchData>(fileNameSave, objList, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }


        public ActionResult SaveWareHouseMaterial(int InventoryMovementType, int WareHouseMaterialId, int QuantityField, string Description, int WareHouseId, int MaterialId)
        {
            string msg = "";
            bool Estado = true;
            //Paso3 : Seleccionar registro de la tabla WareHouseMaterial
            List<WareHouseMaterialSearchData> selectedWareHouseMaterial = WareHouseMaterials.ListWareHouseMaterialsByID(MaterialId, WareHouseId);
            //Paso4 : Seleccionar el registro de InventoryMovementTypes
            int resultInventoryMovementType = InventoryMovementTypes.ListInventoryMovementTypesByID(InventoryMovementType);
            //Paso4 : Validar si PositiveMovements = 0
            if (resultInventoryMovementType == 0)
            {
                foreach (var item in selectedWareHouseMaterial)
                {
                    if ((item.QuantityOnHand - QuantityField) >= 0)
                    {
                        int QuantityOnHand = 0;
                        decimal AverageCost = 0;
                        int QuantityBefore = 0;
                        if (resultInventoryMovementType == 0)
                        {
                            foreach (var m in selectedWareHouseMaterial)
                            {
                                QuantityOnHand = m.QuantityOnHand - QuantityField;
                                QuantityBefore = m.QuantityOnHand;
                            }
                        }
                        else //Actualiza
                        {
                            foreach (var w in selectedWareHouseMaterial)
                            {
                                QuantityOnHand = w.QuantityOnHand + QuantityField;
                                AverageCost = w.CostAvarage;
                                QuantityBefore = w.QuantityOnHand;
                            }
                        }
                        WareHouseMaterialSearchParameters parameter = new WareHouseMaterialSearchParameters();
                        parameter.QuantityBefore = QuantityBefore;
                        //{anteriormente estaba registrandose el "resultInventoryMovementType" en vez de "InventoryMovementType"
                        //lo cual es incorrecto ya que "resultInventoryMovementType" es el campo "PositiveMovement" de la tabla "InventoryMovementTypes"
                        //pero en el requerimiento hace referencia al campo "InventoryMovementTypeID" el cual si es correcto
                        // parameter.InventiryMovementTypeID = resultInventoryMovementType; Comentado por Sacledo Vila G. CIA:G&S}
                        parameter.InventiryMovementTypeID = InventoryMovementType;
                        parameter.QuantityField = QuantityField;
                        parameter.QuantityMov = QuantityField;
                        parameter.QuantityOnHandAfter = QuantityOnHand;
                        parameter.AverageCost = AverageCost;
                        parameter.Description = Description;
                        parameter.UserID = CoreContext.CurrentUser.UserID;
                        parameter.WareHouseMaterialID = WareHouseMaterialId;
                        WareHouseMaterials.InsWarehouseMaterialLogs(parameter);
                        Estado = true;
                        msg = "Manual Movement was applied";
                        return Json(new { result = true, menssage = msg, boolean = Estado });

                    }
                    else
                    {
                        msg = "QuantityAvailable on Inventory can not has negative value";
                        Estado = false; 
                        return Json(new { result = false, menssage = msg, boolean = Estado }); 
                    } 
                }
            }
            else
            {
                int QuantityOnHand = 0;
                decimal AverageCost = 0;
                int QuantityBefore = 0;
                if (resultInventoryMovementType == 0)
                {
                    foreach (var item in selectedWareHouseMaterial)
                    {
                        QuantityOnHand = item.QuantityOnHand - QuantityField;
                    } 
                }
                else //Actualiza
                {
                    foreach (var item in selectedWareHouseMaterial)
                    {
                        QuantityOnHand = item.QuantityOnHand + QuantityField;
                        AverageCost = item.CostAvarage;
                        QuantityBefore = item.QuantityOnHand;
                    }
                }
                WareHouseMaterialSearchParameters parameter = new WareHouseMaterialSearchParameters();
                parameter.QuantityBefore = QuantityBefore;
                //{anteriormente estaba registrandose el "resultInventoryMovementType" en vez de "InventoryMovementType"
                //lo cual es incorrecto ya que "resultInventoryMovementType" es el campo "PositiveMovement" de la tabla "InventoryMovementTypes"
                //pero en el requerimiento hace referencia al campo "InventoryMovementTypeID" el cual si es correcto
                // parameter.InventiryMovementTypeID = resultInventoryMovementType; Comentado por Sacledo Vila G. CIA:G&S}
                parameter.InventiryMovementTypeID = InventoryMovementType;
                parameter.QuantityField = QuantityField;
                parameter.QuantityMov = QuantityField;
                parameter.QuantityOnHandAfter = QuantityOnHand;
                parameter.AverageCost = AverageCost;
                parameter.Description = Description;
                parameter.UserID = CoreContext.CurrentUser.UserID;
                parameter.WareHouseMaterialID = WareHouseMaterialId;
                WareHouseMaterials.InsWarehouseMaterialLogs(parameter);
                Estado = true;
                msg = "Manual Movement was applied";
                return Json(new { result = true, menssage = msg, boolean = Estado });
                //Crea un registro

            }
            
            return Json(new { result = true, menssage = msg, boolean = Estado });
        }

        public ActionResult DetailWareHouseMaterial(int Code)
        {
            List<WareHouseMaterialDetails> Result = WareHouseMaterials.ListWareHouseMaterialByID(Code); 
            return Json(
                new
                {
                    success = true,
                    Result 
                },
                    JsonRequestBehavior.AllowGet
                );
        } 

        #endregion

        #region CSTI - MESCOBAR

        public virtual ActionResult ListWareHouseInventoryReplacement(string ParentProductID, string WarehouseMaterialID)
        {
            int warehouseMaterialID = Convert.ToInt32(WarehouseMaterialID);
            int parentProductID = Convert.ToInt32(ParentProductID);
           
            StringBuilder builder = new StringBuilder();
            int count = 0;

            try
            {
                var WareHouseInventoryReplacements = Warehouse.ListWareHouseInventoryReplacement(parentProductID, warehouseMaterialID);
                List<WareHouseMaterialSearchData> wareHouseMaterialReplacements = new List<WareHouseMaterialSearchData>();

                foreach (var replacement in WareHouseInventoryReplacements)
                {
                    builder.Append("<tr>");
                    builder
                        .AppendCell(replacement.SKU.ToString())
                        .AppendCell(replacement.MaterialName.ToString())
                        .AppendCell(replacement.CostAvarage.ToString())
                        .AppendCell(replacement.QuantityOnHand.ToString())
                        .AppendCell(replacement.ReorderLevel.ToString())
                        .AppendCell(replacement.QuantityAllocated.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, data = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return JsonError(exception.PublicMessage);
            }
        }

        #endregion
    }
}