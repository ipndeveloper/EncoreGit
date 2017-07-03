using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Products.Controllers
{
    public class ShippingRegionsController : BaseProductsController
    {
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View(SmallCollectionCache.Instance.ShippingRegions.ToList());
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(List<ShippingRegion> shippingRegions)
        {
            try
            {
                var oldRegions = ShippingRegion.LoadAll();
                var newRegions = new List<ShippingRegion>();
                StringBuilder builder = new StringBuilder();
                foreach (var lockedRegion in oldRegions.Where(r => !r.Editable))
                {
                    builder.Append("<li><span class=\"UI-icon icon-lock\" title=\"locked\"></span><input type=\"text\" value=\"").Append(lockedRegion.GetTerm()).Append("\" disabled=\"disabled\" /></li>");
                }
                foreach (ShippingRegion region in shippingRegions)
                {
                    ShippingRegion shippingRegion;
                    if (region.ShippingRegionID > 0)
                        shippingRegion = oldRegions.First(l => l.ShippingRegionID == region.ShippingRegionID);
                    else
                    {
                        shippingRegion = new ShippingRegion()
                        {
                            Editable = true
                        };
                        shippingRegion.TermName = region.Name.ToPascalCase().RemoveSpaces();
                    }
                    if (shippingRegion.Name != region.Name && !string.IsNullOrEmpty(region.Name) && CoreContext.CurrentLanguageID == Language.English.LanguageID)
                    {
                        shippingRegion.Name = region.Name;
                    }

                    shippingRegion.Save();

                    var term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(shippingRegion.TermName, CoreContext.CurrentLanguageID);
                    if (term != null && term.Term != region.Name)
                    {
                        term.Term = region.Name;
                        term.Save();
                    }

                    builder.Append("<li><input type=\"text\" name=\"value").Append(shippingRegion.ShippingRegionID).Append("\" value=\"").Append(term == null ? region.Name : term.Term)
                        .Append("\" class=\"shippingRegion\" maxlength=\"50\" /><a href=\"javascript:void(0);\" class=\"delete listValue\"><span src=\"UI-icon icon-x\" title=\"Delete\"></span></a></li>");
                }
                return Json(new { result = true, shippingRegions = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Delete(int shippingRegionId)
        {
            try
            {
                ShippingRegion.Delete(shippingRegionId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Manage()
        {
            return View();
        }

        protected virtual Dictionary<int, List<StateProvince>> Countries
        {
            get
            {
                if (Session["ShippingRegionCountries"] == null)
                    Session["ShippingRegionCountries"] = new Dictionary<int, List<StateProvince>>();
                return Session["ShippingRegionCountries"] as Dictionary<int, List<StateProvince>>;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetStates(int countryId, int regionId)
        {
            try
            {
                StringBuilder unusedStates = new StringBuilder(), regionStates = new StringBuilder();
                List<StateProvince> states;
                if (!Countries.ContainsKey(countryId))
                    Countries.Add(countryId, SmallCollectionCache.Instance.StateProvinces.GetByCountryID(countryId));
                states = Countries[countryId];
                List<ShippingRegion> allShippingRegions = ShippingRegion.LoadBatch(states.Where(s => s.ShippingRegionID.HasValue).Select(s => s.ShippingRegionID.Value).ToList());
                foreach (StateProvince state in states)
                {
                    if (state.ShippingRegionID.HasValue && state.ShippingRegionID == regionId)
                        regionStates.Append("<option value=\"").Append(state.StateProvinceID).Append("\">").Append(state.Name).Append("</option>");
                    else
                        unusedStates.Append("<option value=\"").Append(state.StateProvinceID).Append("\">").Append(state.Name).Append(state.ShippingRegionID.HasValue ? " (" + allShippingRegions.First(sr => sr.ShippingRegionID == state.ShippingRegionID.Value).Name + ")" : "").Append("</option>");
                }
                return Json(new { regionStates = regionStates.ToString(), unusedStates = unusedStates.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetWarehouse(int regionId)
        {
            try
            {
                ShippingRegion shippingRegion = ShippingRegion.LoadFull(regionId);
                return Json(new { result = true, WarehouseID = shippingRegion.WarehouseID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult SaveRegionManagementSettings(int regionId, int warehouseId, int countryId, List<int> states)
        {
            try
            {
                List<StateProvince> stateProvinces;
                if (!Countries.ContainsKey(countryId))
                    Countries.Add(countryId, SmallCollectionCache.Instance.StateProvinces.GetByCountryID(countryId));
                stateProvinces = Countries[countryId];

                // remove region on stateprovince if it isn't in the states list but does exist in the repository
                foreach (var state in stateProvinces.Where(sp => sp.ShippingRegionID == regionId && !states.Contains(sp.StateProvinceID)))
                {
                    state.ShippingRegionID = null;
                    state.Save();
                }

                // add region on stateprovince if it is in the states list but doesn't exist in the repository
                foreach (var state in stateProvinces.Where(sp => sp.ShippingRegionID != regionId && states.Contains(sp.StateProvinceID)))
                {
                    state.ShippingRegionID = regionId;
                    state.Save();
                }

                ShippingRegion shippingRegion = ShippingRegion.LoadFull(regionId);
                if (shippingRegion.WarehouseID != warehouseId)
                {
                    shippingRegion.WarehouseID = warehouseId;
                    shippingRegion.Save();
                }

                return Json(new { result = true, message = "Saved!" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
