using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Products.Common.Models;
using NetSteps.Web.Mvc.Controls.Attributes;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using nsDistributor.Areas.Enroll.Models;
using nsDistributor.Areas.Enroll.Models.Products;
using nsDistributor.Models.Shared;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;
using OrderRules.Service.Interface;
using OrderRules.Service.DTO;
using OrderRules.Service.DTO.Converters;
using OrderRules.Core.Model;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Common.Context;
using nsDistributor.Models.Cart;
using NetSteps.Data.Common.Services;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.Service;
using NetSteps.OrderAdjustments.Common.Model;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Business;
using NetSteps.Promotions.UI.Common.Interfaces;
using System.Text;
using NetSteps.Common.Configuration;
using System.Web;
using nsDistributor.Areas.Enroll.Models.Shared;
using NetSteps.Web.Mvc.Models;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes;
using NetSteps.Commissions.Common.Models;
using NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Events.Common.Services;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Commissions.Service;
using System.Data;
using Microsoft.Reporting.WebForms;
using iTextSharp.text.pdf;
using System.IO;
using CodeBarGeerator;
using System.Globalization;
using System.Data.SqlClient;
using Fasterflect;
using nsDistributor.Models.Paypal;
using NetSteps.Data.Entities.Business.Common;
using NetSteps.Data.Common.Entities;
using IProduct = NetSteps.Products.Common.Models.IProduct;


namespace nsDistributor.Areas.Enroll.Controllers
{
    public class ProductsController : EnrollStepBaseController
    {
        public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

        #region Actions
        [EnrollmentStepSection]
        public virtual ActionResult Index()
        {
            return RedirectToAction(GetSections().First().Action);
        }

        [EnrollmentStepSection]
        public virtual ActionResult EnrollmentKits()
        {
            return SectionsView();
        }

        [EnrollmentStepSection]
        public virtual ActionResult EnrollmentVariantKits()
        {
            Account account = null;
            account = Account.LoadForSession((int)Session["AccountId"]);
            //Session["AccountId"] = CoreContext.CurrentAccount.AccountID;
            CoreContext.CurrentAccount = account;
            //var account = GetEnrollingAccount();
            var initialOrder = GetInitialOrder();

            //var shi = GetNewShippingMethods(account.Addresses[0].AddressID); 
            if (account != null)
            {
                var shi = OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(account.Addresses[0].AddressID);

                foreach (var item in shi)
                {
                    Session["DefauldShippingMethodId"] = item.ShippingMethodID2;
                }
            }
            return SectionsView();
        }

        [HttpGet]
        public virtual ActionResult ProductDetail(int productID)
        {
            using (var productDetailTrace = this.TraceActivity(string.Format("showing ProductDetail for {0}", productID)))
            {
                try
                {
                    var product = ProductCache.GetProductById(productID);
                    var descript = product != null ? product.Translations.GetByLanguageIdOrDefaultForDisplay().LongDescription : String.Empty;
                    if (String.IsNullOrWhiteSpace(descript) && product.IsVariant())
                    {
                        var parentProduct = ProductCache.GetProductById(product.ProductBase.Products.First(p => p.IsVariantTemplate).ProductID);
                        descript = parentProduct != null ? (parentProduct.Translations.GetByLanguageIdOrDefaultForDisplay().LongDescription ?? String.Empty) : String.Empty;
                    }

                    var productDetail = new
                    {
                        name = product != null ? product.Translations.GetByLanguageIdOrDefaultForDisplay().Name : String.Empty,
                        description = descript,
                        price = product != null ? product.GetPrice(_enrollmentContext.AccountTypeID, _enrollmentContext.CurrencyID).ToString() : String.Empty
                    };

                    return new JsonResult
                    {
                        Data = new { success = true, name = productDetail.name, description = productDetail.description, productDetail.price }
                        ,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                catch (Exception excp)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(excp, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return JsonError(exception.PublicMessage);
                }
            }
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult EnrollmentVariantKits(EnrollmentVariantKitsModel model)
        {
            //var account = GetEnrollingAccount();
            var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
            var initialOrder = GetInitialOrder(false);

            if (!ModelState.IsValid)
            {
                EnrollmentVariantKits_LoadResources(model, initialOrder);
                return SectionsView(model);
            }

            try
            {
                //Crear una PreOrden
                int siteID = OrderContext.Order.AsOrder().SiteID.Value; //Por Validar
                PreOrder modelPreOrder = new PreOrder();
                modelPreOrder.AccountID = account.AccountID;
                modelPreOrder.SiteID = siteID;
                int preOrderId = 0;
                int addressID = account.Addresses[0].AddressID;
                int wareHouseId = WarehouseExtensions.WareHouseByAddresID(account.AccountID);
                

                preOrderId = PreOrderExtension.CreatePreOrder(modelPreOrder);
                Session["PreOrder"] = preOrderId; 
                Session["WareHouseId"] = wareHouseId;
                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;                
                var queryAllocations = Order.GenerateAllocation(model.SelectedProductID.Value, 1,
                   OrderContext.Order.AsOrder().OrderID, wareHouseId, EntitiesEnums.MaintenanceMode.Add, preOrderId,
                   CoreContext.CurrentAccount.AccountTypeID, false);
                 

                addToCartKitStart(model.SelectedProductID.Value, 1);

                // Apply updates
                initialOrder.WareHouseID = wareHouseId;
                initialOrder.PreorderID = preOrderId;

                EnrollmentVariantKits_UpdateInitialOrder(model, initialOrder);
                UpdateOrderShipmentAddress(initialOrder, account);
                if (_enrollmentContext.EnrollmentConfig.EnrollmentOrder.ImportShoppingOrder)
                {
                    ImportShoppingOrder(initialOrder);
                }
                OrderContext.Order = initialOrder; 


                OrderContext.Order.ParentOrderID = wareHouseId;
                OrderService.UpdateOrder(OrderContext);
                OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID = Convert.ToInt32(Session["DefauldShippingMethodId"]);
                int ShippingMethodID = OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID.Value;

                initialOrder.Save();

                foreach (var item in OrderContext.Order.AsOrder().OrderCustomers)
                {
                    foreach (var orderItem in item.OrderItems)
                    {
                        int resultOrderItem = Order.UPDOrderItemProduct(orderItem.OrderItemID, OrderContext.Order.AsOrder().OrderID);
                    }
                }
                foreach (var item in OrderContext.Order.AsOrder().OrderShipments)
                {
                    int resultOrderShipments = Order.UPDOrderShipments(item.OrderShipmentID, item.PostalCode, Convert.ToInt32(Session["WareHouseId"]), item.OrderID, Convert.ToString(Session["DateEstimated"]));
                }

                _enrollmentContext.InitialOrder = initialOrder;
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                EnrollmentVariantKits_LoadResources(model, initialOrder);
                return SectionsView(model);
            }

            return SectionCompleted();
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult EnrollmentKits(EnrollmentKitsModel model)
        {
            var account = GetEnrollingAccount();
            var initialOrder = GetInitialOrder();

            if (!ModelState.IsValid)
            {
                EnrollmentKits_LoadResources(model, initialOrder);
                return SectionsView(model);
            }

            try
            {
                // Apply updates
                EnrollmentKits_UpdateInitialOrder(model, initialOrder);
                UpdateOrderShipmentAddress(initialOrder, account);
                OrderService.UpdateOrder(OrderContext);
                initialOrder.Save();

                _enrollmentContext.InitialOrder = initialOrder;
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                EnrollmentKits_LoadResources(model, initialOrder);
                return SectionsView(model);
            }

            return SectionCompleted();
        }

        [EnrollmentStepSection]
        public virtual ActionResult EnrollmentItems()
        {
            NetSteps.Data.Entities.Business.CTE objN = new NetSteps.Data.Entities.Business.CTE();

            if (OrderContext.Order == null || !OrderContext.Order.OrderCustomers.Any())
            {
                OrderContext.Clear();

            }
            var order = OrderContext.Order.AsOrder().OrderCustomers;
            Account account = null;
            account = Account.LoadForSession(order[0].AccountID);
            //CoreContext.CurrentAccount = Account.LoadFull(accountId.Value);
            CoreContext.CurrentAccount = account.Clone();
            CoreContext.CurrentAccount.StartEntityTracking();
            ValidatePaymentByMarket();
            string OrderNumber = PreOrderExtension.GetOrderNumberByAccount(account.AccountID, 11, 4);
           
            if (!OrderNumber.IsNullOrEmpty() || OrderContext.Order == null)
            {
                AccountPropertiesBusinessLogic.GetValueByID(NetSteps.Data.Entities.Constants.OrderPayments.Delete.ToInt(), OrderNumber.ToInt());
                OrderContext.Order = Order.LoadByOrderNumberFull(OrderNumber);
            }
            //Si no exite una Pre Orden
            if (!OrderExtensions.ExistPreOrder(OrderContext.Order.AsOrder().OrderID))
            {
                //Crear
                //Crea la Pre Orden
                int addressID = account.Addresses[0].AddressID;
                int siteID = OrderContext.Order.AsOrder().SiteID.Value; //Por Validar
                int wareHouseId = WarehouseExtensions.WareHouseByAddresID(account.AccountID);
                Session["WareHouseId"] = wareHouseId;

                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                int preOrderId=PreOrderExtension.InsPreOrdersDistributor(account.AccountID, siteID, OrderContext.Order.AsOrder().OrderID);

                Session["PreOrder"] = preOrderId;
                OrderContext.Order.PreorderID = preOrderId;
                OrderContext.Order.WareHouseID = wareHouseId;

                //Inserta a invetario los items
                foreach (var Oitem in order[0].OrderItems.Where(n => n.ParentOrderItem == null))
                {
                    foreach (var item in Order.GenerateAllocation(Oitem.ProductID.Value,
                                                                  Oitem.Quantity,
                                                                  order[0].OrderID,
                                                                  Convert.ToInt32(Session["WareHouseId"]),
                                                                  EntitiesEnums.MaintenanceMode.Add,
                                                                  Convert.ToInt32(Session["PreOrder"]),                                                                  
                                                                  CoreContext.CurrentAccount.AccountTypeID, false))
                    {
                        if (!item.Estado)
                        {
                            return Json(new { result = false, restricted = true, message = item.Message });
                        }
                        else if (item.EstatusNewQuantity)
                        {
                            //quantity = item.NewQuantity;
                        }
                        else if (item.Estado && item.Message != "" && item.NewQuantity > 0)
                        {
                            //quantity = item.NewQuantity;
                        }
                    }
                }

            }
            else
            {
                //Recuperar la PreOrden
                int wareHouseId = WarehouseExtensions.WareHouseByAddresID(account.AccountID);
                //Elimar los WareHouseMaterialAllocations
                foreach (var objWMA in Order.GetMaterialWareHouseMaterialPWS(OrderContext.Order.AsOrder().OrderID))
                {
                    bool Exist = false;
                    //Si los campos son iguales en OrderItems
                    foreach (var objOI in Order.GetMaterialOrderItem(OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID).Where(x => x.MaterialID == objWMA.MaterialID))
                    {
                        Exist = true;
                    }
                    if (!Exist)
                    {
                        //Borrar y actualizar el stock 
                        PreOrderExtension.UpdWarehouseMaterial(objWMA.MaterialID, objWMA.Quantity, objWMA.WareHouseID);
                        //Elimino del HouseMaterialsAllocations
                        PreOrderExtension.DelWareHouseMaterialsAllocationsXPreOrder(objWMA.ProductID, Convert.ToInt32(Session["PreOrder"]));
                    }
                }
                Session["WareHouseId"] = wareHouseId;
                var preOrderId = Order.getPreOrder(account.AccountID, 4, OrderContext.Order.AsOrder().OrderID);
                Session["PreOrder"] = preOrderId;
                OrderContext.Order.PreorderID = preOrderId;
                OrderContext.Order.WareHouseID = wareHouseId;
            }

            var CartModel = GetCartModelData(OrderContext.Order.AsOrder()); 
            ViewBag.CartModel = CartModel;
            validValueQVUSA(CartModel.OrderItems[0].TotalQV); 
            if (OrderContext.Order.OrderID == 0)
            {
                //Crea la Pre Orden
                int addressID = account.Addresses[0].AddressID;
                int siteID = OrderContext.Order.AsOrder().SiteID.Value; //Por Validar
                int wareHouseId = WarehouseExtensions.WareHouseByAddresID(account.AccountID);
                Session["WareHouseId"] = wareHouseId;
            }
            //Inserto el Kit a WareHouseMaterialAllocations 
            OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipmentNoDefault();
            var shippingOrderTypeID = WarehouseExtensions.GetShippingOrderTypeID(OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID);

            Session["ShippingOrderTypeID"] = WarehouseExtensions.GetShippingOrderTypeID(OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID);

            Address defaultShippingAddress = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);

            shipment.ModifiedByUserID = Convert.ToInt32(Session["ShippingOrderTypeID"]);


            var shipping = GetNewShippingMethods(defaultShippingAddress.AddressID);
            ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();

            var shippingMethodWithRates = ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>;
            Session["ExisShippingMetods"] = true;
            if (shippingMethodWithRates.Count() == 0)
            {
                Session["ExisShippingMetods"] = false;
            }

            TempData["sPaymentMethod"] = from x in Order.SelectPaymentMethod(account.AccountID, OrderContext.Order.AsOrder().OrderTypeID)
                                         orderby x.Key
                                         select new SelectListItem()
                                         {
                                             Text = x.Value,
                                             Value = x.Key
                                         };

            StringBuilder builder = new StringBuilder();
            var inventory = Create.New<InventoryBaseRepository>();

            var activeCategories = inventory.GetActiveCategories(ApplicationContext.Instance.StoreFrontID, Account.AccountTypeID);

            var trees = GetCatalogIDs(activeCategories, builder);
            ViewBag.ActiveCategories = activeCategories;
            ViewBag.Categories = builder.ToString();
            ApplyPaymentPreviosBalance();//KTorres 27JUL   Ini
            actBalanceDue();
            WebsiteModel model = new WebsiteModel(); 
            return SectionsView();
        }

        public void validValueQVUSA(decimal totalQV)
        {
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == (int)Constants.Country.UnitedStates)
            { 
                bool validTotalQv = true;
                var valueDecimal = PreOrderExtension.GetValueURL();
                decimal valueTotalQV = valueDecimal[0];
                if (totalQV < valueTotalQV)
                {
                    validTotalQv = false;
                }
                ViewBag.DomainUrl = GetPWSDomain();
                ViewBag.TotalQV = valueTotalQV;
                ViewBag.validTotalQv = validTotalQv;
            }
        }
        

        private void actBalanceDue()
        {
            

            Order order = OrderContext.Order.AsOrder();

           
            var balanceAmount = Totals.GetType().GetProperty("balanceAmount").GetValue(Totals, null);
            //string[] separatorBalanceAmount = Convert.ToString(balanceAmount).Split('$');
            //decimal val_payments = 0;
            //val_payments = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(Convert.ToDecimal(separatorBalanceAmount[0]));
            decimal val_payments = 0;
            val_payments = Convert.ToDecimal(balanceAmount, CoreContext.CurrentCultureInfo);
            OrderContext.Order.AsOrder().Balance = val_payments;
        }
        protected virtual List<int> GetCatalogIDs(List<Category> activeCategories, StringBuilder builder)
        {
            var inventory = Create.New<InventoryBaseRepository>();

            var trees = new List<int>();
            foreach (Catalog catalog in inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
                                                                    .Where(c => c.CatalogTypeID == (short)NetSteps.Data.Entities.Constants.CatalogType.Normal))
            {
                if (!trees.Contains(catalog.CategoryID))
                {
                    Category root = inventory.GetCategoryTree(catalog.CategoryID);
                    SetActiveCategories(root, activeCategories);
                    builder.Append(BuildCategories(root));
                    trees.Add(catalog.CategoryID);
                }
            }
            return trees;
        }


        protected virtual string BuildCategories(Category parent)
        {
            if (parent.ChildCategories != null && parent.ChildCategories.Count(c => c.Display) > 0)
            {
                TagBuilder childBuilder = new TagBuilder("ul");

                StringBuilder builder = new StringBuilder();
                foreach (var category in parent.ChildCategories.OrderBy(c => c.SortIndex).Where(c => c.Display))
                {
                    TagBuilder liBuilder = new TagBuilder("li");
                    liBuilder.AddCssClass("category");

                    TagBuilder aBuilder = new TagBuilder("a");
                    bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
                    var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
                    var url = string.Format("~{0}/Shop/Category/{1}", isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1)), category.CategoryID).ResolveUrl();
                    aBuilder.MergeAttribute("href", url);
                    if (Request.Url.LocalPath == url)
                        aBuilder.AddCssClass("selected");
                    aBuilder.SetInnerText(category.Translations.GetByLanguageIdOrDefaultForDisplay().Name);

                    liBuilder.InnerHtml = aBuilder.ToString() + BuildCategories(category);

                    builder.Append(liBuilder.ToString());
                }

                childBuilder.InnerHtml = builder.ToString();

                return childBuilder.ToString();
            }

            return string.Empty;
        }

        protected virtual void SetActiveCategories(Category parent, List<Category> activeCategories)
        {
            if (activeCategories.Any(c => c.CategoryID == parent.CategoryID))
                parent.Display = true;

            if (parent.ChildCategories != null && parent.ChildCategories.Count > 0)
            {
                foreach (var child in parent.ChildCategories)
                {
                    SetActiveCategories(child, activeCategories);
                    if (child.Display)
                        parent.Display = true;
                }
            }
        }

        protected virtual IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
        {
            var shippingMethods = OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(addressId);
            foreach (var item in shippingMethods)
            {
                Session["DateEstimated"] = item.DateEstimated;
                Session["DefauldShippingMethodId"] = item.ShippingMethodID2;
            }
            OrderService.UpdateOrder(OrderContext);

            return shippingMethods;
        }

        protected virtual IList<IPromotionInfoModel> GetApplicablePromotions(Order order)
        {
            var promotionAdjustments = order.OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
            var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
            return adjustments.Select(adj =>
            {
                bool isAvailable = false;
                if (adj.InjectedOrderSteps.Count() > 0)
                {
                    foreach (var step in adj.InjectedOrderSteps)
                    {
                        isAvailable = StepHasAnItemInStockToBeChosen(step.OrderStepReferenceID);
                        if (isAvailable)
                            break;
                    }
                }
                else
                {
                    isAvailable = !adj.OrderModifications.Any(om => om.ModificationDescription.Contains("Unable"));
                }
                var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
                    (os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
                var promoInfo = Create.New<IPromotionInfoModel>();
                promoInfo.Description = adj.Description;
                promoInfo.StepID = giftStep == null ? null : giftStep.OrderStepReferenceID;
                promoInfo.Available = isAvailable;
                return promoInfo;
            }).ToList();
        }


        public virtual ICartModel GetCartModelData(Order order)
        {
            var customer = order.OrderCustomers[0];
            var model = Create.New<ICartModel>();

            model.ApplicablePromotions = GetApplicablePromotions(order);
            model.OrderItems = GetOrderItemModels(order);
            model.PromotionallyAddedItems = GetPromotionallyAddedItemModels(order);
            model.Subtotal = customer.Subtotal ?? 0;
            model.AdjustedSubtotal = customer.AdjustedSubTotal;
            model.Tax = customer.TaxAmountTotal;
            model.ShippingHandling = customer.ShippingTotal + customer.HandlingTotal;
            model.AdjustedShippingHandling = customer.AdjustedShippingTotal + customer.AdjustedHandlingTotal;
            model.GrandTotal = order.GrandTotal;
            model.CurrencySymbol = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID).CurrencySymbol;
            var outofstockProds = GetOutOfStockProducts(customer);
            if (outofstockProds.Any())
            {
                model.OutOfStockProducts = customer.OrderItems.Where(oi => oi.ProductID.HasValue && outofstockProds.Contains(oi.ProductID.Value)).Select(oi => new { oi.SKU, oi.ProductName });
            }
            else
            {
                model.OutOfStockProducts = Enumerable.Empty<object>();
            }

            return model;
        }
        protected virtual IList<IPromotionallyAddedItemModel> GetPromotionallyAddedItemModels(Order order)
        {
            OrderCustomer orderCustomer = order.OrderCustomers[0];
            int addedItemOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
            var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
            var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);

            return adjustments.Select(grp =>
            {
                var model = Create.New<IPromotionallyAddedItemModel>();
                model.Description = grp.Key.Description;
                model.StepID = grp.Key.InjectedOrderSteps.Any() ? grp.Key.InjectedOrderSteps.First().OrderStepReferenceID : null;
                model.Selections = grp.Select(i =>
                {
                    var item = Create.New<IOrderItemModel>();
                    item.SKU = i.SKU;
                    item.ProductName = i.ProductName;
                    item.Quantity = i.Quantity;
                    item.ImageUrl = GetProductImagePath(i);
                    return item;
                }).ToList();

                return model;
            }).ToList();
        }

        protected virtual string GetProductImagePath(OrderItem orderItem)
        {
            string previewImage = "../../Content/Images/Shopping/no-image.jpg".ResolveUrl();
            var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
            if (product != null && product.MainImage != null)
            {
                previewImage = product.MainImage.FilePath.ReplaceFileUploadPathToken();
            }
            if (product != null && product.IsVariant())
            {
                var productBase = ProductBase.LoadFull(product.ProductBaseID);
                if (productBase != null)
                {
                    var nonVariantProduct = productBase.Products.FirstOrDefault(p => !p.IsVariant());
                    if (nonVariantProduct != null && nonVariantProduct.MainImage != null)
                    {
                        previewImage = nonVariantProduct.MainImage.FilePath.ReplaceFileUploadPathToken();
                    }
                }
            }
            return previewImage;
        }


        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult EnrollmentItems(EnrollmentItemsModel model)
        {
            var account = GetEnrollingAccount();
            var initialOrder = GetInitialOrder();

            EnrollmentItems_ValidateOrderItems(initialOrder);
            EnrollmentItems_ValidateVolume(initialOrder);

            if (!ModelState.IsValid)
            {
                EnrollmentItems_LoadResources(model, initialOrder);
                return SectionsView(model);
            }

            try
            {
                UpdateOrderShipmentAddress(initialOrder, account);
                OrderContext.Order = initialOrder;
                OrderService.UpdateOrder(OrderContext);
                initialOrder.Save();
                _enrollmentContext.InitialOrder = initialOrder;
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                EnrollmentItems_LoadResources(model, initialOrder);
                return SectionsView(model);
            }

            return SectionCompleted();
        }

        [HttpPost]
        public virtual ActionResult GetInitialOrderProducts(MiniShopCategoryModel model)
        {
            try
            {
                var initialOrder = GetInitialOrder();
                var productModels = GetProductModels(model.CategoryID, initialOrder);
                //var modelData = EnrollmentItems_GetModelData(products: productModels);
                var modelData = EnrollmentItems_GetModelData(initialOrder: initialOrder, products: productModels);

                return Json(modelData.AsDictionary(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Log().PublicMessage);
            }
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public virtual ActionResult AddInitialOrderItem(MiniShopProductModel model)
        //{
        //    try
        //    {
        //        if (model.Quantity > 0)
        //        {
        //            var initialOrder = GetInitialOrder(false);
        //            initialOrder.AddItem(model.ProductID, model.Quantity);
        //            OrderContext.Order = initialOrder;
        //            OrderService.UpdateOrder(OrderContext);
        //            initialOrder.Save();

        //            var modelData = EnrollmentItems_GetModelData(
        //                initialOrder: initialOrder,
        //                orderItems: GetOrderItemModels(initialOrder)
        //            );

        //            return Json(modelData.AsDictionary());
        //        }
        //        else
        //        {
        //            return Json(new { success = true, message = Translation.GetTerm("QuantityMustBeGreaterThanZero") });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonError(ex.Log().PublicMessage);
        //    }
        //}

        [HttpPost]
        public virtual ActionResult RemoveInitialOrderItem(MiniShopOrderItemModel model)
        {
            try
            {
                var initialOrder = GetInitialOrder();
                var orderItem = initialOrder.OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID == model.ProductID && x.ParentOrderItemID == null);

                if (orderItem == null)
                {
                    return JsonError(_errorUpdatingAutoship);
                }

                // Decrement quantity by one unless this is the last instance of this product in the order.
                if (model.Quantity > 1)
                {
                    initialOrder.UpdateItem(
                        initialOrder.OrderCustomers[0],
                        orderItem,
                         0//Eliminar el Item de la Orden, valor [0]
                        //model.Quantity - 1 /*Comentado por Antonio Campos Santos*/
                        //model.Quantity - 1
                    );
                }
                else
                {
                    initialOrder.RemoveItem(orderItem);
                }

                OrderContext.Order = initialOrder;
                OrderService.UpdateOrder(OrderContext);
                initialOrder.Save();

                var modelData = EnrollmentItems_GetModelData(
                    initialOrder: initialOrder,
                    orderItems: GetOrderItemModels(initialOrder)
                );

                return Json(modelData.AsDictionary());
            }
            catch (Exception ex)
            {
                return JsonError(ex.Log().PublicMessage);
            }
        }

        //[HttpPost]
        //public virtual ActionResult Update(ICollection<MiniShopUpdate> products)
        //{
        //    try
        //    {
        //        var initialOrder = GetInitialOrder(false);

        //        foreach (var item in products)
        //        {
        //            if (item.Quantity > 0)
        //            {
        //                //var staticProduct = initialOrder.OrderCustomers[0].OrderItems.FirstOrDefault(oi => oi.ProductID == item.ProductID && !oi.IsEditable);
        //                var orderItem = initialOrder.OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID == item.ProductID && x.ParentOrderItemID == null);
        //                initialOrder.UpdateItem(initialOrder.OrderCustomers[0], orderItem, item.Quantity);
        //                OrderContext.Order = initialOrder;
        //                OrderService.UpdateOrder(OrderContext);
        //                initialOrder.Save();

        //            }
        //        }

        //        var modelData = EnrollmentItems_GetModelData(
        //                    initialOrder: initialOrder,
        //                    orderItems: GetOrderItemModels(initialOrder)
        //                );
        //        return Json(modelData.AsDictionary());

        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonError(ex.Log().PublicMessage);
        //    }
        //}

        #region
        protected virtual List<int> GetOutOfStockProducts(OrderCustomer customer)
        {
            var outOfStockProducts = new List<int>();
            if (customer != null)
            {
                foreach (OrderItem oi in customer.OrderItems)
                {
                    Product p = Inventory.GetProduct(oi.ProductID.Value);
                    if (p != null && Product.CheckStock(p.ProductID).IsOutOfStock && p.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer)
                    {
                        outOfStockProducts.Add(p.ProductID);
                    }
                }
            }

            return outOfStockProducts;
        }


        #endregion

        [EnrollmentStepSection]
        public virtual ActionResult AutoshipBundles()
        {
            return SectionsView();
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult AutoshipBundles(AutoshipBundlesModel model)
        {
            if (!ModelState.IsValid)
            {
                AutoshipBundles_LoadResources(model);
                return SectionsView(model);
            }

            try
            {
                if (_enrollmentContext.EnrollmentConfig.Autoship.Skippable
                    && model.SelectedProductID == -1)
                {
                    _enrollmentContext.SkipAutoshipOrder = true;
                    DeleteAutoshipOrder();
                }
                else
                {
                    _enrollmentContext.SkipAutoshipOrder = false;

                    var account = GetEnrollingAccount();
                    var autoshipOrder = GetAutoshipOrder();

                    AutoshipBundles_UpdateAutoshipOrder(model, autoshipOrder);
                    UpdateOrderShipmentAddress(autoshipOrder.Order, account);

                    // The order has been modified. We need to calculate the totals again.
                    autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);
                    autoshipOrder.Save();
                    _enrollmentContext.AutoshipOrder = autoshipOrder;
                }
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log().PublicMessage);
                AutoshipBundles_LoadResources(model);
                return SectionsView(model);
            }

            return SectionCompleted();
        }

        [EnrollmentStepSection]
        public virtual ActionResult AutoshipItems()
        {
            var autoshipOrder = GetAutoshipOrder();

            if (_enrollmentContext.EnrollmentConfig.Autoship.ImportShoppingOrder)
            {
                ImportShoppingOrder(autoshipOrder.Order);
            }

            return SectionsView();
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult AutoshipItems(AutoshipItemsModel model)
        {
            var account = GetEnrollingAccount();
            var autoshipOrder = GetAutoshipOrder();

            AutoshipItems_ValidateOrderItems(autoshipOrder);
            AutoshipItems_ValidateVolume(autoshipOrder);

            if (!ModelState.IsValid)
            {
                AutoshipItems_LoadResources(model, autoshipOrder);
                return SectionsView(model);
            }

            try
            {
                UpdateOrderShipmentAddress(autoshipOrder.Order, account);

                // We modified the order. We need to calculate the totals again.
                autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);
                autoshipOrder.Save();
                _enrollmentContext.AutoshipOrder = autoshipOrder;
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log().PublicMessage);
                AutoshipItems_LoadResources(model, autoshipOrder);
                return SectionsView(model);
            }

            return SectionCompleted();
        }

        [HttpPost]
        public virtual ActionResult GetAutoshipProducts(MiniShopCategoryModel model)
        {
            try
            {
                var autoshipOrder = GetAutoshipOrder();
                var productModels = GetProductModels(model.CategoryID, autoshipOrder.Order);
                var modelData = AutoshipItems_GetModelData(products: productModels);

                return Json(modelData.AsDictionary());
            }
            catch (Exception ex)
            {
                return JsonError(ex.Log().PublicMessage);
            }
        }

        [HttpPost]
        public virtual ActionResult AddAutoshipOrderItem(MiniShopProductModel model)
        {
            try
            {
                var autoshipOrder = GetAutoshipOrder();
                var orderItem = autoshipOrder.Order.OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID == model.ProductID);

                // Increment quantity by one unless this is the first instance of this product in the order.
                if (orderItem != null)
                {
                    autoshipOrder.Order.UpdateItem(
                            autoshipOrder.Order.OrderCustomers[0],
                            orderItem,
                            orderItem.Quantity + 1
                    );
                }
                else
                {
                    autoshipOrder.Order.AddItem(model.ProductID, 1);
                }

                var modelData = AutoshipItems_GetModelData(
                        autoshipOrder: autoshipOrder,
                        orderItems: GetOrderItemModels(autoshipOrder.Order)
                );

                return Json(modelData.AsDictionary());
            }
            catch (Exception ex)
            {
                return JsonError(ex.Log().PublicMessage);
            }
        }

        [HttpPost]
        public virtual ActionResult RemoveAutoshipOrderItem(MiniShopOrderItemModel model)
        {
            try
            {
                var autoshipOrder = GetAutoshipOrder();
                var orderItem = autoshipOrder.Order.OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID == model.ProductID);

                if (orderItem == null)
                {
                    return JsonError(_errorUpdatingAutoship);
                }

                // Decrement quantity by one unless this is the last instance of this product in the order.
                if (model.Quantity > 1)
                {
                    autoshipOrder.Order.UpdateItem(
                            autoshipOrder.Order.OrderCustomers[0],
                            orderItem,
                            model.Quantity - 1
                    );
                }
                else
                {
                    autoshipOrder.Order.RemoveItem(orderItem);
                }

                // We've modified the order. We need to calculate the totals again.
                autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);

                var modelData = AutoshipItems_GetModelData(
                        autoshipOrder: autoshipOrder,
                        orderItems: GetOrderItemModels(autoshipOrder.Order)
                );

                return Json(modelData.AsDictionary());
            }
            catch (Exception ex)
            {
                return JsonError(ex.Log().PublicMessage);
            }
        }

        [EnrollmentStepSection]
        public virtual ActionResult ShippingMethod()
        {
            return SectionsView();
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult ShippingMethod(ShippingMethodModel model)
        {
            var order = GetInitialOrder();

            if (!ModelState.IsValid)
            {
                ShippingMethod_LoadResources(model, order);
                return SectionsView(model);
            }

            try
            {
                order.SetShippingMethod(model.SelectedShippingMethodID);
                OrderService.UpdateOrder(OrderContext);
                order.Save();
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null).PublicMessage);
                ShippingMethod_LoadResources(model, order);
                return SectionsView(model);
            }

            return SectionCompleted();
        }
        #endregion

        #region Enrollment Kits Helpers
        protected virtual List<EnrollmentKitConfig> _enrollmentKitConfigs
        {
            get
            {
                List<EnrollmentKitConfig> enrollmentKitConfigs = _enrollmentContext.EnrollmentKitConfigs;
                if (enrollmentKitConfigs == null)
                {
                    enrollmentKitConfigs = GetEnrollmentKitsFromConfig();
                    _enrollmentContext.EnrollmentKitConfigs = enrollmentKitConfigs;
                }
                return enrollmentKitConfigs;
            }
        }

        protected virtual List<EnrollmentKitConfig> GetEnrollmentKitsFromConfig()
        {
            // WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
            IEnumerable<dynamic> configsEnumerable = EnrollmentConfigHandler
                    .GetProperty("Products", "EnrollmentKits")
                    .EnrollmentKit();

            return new List<EnrollmentKitConfig>(configsEnumerable.Select(x => new EnrollmentKitConfig
            {
                HeaderText = x.HeaderText,
                HeaderTermName = x.HeaderTermName,
                HeaderCssClass = x.HeaderCssClass,
                SKU = x.SKU,
                AccountTypeID = x.AccountTypeID
            }));
        }

        private List<IProduct> enrollmentKitProducts = null;
        protected virtual List<IProduct> GetEnrollmentKitProducts(int accountTypeID)
        {
            if (enrollmentKitProducts == null)
            {
                enrollmentKitProducts = GetProductsByCatalogTypeAndAccountType((int)ConstantsGenerated.CatalogType.EnrollmentKits,
                                                                               accountTypeID);
            }

            return enrollmentKitProducts;
        }

        private List<IProduct> GetProductsByCatalogTypeAndAccountType(int catalogType, int accountTypeID)
        {
            var inventory = Create.New<InventoryBaseRepository>();
            var activeCatalogs = inventory.GetActiveCatalogs(_enrollmentContext.StoreFrontID);

            var catalogRepo = Create.New<ICatalogRepository>();
            var filteredCatalogs = activeCatalogs.Where(ac => IsValidEnrollmentCatalog(ac, catalogRepo, catalogType, accountTypeID));
            var filteredProducts = filteredCatalogs.SelectMany(ca => inventory.GetActiveProductsForCatalog(_enrollmentContext.StoreFrontID, ca.CatalogID, (short)accountTypeID));
            var returnValue = filteredProducts.Cast<IProduct>().ToList();
            return returnValue;
        }

        private bool IsValidEnrollmentCatalog(Catalog catalog, ICatalogRepository catalogRepo, int catalogType, int accountTypeID)
        {
            if (catalog.CatalogTypeID != catalogType)
            {
                return false;
            }

            var catalogAccountTypes = catalogRepo.GetAccountTypeIDsForCatalog(catalog);
            if (catalogAccountTypes.Any() && !catalogAccountTypes.Any(at => at == accountTypeID))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Finds a valid enrollment kit in an order and returns its ProductID.
        /// </summary>
        protected virtual int? GetSelectedEnrollmentKitProductID(Order order)
        {
            var enrollmentKitProductIDs = this.GetEnrollmentKitProducts(_enrollmentContext.AccountTypeID).Select(p => p.ProductID);
            var orderItem = order.OrderCustomers.First().OrderItems.FirstOrDefault(oi => enrollmentKitProductIDs.Contains(oi.ProductID.Value));

            return orderItem != null ? orderItem.ProductID : null;
        }

        protected virtual void SetSelectedEnrollmentKit(Order order, int selectedProductID)
        {
            // get all enrollment kit products.  If any enrollment kit product is currently in the order, remove it before adding the new one.
            var enrollmentKitProductIDs = GetEnrollmentKitProducts(_enrollmentContext.AccountTypeID).Select(x => x.ProductID).ToList();
            var existingEnrollmentKits = order.GetAllOrderItems().Where(x => enrollmentKitProductIDs.Contains((int)x.ProductID));

            if (existingEnrollmentKits.Any())
            {
                existingEnrollmentKits.Each(x => order.RemoveItem(x));
            }

            if (enrollmentKitProducts.FirstOrDefault(x => x.ProductID == selectedProductID) != null)
            {
                order.AddOrUpdateOrderItem(order.OrderCustomers[0], new List<OrderItemUpdateInfo> { new OrderItemUpdateInfo { ProductID = selectedProductID, Quantity = 1 } }, false);
                var orderItem = order.OrderCustomers[0].OrderItems.Where(oi => oi.ProductID == selectedProductID).FirstOrDefault();

                // This is a flag to prevent the kit from being removed on the EnrollmentItems section.
                orderItem.IsEditable = false;
            }
        }

        protected virtual EnrollmentKitsModel EnrollmentKits_CreateModel()
        {
            return new EnrollmentKitsModel();
        }

        protected virtual void EnrollmentKits_LoadModel(EnrollmentKitsModel model, bool active)
        {
            var initialOrder = GetInitialOrder();
            var selectedProductID = GetSelectedEnrollmentKitProductID(initialOrder);
            model.LoadValues(selectedProductID);
            EnrollmentKits_LoadResources(model, initialOrder);
        }

        protected virtual void EnrollmentKits_LoadResources(EnrollmentKitsModel model, Order initialOrder)
        {
            var iProducts = GetEnrollmentKitProducts(_enrollmentContext.AccountTypeID);
            // Here we make the, currently correct, assumption that the IProducts we get are really Products under the covers.
            var products = iProducts.Select(p => (Product)p);

            model.LoadResources(
                    CoreContext.CurrentCultureInfo,
                    products,
                    _enrollmentContext
            );

            LoadPageHtmlContent();
        }

        protected virtual void EnrollmentKits_UpdateInitialOrder(EnrollmentKitsModel model, Order initialOrder)
        {
            SetSelectedEnrollmentKit(initialOrder, model.SelectedProductID.Value);
        }
        #endregion

        #region Enrollment Items Helpers
        protected virtual EnrollmentItemsModel EnrollmentItems_CreateModel()
        {
            return new EnrollmentItemsModel();
        }

        protected virtual void EnrollmentItems_LoadModel(EnrollmentItemsModel model, bool active)
        {
            var order = OrderContext.Order.AsOrder();
            Account account = GetEnrollingAccount();
            //var account = GetEnrollingAccount();
            account = Account.LoadForSession(account.AccountID);
            //CoreContext.CurrentAccount = Account.LoadFull(accountId.Value);
            CoreContext.CurrentAccount = account.Clone();
            CoreContext.CurrentAccount.StartEntityTracking();

            string OrderNumber = PreOrderExtension.GetOrderNumberByAccount(account.AccountID, 11, 4);
            if (!OrderNumber.IsNullOrEmpty())
            {
                OrderContext.Order = Order.LoadByOrderNumberFull(OrderNumber);
            }
            var initialOrder = order;
            if (OrderContext.Order.OrderID == 0)
            {

                initialOrder = GetInitialOrder(false);
            }

            if (active)
            {
                EnrollmentItems_LoadResources(model, initialOrder);
            }
            else
            {
                var orderItemModels = GetOrderItemModels(initialOrder);

                model.LoadResources(
                    EnrollmentItems_GetModelData(
                        initialOrder: initialOrder,
                        orderItems: orderItemModels
                    )
                );

            }
        }

        protected virtual void EnrollmentItems_LoadResources(EnrollmentItemsModel model, Order initialOrder)
        {
            var categoryModels = GetCategoryModels((short)Constants.OrderType.EnrollmentOrder);
            var productModels = categoryModels.Any() ? GetProductModels(categoryModels.First().CategoryID, initialOrder) : null;
            var orderItemModels = GetOrderItemModels(initialOrder);
            var orderModel = initialOrder;
            Session["Order"] = initialOrder;
            model.LoadResources(
                    EnrollmentItems_GetModelData(
                            initialOrder: initialOrder,
                            categories: categoryModels,
                            products: productModels,
                            orderItems: orderItemModels
                    )
            );
            //var model = Index_CreateModel(OrderContext.Order.AsOrder());  
        }

        protected virtual void EnrollmentItems_ValidateOrderItems(Order initialOrder)
        {
            if (!initialOrder.OrderCustomers[0].OrderItems.Any())
            {
                ModelState.AddModelError(string.Empty, _errorOrderMustContainAtLeastOneItem);
            }
        }

        protected virtual void EnrollmentItems_ValidateVolume(Order initialOrder)
        {
            decimal? minimumVolume = _enrollmentContext.EnrollmentConfig.EnrollmentOrder.MinimumCommissionableTotal;

            if ((initialOrder.CommissionableTotal ?? 0) < (minimumVolume ?? 0))
            {
                ModelState.AddModelError(string.Empty, _errorOrderDoesNotMeetMinimumVolume);
            }
        }

        public virtual TotalsOrderItemModel TotalsOrderItem { get; set; }

        /// <summary>
        /// Builds a <see cref="DynamicDictionary"/> of data to either be used to load a viewmodel or to return as JSON.
        /// All parameters are optional.
        /// </summary>
        protected virtual dynamic EnrollmentItems_GetModelData(
                Order initialOrder = null,
                IEnumerable<MiniShopCategoryModel> categories = null,
                IEnumerable<MiniShopProductModel> products = null,
                IDictionary<string, object> orderEntryModelData = null,
            //IEnumerable<MiniShopOrderItemModel> orderItems = null
                IList<IOrderItemModel> orderItems = null
            )
        {
            decimal? minimumVolume = _enrollmentContext.EnrollmentConfig.EnrollmentOrder.MinimumCommissionableTotal;

            //var ApplicablePromotions = GetApplicablePromotions(initialOrder);
            OrderContext.Order = initialOrder;
            var ApplicablePromotions = GetApplicablePromotions(OrderContext);
            var FreeGiftModels = GetFreeGiftModels(initialOrder);

            var modelData = MiniShopModel.GetModelData(
                    order: initialOrder,
                    minimumVolume: minimumVolume,
                    categories: categories,
                    products: products,
                    orderItems: orderItems,
                    applicablePromotions: ApplicablePromotions,
                    freeGiftModels: FreeGiftModels

            );

            //var model = Index_CreateModel(OrderContext.Order.AsOrder());
            //modelData.IndexModel = Index_CreateModel(initialOrder);
            var indexModel = Index_CreateModel(initialOrder);
            modelData.OptionsJsonModel = ((IOrderEntryModel)indexModel.OrderEntryModel).OptionsJson();



            //modelData.OptionsJsonModel = indexModel.OrderEntryModel.OptionsJson();
            modelData.DataJsonModel = indexModel.OrderEntryModel.DataJson();

            modelData.ApplicablePromotions = ApplicablePromotions;
            modelData.FreeGiftModels = FreeGiftModels;
            //TotalsOrderItem = new TotalsOrderItemModel();
            //modelData.TotalsOrderItem = TotalsOrderItem.LoadResources(modelData.TotalQV, modelData.TotalCV, modelData.TotalSubTotal, modelData.TotalPrice);

            modelData.GetProductsUrl = Url.Action("GetInitialOrderProducts");
            modelData.AddOrderItemUrl = Url.Action("AddInitialOrderItem");
            modelData.RemoveOrderItemUrl = Url.Action("RemoveInitialOrderItem");
            modelData.UpdateOrderItemsUrl = Url.Action("Update");
            modelData.AddOrderItemLabel = Translation.GetTerm("AddToOrder", "Add to Order");
            //modelData.Order = initialOrder;
            return modelData;
        }
        #endregion

        protected virtual object GetApplicablePromotions(IOrderContext orderContext)
        {
            var promotionAdjustments = orderContext.Order.AsOrder().OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
            //var promotionAdjustments = order.OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
            var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
            var inventoryService = Create.New<IInventoryService>();
            return adjustments.Select(adj =>
            {

                var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
                    (os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
                var promotionOutOfStock = false;
                if (giftStep == null)
                {
                    promotionOutOfStock = adj.OrderAdjustmentOrderModifications.Any(mod => mod.ModificationOperationID == (int)OrderAdjustmentOrderOperationKind.Message);
                }
                else
                {
                    var productSelectionStep = (IUserProductSelectionOrderStep)giftStep;
                    promotionOutOfStock = !(productSelectionStep.AvailableOptions.Any(option => inventoryService.GetProductAvailabilityForOrder(orderContext, option.ProductID, option.Quantity).CanAddNormally == option.Quantity));
                }
                return new { Description = adj.Description, StepID = giftStep == null ? null : giftStep.OrderStepReferenceID.ToString(), PromotionOutOfStock = promotionOutOfStock };
            }
            );
        }

        protected virtual IEnumerable<object> GetFreeGiftModels(Order order)
        {
            OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
            int addedItemOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
            var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
            var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);

            return adjustments.Select(grp => new
            {
                Description = grp.Key.Description,
                StepID = grp.Key.InjectedOrderSteps.Any() ? grp.Key.InjectedOrderSteps.First().OrderStepReferenceID : null,
                Selections = grp.Select(i => new { SKU = i.SKU, Name = i.ProductName, Quantity = i.Quantity })
            });
        }

        #region Autoship Bundles Helpers

        public virtual List<IProduct> GetAutoshipBundleProducts(int accountTypeID)
        {
            return GetProductsByCatalogTypeAndAccountType((int)Constants.CatalogType.AutoshipBundles, accountTypeID);
        }

        /// <summary>
        /// Finds a valid autoship bundle in an order and returns its ProductID.
        /// </summary>
        protected virtual int? GetSelectedAutoshipBundleProductID(Order order)
        {
            var autoshipBundleProductIDs = GetAutoshipBundleProducts(_enrollmentContext.AccountTypeID).Select(x => x.ProductID);

            var orderItem = order.OrderCustomers[0].OrderItems.FirstOrDefault(oi => autoshipBundleProductIDs.Contains(oi.ProductID.Value));

            return orderItem != null ? orderItem.ProductID : null;
        }

        protected virtual void SetSelectedAutoshipBundle(Order order, int selectedProductID)
        {
            // TODO: Just remove the bundle, not all items
            order.RemoveAllItems(order.OrderCustomers[0]);

            var product = GetAutoshipBundleProducts(_enrollmentContext.AccountTypeID).FirstOrDefault(x => x.ProductID == selectedProductID);
            if (product != null)
            {
                order.AddOrUpdateOrderItem(order.OrderCustomers.First(), new List<OrderItemUpdateInfo> { new OrderItemUpdateInfo { ProductID = product.ProductID, Quantity = 1 } }, false);
                var orderItem = order.OrderCustomers.First().OrderItems.Where(oi => oi.ProductID == selectedProductID).FirstOrDefault();
                //This is a flag to prevent the bundle from being removed on the AutoshipItems section.
                orderItem.IsEditable = false;
            }
        }

        protected virtual AutoshipBundlesModel AutoshipBundles_CreateModel()
        {
            return new AutoshipBundlesModel();
        }

        protected virtual void AutoshipBundles_LoadModel(AutoshipBundlesModel model, bool active)
        {
            var autoshipOrder = GetAutoshipOrder(false);

            bool skippable = _enrollmentContext.EnrollmentConfig.Autoship.Skippable;

            int? selectedProductID = null;
            if (skippable
                && _enrollmentContext.SkipAutoshipOrder)
            {
                selectedProductID = -1;
            }
            else if (autoshipOrder != null)
            {
                selectedProductID = GetSelectedAutoshipBundleProductID(autoshipOrder.Order);
            }

            model.LoadValues(
                    selectedProductID,
                    skippable
            );

            AutoshipBundles_LoadResources(model);
        }

        protected virtual void AutoshipBundles_LoadResources(AutoshipBundlesModel model)
        {
            var iProducts = GetAutoshipBundleProducts(_enrollmentContext.AccountTypeID);
            // Here we make the, currently correct, assumption that the IProducts we get are really Products under the covers.
            var products = iProducts.Select(p => (Product)p);
            products = products.OrderBy(o => o.Prices.FirstOrDefault(p => p.ProductPriceTypeID == _enrollmentContext.AccountTypeID).Price);

            model.LoadResources(CoreContext.CurrentCultureInfo,
                                products,
                                _enrollmentContext
                                );

            LoadPageHtmlContent();
        }

        protected virtual void AutoshipBundles_UpdateAutoshipOrder(AutoshipBundlesModel model, AutoshipOrder autoshipOrder)
        {
            SetSelectedAutoshipBundle(autoshipOrder.Order, model.SelectedProductID.Value);
        }
        #endregion

        #region Autoship Items Helpers
        protected virtual AutoshipItemsModel AutoshipItems_CreateModel()
        {
            return new AutoshipItemsModel();
        }

        protected virtual void AutoshipItems_LoadModel(AutoshipItemsModel model, bool active)
        {
            if (!active)
            {
                return;
            }

            var autoshipOrder = GetAutoshipOrder();
            AutoshipItems_LoadResources(model, autoshipOrder);
        }

        protected virtual void AutoshipItems_LoadResources(AutoshipItemsModel model, AutoshipOrder autoshipOrder)
        {
            var categoryModels = GetCategoryModels((short)Constants.OrderType.AutoshipTemplate);
            var productModels = categoryModels.Any() ? GetProductModels(categoryModels.First().CategoryID, autoshipOrder.Order) : null;
            var orderItemModels = GetOrderItemModels(autoshipOrder.Order);

            model.LoadResources(
                    AutoshipItems_GetModelData(
                            autoshipOrder: autoshipOrder,
                            categories: categoryModels,
                            products: productModels,
                            orderItems: orderItemModels
                    )
            );
        }

        protected virtual void AutoshipItems_ValidateOrderItems(AutoshipOrder autoshipOrder)
        {
            if (!autoshipOrder.Order.OrderCustomers[0].OrderItems.Any())
            {
                ModelState.AddModelError(string.Empty, _errorAutoshipMustContainAtLeastOneItem);
            }
        }

        protected virtual void AutoshipItems_ValidateVolume(AutoshipOrder autoshipOrder)
        {
            decimal? minimumVolume = null;

            var autoshipSchedule = (AutoshipSchedule)GetAutoshipSchedule(autoshipOrder);
            if (autoshipSchedule != null)
            {
                minimumVolume = autoshipSchedule.MinimumCommissionableTotal;
            }

            if ((autoshipOrder.Order.CommissionableTotal ?? 0) < (minimumVolume ?? 0))
            {
                ModelState.AddModelError(string.Empty, _errorAutoshipDoesNotMeetMinimumVolume);
            }
        }

        protected virtual dynamic AutoshipItems_GetModelData(
                AutoshipOrder autoshipOrder = null,
                IEnumerable<MiniShopCategoryModel> categories = null,
                IEnumerable<MiniShopProductModel> products = null,
            //IEnumerable<MiniShopOrderItemModel> orderItems = null
            IList<IOrderItemModel> orderItems = null
            )
        {
            decimal? minimumVolume = null;

            if (autoshipOrder != null)
            {
                var autoshipSchedule = GetAutoshipSchedule(autoshipOrder);
                if (autoshipSchedule != null)
                {
                    minimumVolume = autoshipSchedule.MinimumCommissionableTotal;
                }
            }

            var modelData = MiniShopModel.GetModelData(
                    order: autoshipOrder == null ? null : autoshipOrder.Order,
                    minimumVolume: minimumVolume,
                    categories: categories,
                    products: products,
                    orderItems: orderItems

            );

            modelData.GetProductsUrl = Url.Action("GetAutoshipProducts");
            modelData.AddOrderItemUrl = Url.Action("AddAutoshipOrderItem");
            modelData.RemoveOrderItemUrl = Url.Action("RemoveAutoshipOrderItem");
            modelData.AddOrderItemLabel = Translation.GetTerm("AddToAutoship", "Add to Autoship");

            return modelData;
        }
        #endregion

        #region Nueva
        protected virtual string _errorInvalidPromotionCode(string promotionCode) { return Translation.GetTerm("ErrorInvalidPromotionCode", "The promotion could not be applied. Invalid promotion code: '{0}'.", promotionCode); }
        protected virtual string _errorNoItemsInOrder { get { return Translation.GetTerm("PleaseAddItemsToOrderBeforeUpdating", "Please add items to Order before updating."); } }

        [NonAction]
        public virtual HtmlString GetGroupItemsHtml(string parentGuid, int groupId)
        {
            if (OrderContext.Order.AsOrder() == null)
                return new HtmlString("");

            var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
            StringBuilder builder = new StringBuilder();

            var orderItem = customer.OrderItems.GetByGuid(parentGuid);
            var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
            var dynamicKit = product.DynamicKits[0];
            var groupItems = orderItem.ChildOrderItems.Where(index => index.DynamicKitGroupID == groupId);

            foreach (var item in groupItems)
            {
                Product childProduct = Inventory.GetProduct(item.ProductID.Value);
                for (int q = 0; q < item.Quantity; q++)
                {
                    TagBuilder span = new TagBuilder("span");
                    span.AddCssClass("block");

                    span.InnerHtml = new StringBuilder()
                            .Append("<input type=\"hidden\" value=\"" + item.Guid.ToString("N") + "\" class=\"orderItemGuid\" />")
                            .Append("<a href=\"javascript:void(0)\" class=\"UI-icon icon-x RemoveItem\"></a>&nbsp;" + childProduct.SKU + " " + childProduct.Translations.Name()).ToString();

                    builder.Append(span.ToString());
                }
            }

            var results = builder.ToString();

            return new HtmlString(string.IsNullOrEmpty(results) ? "" : results);
        }

        private int _availableBundleCount { get; set; }

        [NonAction]
        private void CalculateQualificationTotal()
        {
            decimal sum = 0;
            foreach (var item in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
            {
                foreach (var price in item.OrderItemPrices)
                {
                    if (price.ProductPriceTypeID == 21) sum += price.UnitPrice * item.Quantity;
                }
            }
            OrderContext.Order.AsOrder().QualificationTotal = sum;
        }


        public virtual string GetDynamicBundleUpSale()
        {
            var order = OrderContext.Order.AsOrder();
            if (order == null)
            {
                return string.Empty;
            }
            var customer = order.OrderCustomers[0];
            if (customer.IsTooBigForBundling())
            {
                return string.Empty;
            }

            var possibleDynamicKitProducts = order.GetPotentialDynamicKitUpSaleProducts(customer, OrderContext.SortedDynamicKitProducts).ToList();
            this._availableBundleCount = possibleDynamicKitProducts.Count();

            var sb = new StringBuilder();
            var sbi = new StringBuilder();

            for (int i = 0; i < _availableBundleCount; i++)
            {
                Guid guid = Guid.NewGuid();
                var newID = guid.ToString("N");

                //truncate += product.Translations.Name() + ", ";
                if (possibleDynamicKitProducts.Count() <= 1 || i != 0)
                {
                    continue;
                }
                //sb.Append("<a href=\"#\" id=\"BundleTrigger" + newID + "\" class=\"jqModal bold\">");
                //sb.Append(possibleDynamicKitProducts.Count() + " bundles available");
                //sb.Append("</a>");

                //sbi.Append("<div id=\"BundleModal" + newID + "\" class=\"jqmWindow LModal bundleModal\" style=\"display:none;\">" +
                sbi.Append("<div class=\"mContent\">" +
                            "<h2>" + Translation.GetTerm("BundlesAvailableModal_Heading", "Bundles Available") + "</h2>" +
                            "<p class=\"mb10 bundleAvailableTerm\">" + Translation.GetTerm("BundlesAvailableModal_Text", "Click on an available bundle below to save on your order!") + "</p>" +
                            "<ul class=\"flatList bundleList\">");
                for (var j = 0; j < possibleDynamicKitProducts.Count(); j++)
                {
                    var product = possibleDynamicKitProducts[j];
                    sbi.Append("<li class=\"UI-lightBg brdrAll bundleOption\">" +
                                "<input type=\"hidden\" class=\"dynamicKitProductSuggestion\" value=\"" + product.ProductID + "\" />" +
                                "<a href=\"javascript:void(0);\" class=\"block pad5 brdrAll CreateBundle\">" + product.Translations.Name() + "</a></li>");
                }

                sbi.Append(
                    "</ul>"
                    +
                    "<p class=\"mt10\"><a href=\"javascript:void(0);\" class=\"jqmClose FL cancel close\">Close</a></p>"
                    + "<span class=\"clr\"></span>" + "</div>"); //</div>");

                //sbi.Append("<script type=\"text/javascript\">$(function() { $('#BundleModal" + newID + "').jqm({ trigger: '#BundleTrigger" + newID + "'," +
                //                                "onShow: function (h) {h.w.css({" +
                //                                                "top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + ($(window).scrollTop() - 20) + 'px'," +
                //                                                "left: Math.floor(parseInt($(window).width() / 2)) + 'px'" +
                //                                        "}).fadeIn(); } }) });</script>");
            }

            return sb.ToString() + sbi.ToString();
        }

        protected virtual IEnumerable<string> GetOutOfStockItemsString(Order order)
        {
            return order.OrderCustomers.SelectMany(oi => oi.ParentOrderItems)
                .Where(oi => Inventory.IsOutOfStock(Inventory.GetProduct(oi.ProductID.Value))/* && !Inventory.IsAvailable(oi.ProductID ?? 0)*/ && !oi.HasChildOrderItems)
                .Select(oi => oi.SKU + " - " + oi.ProductName);
        }

        protected virtual List<IDisplayInfo> GetPromotionsDisplayInfo()
        {
            return new List<IDisplayInfo>();
        }

        protected virtual string GetPromotionsHtml(Order order)
        {
            List<IDisplayInfo> promoDisplayInfos = GetPromotionsDisplayInfo();
            string promotionsHtml = RenderRazorPartialViewToString("~/Views/Promotions/_AppliedPromotions.cshtml", promoDisplayInfos);
            return promotionsHtml;
        }


        protected virtual dynamic LoadOrderEntryModelData(dynamic data, Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);
            // Code contracts rewriter doesn't work with dynamics
            if (data == null)
            {
                throw new ArgumentNullException("options");
            }

            data.Subtotal = GetSubtotal(order);
            data.OrderItemModels = GetOrderItemModels(order);

            return data;
        }


        protected virtual string GetSubtotal(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            return order.Subtotal.ToString(order.CurrencyID);
        }

        protected virtual IDictionary<string, object> GetOrderEntryModelData(Order order)
        {
            Contract.Requires(order != null);

            return LoadOrderEntryModelData(new DynamicDictionary(), order).AsDictionary();
        }

        protected virtual dynamic LoadOrderEntryModelData(dynamic data, IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            // Code contracts rewriter doesn't work with dynamics
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            data.Subtotal = orderContext.Order.AsOrder().OrderCustomers[0].Subtotal.ToString(orderContext.Order.CurrencyID);
            data.SubtotalAdjusted = orderContext.Order.OrderCustomers[0].AdjustedSubTotal.ToString(orderContext.Order.CurrencyID);
            //CGI(CMR)-29/10/2014-Inicio
            //data.OrderItemModels = GetOrderItemModels(orderContext.Order.AsOrder()); //comentado
            IList<IOrderItemModel> orderItemModels = GetOrderItemModels(orderContext.Order.AsOrder());
            data.OrderItemModels = orderItemModels;

            /*CS.29JUN2016.Inicio.Quitar formato Decimal y símbolo moneda*/
            int totalQV = Convert.ToInt32(orderItemModels.Sum(x => (x.TotalQV != null ? x.TotalQV : 0)));
            data.TotalQV_Sum = totalQV.ToString();
            /*CS.29JUN2016.Fin.Quitar formato Decimal y símbolo moneda*/
            //CGI(CMR)-29/10/2014-Fin

            //CGI(CMR)-07/04/2015-Inicio
            //CV

            data.OriginalCommissionableTotal_Sum = orderItemModels.Sum(x => (x.OriginalCommissionableTotal.Length > 0 ? x.OriginalCommissionableTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(orderContext.Order.CurrencyID);
            data.AdjustedCommissionableTotal_Sum = orderItemModels.Sum(x => (x.AdjustedCommissionableTotal.Length > 0 ? x.AdjustedCommissionableTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(orderContext.Order.CurrencyID);

            //Qty
            data.CountItems = orderItemModels.Sum(x => x.Quantity);
            //SubTotal
            data.OriginalTotal_Sum = orderItemModels.Sum(x => (x.OriginalTotal.Length > 0 ? x.OriginalTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(orderContext.Order.CurrencyID);
            data.AdjustedTotal_Sum = orderItemModels.Sum(x => (x.AdjustedTotal.Length > 0 ? x.AdjustedTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(orderContext.Order.CurrencyID);

            //***************************** start only test *********************************
            //foreach (var item in orderItemModels)
            //{
            //    string OriginalTotal = item.OriginalTotal;
            //    string AdjustedTotal = item.AdjustedTotal;

            //    decimal OriginalTotal_dec = Convert.ToDecimal(item.OriginalTotal.Replace("$", "")).FormatGlobalizationDecimal();
            //    decimal AdjustedTotal_dec = Convert.ToDecimal(item.AdjustedTotal.Replace("$", "")).FormatGlobalizationDecimal();

            //    decimal OriginalTotal_dec1 = Convert.ToDecimal(item.OriginalTotal.Replace("$", ""));
            //    decimal AdjustedTotal_dec1 = Convert.ToDecimal(item.AdjustedTotal.Replace("$", ""));

            //    decimal OriginalTotal_dec2 = item.OriginalTotal.Replace("$", "").FormatGlobalizationDecimal();
            //    decimal AdjustedTotal_dec2 = item.AdjustedTotal.Replace("$", "").FormatGlobalizationDecimal();

            //    decimal a = OriginalTotal_dec2.GetRoundedNumber(2);
            //}

            //***************************** end only test *********************************

            data.OriginalTotal_Texto = data.OriginalTotal_Sum + "(" + Translation.GetTerm("Subtotal", "Sub total") + ")";
            data.AdjustedTotal_Texto = data.AdjustedTotal_Sum + "(" + Translation.GetTerm("Subtotaldco", "Sub total dco") + ")";

            //CGI(CMR)-07/04/2015-Fin
            data.ApplicablePromotions = GetApplicablePromotions(orderContext);
            data.FreeGiftModels = GetFreeGiftModels(orderContext.Order.AsOrder());

            var categoryModels = GetCategoryModels((short)Constants.OrderType.EnrollmentOrder);
            data.categoryModels = categoryModels;
            data.productModels = categoryModels.Any() ? GetProductModels(categoryModels.First().CategoryID, orderContext.Order.AsOrder()) : null;

            return data;
        }

        private bool validatePaymenType(int PaymentConfiguration)
        {
            bool ret = false;

            int PaymentTypeID = Order.GetApplyPaymentType(PaymentConfiguration);
            // PaymentTypeID = 6(Product Credit)
            var list = NetSteps.Data.Entities.Business.CTE.paymentTables.ToList().Where(x => x.PaymentType != 6).ToList();
            if (list.Count > 0)
            {
                var i = list.Where(x => x.PaymentType != PaymentTypeID).ToList().Count;
                if (i > 0)
                {
                    ret = true;
                }

            }

            return ret;
        }
        public void RemovePaymentsTable(int Indice)
        {
            PaymentsTable objE = new PaymentsTable();
            objE.ubic = 0;
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            objE.OrderPaymentId = Indice;
            PaymentsMethodsExtensions.UpdPaymentsTable(objE);
        }
        public virtual ActionResult RemovePayment(string paymentId, int indice)
        {
            try
            {

                RemovePaymentsTable(indice);
                //var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                //var orderPayment = customer.OrderPayments.FirstOrDefault(oi => oi.Guid.ToString("N") == paymentId);
                //if (orderPayment == null)
                //    return Json(new
                //    {
                //        result = false,
                //        message = Translation.GetTerm("PaymentDoesNotExist", "That payment does not exist.")
                //    });
                //customer.RemovePayment(orderPayment);
                //OrderService.UpdateOrder(OrderContext);
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                customer.RemovePayment(paymentId);
                OrderService.UpdateOrder(OrderContext);
                return Json(new
                {
                    result = true,
                    totals = Totals,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder())
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ApplyPayment(PaymentTypeModel paymentTypeModel)
        {
            try
            {
                
                /*CS.05MAY2016.Inicio*/
                //var amount = Convert.ToDecimal(paymentTypeModel.AmountConfiguration.FormatGlobalizationDecimal());
                var amount = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(paymentTypeModel.AmountConfiguration);
                paymentTypeModel.Amount = amount;
                /*CS.05MAY2016.Fin*/

                IPayment payment = new Payment()
                {
                    DecryptedAccountNumber = paymentTypeModel.AccountNumber,
                    CVV = paymentTypeModel.Cvv,
                    PaymentType = ConstantsGenerated.PaymentType.EFT,
                    NameOnCard = paymentTypeModel.AccountNumber,
                    BankName = paymentTypeModel.NameOnCard
                };
                //============================================================================================
                // Para Brasil('N') el flujo sigue su curso normal , mientras que para USA('S')  tiene que ser siempre el PaymentTypeID = 1;
                
                //if (Session["GeneralParameterVal"].ToString() == "N")
                //    payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value);
                //else
                    payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value); ;
              
                BasicResponseItem<OrderPayment> response = OrderContext.Order.AsOrder().ApplyPaymentToCustomers(paymentTypeModel.PaymentType, paymentTypeModel.Amount, payment, user: CoreContext.CurrentUser);

                //validationResponse = ValidatePayment(amount, order, PaymentTypeID, customer, user);
                if (!response.Success)
                {
                    return Json(new { result = false, message = response.Message });
                }
                ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                objE.OrderPaymentId = response.Item.OrderPaymentID;
                objE.Amount = paymentTypeModel.Amount;
                objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;
                objE.NameOnCard = "";
                objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;

                if (payment.PaymentTypeID > 1)
                {
                    objE.NumberCuota = null;
                }
                else
                {
                    objE.NumberCuota = paymentTypeModel.NumberCuota.Value;
                }
                objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                CTE.ApplyPayment(objE);
                
                int OrderStatusId = Order.GetApplyPayment(objE);

                OrderContext.Order.AsOrder().OrderPayments.Each(o => o.ExpirationStatusID = (int)ConstantsGenerated.ExpirationStatuses.Unexpired);
                OrderService.UpdateOrder(OrderContext);

                Session["Order"] = OrderContext.Order.AsOrder();
                if (OrderStatusId > 0)
                {
                    return Json(new { result = true, message = string.Empty, totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()), paymentId = 0 });
                    // return Json(new { result = true, message = string.Empty, totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()), paymentId = paymentResponse == null ? 0 : paymentResponse.OrderPaymentID });
                }
                return Json(new { result = false, message = "" });


            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ApplyPaymentPreviosBalance()
        {
            try
            {
                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                decimal Amount = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);
                ViewBag.Balance = Amount.ToString(OrderContext.Order.CurrencyID);
                
                int exitPB = 0;
                exitPB = OrderContext.Order.AsOrder().OrderPayments.Where(x => x.BankName == "Product Credit").ToList().Count();

                if (Amount == 0 || exitPB > 0)
                {
                    return Json(new { result = false, message = "" });
                }
                else
                {
                    PaymentTypeModel paymentTypeModel = new PaymentTypeModel();
                    paymentTypeModel.PaymentMethodID = 60;
                    paymentTypeModel.NameOnCard = "Product Credit";
                    paymentTypeModel.Amount = Amount;

                    IPayment payment = new Payment()
                    {
                        DecryptedAccountNumber = paymentTypeModel.AccountNumber,
                        CVV = paymentTypeModel.Cvv,
                        PaymentType = ConstantsGenerated.PaymentType.EFT,
                        NameOnCard = paymentTypeModel.AccountNumber,
                        BankName = paymentTypeModel.NameOnCard
                    };

                    payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value);

                    BasicResponseItem<OrderPayment> response = OrderContext.Order.AsOrder().ApplyPaymentToCustomerPreviosBalance(paymentTypeModel.PaymentType, paymentTypeModel.Amount, payment, user: CoreContext.CurrentUser);

                    if (response.Success)
                    {
                        OrderService.UpdateOrder(OrderContext);
                    }
                    
                    ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                    objE.OrderPaymentId = response.Item.OrderPaymentID;
                    objE.Amount = paymentTypeModel.Amount;
                    objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;
                    objE.NameOnCard = "";
                    objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;

                    if (payment.PaymentTypeID > 1)
                    {
                        objE.NumberCuota = null;
                    }
                    else
                    {
                        if (paymentTypeModel.NumberCuota != null)
                            objE.NumberCuota = paymentTypeModel.NumberCuota.Value;

                    }
                    NetSteps.Data.Entities.Business.CTE.paymentTables = new List<PaymentsTable>();
                    objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                    NetSteps.Data.Entities.Business.CTE.ApplyPayment(objE);

                     //Order.GetApplyPayment(objE);

                    return Json(new { result = false, message = "" });
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public bool validQVUSA(decimal qualificationTotal)
        {
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == (int)Constants.Country.UnitedStates)
            {
                bool validTotalQv = true;
                var valueDecimal = PreOrderExtension.GetValueURL();
                decimal valueTotalQV = valueDecimal[0];
                var totlaQv = qualificationTotal;
                if (totlaQv < valueTotalQV)
                {
                    validTotalQv = false;
                }
                ViewBag.TotalQV = valueTotalQV;
                return validTotalQv;
            }
            return false;
        }

        protected virtual object Totals
        {
            get
            {
                Order order = OrderContext.Order.AsOrder();
                CalculateQualificationTotal();
                if (order == null)
                    return null;
                decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != NetSteps.Data.Entities.Constants.OrderPaymentStatus.Cancelled.ToShort()).Sum(p => p.Amount);
                decimal shippin = 0;
                shippin = (order.ShippingTotalOverride.HasValue) ? order.HandlingTotal.ToDecimal() : 0;

                decimal totalAPagar = (paymentTotal + shippin);
                decimal balance = order.GrandTotal.GetRoundedNumber() - totalAPagar;
                if (paymentTotal > order.GrandTotal.GetRoundedNumber())
                {
                    if (balance < 0)
                        balance = balance * (-1);
                }
                else
                {
                    if (balance > 0)
                        balance = balance * (-1);
                }

                validQVUSA(order.QualificationTotal.ToDecimal());

                return new
                {
                    subtotalAdjusted = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID),
                    subtotal = order.Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    //commissionableTotal = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
                    commissionableTotal = order.CommissionableTotal,
                    /*CS.03JUL2016.Comentado.Inicio*/
                    //qualificationTotal = order.QualificationTotal.ToDecimal().ToString(order.CurrencyID),
                    /*CS.03JUL2016.Comentado.Fin*/
                    qualificationTotal = order.QualificationTotal.ToDecimal().ToString(),
                    taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
                    shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                    handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
                    grandTotal = order.GrandTotal.ToDecimal().ToString(order.CurrencyID),
                    paymentTotal = paymentTotal.ToString(order.CurrencyID),
                    balanceDue = balance.ToString(order.CurrencyID),
                    balanceAmount = balance,
                    numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity))),
                    validTotalQv = validQVUSA(order.QualificationTotal.ToDecimal())
                };
            }
        }

        public void CalculateTotal()
        {
            try
            {
                Order order = OrderContext.Order.AsOrder();
                //var comisionableTotal = Totals.GetType().GetProperty("commissionableTotal").GetValue(Totals, null);
                //string[] separatorCommissionable = Convert.ToString(comisionableTotal).Split('$');

                var comisionableTotal = Totals.GetType().GetProperty("commissionableTotal").GetValue(Totals, null);
                var separatorCommissionable = Convert.ToDecimal(comisionableTotal).ToString(order.CurrencyID);
                ViewBag.CreditAvailable = Order.GetCredit().ToString('S');

                ViewBag.CommisionableTotal = separatorCommissionable;
                //ViewBag.CommisionableTotal = Convert.ToDecimal(separatorCommissionable[1]).ToString('S');
                /*CS.03JUL2016.Comentado.Inicio*/
                //ViewBag.QualificationTotal = Convert.ToDecimal(separatorCommissionable[1]).ToString('S');
                /*CS.03JUL2016.Comentado.Fin*/

                ViewBag.QualificationTotal = separatorCommissionable;
                //ViewBag.QualificationTotal = Convert.ToDecimal(separatorCommissionable[1]).ToString();
                Session["ProductCredit"] = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);
               
                ViewBag.ProductCredit = (Convert.ToDecimal(Session["ProductCredit"])) < 0 ? (Convert.ToDecimal(Session["ProductCredit"]) * -1).ToString('S') : Convert.ToDecimal(Session["ProductCredit"]).ToString('S');
                ViewBag.ProductCreditStatus = Convert.ToDecimal(Session["ProductCredit"]);

                ViewBag.ProductCredit = Order.GetProductCreditByAccount(Convert.ToInt32(Session["AccountId"])).ToString('S');

                var MarketID = CoreContext.CurrentLanguageID;// CoreContext.CurrentMarketId;  siempre sale 56 , q es para brasil
                var s = OrderExtensions.GeneralParameterVal(MarketID);
                if (s == 1)
                    ViewBag.NewPaymentVisible = true;
                else
                    ViewBag.NewPaymentVisible = false;
            }
            catch (Exception)
            {

            }
        }

        protected virtual string ShippingMethods
        {
            get
            {
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();

                OrderCustomer customer = OrderContext.Order.AsOrder().OrderCustomers[0];
                var shipmentAdjustmentAmount = customer.ShippingAdjustmentAmount;

                if (shipment != null && customer != null)
                {
                    StringBuilder builder = new StringBuilder();

                    IEnumerable<ShippingMethodWithRate> shippingMethods = null;

                    try
                    {
                        shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipment);
                        if (shippingMethods != null)
                            shippingMethods = shippingMethods.OrderBy(sm => sm.ShippingAmount).ToList();
                        if (shippingMethods != null && !shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()) && shippingMethods.Count() > 0)
                        {
                            var cheapestShippingMethod = shippingMethods.First();
                            shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
                            OrderService.UpdateOrder(OrderContext);
                        }
                        if (shippingMethods != null && shippingMethods.Count() > 0)
                        {
                            foreach (var shippingMethod in shippingMethods)
                            {
                                Session["DateEstimated"] = shippingMethod.DateEstimated;
                                builder.Append("<li class=\"AddressProfile\"><input value=\"").Append(shippingMethod.ShippingMethodID)
                                    .Append("\"")
                                    .Append(" id=\"shippingMethod").Append(shippingMethod.ShippingMethodID)
                                    .Append("\"")
                                    .Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? " checked=\"checked\"" : "")
                                    .Append("type=\"radio\" name=\"shippingMethod\" class=\"Radio\" />")
                                    .Append("<label for=\"shippingMethod")
                                    .Append(shippingMethod.ShippingMethodID)
                                    .Append("\"><b class=\"mr10\">")
                                    //.Append(shippingMethod.Name)/**/
                                    .Append(Translation.GetTerm("Shipping", "Frete"))
                                    .Append("</b>");

                                if (shipmentAdjustmentAmount != 0)
                                {
                                    builder.Append("<span class=\"shipMethodPrice originalPrice strikethrough\">")
                                        .Append(shippingMethod.ShippingAmount.ToString(OrderContext.Order.AsOrder().CurrencyID))
                                        .Append("</span>&nbsp;")
                                        .Append("<span class=\"shipMethodPrice discountPrice\">")
                                        .Append((shippingMethod.ShippingAmount - shipmentAdjustmentAmount).ToString(OrderContext.Order.AsOrder().CurrencyID))
                                        .Append("</span>");
                                }
                                else
                                {
                                    builder.Append(shippingMethod.ShippingAmount.ToString(OrderContext.Order.AsOrder().CurrencyID))
                                    .Append("</label></li>");
                                }
                            }
                        }
                        else
                        {
                            builder.Append("<li class=\"AddressProfile\">").Append(Translation.GetTerm("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order.")).Append("</li>");
                        }

                        return builder.ToString();
                    }
                    catch (Exception ex)
                    {
                        var productShippingExcludedShippingException = ex as ProductShippingExcludedShippingException;
                        if (productShippingExcludedShippingException != null)
                        {
                            builder.Append("<li class=\"AddressProfile\"><div>").Append(Translation.GetTerm("InvalidShippingForProducts", "No available shipping methods, try changing your shipping address, updating your order subtotal or call customer service.")).Append("</div><ul>");

                            foreach (var product in (productShippingExcludedShippingException).ProductsThatHaveExcludedShipping)
                                builder.Append(string.Format("<li>{0}</li>", product.Name));

                            builder.Append("</ul></li>");
                            return builder.ToString();
                        }

                        throw new Exception(ex.Message, ex);
                    }
                }

                return null;
            }
        }

        protected virtual IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> AddOrUpdateOrderItems(IEnumerable<IOrderItemQuantityModification> changes)
        {
            var account = GetEnrollingAccount();
            IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> results = null;
            try
            {
                results = OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                //OrderService.UpdateOrder(OrderContext); /*CS:12AB2016:Comentado*/
            }
            catch (NetStepsBusinessException bsbEx)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(bsbEx, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: account != null ? account.AccountID.ToIntNullable() : null);
            }

            return results;
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult AddInitialOrderItem(MiniShopProductModel model)
        {
            var account = GetEnrollingAccount();
            if (ProductQuotasRepository.ProductIsRestricted(model.ProductID, model.Quantity, account.AccountID, account.AccountTypeID))
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });

            #region Valida Precios Activos del Producto agregado
             
            int currendyid = OrderContext.Order.AsOrder().CurrencyID;
            int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

            if (ProductPricesExtensions.GetPriceByPriceType(model.ProductID, ppt, currendyid) <= 0)
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductNotActive", "The added product is not active") });

            #endregion

            return PerformAddToCart(model.ProductID, model.Quantity, null, null);
        }

        public virtual ActionResult addToCartKitStart(int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            return PerformAddToCartKitStart(productId, quantity, parentGuid, dynamicKitGroupId);
        }

        public virtual ActionResult AddToCart(int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            // Developed by BAL - CSTI - A02

            if (quantity <= 0)
            {
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });
            }
            var account = GetEnrollingAccount();

            #region ExistsProductInOrder & Product Quota

            bool ExistsProductInOrder = false;
            int NewQuantity = 0;
            var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0 ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID.Equals(productId)) : null;
            if (itemAdded != null) { ExistsProductInOrder = true; NewQuantity = itemAdded.Quantity + quantity; }

            #endregion

            if (ProductQuotasRepository.ProductIsRestricted(productId, ExistsProductInOrder ? NewQuantity : quantity, account.AccountID, account.AccountTypeID))
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });

            #region Valida Precios Activos del Producto agregado

            int currendyid = OrderContext.Order.AsOrder().CurrencyID;
            int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

            if (ProductPricesExtensions.GetPriceByPriceType(productId, ppt, currendyid) <= 0)
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductNotActive", "The added product is not active") });

            #endregion

            return PerformAddToCart(productId, quantity, parentGuid, dynamicKitGroupId);
        }


        private JsonResult HasChangedKit(int productId, int newQuantity)
        {
            var queryOrderITems = OrderContext.Order.OrderCustomers.First().OrderItems;

            var validationKit = PreOrderExtension.GetKitComposition(productId);
            var OrderItemQuantity = 0;
            var queryOrderItem = queryOrderITems.FirstOrDefault(l => l.ProductID == productId && l.ParentOrderItem == null);

            if (queryOrderItem == null)
                return null;

            OrderItemQuantity = queryOrderItem.Quantity;
            var hasChangedQuantity = (newQuantity != OrderItemQuantity);
            if (hasChangedQuantity)
            {
                foreach (var kitChild in ((OrderItem)queryOrderItem).ChildOrderItems)
                {
                    var queryKit = validationKit.Find(o => kitChild.MaterialID != null && ((o.MaterialID == kitChild.MaterialID.Value) &&
                                                                                           (kitChild.Quantity / queryOrderItem.Quantity) == o.Quantity));
                    if (queryKit == null)
                    {
                        return Json(new { result = false, message = Translation.GetTerm("KitChanged", "The kit ({0}) has changed. Please remove and add it again.", ((OrderItem)queryOrderItem).SKU) });
                    }
                }
            }
            return null;
        }

        public virtual void ConsolidateOrderItemsKits(int productId)
        {
            var orderItems = OrderContext.Order.OrderCustomers.First().OrderItems.Where(p => p.ProductID == productId && ((OrderItem)p).MaterialID == null);
            var orderItemsList = orderItems as IList<IOrderItem> ?? orderItems.ToList();
            var listToRemove = new List<Guid>();
            if (orderItemsList.Count() == 2 && ((OrderItem)orderItemsList.First()).ChildOrderItems.Count() > 0)
            {
                var queryFirst = ((OrderItem)orderItemsList.ToList()[0]);
                var querySecond = ((OrderItem)orderItemsList.ToList()[1]);
                queryFirst.Quantity += querySecond.Quantity;
                listToRemove.Add(querySecond.Guid);
                foreach (var child in querySecond.ChildOrderItems)
                {
                    var childtemp = child;
                    var queryChids = OrderContext.Order.OrderCustomers.First()
                        .OrderItems.Where(o => o.ProductID == childtemp.ProductID && ((OrderItem)o).MaterialID == childtemp.MaterialID).ToList();
                    var firstChild = (OrderItem)queryChids[0];
                    var secondChild = (OrderItem)queryChids[1];
                    firstChild.Quantity += secondChild.Quantity;
                    listToRemove.Add(secondChild.Guid);
                }
                OrderContext.Order.OrderCustomers.First().OrderItems.RemoveWhere(o => listToRemove.Contains(((OrderItem)o).Guid));
            }
        }

        [NonAction]
        protected ActionResult PerformAddToCartKitStart(int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, IDictionary<string, string> itemProperties = null)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments
            var account = GetEnrollingAccount();
            try
            {
                var warehouseID = Convert.ToInt32(Session["WareHouseId"]);
                if (quantity == 0)
                {
                    return Json(new
                    {
                        result = false,
                        message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), "0")
                    });
                }

                int wareHouseID = Convert.ToInt32(Session["WareHouseId"]);
                int preOrder = Convert.ToInt32(Session["PreOrder"]);

                string messageValidKit = "";

                #region Validations
                var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0
               ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(
                   x => x.ProductID.Equals(productId))
               : null;

                var hasChangedKit = HasChangedKit(productId, quantity);
                if (hasChangedKit != null) return hasChangedKit;
                #endregion


                bool showOutOfStockMessage = false;
                string outOfStockMessage = "";
                var product = Inventory.GetProduct(productId);
                product.ProductBackOrderBehaviorID = 4;

                var currentQuantity = (itemAdded != null && itemAdded.Quantity > 0) ? itemAdded.Quantity + quantity : quantity;

                OrderContext.Order.WareHouseID = wareHouseID;
                if (itemAdded != null)
                {
                    Order.RemoveLineOrder(productId, wareHouseID, preOrder, CoreContext.CurrentAccount.AccountTypeID, false);
                }

                //This is only for the current product 
                bool isDynamicKit = product.IsDynamicKit();
                bool isBundleGroupComplete = false;
                var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
                OrderItem bundleItem = null;
                Product bundleProduct = null;
                DynamicKitGroup group = null;

                Guid pguid;
                if (!String.IsNullOrWhiteSpace(parentGuid) && Guid.TryParse(parentGuid, out pguid) && dynamicKitGroupId.HasValue)
                {
                    bundleItem = customer.OrderItems.Cast<OrderItem>().FirstOrDefault(oc => oc.Guid == pguid);
                    bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    group = bundleProduct.DynamicKits[0].DynamicKitGroups.Where(g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
                    int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
                    if ((currentCount + quantity) > group.MinimumProductCount)
                    {
                        return Json(new
                        {
                            result = false,
                            message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), quantity)
                        });
                    }
                }

                Session["GeneralParameterOcultarCV"] = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");

                var mod = Create.New<IOrderItemQuantityModification>();
                mod.ProductID = productId;
                mod.Quantity = quantity;
                mod.ModificationKind = OrderItemQuantityModificationKind.Add;
                mod.Customer = OrderContext.Order.OrderCustomers[0];

                if (bundleItem != null && bundleProduct != null && group != null)
                {
                    OrderService.AddOrderItemsToOrderBundle(OrderContext, bundleItem, new IOrderItemQuantityModification[] { mod }, dynamicKitGroupId.Value);
                    int currentCount = bundleItem.ChildOrderItems.Where(i => i.DynamicKitGroupID == dynamicKitGroupId).Sum(i => i.Quantity);
                    isBundleGroupComplete = currentCount == group.MinimumProductCount;
                }
                else
                {
                    OrderContext.Order.PreorderID = preOrder;
                    OrderContext.Order.WareHouseID = warehouseID;
                    OrderContext.Order.ParentOrderID = null;
                    AddOrUpdateOrderItems(new[] { mod });
                    ConsolidateOrderItemsKits(productId);
                } 
                ApplyPaymentPreviosBalance();//KTORRES 27JUL  Add Cart
                OrderContext.Order.ParentOrderID = warehouseID;
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrder(OrderContext);

                Session["Order"] = OrderContext.Order.AsOrder();

                CalculateTotal();
                if (messageValidKit != "")
                {
                    outOfStockMessage = messageValidKit;
                }

                return Json(new
                {
                    totals = Totals,
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext),
                    //applicablePromotionsHTML = this.GetPromotionsHtml(OrderContext.Order.AsOrder()),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,/*CS.03JUL2016.Comentado*/
                    allow = true,
                    showOutOfStockMessage,
                    isBundle = isDynamicKit,
                    bundleGuid = isDynamicKit ? customer.OrderItems.Cast<OrderItem>().FirstOrDefault(i => i.ProductID == product.ProductID && !i.HasChildOrderItems).Guid.ToString("N") : string.Empty,
                    productId = product.ProductID,
                    groupItemsHtml = parentGuid == null ? "" : GetGroupItemsHtml(parentGuid, dynamicKitGroupId.Value).ToString(),
                    isBundleGroupComplete = isBundleGroupComplete,
                    childItemCount = parentGuid == null ? 0 : customer.OrderItems.Cast<OrderItem>().GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
                    orderCustomerId = ((OrderCustomer)customer).OrderCustomerID,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,
                    message = outOfStockMessage,
                    dateEstimated = Convert.ToString(Session["DateEstimated"]),
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder())
                });
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                var excludedProducts = ex.ProductsThatHaveExcludedShipping.Select(prod => prod.Name);
                return Json(new { result = false, message = ex.PublicMessage, products = excludedProducts });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (account != null) ? account.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        

        [NonAction]
        protected ActionResult PerformAddToCart(int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, IDictionary<string, string> itemProperties = null)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments
            var account = GetEnrollingAccount();
            try
            {
                var warehouseID = Convert.ToInt32(Session["WareHouseId"]);
                if (quantity == 0)
                {
                    return Json(new
                    {
                        result = false,
                        message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), "0")
                    });
                }

                int wareHouseID = Convert.ToInt32(Session["WareHouseId"]);
                int preOrder = Convert.ToInt32(Session["PreOrder"]);

                string messageValidKit = ""; 

                #region Validations
                var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0
               ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(
                   x => x.ProductID.Equals(productId))
               : null;

                var hasChangedKit = HasChangedKit(productId, quantity);
                if (hasChangedKit != null) return hasChangedKit;
                #endregion
                
               
                bool showOutOfStockMessage = false;
                string outOfStockMessage = "";
                var product = Inventory.GetProduct(productId);
                product.ProductBackOrderBehaviorID = 4; 

                var currentQuantity = (itemAdded != null && itemAdded.Quantity > 0) ? itemAdded.Quantity + quantity : quantity;

                OrderContext.Order.WareHouseID = wareHouseID;
                if (itemAdded != null)
                {
                    Order.RemoveLineOrder(productId, wareHouseID, preOrder, CoreContext.CurrentAccount.AccountTypeID, false);
                }

                //This is only for the current product
                var queryAllocations = Order.GenerateAllocation(productId, currentQuantity,
                   OrderContext.Order.AsOrder().OrderID, wareHouseID, EntitiesEnums.MaintenanceMode.Add, preOrder,
                   CoreContext.CurrentAccount.AccountTypeID, false);
                foreach (var item in queryAllocations)
                {
                    if (!item.Estado) { return Json(new { result = false, restricted = true, message = item.Message }); }

                    if ((item.EstatusNewQuantity) || (item.Estado && string.IsNullOrEmpty(item.Message) && item.NewQuantity > 0))
                    {
                        messageValidKit = item.Message;
                        quantity = item.NewQuantity;
                    }
                }


                bool isDynamicKit = product.IsDynamicKit();
                bool isBundleGroupComplete = false;
                var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
                OrderItem bundleItem = null;
                Product bundleProduct = null;
                DynamicKitGroup group = null;

                Guid pguid;
                if (!String.IsNullOrWhiteSpace(parentGuid) && Guid.TryParse(parentGuid, out pguid) && dynamicKitGroupId.HasValue)
                {
                    bundleItem = customer.OrderItems.Cast<OrderItem>().FirstOrDefault(oc => oc.Guid == pguid);
                    bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    group = bundleProduct.DynamicKits[0].DynamicKitGroups.Where(g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
                    int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
                    if ((currentCount + quantity) > group.MinimumProductCount)
                    {
                        return Json(new
                        {
                            result = false,
                            message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), quantity)
                        });
                    }
                }

                Session["GeneralParameterOcultarCV"] = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
               
                    var mod = Create.New<IOrderItemQuantityModification>();
                    mod.ProductID = productId;
                    mod.Quantity = quantity;
                    mod.ModificationKind = OrderItemQuantityModificationKind.Add;
                    mod.Customer = OrderContext.Order.OrderCustomers[0];
                
                    if (bundleItem != null && bundleProduct != null && group != null)
                    {
                        OrderService.AddOrderItemsToOrderBundle(OrderContext, bundleItem, new IOrderItemQuantityModification[] { mod }, dynamicKitGroupId.Value);
                        int currentCount = bundleItem.ChildOrderItems.Where(i => i.DynamicKitGroupID == dynamicKitGroupId).Sum(i => i.Quantity);
                        isBundleGroupComplete = currentCount == group.MinimumProductCount;
                    }
                    else
                    {
                        OrderContext.Order.PreorderID = preOrder;
                        OrderContext.Order.WareHouseID = warehouseID;
                        OrderContext.Order.ParentOrderID = null;
                        AddOrUpdateOrderItems(new[] { mod });
                        ConsolidateOrderItemsKits(productId);
                    }

                
                ApplyPaymentPreviosBalance();//KTORRES 27JUL  Add Cart
                OrderContext.Order.ParentOrderID = warehouseID;                
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrder(OrderContext);

                var orderEntryModel = GetOrderEntryModelData(OrderContext.Order.AsOrder()); 

                Session["Order"] = OrderContext.Order.AsOrder();

                CalculateTotal();
                if (messageValidKit != "")
                {
                    outOfStockMessage = messageValidKit;
                }
                actBalanceDue();
           
                return Json(new
                {
                    totals = Totals,
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext),
                    //applicablePromotionsHTML = this.GetPromotionsHtml(OrderContext.Order.AsOrder()),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,/*CS.03JUL2016.Comentado*/
                    allow = true,
                    showOutOfStockMessage,
                    isBundle = isDynamicKit,
                    bundleGuid = isDynamicKit ? customer.OrderItems.Cast<OrderItem>().FirstOrDefault(i => i.ProductID == product.ProductID && !i.HasChildOrderItems).Guid.ToString("N") : string.Empty,
                    productId = product.ProductID,
                    groupItemsHtml = parentGuid == null ? "" : GetGroupItemsHtml(parentGuid, dynamicKitGroupId.Value).ToString(),
                    isBundleGroupComplete = isBundleGroupComplete,
                    childItemCount = parentGuid == null ? 0 : customer.OrderItems.Cast<OrderItem>().GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
                    orderCustomerId = ((OrderCustomer)customer).OrderCustomerID,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,             
                    message = outOfStockMessage,
                    dateEstimated = Convert.ToString(Session["DateEstimated"]),
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder())
                });
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                var excludedProducts = ex.ProductsThatHaveExcludedShipping.Select(prod => prod.Name);
                return Json(new { result = false, message = ex.PublicMessage, products = excludedProducts });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (account != null) ? account.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        public bool StepHasAnItemInStockToBeChosen(string stepId)
        {
            try
            {
                var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
                var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
                var inventoryService = Create.New<IInventoryService>();
                var options = step.AvailableOptions.Select(o =>
                {
                    var product = Inventory.GetProduct(o.ProductID);
                    return inventoryService.GetProductAvailabilityForOrder(OrderContext, o.ProductID, 1);
                });
                foreach (var option in options)
                {
                    if (option.CanAddBackorder > 0 || option.CanAddNormally > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return false;
            }
        }

        //protected virtual object GetApplicablePromotions(Order order)
        //{
        //    var promotionAdjustments = order.OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
        //    var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
        //    return adjustments.Select(adj =>
        //    {
        //        bool isAvailable = false;
        //        if (adj.InjectedOrderSteps.Count() > 0)
        //        {
        //            foreach (var step in adj.InjectedOrderSteps)
        //            {
        //                isAvailable = StepHasAnItemInStockToBeChosen(step.OrderStepReferenceID);
        //                if (isAvailable)
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            isAvailable = !adj.OrderModifications.Any(om => om.ModificationDescription.Contains("Unable"));
        //        }
        //        var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
        //            (os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
        //        return new { Description = adj.Description, StepID = giftStep == null ? null : giftStep.OrderStepReferenceID.ToString(), isAvailable };
        //    });
        //}

        protected virtual object GetOrderItemHtml(Order order, OrderItem orderItem)
        {
            ViewDataDictionary vdd = new ViewDataDictionary();

            var autoshipSchedule = Session["AutoshipSchedule" + "_" + order.OrderID.ToString()] as AutoshipSchedule;
            bool fixedAutoship = autoshipSchedule != null && autoshipSchedule.AutoshipScheduleProducts.Count > 0;

            vdd.Add("FixedAutoship", fixedAutoship);
            vdd.Add("CurrencyID", order.CurrencyID);

            string result = RenderPartialToString("~/Areas/Orders/Views/Shared/PartialOrderEntryLineItem.ascx", vdd, model: orderItem);

            return result;
        }

        protected virtual List<object> GetOrderItemsHtml(Order order)
        {
            ViewDataDictionary vdd = new ViewDataDictionary();
            List<object> orderItems = new List<object>();
            OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
            var addedItemOperationID = (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
            var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
            var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);
            foreach (OrderItem orderItem in nonPromotionalItems)
            {
                orderItems.Add(new
                {
                    orderItemId = orderItem.Guid.ToString("N"),
                    orderItem = GetOrderItemHtml(order, orderItem)
                });
            }
            //if (adjustments.Count() > 0)
            //{
            //    orderItems.Add(new
            //    {
            //        orderItemId = Guid.NewGuid(),
            //        orderItem = RenderPartialToString("~/Areas/Orders/Views/Shared/PartialOrderEntryPromotionItems.ascx", vdd, model: adjustments)
            //    });
            //}
            return orderItems;
        }




        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public ActionResult SaveBundle(string bundleGuid)
        {
            var account = GetEnrollingAccount();
            try
            {
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                var orderItem = customer.OrderItems.GetByGuid(bundleGuid);
                var kitProduct = Inventory.GetProduct(orderItem.ProductID.Value);
                if (kitProduct == null)
                    return Json(new { result = false, message = string.Format("Could not find a product with SKU '{0}'", kitProduct.SKU) });

                if (!Order.IsDynamicKitValid(orderItem))
                    return Json(new { result = false, message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", kitProduct.Translations.Name()) });

                OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    result = true,
                    itemsInCart = customer.OrderItems.Count,
                    total = OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID),
                    productName = kitProduct.Translations.Name(),
                    image = kitProduct.MainImage == null ? "" : kitProduct.MainImage.FilePath.ReplaceFileUploadPathToken()//,
                    //orderItems = GetFormattedCartPreview().ToString()
                });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }
        #endregion

        #region Shipping Method Helpers
        protected virtual ShippingMethodModel ShippingMethod_CreateModel()
        {
            return new ShippingMethodModel();
        }

        protected virtual void ShippingMethod_LoadModel(ShippingMethodModel model, bool active)
        {
            var order = GetInitialOrder();

            // These must be called in this order
            ShippingMethod_LoadResources(model, order);
            ShippingMethod_LoadValues(model, order);
        }

        protected virtual void ShippingMethod_LoadResources(ShippingMethodModel model, Order order)
        {
            var shippingMethods = order.GetShippingMethods(order.GetDefaultShipment());

            model.LoadResources(
                    shippingMethods,
                    order
            );

            LoadPageHtmlContent();
        }

        protected virtual void ShippingMethod_LoadValues(ShippingMethodModel model, Order order)
        {
            int? orderShippingMethodID = order.GetDefaultShippingMethodID();

            if (orderShippingMethodID == null
                    || model.ShippingMethods == null
                    || !model.ShippingMethods.Any(x => x.ShippingMethodID == orderShippingMethodID))
            {
                model.SelectedShippingMethodID = null;
            }
            else
            {
                model.SelectedShippingMethodID = model.ShippingMethods
                        .First(x => x.ShippingMethodID == orderShippingMethodID.Value)
                        .ShippingMethodID;
            }
        }
        #endregion

        #region Billing Helpers

        [EnrollmentStepSection]
        public virtual ActionResult Billing()
        {
            return SectionsView();
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult Billing(BillingModel model)
        {
            var account = GetEnrollingAccount();

            if (!ModelState.IsValid)
            {
                Billing_LoadResources(model, account);
                return SectionsView(model);
            }

            try
            {
                // Apply updates
                if (!_enrollmentContext.EnrollmentConfig.Billing.HideBillingAddress)
                {
                    Billing_UpdateAddress(model, account);
                }

                Billing_UpdateAccountPaymentMethod(model, account);
                account.Save();

                // Update context
                Billing_UpdateContext(model);
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                Billing_LoadResources(model, account);
                return SectionsView(model);
            }

            _enrollmentContext.EnrollingAccount = account;
            return SectionCompleted();
        }

        protected virtual BillingModel Billing_CreateModel()
        {
            return new BillingModel();
        }

        protected virtual void Billing_LoadModel(BillingModel model, bool active)
        {
            var account = GetEnrollingAccount();
            var billingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing) ?? new Address();
            var accountPaymentMethod = GetAccountPaymentMethod(account, GetPaymentTypeIDs()) ?? new AccountPaymentMethod();

            model.LoadValues(
                _enrollmentContext.BillingAddressSourceTypeID,
                _enrollmentContext.CountryID,
                _enrollmentContext.LanguageID,
                billingAddress,
                accountPaymentMethod);

            // We only need this if the section is active
            if (active)
            {
                Billing_LoadResources(model, account);
            }
        }

        protected virtual void Billing_LoadResources(BillingModel model, Account account)
        {
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping) ?? new Address();
            var paymentTypes = GetPaymentTypes().Cast<PaymentType>();
            model.LoadResources(
                mainAddress,
                shippingAddress,
                paymentTypes,
                _enrollmentContext.EnrollmentConfig.Billing.HideBillingAddress
            );
        }

        protected virtual void Billing_UpdateAddress(BillingModel model, Account account)
        {
            // Load/Create Address
            var billingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing);
            if (billingAddress == null)
            {
                billingAddress = new Address();
                billingAddress.AddressTypeID = (short)Constants.AddressType.Billing;
                billingAddress.IsDefault = true;
                billingAddress.ProfileName = Translation.GetTerm("DefaultBilling", "Default Billing");
                account.Addresses.Add(billingAddress);
            }

            // These are used if BillingAddressSource is set
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping) ?? new Address();

            // Set properties from model
            model.ApplyTo(billingAddress, mainAddress, shippingAddress);
        }

        protected virtual void Billing_UpdateAccountPaymentMethod(BillingModel model, Account account)
        {
            // Load/Create AccountPaymentMethod
            var accountPaymentMethod = GetAccountPaymentMethod(account, GetPaymentTypeIDs());
            if (accountPaymentMethod == null)
            {
                accountPaymentMethod = new AccountPaymentMethod();
                accountPaymentMethod.IsDefault = true;
                accountPaymentMethod.ProfileName = Translation.GetTerm("DefaultBilling", "Default Billing");
                account.AccountPaymentMethods.Add(accountPaymentMethod);
            }

            // Set properties from model
            model.ApplyTo(accountPaymentMethod);

            // Update address on account payment method
            UpdateAccountPaymentMethodAddress(account);
        }

        protected virtual void Billing_UpdateContext(BillingModel model)
        {
            model.ApplyTo(_enrollmentContext);
        }
        #endregion

        #region Website Helpers

        [EnrollmentStepSection]
        public virtual ActionResult Website()
        {
            return SectionsView();
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult Website(WebsiteModel model)
        {
            var account = GetEnrollingAccount();

            if (!ModelState.IsValid)
            {
                Website_LoadResources(model);
                return SectionsView(model);
            }

            // Validate URL availability
            if (!IsSiteUrlAvailable(account, model.Subdomain))
            {
                ModelState.AddModelError("Subdomain", _errorUrlUnavailableString);
                Website_LoadResources(model);
                return SectionsView(model);
            }

            try
            {
                // Update context
                Website_UpdateContext(model, account);
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                Website_LoadResources(model);
                return SectionsView(model);
            }

            return SectionCompleted();
        }

        public virtual ActionResult WebsiteRequirementsUnmet()
        {
            return SectionCompleted();
        }

        public virtual ActionResult CheckUrlAvailability(string subdomain)
        {
            var account = GetEnrollingAccount();
            try
            {
                return Json(new { available = !string.IsNullOrWhiteSpace(subdomain) && IsSiteUrlAvailable(account, subdomain) });
            }
            catch (Exception ex)
            {
                return JsonError(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
            }
        }

        protected virtual WebsiteModel Website_CreateModel()
        {
            return new WebsiteModel();
        }

        protected virtual void Website_LoadModel(WebsiteModel model, bool active)
        {
            //MontoMinimoWebSiteEnroll obj = new MontoMinimoWebSiteEnroll();
            var datos = RequirementRule.ObtenerMontoMinimoWebSiteEnroll();
            decimal MontoMinimoWebSite = 0;
            if (datos == null)
                MontoMinimoWebSite = 0;
            else
                MontoMinimoWebSite = Convert.ToDecimal(datos.Valor);
            model.LoadValues(_enrollmentContext.SiteSubscriptionUrl,
                _enrollmentContext.EnrollmentConfig.Steps.CurrentItem.Sections.CurrentItem.Skippable,
                MontoMinimoWebSite,
                _enrollmentContext.InitialOrder.Subtotal.GetValueOrDefault());
            Website_LoadResources(model);
        }

        protected virtual void Website_LoadResources(WebsiteModel model)
        {
            model.LoadResources(GetPWSDomain());
            LoadPageHtmlContent();
        }

        protected virtual void Website_UpdateContext(WebsiteModel model, Account account)
        {
            model.ApplyTo(_enrollmentContext);
        }
        #endregion

        #region Other Helpers
        protected virtual ActionResult SectionsView(SectionModel currentSectionModel = null)
        {
            return View("Index", GetSectionModels(currentSectionModel));
        }

        protected virtual OrderedList<IEnrollmentStepSectionConfig> GetSections()
        {
            return _enrollmentContext.EnrollmentConfig.Steps.CurrentItem.Sections;
        }

        protected virtual IEnumerable<SectionModel> GetSectionModels(SectionModel currentSectionModel = null)
        {
            var sections = GetSections();
            var models = new List<SectionModel>();
            char titleIndex = 'a';
            byte payment = 0;
            foreach (var section in sections)
            {
                payment++;
                SectionModel model = null;
                bool active = section == sections.CurrentItem;
                //if (payment == 1) active = false;
                //else if (payment == 4) active = true;
                // Only "load" models that are going to be rendered (i.e. active & completed sections).
                bool loadModel = active || section.Completed;

                // If this is a post, the current model is already loaded.
                if (active)
                {
                    model = currentSectionModel;
                }

                if (model == null)
                {
                    // Create and load the model using the appropriate section methods.
                    model = CreateSectionModel(section);
                    if (loadModel)
                    {
                        LoadSectionModel(section, model, active);
                    }
                }

                if (model == null)
                {
                    throw new Exception(string.Format("Error loading Products model for section '{0}'.", section.Name));
                }

                model.LoadBaseResources(
                        active,
                        section.Action,
                        string.Format("{0}. {1}", titleIndex++, Translation.GetTerm(section.TermName, section.Name)),
                        string.Format("_{0}{1}", section.Action, active ? "Edit" : "Details"),
                        section.Completed
                );
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Finds and calls the appropriate 'CreateModel' method to instantiate a new section model.
        /// Every section in the enrollment config must have a corresponding 'CreateModel' method
        /// in this controller and the method must be named [Action]_CreateModel.
        /// Example: protected virtual EnrollmentKitsModel EnrollmentKits_CreateModel()
        /// </summary>
        /// <param name="section">The <see cref="SectionConfig"/> object describing the section</param>
        /// <returns>A new section model for the provided section</returns>
        protected virtual SectionModel CreateSectionModel(IEnrollmentStepSectionConfig section)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }

            // The controller type to search
            var type = this.GetType();

            // The method name to find
            var createModelMethodName = section.Action + "_CreateModel";

            // Find the 'CreateModel' method
            var createModelMethod = type
                    .FindMembers(
                            System.Reflection.MemberTypes.Method,
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                            Type.FilterNameIgnoreCase,
                            createModelMethodName
                    )
                    .FirstOrDefault()
                    as System.Reflection.MethodBase;

            if (createModelMethod == null)
            {
                throw new InvalidOperationException(string.Format("Method '{0}' not found on type '{1}'.",
                        createModelMethodName, type.Name));
            }

            // Invoke the 'CreateModel' method and return the result
            return createModelMethod.Invoke(this, null) as SectionModel;
        }

        /// <summary>
        /// Finds and calls the appropriate 'LoadModel' method to load a section model.
        /// Every section in the enrollment config must have a corresponding 'LoadModel' method in this
        /// controller, the method must be named [Action]_LoadModel, and must take the same three arguments.
        /// Example: protected virtual void EnrollmentKits_LoadModel(EnrollmentKitsModel model, bool active)
        /// </summary>
        /// <param name="section">The <see cref="SectionConfig"/> object describing the section</param>
        /// <returns>A new section model for the provided section</returns>
        protected virtual SectionModel LoadSectionModel(IEnrollmentStepSectionConfig section, SectionModel model, bool active)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            // The controller type to search
            var type = this.GetType();

            // The method name to find
            var loadModelMethodName = section.Action + "_LoadModel";

            // Find the 'LoadModel' method
            var loadModelMethod = type
                    .FindMembers(
                            System.Reflection.MemberTypes.Method,
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                            Type.FilterNameIgnoreCase,
                            loadModelMethodName
                    )
                    .FirstOrDefault()
                    as System.Reflection.MethodBase;

            if (loadModelMethod == null)
            {
                throw new InvalidOperationException(string.Format("Method '{0}' not found on type '{1}'.",
                        loadModelMethodName, type.Name));
            }

            // Invoke the 'LoadModel' method
            loadModelMethod.Invoke(this, new object[] { model, active });

            return model;
        }

        protected virtual ActionResult SectionCompleted()
        {
            var sections = GetSections();

            sections.CurrentItem.Completed = true;

            if (!string.IsNullOrWhiteSpace(_enrollmentContext.ReturnUrl))
            {
                string returnUrl = _enrollmentContext.ReturnUrl;
                _enrollmentContext.ReturnUrl = null;
                return Redirect(returnUrl);
            }

            if (sections.HasNextItem)
            {
                return RedirectToAction(sections.NextItem.Action);
            }
            else
            {
                return StepCompleted();
            }
        }

        private decimal CreditApplied()
        {

           
            var getBalanceDue = Totals.GetType().GetProperty("balanceDue").GetValue(Totals, null);
            //string[] strBalanceDue = Convert.ToString(getBalanceDue).Split('$');
            //decimal balancDue = 0;
            //balancDue = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(Convert.ToDecimal(strBalanceDue[1].ToString()));
            decimal balancDue = 0;

            balancDue = Convert.ToDecimal(getBalanceDue, CoreContext.CurrentCultureInfo);
            return balancDue;
        }

        
        public virtual ActionResult SubmitOrder(string invoiceNotes, string email, string url)
        {
            try
            {
                /* PayPal_001
                 * Se agrega la funcionalidad para validar que el pago con tarjeta de credito se halla realizado
                 * Author : JMorales
                 * Date   : 01/09/2016
                 */
                int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
                if (countryId == (int)Constants.Country.UnitedStates)
                {
                    if (url != "")
                    {
                        var account = Account.LoadByAccountNumber(OrderContext.Order.OrderCustomers[0].AccountID.ToString());
                        SaveSiteSubscriptions(_enrollmentContext.SiteID, account.FullName, null, 1, _enrollmentContext.LanguageID, null);
                        SaveSiteUrls(account, url);
                        //Site site = new Site
                        //{
                        //    AccountID = OrderContext.Order.OrderCustomers[0].AccountID,
                        //    AccountNumber = OrderContext.Order.OrderCustomers[0].AccountID.ToString(),
                        //    CreatedByUserID = ApplicationContext.Instance.CurrentUserID,
                        //    BaseSiteID = 3,
                        //    MarketID = (int)Constants.Market.USA,
                        //    IsBase = false,
                        //    DateCreated = DateTime.Now,
                        //    DateSignedUp = DateTime.Now,
                        //    SiteTypeID = (int)Constants.SiteType.Replicated,
                        //    Name = url,
                        //    DefaultLanguageID = 1,
                        //    SiteStatusID = (short)Constants.SiteStatus.Active
                        //};
                        //site.Save();

                        //site = Site.LoadByAccountID(OrderContext.Order.OrderCustomers[0].AccountID).FirstOrDefault();
                        //if (SiteUrl.IsAvailable(site.SiteID, url))
                        //{
                        //    if (site.SiteUrls.Count == 0)
                        //        site.SiteUrls.Add(new SiteUrl());

                        //    site.SiteUrls[0].StartEntityTracking();
                        //    site.SiteUrls[0].Url = url;
                        //    site.SiteUrls[0].Save();
                        //}
                        }
                    }

                if (!validaPayPal_Paid())
                {
                    return Json(new { result = false, message = Translation.GetTerm("NoPayPalPaid", "Erro de pagamento processamento, tente novamente") });
                }

                /* End PayPal_001*/
                
                int PreOrderId = Convert.ToInt32(Session["PreOrder"]);
               
                if (!invoiceNotes.ToCleanString().IsNullOrEmpty())
                    OrderContext.Order.AsOrder().InvoiceNotes = invoiceNotes;

                // Submit the order
                OrderContext.Order.AsOrder().IsTemplate = true;
                OrderContext.Order.AsOrder().Balance = 0;
                DateTime _dateCurrent = DateTime.Now;
                decimal totalAppliedAmount = 0;

                PaymentsTable objE = new PaymentsTable();
                objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                var paymentTables = Order.GetPaymentsTable(objE);

                int contadorMenos = 0;
                foreach (var item in OrderContext.Order.AsOrder().OrderPayments)
                {
                    contadorMenos--;
                    foreach (var paymentTable in paymentTables)
                    {

                        if (contadorMenos == paymentTable.OrderPaymentId)
                        {

                            totalAppliedAmount = totalAppliedAmount + paymentTable.AppliedAmount.ToDecimal();

                            if (paymentTable.PaymentStatusID == 4)
                            {
                                item.OrderPaymentStatusID = 2;
                            }
                            else if (paymentTable.PaymentStatusID == 11 || paymentTable.PaymentStatusID == 18)
                            {
                                item.OrderPaymentStatusID = 1;
                            }

                            if (item.TransactionID == null)
                                item.TransactionID = paymentTable.AutorizationNumber;

                            item.PaymentTypeID = paymentTable.PaymentType;
                            item.ExpirationDateUTC = paymentTable.ExpirationDate;
                            item.DateLastModifiedUTC = DateTime.Now;
                            item.CreditCardTypeID = null;
                            item.NameOnCard = null;
                            item.BillingCity = null;
                            item.BillingCountryID = null;
                            item.BillingState = null;
                            item.BillingStateProvinceID = null;
                            item.BillingPostalCode = null;
                            item.BillingCountry = null;
                            item.BillingPostalCode = null;
                            item.BillingPhoneNumber = null;
                            item.IdentityNumber = null;
                            item.IdentityState = null;
                            item.RoutingNumber = null;

                            item.DeferredAmount = (paymentTable.NumberCuota.HasValue) ? paymentTable.NumberCuota : 0;
                            item.DeferredTransactionID = null;
                            //item.DataVersion = null;
                            item.Request = null;
                            item.AccountNumberLastFour = null;

                            item.SourceAccountPaymentMethodID = null;
                            item.BankAccountTypeID = null;
                            item.BankName = null;
                            item.NachaClassType = null;
                            item.NachaSentDate = null;
                            item.ExpirationStatusID = (int)ConstantsGenerated.ExpirationStatuses.Unexpired;
                        }
                    }
                }

                int shippingOrderType = OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID.Value;
                OrderContext.Order.AsOrder().ParentOrderID = null;
                OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID = Order.GetShippingMethodID(shippingOrderType);
                var result = OrderService.SubmitOrder(OrderContext);

                if (!result.Success)
                {
                    return Json(new { result = false, validrule = false, message = result.Message, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
                }
                else
                {
                    if (Session["ProductCredit"] == null)
                    {                        
                        Session["ProductCredit"] = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);                     
                    }
                    //========================================================================================
                    //Actualizar los campos nuevos
                    contadorMenos = 0;
                    var OrderPayment = OrderContext.Order.AsOrder().OrderPayments;

                    foreach (var item in OrderPayment)
                    {
                        contadorMenos--;

                        foreach (var paymentTable in paymentTables)
                        {
                            if (contadorMenos == paymentTable.OrderPaymentId)
                            {
                                OrderPaymentsParameters objEP = new OrderPaymentsParameters();
                                objEP.InitialAmount = paymentTable.AppliedAmount.ToDecimal();
                              
                                objEP.ProcessedDateUTC = null; objEP.ProcessOnDateUTC = null;
                                if (paymentTable.PaymentStatusID.Equals(4))
                                {
                                    objEP.ProcessedDateUTC = DateTime.Now;
                                    objEP.ProcessOnDateUTC = DateTime.Now;
                                }                              

                                var accountID = OrderContext.Order.OrderCustomers[0].AccountID;
                                objEP.ModifiedByUserID = AccountPropertiesBusinessLogic.GetValueByID(5, accountID).CreatedByUserID;
                                objEP.ProductCredit = decimal.Parse(Session["ProductCredit"].ToString());

                                 Order.UPDPaymentConfigurations(item.OrderPaymentID,
                                   item.OrderID,
                                   paymentTable.PaymentConfigurationID.Value,
                                   paymentTable.NumberCuota
                                   , objEP, OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ANM"));

                                 if ((decimal.Parse(Session["ProductCredit"].ToString()) != 0) && paymentTable.PaymentConfigurationID.Value == 60)
                                 {
                                     decimal productCredit = decimal.Parse(Session["ProductCredit"].ToString());
                                     var endingAmount = CreditApplied();
                                     decimal EntryAmount = endingAmount - productCredit;

                                     int EntryReasonID = (endingAmount > 0) ? 5 : 10;
                                     int EntryTypeID = Constants.LedgerEntryOrigins.OrderEntry.ToInt();
                                     int EntryOrigin = Constants.LedgerEntryOrigins.OrderEntry.ToInt();

                                     NetSteps.Data.Entities.Business.CTE.ApplyCredit(OrderContext.Order.AsOrder().ConsultantID, EntryReasonID, EntryOrigin, EntryTypeID, EntryAmount, item.Order.AsOrder().OrderID, item.OrderPaymentID);
                                 }
                                 

                                ValidateSendEmailBoleto(item.OrderPaymentID);

                            }
                        }
                    }

                    //========================================================================================
                    // SPOnLineMLM ( BDcommissions) solo cuando la Orden ya esta Pagada ( status: 4 )
                    var OrderStatusID = 
                        AccountPropertiesBusinessLogic.GetValueByID(11, OrderContext.Order.OrderID).OrderStatusID;

                    if (OrderStatusID == Constants.OrderStatus.Paid.ToShort())
                    {
                        PersonalIndicardorAsynExtensions personalIndicardorAsyn = new PersonalIndicardorAsynExtensions();
                        personalIndicardorAsyn.UpdatePersonalIndicatorAsyn(OrderContext.Order.OrderID, OrderStatusID);
                        //OrderExtensions.UpdatePersonalIndicator(OrderContext.Order.OrderID, OrderStatusID);
                        //INI - GR6356 - CDAS              
                        var account = Account.Load(OrderContext.Order.OrderCustomers[0].AccountID);
                        if (account != null && account.AccountStatusID != (short)Constants.AccountStatus.Active)
                        {
                            account.AccountStatusID = (short)Constants.AccountStatus.Active;
                            account.EnrollmentDateUTC = DateTime.Now;
                            account.Save();
                        }
                        //FIN - GR6356 - CDAS
                    }
                    //========================================================================================
                    //INI - GR6356 - CDAS
                    if (OrderStatusID == (short)Constants.OrderStatus.PendingPerPaidConfirmation)
                    {
                        var account = Account.Load(OrderContext.Order.OrderCustomers[0].AccountID);
                        if (account != null && account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment)
                        {
                            account.EnrollmentDateUTC = null;
                            account.Save();
                        }
                    }
                    //FIN - GR6356 - CDAS

                    // se insertar los movimientos de la orden en la tabla  ordertracking  
                    //Se recorre para actualizar los campos que faltan
                    var orderID = OrderContext.Order.AsOrder().OrderID;
                    var postalCode = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode;
                    var warehouseID = Convert.ToInt32(Session["WareHouseId"]);
                    var dateEstimated = Convert.ToString(Session["DateEstimated"]);

                    SetKitItemPrices(OrderContext.Order);

                    var resultOrderShipments = PaymentsMethodsExtensions.ManagementKit(postalCode,
                        warehouseID,
                        orderID,
                        dateEstimated);
                    insertDispatchProductsFill();
                    var scheduler = Create.New<IEventScheduler>();
                    scheduler.ScheduleOrderCompletionEvents(OrderContext.Order.AsOrder().OrderID); 
                    var orderNumber = OrderContext.Order.AsOrder().OrderNumber;
                    ApplyConfigurationCumulative(OrderContext); 
                    _enrollmentContext.EnrollmentComplete = true;
                    OrderContext.Clear();
                    return Json(new { result = true, orderNumber = orderNumber }); 
                    //Insertar los precios para los componentes de kits en la orden
                    //OrderExtensions.InsertKitItemPrices(OrderContext.Order.AsOrder().OrderID);
                }  
                //Actualizar el WareHouseMaterialAllocations
               
                var orderNumbers = OrderContext.Order.AsOrder().OrderNumber;
                OrderContext.Clear();

                _enrollmentContext.EnrollmentComplete = true;
                /*SectionCompleted();*/
                ///*CS.22JUN2016.Comentado*/
                return Json(new { result = true, orderNumber = orderNumbers });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, validrule = false, message = exception.PublicMessage, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
            }
        }

        private void SetKitItemPrices(IOrder order)
        {
            OrderService.AddKitItemPrices(order);
        }


        private void LoadAccountCredit()
        {
            var rep = AccountPropertiesBusinessLogic.GetValueByID(4, CurrentAccount.AccountID);
            var tb = new CreditPaymentTable()
            {
                //Posibles valores P = Puntos , D= Dinero
                TipoCredito = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "TPC"),
                ValorComparacion = "0",
                CreditoDisponible = (rep == null) ? "0" : rep.AccountCreditDis.ToString(),
                EstadoCredito = (rep == null) ? "N" : rep.AccountCreditEst

            };
            NetSteps.Data.Entities.Business.CTE.creditPayment = tb;

        }
        private int ValidateAccountCredit(int PaymentConfiguration)
        {
            Order order = OrderContext.Order.AsOrder();
            int resp = 0;
            LoadAccountCredit();
            var tabl = NetSteps.Data.Entities.Business.CTE.creditPayment;
            decimal? ValorComparacion = 0;

            var AfectaCredito = AccountPropertiesBusinessLogic.GetValueByID(2, PaymentConfiguration).PaymentCredit;
            if (AfectaCredito == "S")
            {
                NetSteps.Data.Entities.Business.CTE.creditPayment.AfectaCredito = "S";
                if (tabl.EstadoCredito == "S")
                {
                    resp = 1;
                }

                if (tabl.EstadoCredito == "N")
                {
                    switch (tabl.TipoCredito)
                    {
                        case "P":
                            ValorComparacion = order.QualificationTotal.ToDecimal();
                            break;
                        case "D":
                            ValorComparacion = order.GrandTotal.ToDecimal();// + CreditBalanceDue =  Balance
                            break;
                        default:
                            break;

                    }

                    NetSteps.Data.Entities.Business.CTE.creditPayment.ValorComparacion = ValorComparacion.ToString().Replace(",", ".");
                    if (tabl.CreditoDisponible.ToDecimal() < ValorComparacion)
                        resp = 2;
                }
            }
            else
            {
                NetSteps.Data.Entities.Business.CTE.creditPayment.AfectaCredito = "N";
            }
            return resp;
        }

        private void inserlog(string msg)
        {
            //Exception ex= new Exception();
            //ex.Message = msg;
            //EntityExceptionHelper.GetAndLogNetStepsException(ex.Message="sasa", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            ErrorLog errorLog = new ErrorLog();
            errorLog.Message = msg;
            errorLog.Save();

        }
        private decimal CreditApplied(decimal totalAppliedAmount)
        {

            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];

            var getBalanceDue = Totals.GetType().GetProperty("balanceDue").GetValue(Totals, null);
            decimal balancDue = 0;
             balancDue = Convert.ToDecimal(getBalanceDue ,CoreContext.CurrentCultureInfo);


            //string[] strBalanceDue = Convert.ToString(getBalanceDue).Split('$');
            //decimal balancDue = 0;
            //if (KeyDecimals == "ES")
            //{
            //    var culture = CultureInfoCache.GetCultureInfo("En");
            //    balancDue = Convert.ToDecimal(strBalanceDue[1].ToString(), culture);
            //}
            //else
            //{
            //    balancDue = Convert.ToDecimal(strBalanceDue[1].ToString().Replace(",", "."));
            

            return balancDue;
        }

        private void ApplyConfigurationCumulative(IOrderContext orderContext)
        {
            int promotionID = 0;
            bool cumulative = false;
            foreach (var mod in orderContext.Order.AsOrder().OrderCustomers
                .SelectMany(oi => oi.OrderItems
                    .SelectMany(oaolm => oaolm.OrderAdjustmentOrderLineModifications)))
            {
                if (mod.OrderAdjustment.Extension is PromotionOrderAdjustment)
                {
                    promotionID = ((PromotionOrderAdjustment)mod.OrderAdjustment.Extension).PromotionID;
                    var promotionQualification = PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic.Instance.GetByPromotionID(promotionID);
                    cumulative = promotionQualification != null ? promotionQualification.Cumulative ?? false : false;
                }
            }

            if (cumulative)
            {
                Order order = OrderContext.Order.AsOrder();
                int accountId = order.OrderCustomers[0].AccountID;
                decimal total = order.OrderCustomers.Select(m => m.OrderItems.Select(oi => oi.ItemPrice * oi.Quantity).Sum()).Sum();

                PromoPromotionLogic.Instance.ApplyConfigurationCumulative(accountId, total, promotionID);
            }

            if (NetSteps.Data.Entities.Business.Logic.PromoPromotionTypeConfigurationsLogic.Instance.GetActiveID() == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.PromotionEngineType.DiscountType4)
            {
                if (promotionID > 0)
                    PromotionConfigurationControlBusinessLogic.Instance.UpdatePromotion(4, promotionID);
            }
        }


        /// <summary>
        /// Returns a list of first-tier categories from the corresponding catalog.
        /// </summary>
        protected virtual IEnumerable<MiniShopCategoryModel> GetCategoryModels(short orderTypeID)
        {
            Catalog catalog = null;

            if (orderTypeID == (short)Constants.OrderType.EnrollmentOrder)
            {
                catalog = Inventory
                        .GetActiveCatalogs(_enrollmentContext.StoreFrontID)
                        .FirstOrDefault(x => x.CatalogTypeID == (short)Constants.CatalogType.EnrollmentItems);
            }
            else if (orderTypeID == (short)Constants.OrderType.AutoshipTemplate)
            {
                catalog = Inventory
                        .GetActiveCatalogs(_enrollmentContext.StoreFrontID)
                        .FirstOrDefault(x => x.CatalogTypeID == (short)Constants.CatalogType.AutoshipItems);
            }

            if (catalog != null)
            {
                // Get all "active" categories - meaning they have products.
                var activeCategories = Inventory.GetActiveCategories(_enrollmentContext.StoreFrontID, (short?)_enrollmentContext.AccountTypeID);
                if (activeCategories != null)
                {
                    // Filter the "active" categories based on the catalog's category tree.
                    var childCategories = activeCategories
                            .Where(x => x.ParentCategoryID == catalog.CategoryID)
                            .OrderBy(x => x.SortIndex).ThenBy(x => x.CategoryID);

                    return childCategories.Select(x => new MiniShopCategoryModel().LoadResources(x, activeCategories, 1));
                }
            }

            return Enumerable.Empty<MiniShopCategoryModel>();
        }
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Update(List<ProductQuantityContainer> products)
        {
            try
            {
                if (OrderContext.Order == null)
                    return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });

                if (products == null || products.Count == 0)
                    return Json(new { result = false, message = Translation.GetTerm("ErrorUpdatingCartNoItems", "Error updating cart. No items specified.") });

                #region ExistsProductInOrder & Product Quota

          
                #endregion

                int productID = 0;
                int quantity = 0;
                var queryOrderITems = OrderContext.Order.OrderCustomers.First().OrderItems;

                products.Each(u => u.HasQuantityChange = true);
                foreach (var prod in queryOrderITems.Where(i => ((OrderItem)i).ChildOrderItems.Count > 0))
                {
                       var queryNewProducts = products.FirstOrDefault(l => l.ProductID == prod.ProductID.Value);
                    if (queryNewProducts != null)
                    {
                        var hasChangedKit = HasChangedKit(prod.ProductID.Value, queryNewProducts.Quantity);
                        if (hasChangedKit != null) return hasChangedKit;                  
                    }                    
                }

                foreach (var noKitOrderItem in queryOrderITems.Where(i => ((OrderItem)i).ChildOrderItems.Count == 0))
                {
                    var queryProducts = products.FirstOrDefault(o => o.ProductID == noKitOrderItem.ProductID.Value);
                    var queryOrderItemData = queryOrderITems.FirstOrDefault(l => noKitOrderItem.ProductID != null && l.ProductID == noKitOrderItem.ProductID.Value);
                    if (queryOrderItemData != null && (queryProducts != null))
                        queryProducts.HasQuantityChange = (queryProducts.Quantity != queryOrderItemData.Quantity);
                }
                var orderCustomer = (OrderCustomer)OrderContext.Order.OrderCustomers[0];

                foreach (var item in products.Where(n => n.HasQuantityChange))
                {
                    if (ProductQuotasRepository.ProductIsRestricted(item.ProductID, item.Quantity, CoreContext.CurrentAccount.AccountID, CoreContext.CurrentAccount.AccountTypeID))
                        return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });
                }
                
                var changes = products.Where(n => n.HasQuantityChange)
                    .Select(item =>
                    {
                        var mod = Create.New<IOrderItemQuantityModification>();
                        mod.ProductID = item.ProductID;
                        mod.Quantity = item.Quantity;
                        mod.ModificationKind = OrderItemQuantityModificationKind.SetToQuantity;
                        mod.Customer = orderCustomer;
                        return mod;
                    });
                string messageValidKit = "";
               
                foreach (var p in products)
                {
                    foreach (var item in Order.GenerateAllocation(p.ProductID,
                                                                  p.Quantity,
                                                                  OrderContext.Order.AsOrder().OrderID,
                                                                  Convert.ToInt32(Session["WareHouseId"]),
                                                                  EntitiesEnums.MaintenanceMode.Update, 
                                                                  Convert.ToInt32(Session["PreOrder"]),                                                                  
                                                                  CoreContext.CurrentAccount.AccountTypeID, false))
                    {
                        if (!item.Estado)
                        {
                            return Json(new { result = false, restricted = true, message = item.Message });
                        }
                        else if (item.EstatusNewQuantity)
                        {
                            messageValidKit = item.Message;
                            quantity = item.NewQuantity;
                        }
                        else if (item.Estado && item.Message != "" && item.NewQuantity > 0)
                        {
                            messageValidKit = item.Message;
                            p.Quantity = item.NewQuantity;
                        }
                    }
                }
                       
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
               
                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                OrderService.UpdateOrder(OrderContext);
                //KTORRES 27JUL     Update cart  
                ApplyPaymentPreviosBalance();
                Session["Order"] = OrderContext.Order.AsOrder();

                var outOfStockProducts = GetOutOfStockProducts(orderCustomer);
                ViewBag.OutOfStockProducts = outOfStockProducts;
                bool validTotalQv = true;
                //var valueDecimal = PreOrderExtension.GetValueURL();
                //decimal valueTotalQV = valueDecimal[0];
                //var totlaQv = OrderContext.Order.AsOrder().QualificationTotal;
                //if (totlaQv <= valueTotalQV)
                //{
                //    validTotalQv = false;
                //}
                //ViewBag.TotalQV = valueTotalQV;

                return Json(new
                {
                    totals = Totals,
                    result = true,
                    subtotal = orderCustomer.Subtotal.ToString(OrderContext.Order.CurrencyID),
                    adjustedSubtotal = orderCustomer.AdjustedSubTotal.ToString(OrderContext.Order.CurrencyID),
                    outOfStockProducts = outOfStockProducts,
                    CartModel = GetCartModelData(OrderContext.Order.AsOrder()),
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder())//,
                    //validTotalQv
                });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void ValidatePaymentByMarket()
        {
            // N : BRASIL
            // S : USA
            Session["GeneralParameterVal"] = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ANM");

        }

        public virtual ActionResult IsCreditCard(int CollectionEntityID)
        {
            var IsTarget = NetSteps.Data.Entities.Business.PaymentMethods.IsTargetCreditByPaymentConfiguration(CollectionEntityID);
            var numberTarget = NetSteps.Data.Entities.Business.PaymentMethods.GetNumberCuotasByPaymentConfigurationID(CollectionEntityID);
            int PaymentTypeID = Order.GetApplyPaymentType(CollectionEntityID);
            if (!IsTarget)
            {
                if (PaymentTypeID == 11 && OrderContext.Order.OrderStatusID == 1)
                {
                    return Json(new { result = true, IsBoleta = true, totals = Totals }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = true, numberTarget = numberTarget }, JsonRequestBehavior.AllowGet);
            }
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Remove(string orderItemGuid, string parentGuid = null, int? quantity = null)
        {
            try
            {
                if (OrderContext.Order == null)
                {
                    return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });
                }
                //var Sku = string.Format("{0:00000}", Convert.ToInt32(orderItemGuid));
                var customer = OrderContext.Order.AsOrder().OrderCustomers.First();
                var item = customer.OrderItems.FirstOrDefault(oi => (oi as OrderItem).Guid.ToString("N") == orderItemGuid);
                var dynamicKitGroupId = (item as OrderItem).DynamicKitGroupID;//var item = customer.OrderItems.FirstOrDefault(oi => oi.SKU.ToString() == orderItemGuid);
                if (item == null)
                    return Json(new { result = false, message = string.Format("Could not find item '{0}'", orderItemGuid) });
                List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>();

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsOld.Add(new OrderItemUpdate() { Product = productRemove.ProductID.Value, Quantity = productRemove.Quantity, status = false });
                }

                var removeModification = Create.New<IOrderItemQuantityModification>();
                removeModification.Customer = customer;
                removeModification.ModificationKind = OrderItemQuantityModificationKind.Delete;
                removeModification.ProductID = item.ProductID.Value;
                removeModification.Quantity = 0;
                removeModification.ExistingItem = item;
                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                ApplyPaymentPreviosBalance();
                var changes = new IOrderItemQuantityModification[] { removeModification };
                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                OrderContext.Order.ParentOrderID = Convert.ToInt32(Convert.ToInt32(Session["WareHouseId"]));
                OrderService.UpdateOrder(OrderContext);

                int Valuequantity = 0;
                Valuequantity = item.Quantity;

                OrderItem bundleItem = null;

                if (!parentGuid.IsNullOrEmpty() && dynamicKitGroupId.HasValue)
                {
                    bundleItem = customer.OrderItems.FirstOrDefault(oc => oc.Guid.ToString("N") == parentGuid);
                }

                bool isBundleGroupComplete = false;
                if (bundleItem != null)
                {
                    var bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    var group =
                        bundleProduct.DynamicKits[0].DynamicKitGroups.Where(
                            g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
                    int currentCount =
                        bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId)
                            .Sum(oi => oi.Quantity);
                    isBundleGroupComplete = currentCount == group.MinimumProductCount;
                }
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();

                var remove = Order.GenerateAllocation(item.ProductID.Value,
                                                   1,
                                                   OrderContext.Order.AsOrder().OrderID,
                                                  Convert.ToInt32(Session["WareHouseId"]),
                                                  EntitiesEnums.MaintenanceMode.Delete,
                                                  Convert.ToInt32(Session["PreOrder"]),
                                                  CoreContext.CurrentAccount.AccountTypeID,
                                                  false);
                    

                var outOfStockProducts = GetOutOfStockProducts(customer);
                ViewBag.OutOfStockProducts = outOfStockProducts;
                //KTC 27JUL   Remove Cart
                ApplyPaymentPreviosBalance();
                Session["Order"] = OrderContext.Order.AsOrder();
                bool validTotalQv = true;
                //var valueDecimal = PreOrderExtension.GetValueURL();
                //decimal valueTotalQV = valueDecimal[0];
                //var totlaQv = OrderContext.Order.AsOrder().QualificationTotal;
                //if (totlaQv <= valueTotalQV)
                //{
                //    validTotalQv = false;
                //}
                //ViewBag.TotalQV = valueTotalQV;

                return Json(new
                {
                    result = true,
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    itemsInCart = customer.ParentOrderItems.Sum(poi => poi.Quantity),
                    adjustedSubtotal = customer.AdjustedSubTotal.ToString(OrderContext.Order.CurrencyID),
                    subtotal = customer.Subtotal.ToString(OrderContext.Order.CurrencyID),
                    total = OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID),
                    totals = Totals,
                    outOfStockProducts = outOfStockProducts,
                    CartModel = GetCartModelData(OrderContext.Order.AsOrder()),
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder())//,
                    //validTotalQv
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //protected virtual IEnumerable<MiniShopOrderItemModel> GetOrderItemModels(Order order)
        //{
        //    if (!order.OrderCustomers.Any())
        //    {
        //        return Enumerable.Empty<MiniShopOrderItemModel>();
        //    }

        //    // If the order contains an enrollment kit or autoship bundle, we display an "edit" link instead of a "remove" link. - Lundy
        //    string editAction = GetMiniShopOrderItemEditAction(order.OrderTypeID);

        //    return order.OrderCustomers[0].OrderItems.Select(x => new MiniShopOrderItemModel()
        //        .LoadResources(x, x.IsEditable ? null : Url.Action(editAction)));
        //}

        protected virtual IList<IOrderItemModel> GetOrderItemModels(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);
            Contract.Requires<ArgumentException>(order.OrderCustomers != null);
            Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

            return order.OrderCustomers[0].OrderItems
                .Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem) &&
                    x.ParentOrderItemID == null)
                .Select(GetOrderItemModel)
                .ToList();
        }

        protected virtual IOrderItemModel GetOrderItemModel(OrderItem orderItem)
        {
            Contract.Requires<ArgumentNullException>(orderItem != null);
            Contract.Requires<ArgumentNullException>(orderItem.OrderCustomer != null);
            Contract.Requires<ArgumentNullException>(orderItem.OrderCustomer.Order != null);

            var orderCustomer = orderItem.OrderCustomer;
            var order = orderCustomer.Order;
            var currencyId = order.CurrencyID;
            var orderItemModel = Create.New<IOrderItemModel>();

            var orderItemProduct = Inventory.GetProduct(orderItem.ProductID.Value);
            var preAdjustmentUnitPrice = orderItem.GetPreAdjustmentPrice();
            var finalUnitPrice = orderItem.ItemPriceActual ?? orderItem.GetAdjustedPrice();

            orderItemModel.Guid = orderItem.Guid.ToString("N");
            orderItemModel.ProductID = orderItem.ProductID ?? 0;
            orderItemModel.SKU = orderItem.SKU;
            orderItemModel.ProductName = orderItemProduct.Translations.Name();
            orderItemModel.retailPricePerItem = NetSteps.Data.Entities.ProductPricesExtensions.GetPriceByPriceType(orderItem.ProductID.Value, 1, currencyId).GetRoundedNumber(2).ToString(currencyId);
            orderItemModel.AdjustedUnitPrice = finalUnitPrice.ToString(currencyId);
            orderItemModel.OriginalUnitPrice = preAdjustmentUnitPrice.ToString(currencyId);

            orderItemModel.AdjustedTotal = (finalUnitPrice * orderItem.Quantity).ToString(currencyId);
            orderItemModel.OriginalTotal = (preAdjustmentUnitPrice * orderItem.Quantity).ToString(currencyId);

            orderItemModel.Quantity = orderItem.Quantity;

            orderItemModel.OriginalCommissionableTotal = (orderItem.GetPreAdjustmentPrice(orderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(currencyId);
            orderItemModel.AdjustedCommissionableTotal = (orderItem.GetAdjustedPrice(orderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(currencyId);

            foreach (var message in orderItem.OrderItemMessages
                .Select(m => m.Message))
            {
                orderItemModel.Messages.Add(message);
            }

            // Hostess rewards show the discount amount next total.
            if (orderItem.IsHostReward)
            {
                // when hostess rewards are an order adjustment type we can refactor this.
                orderItemModel.AdjustedTotal = string.Format("{0} {1}",
                    orderItemModel.AdjustedTotal,
                    Translation.GetTerm("HostRewardDiscount", "(discounted {0})",
                        (orderItem.Discount ?? (orderItem.DiscountPercent.HasValue ? (orderItem.ItemPrice * orderItem.Quantity) * orderItem.DiscountPercent.Value : 0)).ToString(currencyId)
                    )
                );
            }

            orderItemModel.IsStaticKit = orderItemProduct.IsStaticKit();
            orderItemModel.IsDynamicKit = true;// orderItemProduct.IsDynamicKit();
            if (orderItemModel.IsDynamicKit)
            {
                orderItemModel.IsDynamicKitFull = true;// orderItem.ChildOrderItems.Sum(oi => oi.Quantity) >= orderItemProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
            }
            orderItemModel.IsHostReward = orderItem.IsHostReward;
            orderItemModel.BundlePackItemsUrl = Url.Action("BundlePackItems", new { productId = orderItem.ProductID, bundleGuid = orderItem.Guid.ToString("N"), orderCustomerId = orderItem.OrderCustomer.Guid.ToString("N") });

            orderItemModel.KitItemsModel = Create.New<IKitItemsModel>();
            orderItemModel.KitItemsModel.KitItemModels = new List<IKitItemModel>();  // Non-kits still need an empty list
            if (orderItemProduct.IsStaticKit())
            {
          
                List<MaterialName> objList = new List<MaterialName>();          
                foreach (var item in orderItem.ChildOrderItems)
                {
                        MaterialName objE = new MaterialName();
                        objE.Name = item.ProductName;
                        objE.Quantity = item.Quantity;
                        objE.SKU = item.SKU;
                        objList.Add(objE);
                } 
                //Is Kit
                orderItemModel.KitItemsModel.KitItemModels = objList
                    .Select(k =>
                    {
                        var kitItemModel = Create.New<IKitItemModel>();
                        kitItemModel.ProductName = k.Name;
                        kitItemModel.Quantity = k.Quantity;
                        kitItemModel.SKU = k.SKU;
                        return kitItemModel;
                    }).ToList();
            }
         
            //CGI(CMR)-29/10/2014-Inicio
            decimal totalQV = 0;
            totalQV = orderItem.OrderItemPrices
                                    .Where(ip => ip.ProductPriceTypeID == (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV)
                                    .Sum(ip => ((ip.UnitPrice == null ? 0 : ip.UnitPrice) * (orderItem.Quantity == null ? 0 : (decimal)orderItem.Quantity)));

            orderItemModel.TotalQV = totalQV;
            /*CS29JUN2016.Inio.Comentado.Quitar Formato Moneda y Decimal*/
            //orderItemModel.TotalQV_Currency = totalQV.ToString(currencyId);
            /*CS29JUN2016.Fin.Comentado.Quitar Formato Moneda y Decimal*/
            orderItemModel.TotalQV_Currency = Convert.ToInt32(totalQV).ToString();
            //CGI(CMR)-29/10/2014-Inicio

            return orderItemModel;
        }

        protected virtual string GetMiniShopOrderItemEditAction(short orderTypeID)
        {
            switch (orderTypeID)
            {
                case (short)Constants.OrderType.EnrollmentOrder:
                    return "EnrollmentVariantKits";
                case (short)Constants.OrderType.AutoshipTemplate:
                    return "AutoshipBundles";
            }

            return null;
        }

        protected virtual IEnumerable<MiniShopProductModel> GetProductModels(int categoryID, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            var allProducts = Inventory.GetActiveProductsForCategorys(
                    _enrollmentContext.StoreFrontID,
                    categoryID,
                    (short?)_enrollmentContext.AccountTypeID
            );

            var validProducts = Inventory.ExcludeInvalidProducts(
                    allProducts,
                    (short?)_enrollmentContext.AccountTypeID,
                    order.CurrencyID
            );

            var productModels = validProducts
                    .OrderByDescending(x => x.CatalogItems.Max(ci => ci.StartDateUTC))
                    .Select(x => new MiniShopProductModel().LoadResources(
                            x,
                            _enrollmentContext.AccountTypeID,
                            order.CurrencyID,
                            order.OrderTypeID
                    ));

            //productModels = productModels.Where(donde => donde.YourPrice.Replace("$", "").Replace("$", "").Trim().Length > 0);
            return productModels.Where(donde => donde.ValorPrice > 0);
        }

        protected virtual EnrollmentVariantKitsModel EnrollmentVariantKits_CreateModel()
        {
            return new EnrollmentVariantKitsModel();
        }

        protected virtual void EnrollmentVariantKits_LoadModel(EnrollmentVariantKitsModel model, bool active)
        {
            var initialOrder = GetInitialOrder(false);
            var selectedProductID = GetSelectedEnrollmentKitProductID(initialOrder);
            model.LoadValues(selectedProductID);
            EnrollmentVariantKits_LoadResources(model, initialOrder);
        }

        protected virtual void EnrollmentVariantKits_LoadResources(EnrollmentVariantKitsModel model, Order initialOrder)
        {
            var iProducts = GetEnrollmentKitProducts(_enrollmentContext.AccountTypeID);
            // Here we make the, currently correct, assumption that the IProducts we get are really Products under the covers.

            var selKit = iProducts.FirstOrDefault(p => p.ProductID == (model.SelectedVariantProductID ?? model.SelectedProductID));
            if (selKit != null)
            {
                model.SelectedKit = new EnrollmentVariantKitModel().LoadResources((Product)selKit, _enrollmentContext) as EnrollmentVariantKitModel;
            }
			//INI - GR4171 - Se modifica orden de visualizacion - CDAS
            //var products = iProducts.Where(p => p.IsVariantTemplate || Product.GetVariantsCount(p.ProductID) == 1).Select(p => (Product)p);
			  var products = iProducts.Where(p => p.IsVariantTemplate || Product.GetVariantsCount(p.ProductID) == 1).Select(p => (Product)p).OrderByDescending(p => p.ProductID);
			//FIN - GR4171 - Se modifica orden de visualizacion - CDAS
            model.LoadResources(
            global::nsDistributor.CoreContext.CurrentCultureInfo,
                    products,
                    _enrollmentContext
            );

            LoadPageHtmlContent();
        }

        bool IsVariantOf(int variantID, int parentID)
        {
            var variant = Product.GetVariants(parentID).FirstOrDefault(p => p.ProductID == variantID);
            return variant != null;
        }

        protected virtual void EnrollmentVariantKits_UpdateInitialOrder(EnrollmentVariantKitsModel model, Order initialOrder)
        {
            bool validVariantID = model.SelectedVariantProductID.HasValue
                && model.SelectedVariantProductID.Value != 0
                && IsVariantOf(model.SelectedVariantProductID.Value, model.SelectedProductID.Value);
            int selectedProductID = validVariantID ? model.SelectedVariantProductID.Value : model.SelectedProductID.Value;
            SetSelectedEnrollmentKit(initialOrder, selectedProductID);
        }

        #endregion

        #region Strings

        protected virtual string _errorUpdatingOrder { get { return Translation.GetTerm("ErrorUpdatingOrder", "There was an error updating your order."); } }
        protected virtual string _errorOrderDoesNotMeetMinimumVolume { get { return Translation.GetTerm("ErrorOrderDoesNotMeetMinimumVolume", "Your order does not meet the minimum volume requirement."); } }
        protected virtual string _errorOrderMustContainAtLeastOneItem { get { return Translation.GetTerm("ErrorOrderMustContainAtLeastOneItem", "Your order must contain at least one item."); } }
        protected virtual string _errorUpdatingAutoship { get { return Translation.GetTerm("ErrorUpdatingAutoship", "There was an error updating your autoship."); } }
        protected virtual string _errorAutoshipDoesNotMeetMinimumVolume { get { return Translation.GetTerm("ErrorAutoshipDoesNotMeetMinimumVolume", "Your autoship does not meet the minimum volume requirement."); } }
        protected virtual string _errorAutoshipMustContainAtLeastOneItem { get { return Translation.GetTerm("ErrorAutoshipMustContainAtLeastOneItem", "Your autoship must contain at least one item."); } }
        protected virtual string _errorUrlUnavailableString { get { return Translation.GetTerm("ErrorUrlUnavailable", "That website prefix is no longer available."); } }

        #endregion

        // CSTI(mescobar)-28-01-2015-Inicio
        public virtual ActionResult ValidateRule()
        {
            try
            {

                // CGI(JCT)-2490-13/05/2015-Inicio
                OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
                var addedItemOperationID = 1;
                var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));

                /*Reglas de Salida*/
                decimal qvRule = 0;
                decimal retailRule = 0;
                decimal subtotalRule = (decimal)OrderContext.Order.Subtotal;
                /*Reglas de Entrada*/
                List<int> productsRule = new List<int>();
                List<int> productTypesRule = new List<int>();
                int constQV = (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV;
                int constRetail = (int)NetSteps.Data.Entities.Constants.ProductPriceType.Retail;
                foreach (OrderItem orderItem in nonPromotionalItems)
                {
                    var productInfo = Product.Load((int)orderItem.ProductID);
                    productsRule.Add(productInfo.ProductID);
                    productTypesRule.Add(ProductBase.Load(productInfo.ProductBaseID).ProductTypeID);
                    qvRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                    .Where(x => x.ProductPriceTypeID == constQV)
                                        .Select(y => y.OriginalUnitPrice).FirstOrDefault());
                    retailRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                    .Where(x => x.ProductPriceTypeID == constRetail)
                                        .Select(y => y.OriginalUnitPrice).FirstOrDefault());
                }
                int storeFrontRule = ApplicationContext.Instance.StoreFrontID;
                short orderTypeRule = OrderContext.Order.OrderTypeID;
                int accountRule = orderCustomer.AccountID;
                short accountTypeRule = orderCustomer.AccountTypeID;

                var ruleBasicFilter = Create.New<IOrderRulesService>().GetRules().Where(x => x.RuleStatus == (int)RuleStatus.Active &&
                                                    (x.StartDate.IsNullOrEmpty() ? true : x.StartDate <= DateTime.Now ? true : false) &&
                                                    (x.EndDate.IsNullOrEmpty() ? true : x.EndDate >= DateTime.Now ? true : false)).ToList();

                List<RulesDTO> dtoRuleComparer = new List<RulesDTO>();
                var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
                foreach (var item in ruleBasicFilter)
                {
                    dtoRuleComparer.Add(ordeRuleConverter.Convert(item));
                }

                /*Filtrar Reglas a las que aplica la order*/
                var appliedRules = dtoRuleComparer.Where(x => (x.RuleValidationsDTO.Where(y => (y.AccountIDs.Count == 0 ? true : y.AccountIDs.Contains(accountRule) ? true : false)
                                                                                    && (y.AccountTypeIDs.Count == 0 ? true : y.AccountTypeIDs.Contains(accountTypeRule) ? true : false)
                                                                                    && (y.OrderTypeIDs.Count == 0 ? true : y.OrderTypeIDs.Contains(orderTypeRule) ? true : false)
                                                                                    && (y.StoreFrontIDs.Count == 0 ? true : y.StoreFrontIDs.Contains(storeFrontRule) ? true : false)
                                                                                    && (y.ProductIDs.Count == 0 ? true : productsRule.Distinct().Intersect(y.ProductIDs).Any() ? true : false)
                                                                                    && (y.ProductTypeIDs.Count == 0 ? true : productTypesRule.Distinct().Intersect(y.ProductTypeIDs).Any() ? true : false)).Any())).ToList();

                /*Filtrar Reglas no cumplidas*/
                var unfulfilledRules = appliedRules.Where(x => (x.RuleValidationsDTO.Where(y => (y.CustomerPriceSubTotalDTO.Count == 0 ? false : y.CustomerPriceSubTotalDTO.FirstOrDefault().MinimumAmount > subtotalRule ? true : false)
                                                                                            || (y.CustomerPriceTotalDTO.Count == 0 ? false : (y.CustomerPriceTotalDTO
                                                                        .Where(z => z.ProductPriceTypeID == constQV && z.MinimumAmount > qvRule).Any() || y.CustomerPriceTotalDTO
                                                                        .Where(z => z.ProductPriceTypeID == constRetail && z.MinimumAmount > retailRule).Any()))).Any())).ToList();

                /*Concatenar mensajes*/
                var messageRule = string.Empty;
                foreach (var faildRule in unfulfilledRules)
                {
                    TermTranslation translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName && tt.LanguageID == CurrentLanguageID);
                    if (translation == default(TermTranslation))
                    {
                        translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName);
                        messageRule += translation.Term;
                    }
                    else
                    {
                        messageRule += translation.Term;
                    }
                }

                // return Json(new { success = false, validrule = true, message = messageRule });
                // CGI(JCT)-2490-13/05/2015-Fin

                /* CGI (AAHA) - R2599 - Inicio */
                if (messageRule != string.Empty)
                {
                    return Json(new { success = false, validrule = true, message = messageRule });
                }
                else
                {
                    return Json(new { success = true, message = messageRule });
                }
                /* CGI (AAHA) - R2599 - Fin ---*/

            }
            catch (Exception ex)
            {
                return Json(new { success = false, validrule = false, message = ex.Message.ToString() });
            }
        }
        // CSTI(mescobar)-28-01-2015-Fin

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Search(string query)
        {
            try
            {
                query = query.ToCleanString();
                var searchResults = Product.SlimSearch(query).Where(p => !p.IsVariantTemplate);
                return Json(searchResults.Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Name }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult QuickAddProduct(int productID, int? marketID = null)
        {
            try
            {
                int currencyID = GetDefaultCurrencyIDFromMarket(marketID.HasValue ? marketID.Value : CoreContext.CurrentMarketId);
                var product = Inventory.GetProduct(productID);
                var model = new ProductModel();
                return Json(new { result = true, product = model.LoadResources(product, DiscountedPriceTypesToDisplay, currencyID) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region
        //[FunctionFilter("Products", "~/", NetSteps.Data.Entities.Constants.SiteType.Corporate)]
        public virtual ActionResult SearchProducts(string query, bool includeDynamicKits = true)
        {
            try
            {
                List<Product> ProductList = FindProducts(query, includeDynamicKits).Where(p => !p.IsVariantTemplate).Take(10).ToList();
                return Json(ProductList.Where(p => p.Active).Select(p => new
                {
                    id = p.ProductID,
                    text = p.SKU + " - " + p.Translations.Name(),
                    isDynamicKit = p.IsDynamicKit(),
                    needsBackOrderConfirmation = Inventory.IsOutOfStock(p) && p.ProductBackOrderBehaviorID == NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToInt(),
                    customizationType = p.CustomizationType()
                }));


            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        protected virtual IEnumerable<Product> FindProducts(string query, bool includeDynamicKits)
        {
            return Product.SearchProductForOrder(query).Where(p => (!Inventory.IsOutOfStock(p) || p.ProductBackOrderBehaviorID != NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide.ToInt()));
        }

        protected virtual IIndexModel Index_CreateModel(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            var model = Create.New<IIndexModel>();
            model.OrderEntryModel = Create.New<IOrderEntryModel>();
            LoadOrderEntryModelOptions(model.OrderEntryModel.Options);
            LoadOrderEntryModelData(model.OrderEntryModel.Data, OrderContext);
            model.OrderEntryModel.Order = order;

            return model;
        }

        protected virtual dynamic LoadOrderEntryModelOptions(dynamic options)
        {
            // Code contracts rewriter doesn't work with dynamics
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            options.RemoveUrl = Url.Action("RemoveFromCart");
            options.RemoveErrorMessage = Translation.GetTerm("ErrorRemovingProduct", "The product could not be removed");
            options.UpdateQuantitiesUrl = Url.Action("UpdateCart");
            options.UpdateQuantitiesErrorMessage = Translation.GetTerm("ErrorUpdatingCart", "The cart could not be updated");

            return options;
        }

        #endregion

        public virtual IEnumerable<int> DiscountedPriceTypesToDisplay
        {
            get
            {
                var service = Create.New<IPriceTypeService>();
                return service.GetCurrencyPriceTypes().Select(pt => pt.PriceTypeID);
            }
        }

        protected virtual int GetDefaultCurrencyIDFromMarket(int marketID)
        {
            return Market.Load(marketID).GetDefaultCurrencyID();
        }

        /*CSTI(CS)-05/03/2016-Inicio*/
        #region Payment Methods

        [EnrollmentStepSection]
        public virtual ActionResult PaymentMethods()
        {
            var account = GetEnrollingAccount();

            return SectionsView();
        }

        [HttpPost, EnrollmentStepSection]
        public virtual ActionResult PaymentMethods(int id, DisbursementMethodKind preference, bool? useAddressOfRecord
            , bool? isActive, string profileName, string payableTo, string address1, string address2, string address3
            , string city, string county, string state, string zip, int? country, bool? agreementOnFile, List<EFTAccountModel> accounts)
        {
            var account = GetEnrollingAccount();
            var disbursementProfile = new AccountEnrollDisbursementProfile();
            try
            {
                disbursementProfile.id = id;
                disbursementProfile.preference = preference;
                disbursementProfile.useAddressOfRecord = useAddressOfRecord ?? false;
                disbursementProfile.isActive = isActive;
                disbursementProfile.agreementOnFile = agreementOnFile ?? false;
                disbursementProfile.accounts = ((accounts == null) ? new List<IEFTAccount>() : accounts.Select(x => x.Convert()));

                var stateCode = GetStateCode(state);
                int stateId;
                Int32.TryParse(state, out stateId);

                var viewAddress = new Address
                {
                    ProfileName = profileName,
                    Attention = payableTo,
                    AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    County = county,
                    State = stateCode,
                    StateProvinceID = stateId,
                    PostalCode = zip,
                    CountryID = country ?? (int)ConstantsGenerated.Country.UnitedStates
                };

                disbursementProfile.address = viewAddress;
                account.SetAccountEnrollDisbursementProfile(disbursementProfile);

                return SectionCompleted(false);
                //return Json(new { Url = "Products/EnrollmentVariantKits" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (account != null) ? (int?)account.AccountID : null);
                return JsonError(exception.PublicMessage);
            }
            //_enrollmentContext.EnrollingAccount = account;
        }

        protected virtual ActionResult SectionCompleted(bool MVCAutomation = true)
        {
            var sections = GetSections();

            sections.CurrentItem.Completed = true;

            if (!string.IsNullOrWhiteSpace(_enrollmentContext.ReturnUrl))
            {
                string returnUrl = _enrollmentContext.ReturnUrl;
                _enrollmentContext.ReturnUrl = null;

                if (MVCAutomation)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Json(new { TypeRedirect = 0, Url = returnUrl });
                }
            }

            if (sections.HasNextItem)
            {
                if (MVCAutomation)
                {
                    return RedirectToAction(sections.NextItem.Action);
                }
                else
                {
                    return Json(new { TypeRedirect = 1, RouteValueDictionary = new { Action = sections.NextItem.Action, Controller = this.ControllerContext.RouteData.Values["controller"].ToString() } });
                }
            }
            else
            {
                return StepCompleted(MVCAutomation);
            }
        }

        /// <summary>
        /// Takes in a state value and will return the state code if it is an ID
        /// </summary>
        /// <param name="state">state value</param>
        /// <returns>state abbreviation</returns>
        private string GetStateCode(string state)
        {
            //set statecode to whatever is currently in state
            var stateCode = state;

            //If state is an int, get the ID from cache
            int stateId;
            if (Int32.TryParse(state, out stateId))
            {
                var stateCodes = SmallCollectionCache.Instance.StateProvinces;
                stateCode = stateCodes.GetById(stateId).StateAbbreviation;
            }
            return stateCode;
        }

        protected virtual PaymentMethodsModel PaymentMethods_CreateModel()
        {
            var paymentMethodsModel = new PaymentMethodsModel();
            var account = GetEnrollingAccount();


            paymentMethodsModel.CreateModel(account);
            paymentMethodsModel.ChangeCountryURL = "/Accounts/BillingShippingProfiles/GetAddressControl";
            paymentMethodsModel.PostalCodeLookupURL = "/Checkout/LookupZip";

            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            paymentMethodsModel.MainAddressHtmlParent = mainAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();

            return paymentMethodsModel;
        }

        protected virtual void PaymentMethods_LoadModel(PaymentMethodsModel model, bool active)
        {
            Account account = GetEnrollingAccount();
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping) ?? new Address();

            model.LoadValues(_enrollmentContext.IsSameShippingAddress, _enrollmentContext.CountryID, mainAddress, shippingAddress);

            // We only need this if the section is active
            if (active)
            {
                PaymentMethods_LoadResources(model, account);
            }
        }

        protected virtual void PaymentMethods_LoadResources(PaymentMethodsModel model, Account account)
        {
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            model.LoadResources(mainAddress);
        }

        protected virtual void PaymentMethods_UpdateCheck(PaymentMethodsModel model, Account account)
        {

        }

        protected virtual void PaymentMethods_UpdateDirectDeposit(PaymentMethodsModel model, Account account)
        {

        }

        protected virtual void PaymentMethods_UpdateContext(PaymentMethodsModel model)
        {
        }

        #endregion
        /*CSTI(CS)-05/03/2016-Fin*/

        #region Payment
        /*CSTI(CS:08/03/2016)*/
        //[FunctionFilter("Enroll-Disbursement Profiles", "~/Enroll/Agreements/Index")]
        public virtual ActionResult Save(
            //bool paymentPreference,
             int id,
             DisbursementMethodKind preference,
             bool? useAddressOfRecord,
             bool? isActive,
             string profileName,
             string payableTo,
             string address1,
             string address2,
             string address3,
             string city,
             string state,
             string zip,
             int? country,
             bool? agreementOnFile,
             List<EFTAccountModel> accounts,
            IEnumerable<int> bankID)
        {
            try
            {

                //var account = CurrentAccount;
                var account = GetEnrollingAccount();
                if (account == null)
                {
                    //return Redirect("~/Accounts");
                }


                if (preference.ToString() == "EFT")
                {

                    var stateCode = GetStateCode(state);
                    int stateId;
                    Int32.TryParse(state, out stateId);
                    var viewAddress = new Address
                    {

                        ProfileName = profileName,
                        Attention = payableTo,
                        AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                        Address1 = address1,
                        Address2 = address2,
                        Address3 = address3,
                        City = city,
                        State = stateCode,
                        StateProvinceID = stateId,
                        PostalCode = zip,
                        CountryID = country ?? (int)ConstantsGenerated.Country.UnitedStates
                    };

                    useAddressOfRecord = useAddressOfRecord ?? false;
                    agreementOnFile = agreementOnFile ?? false;

                    var ieftAccounts = Enumerable.Empty<IEFTAccount>();


                    if (accounts != null && accounts.Any())
                    {
                        ieftAccounts = accounts.Select(x => x.Convert());
                    }

                    CommissionsService.SaveDisbursementProfile(
                                         id,
                                         account,
                                         viewAddress,
                                         preference,
                                         (bool)useAddressOfRecord,
                                         ieftAccounts,
                                         (bool)agreementOnFile);

                    List<MinimumSearchData> lis_ieftAccounts = new List<MinimumSearchData>();
                    if (ieftAccounts.Where(x => x.DisbursementProfileId == 0).Count() > 0)
                    {
                        lis_ieftAccounts = AccountExtensions.GetDisbursementProfileIDs(account.AccountID);
                        UpdateDisbursementProfiles(lis_ieftAccounts, bankID);
                    }
                    else
                    {
                        foreach (var item in ieftAccounts)
                        {
                            lis_ieftAccounts.Add(new MinimumSearchData
                            {
                                ID = item.DisbursementProfileId
                            });
                        };

                        UpdateDisbursementProfiles(lis_ieftAccounts, bankID);
                    }
                }
                else
                {

                    var stateCode = GetStateCode(state);
                    int stateId;
                    Int32.TryParse(state, out stateId);
                    var viewAddress = new Address
                    {

                        ProfileName = profileName,
                        Attention = payableTo,
                        AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                        Address1 = address1,
                        Address2 = address2,
                        Address3 = address3,
                        City = city,
                        State = stateCode,
                        StateProvinceID = stateId,
                        PostalCode = zip,
                        CountryID = country ?? (int)ConstantsGenerated.Country.UnitedStates
                    };

                    useAddressOfRecord = useAddressOfRecord ?? false;
                    agreementOnFile = agreementOnFile ?? false;

                    var ieftAccounts = Enumerable.Empty<IEFTAccount>();
                    if (accounts != null && accounts.Any())
                    {
                        ieftAccounts = accounts.Select(x => x.Convert());
                    }

                    CommissionsService.SaveDisbursementProfile(
                                         id,
                                         account,
                                         viewAddress,
                                         preference,
                                         (bool)useAddressOfRecord,
                                         ieftAccounts,
                                         (bool)agreementOnFile);
                }

                //return Json(new { result = true });
                /*CS:14ABR.Inicio*/
                _enrollmentContext.EnrollmentComplete = true;
                /*CS:14ABR.Fin*/
                return Json(new { result = true });
                //return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.NextItem);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateDisbursementProfiles(List<MinimumSearchData> ieftAccounts, IEnumerable<int> bankID)
        {

            if (ieftAccounts.First().ID != null && bankID.First() != null)
            {
                int bankID01 = Convert.ToInt32(bankID.First());
                int DisbursementProfileId01 = Convert.ToInt32(ieftAccounts.First().ID);
                AccountExtensions.UpdateDisbursementProfileBank(DisbursementProfileId01, bankID01);
            }

            if (ieftAccounts.Last().ID != null && bankID.Last() != null && bankID.Last() != bankID.First())
            {
                int DisbursementProfileId02 = Convert.ToInt32(ieftAccounts.Last().ID);
                int bankID02 = Convert.ToInt32(bankID.Last());
                AccountExtensions.UpdateDisbursementProfileBank(DisbursementProfileId02, bankID02);
            }
        }
        #endregion

        private void insertDispatchProductsFill()
        {
            // ------------------------------------------------------------------------------------------------------------------------------------------------
            /*
             * Recorre la variable de session registrada con los Dispatch filtrados y los almacena en la base de datos dentro de la tabla [DispatchItemControls]
             * wv: 20160531
             * Session => listDispatChProducts
             * 
             */
            var lstProductsVal = (List<DispatchProducts>)Session["listDispatChEnRoll"];
            bool endInsert = false;
            Dictionary<int, bool> periodIDSel = Periods.GetPeriodByDate(DateTime.Now);
            int periodVal = Convert.ToInt32(periodIDSel.Keys.ElementAt(0));
            int resultado;
            if (lstProductsVal != null)
            {
                foreach (var item in lstProductsVal)
                {
                    try
                    {
                        resultado = OrderExtensions.insertDispatchProducts(item.OrderDispatchID, OrderContext.Order.OrderCustomers[0].AccountID, item.Quantity, OrderContext.Order.OrderID, periodVal, item.OrderItemID, item.ProductId, item.Name, item.SKU);
                    }
                    catch
                    {
                        endInsert = true;
                    }
                }
            }

            // ------------------------------------------------------------------------------------------------------------------------------------------------
        }



        #region Envio Correo de Boletos

        public void ValidateSendEmailBoleto(int TicketNumber)
        {
            try
            {
                List<PaymentInfoBancoOrden> lstInformacionFacturacion = PaymetTycketsReportBusinessLogic.GetInformacionBanco(TicketNumber);

                if (lstInformacionFacturacion != null)
                {


                    PaymentInfoBancoOrden obj = lstInformacionFacturacion.First();
                    if (obj.PaymentTypeID == 11)
                    {


                        var ENT = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ENT");

                        if (ENT == "S")
                        {

                            ExportarBoleta(TicketNumber, obj.BankCode);
                        }

                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public void ExportarBoleta(int OrderPaymentID, int BankCode)
        {

            try
            {


                string BankName = string.Empty;
                Byte[] ResponseFile = null;

                switch (BankCode)
                {
                    case 1:// "Banco Do Brasil":
                        ResponseFile = CrearTicketBB(OrderPaymentID);
                        break;
                    case 104://"Caixa":
                        ResponseFile = CrearTicketCaixa(OrderPaymentID);
                        break;
                    case 341://"Itaú":
                        ResponseFile = CrearTicketItau(OrderPaymentID);
                        break;
                    default:
                        break;
                }
                byte[] Libro = ExtractPages(ResponseFile);




                string nameFile = string.Format("Ticket{0}-{1}{2}{3}.pdf", OrderPaymentID.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString());

                byte[] output = Libro;
                var ruta = ConfigurationManager.AppSettings["FileUploadWebPath"];


                var path = ruta + nameFile; //@"\\10.12.6.183\FileUploads\ReportsPDF\" + nameFile;//Server.MapPath("~/Reports/FilesTemp/" + nameFile);  //   se coloca  o exporta el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                System.IO.File.WriteAllBytes(path, output);
                File(path, "application/pdf", Path.GetFileName(path));


                AccountPropertiesBusinessLogic.GetEmailTemplate(OrderPaymentID, nameFile);

                // return File(Libro, "application/pdf", string.Format("Ticket{0}-{1}{2}{3}.pdf", OrderPaymentID.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString()));
            }
            catch (Exception ex)
            {

                //throw ex;
                //string msg = Translation.GetTerm("PDFNotSentForData", "You can not export values by format");
                //var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                //return Json(new { result = false, message = msg });// exception.PublicMessage });
            }
        }

        #region crear codigo de barra
        #region TicketBB
        private string CreateCodeBarTicketBB(DataTable dtOrder, DataTable dtInfoBank)
        {
            string Code = "";
            try
            {
                Code = dtInfoBank.Rows[0]["BankCode"].ToString();//(Fields!BankCode.Value, "dtsInfoBank") & 
                Code += dtInfoBank.Rows[0]["CurrencieBankID"].ToString(); // First(Fields!CurrencyCode.Value, "dtsInfoBank") &  
                Code += CodeBarFormulaTicketBB.DVTCalculate
                        (
                            dtInfoBank.Rows[0]["BankCode"].ToString(),//First(Fields!BankCode.Value, "dtsInfoBank"),
                            dtInfoBank.Rows[0]["CurrencieBankID"].ToString(),// First(Fields!CurrencyCode.Value, "dtsInfoBank"),/* 
                            dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(), // cstr(First(Fields!FactorVencimientoCalculate.Value, "dtsOrder")) ,
                            dtOrder.Rows[0]["AmountTotal"].ToString(),  //First(Fields!AmountTotal.Value, "dtsOrder"),
                            "000000",
                            dtInfoBank.Rows[0]["CodigoConvenio"].ToString(),//  First(Fields!CodigoConvenio.Value, "dtsInfoBank"),
                            dtInfoBank.Rows[0]["NumeroTitulo"].ToString(),//  First(Fields!NumeroTitulo.Value, "dtsInfoBank"),
                            dtInfoBank.Rows[0]["Cartera"].ToString()// First(Fields!Cartera.Value, "dtsInfoBank")
                       );

                Code += dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString();// CSTR(FIRST(Fields!FactorVencimientoCalculate.Value, "dtsOrder")) 
                Code += dtOrder.Rows[0]["AmountTotal"].ToString();// First(Fields!AmountTotal.Value, "dtsOrder") 
                Code += "000000";
                Code += dtInfoBank.Rows[0]["CodigoConvenio"].ToString();// First(Fields!CodigoConvenio.Value, "dtsInfoBank")
                Code += dtInfoBank.Rows[0]["NumeroTitulo"].ToString();// First(Fields!NumeroTitulo.Value, "dtsInfoBank");
                Code += dtInfoBank.Rows[0]["Cartera"].ToString(); //First(Fields!Cartera.Value, "dtsInfoBank") 
                return Code;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region TicketItau
        private string CreateCodebarTicketItau(DataTable dtOrder, DataTable dtInfoBank)
        {

            string Code = string.Empty;

            try
            {
                Code = dtInfoBank.Rows[0]["BankCode"].ToString();//(Fields!BankCode.Value, "dtsInfoBank") & 
                Code += dtInfoBank.Rows[0]["CurrencyCode"].ToString();//(Fields!BankCode.Value, "dtsInfoBank") & 

                Code += CodeBarFormulaTicketItau.DVTCalculate
                    (
                        dtInfoBank.Rows[0]["BankCode"].ToString(),
                        dtInfoBank.Rows[0]["CurrencyCode"].ToString(),
                        "",
                        dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(),
                        dtOrder.Rows[0]["AmountTotal"].ToString(),
                            (
                                dtInfoBank.Rows[0]["Cartera"].ToString() +//First(Fields!Cartera.Value, "dtsInfoBank")
                                dtInfoBank.Rows[0]["NumeroTitulo"].ToString() +//  First(Fields!NumeroTitulo.Value, "dtsInfoBank")

                                CodeBarFormulaTicketItau.DVNNCalculate
                                (
                                    dtInfoBank.Rows[0]["BankAgence"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                    dtInfoBank.Rows[0]["Cuenta"].ToString(),// cstr(First(Fields!Cuenta.Value, "dtsInfoBank")),
                                    dtInfoBank.Rows[0]["Cartera"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                    dtInfoBank.Rows[0]["NumeroTitulo"].ToString()// First(Fields!NumeroTitulo.Value, "dtsInfoBank")
                                )
                        ),
                          dtInfoBank.Rows[0]["BankAgence"].ToString(),
                          dtInfoBank.Rows[0]["Cuenta"].ToString(),
                          "000");
                Code += dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString();
                Code += dtOrder.Rows[0]["AmountTotal"].ToString();

                Code += dtInfoBank.Rows[0]["Cartera"].ToString();//First(Fields!Cartera.Value, "dtsInfoBank")
                Code += dtInfoBank.Rows[0]["NumeroTitulo"].ToString();//  First(Fields!NumeroTitulo.Value, "dtsInfoBank")

                Code += CodeBarFormulaTicketItau.DVNNCalculate
                                  (
                                      dtInfoBank.Rows[0]["BankAgence"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                      dtInfoBank.Rows[0]["Cuenta"].ToString(),// cstr(First(Fields!Cuenta.Value, "dtsInfoBank")),
                                      dtInfoBank.Rows[0]["Cartera"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                      dtInfoBank.Rows[0]["NumeroTitulo"].ToString()// First(Fields!NumeroTitulo.Value, "dtsInfoBank")
                                  );
                Code += dtInfoBank.Rows[0]["BankAgence"].ToString().Substring(0, 4);
                Code += dtInfoBank.Rows[0]["Cuenta"].ToString();
                Code += "000";
                //

                return Code;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Caixa
        private static string DVT3Calculate(DataTable dtOrder, DataTable dtInfoBank)
        {

            string code = "";
            code += CodeBarFormulaTicketCaixa.DVT3Calculate
                   (
                            dtInfoBank.Rows[0]["BankCode"].ToString(),//A
                            dtInfoBank.Rows[0]["CurrencyCode"].ToString(),//B
                            "",//C
                            dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(),//D
                            dtOrder.Rows[0]["AmountTotal"].ToString(),//E
                            dtInfoBank.Rows[0]["Cuenta"].ToString(),//F
                            dtInfoBank.Rows[0]["Cuenta"].ToString(),//G
                            ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(0, 3),//H
                            "1",//I
                            ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(3, 3),//J
                            "4",//K
                            ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(6, 9),//M

                          CodeBarFormulaTicketCaixa.DVCLCalculate(//L
                               ("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6),
                               ("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1, 1)).Substring(0, 6),
                               ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(0, 3),
                               "1",
                               ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(3, 3),
                               "4",
                                ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(6, 9)
                          )
                          );
            return code;
        }
        private static string CreateCodebarCaixa(DataTable dtOrder, DataTable dtInfoBank)
        {
            string Code = "";
            try
            {

                Code += dtInfoBank.Rows[0]["BankCode"].ToString().Substring(0, 3);
                Code += dtInfoBank.Rows[0]["CurrencyCode"].ToString().Substring(0, 1);
                Code += CodeBarFormulaTicketCaixa.DVT3Calculate
                    (
                             dtInfoBank.Rows[0]["BankCode"].ToString(),//A
                             dtInfoBank.Rows[0]["CurrencyCode"].ToString(),//B
                             "",//C
                             dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(),//D
                             dtOrder.Rows[0]["AmountTotal"].ToString(),//E

                           ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5)),//F
                             dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1),//G


                             ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3),//H
                             "1",//I
                             ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3),//J
                             "4",//K
                             ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9),//M

                           CodeBarFormulaTicketCaixa.DVCLCalculate(//L
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6),
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1, 1)).Substring(0, 6),
                                 ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5)),
                                dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1),
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3),
                                "1",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3),
                                "4",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9)
                           )
                           );



                Code += dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString();
                Code += dtOrder.Rows[0]["AmountTotal"].ToString();
                //Code += ("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6);
                //Code += dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(4, 1);
                Code += ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5));
                Code += dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1);
                Code += ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3);
                Code += "1";
                Code += ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3);
                Code += "4";
                Code += ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9);
                Code += CodeBarFormulaTicketCaixa.DVCLCalculate(//L
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6),
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1, 1)).Substring(0, 6),
                                ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5)),
                                dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1),
                    //"000152", "4","800","1","000","4","000033282"
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3),
                                "1",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3),
                                "4",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9)
                           );


                return Code;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        private static Byte[] CodeBar(string text)
        {
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.StartStopText = true;
            code128.Code = text;
            var bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            Byte[] bitmapData = null;
            using (var ms = new System.IO.MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapData = ms.ToArray();
            }
            return bitmapData;
        }

        #endregion
        private byte[] CrearTicketBB(int TicketNumber)
        {

            try
            {

                DataSet dsData = new DataSet();
                dsData = PaymetTycketsReportBusinessLogic.GenerateTicketBB(TicketNumber);

                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                LocalReport report = new LocalReport();
                report.ReportPath = Server.MapPath("~/Reports/GenerateTicketBB.rdlc");

                ReportDataSource rdsInfoBank = new ReportDataSource();
                rdsInfoBank.Name = "dtsInfoBank";//This refers to the dataset name in the RDLC file  
                rdsInfoBank.Value = dsData.Tables[0];


                ReportDataSource rdsdtOrder = new ReportDataSource();
                rdsdtOrder.Name = "dtsOrder";//This refers to the dataset name in the RDLC file  
                rdsdtOrder.Value = dsData.Tables[1];

                ReportDataSource rdsdtDetails = new ReportDataSource();
                rdsdtDetails.Name = "dtsDetailsBank";
                rdsdtDetails.Value = dsData.Tables[2];

                Byte[] bitmapData = CodeBar(CreateCodeBarTicketBB(dsData.Tables[1], dsData.Tables[0]));

                DataTable dtCodeBar = new DataTable("dtImage");
                DataColumn dc = new DataColumn("CodeBar");
                dc.DataType = typeof(byte[]);
                dtCodeBar.Columns.Add(dc);
                dtCodeBar.Rows.Add(new Object[] { bitmapData });

                ReportDataSource rdsdstImage = new ReportDataSource();
                rdsdstImage.Name = "dstImage";
                rdsdstImage.Value = dtCodeBar;

                report.DataSources.Add(rdsInfoBank);
                report.DataSources.Add(rdsdtOrder);
                report.DataSources.Add(rdsdtDetails);
                report.DataSources.Add(rdsdstImage);

                Byte[] mybytes = report.Render("pdf", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings); //for exporting to PDF  
                return mybytes;
            }
            catch (Exception ex)
            {


                throw ex;

            }
        }
        private byte[] CrearTicketCaixa(int TicketNumber)
        {


            try
            {


                DataSet dsData = new DataSet();
                dsData = PaymetTycketsReportBusinessLogic.GenerateTicketCaixa(TicketNumber);

                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                LocalReport report = new LocalReport();
                report.ReportPath = Server.MapPath("~/Reports/GenerateTicketCaixa.rdlc");

                ReportDataSource rdsOrdenProductos = new ReportDataSource();
                rdsOrdenProductos.Name = "dtsInfoBank";
                rdsOrdenProductos.Value = dsData.Tables[0];


                ReportDataSource rdsdtOrders = new ReportDataSource();
                rdsdtOrders.Name = "dtsOrder";
                rdsdtOrders.Value = dsData.Tables[1];

                ReportDataSource rdsdtDetails = new ReportDataSource();
                rdsdtDetails.Name = "dtsDetailsBank";
                rdsdtDetails.Value = dsData.Tables[2];

                string PmDVT3Calculate = DVT3Calculate(dsData.Tables[1], dsData.Tables[0]);


                Byte[] bitmapData = CodeBar(CreateCodebarCaixa(dsData.Tables[1], dsData.Tables[0]));

                DataTable dtCodeBar = new DataTable("dtImage");
                DataColumn dc = new DataColumn("CodeBar");
                dc.DataType = typeof(byte[]);
                dtCodeBar.Columns.Add(dc);
                dtCodeBar.Rows.Add(new Object[] { bitmapData });

                ReportDataSource rdsdstImage = new ReportDataSource();
                rdsdstImage.Name = "dstImage";
                rdsdstImage.Value = dtCodeBar;

                report.DataSources.Add(rdsOrdenProductos);
                report.DataSources.Add(rdsdtOrders);
                report.DataSources.Add(rdsdtDetails);
                report.DataSources.Add(rdsdstImage);

                report.SetParameters(new List<ReportParameter>() { new ReportParameter("PmDVT3Calculate", PmDVT3Calculate) });

                Byte[] mybytes = report.Render("pdf", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings); //for exporting to PDF  


                return mybytes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private byte[] CrearTicketItau(int TicketNumber)
        {

            try
            {


                DataSet dsData = new DataSet();
                dsData = PaymetTycketsReportBusinessLogic.GenerateTicketItau(TicketNumber);

                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                LocalReport report = new LocalReport();
                report.ReportPath = Server.MapPath("~/Reports/GenerateTicketItau.rdlc");

                ReportDataSource rdsInfoBank = new ReportDataSource();
                rdsInfoBank.Name = "dtsInfoBank";//This refers to the dataset name in the RDLC file  
                rdsInfoBank.Value = dsData.Tables[0];


                ReportDataSource rdsdtOrder = new ReportDataSource();
                rdsdtOrder.Name = "dtsOrder";//This refers to the dataset name in the RDLC file  
                rdsdtOrder.Value = dsData.Tables[1];

                ReportDataSource rdsdtDetails = new ReportDataSource();
                rdsdtDetails.Name = "dtsDetailsBank";
                rdsdtDetails.Value = dsData.Tables[2];

                Byte[] bitmapData = CodeBar(CreateCodebarTicketItau(dsData.Tables[1], dsData.Tables[0]));

                DataTable dtCodeBar = new DataTable("dtImage");
                DataColumn dc = new DataColumn("CodeBar");
                dc.DataType = typeof(byte[]);
                dtCodeBar.Columns.Add(dc);
                dtCodeBar.Rows.Add(new Object[] { bitmapData });
                ReportDataSource rdsdstImage = new ReportDataSource();
                rdsdstImage.Name = "dstImage";
                rdsdstImage.Value = dtCodeBar;

                report.DataSources.Add(rdsInfoBank);
                report.DataSources.Add(rdsdtOrder);
                report.DataSources.Add(rdsdtDetails);
                report.DataSources.Add(rdsdstImage);


                Byte[] mybytes = report.Render("pdf", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings); //for exporting to PDF  
                return mybytes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static byte[] ExtractPages(Byte[] sourcePdfPath)
        {
            iTextSharp.text.pdf.PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            iTextSharp.text.pdf.PdfCopy pdfCopyProvider = null;
            iTextSharp.text.pdf.PdfImportedPage importedPage = null;
            System.IO.MemoryStream target = new System.IO.MemoryStream();
            reader = new iTextSharp.text.pdf.PdfReader(sourcePdfPath);
            int numberOfPages = reader.NumberOfPages;

            sourceDocument = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
            pdfCopyProvider = new iTextSharp.text.pdf.PdfCopy(sourceDocument, target);
            sourceDocument.Open();
            for (int i = 1; i <= numberOfPages; i++)
            {
                String pageText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i);

                if (pageText.Equals(""))
                    continue;

                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }

            sourceDocument.Close();
            reader.Close();

            return target.ToArray();
        }
        #endregion

        #region Valida Pago con PayPal Correcto

        private bool validaPayPal_Paid()
        {

            bool valida = false;
            int paymentTypeID = 0;
            string payPal_PaidID = string.Empty;
            PaymentDeclinedModel paymentDecline = new PaymentDeclinedModel();

            PaymentsTable objE = new PaymentsTable();
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            var ct = Order.GetPaymentsTable(objE);

            if (OrderContext.Order.AsOrder().OrderPayments.Count > 1)
            {
                for (int i = 0; i < OrderContext.Order.AsOrder().OrderPayments.Count; i++)
                {
                    if (OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID == 1)
                    {
                        paymentTypeID = OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID;
                        payPal_PaidID = OrderContext.Order.AsOrder().OrderPayments[i].TransactionID;

                        var paymentTable = ct[i];
                        paymentDecline.PaymentDecMon = paymentTable.AppliedAmount.ToString();
                        paymentDecline.PaymentDecCuo = paymentTable.NumberCuota.ToInt();
                    }
                }
            }
            else
            {
                paymentTypeID = OrderContext.Order.AsOrder().OrderPayments[0].PaymentTypeID;
                payPal_PaidID = OrderContext.Order.AsOrder().OrderPayments[0].TransactionID;

                if (paymentTypeID == 1)
                {
                    var paymentTable = ct[0];
                    paymentDecline.PaymentDecMon = paymentTable.AppliedAmount.ToString();
                    paymentDecline.PaymentDecCuo = paymentTable.NumberCuota.ToInt();
                }
            }


            if (paymentTypeID == 1 && !payPal_PaidID.IsNull())
            {
                valida = true;
            }
            else if (paymentTypeID != 1)
            {
                valida = true;
            }
            else
            {
                paymentDecline.TypeError = "{Type: No IFrame PayPal, Message: [No se Activo la Pantalla de Pasarela de pago con PayPal] }";
                paymentDecline.PaymentGatewayID = '0';
                paymentDecline.AccountId = OrderContext.Order.AsOrder().OrderCustomers[0].AccountID != null ? OrderContext.Order.AsOrder().OrderCustomers[0].AccountID : 0;
                paymentDecline.OrderID = OrderContext.Order.AsOrder().OrderNumber != null ? OrderContext.Order.AsOrder().OrderNumber.ToInt() : 0;

                var statusID = DataAccess.ExecWithStoreProcedureSave("Core", "InsertPaymentDeclined",
                                                                new SqlParameter("TypeError", SqlDbType.VarChar) { Value = paymentDecline.TypeError },
                                                                new SqlParameter("OrderID", SqlDbType.Int) { Value = paymentDecline.OrderID },
                                                                new SqlParameter("PaymentDecMon", SqlDbType.Decimal) { Value = paymentDecline.PaymentDecMon },
                                                                new SqlParameter("PaymentDecCuo", SqlDbType.Int) { Value = paymentDecline.PaymentDecCuo },
                                                                new SqlParameter("AccountId", SqlDbType.Int) { Value = paymentDecline.AccountId },
                                                                new SqlParameter("PaymentGatewayID", SqlDbType.SmallInt) { Value = paymentDecline.PaymentGatewayID });

                valida = false;
            }

            return valida;
        }

        #endregion 

    }
}



