using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Products.Controllers
{
    public class ResourceTypesController : BaseController
    {
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View(ProductFileType.LoadAll());
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(List<ProductFileType> resourceTypes)
        {
            try
            {
				if (resourceTypes == null)
				{
					return Json(new { result = false, message = Translation.GetTerm("Portal_ProductResourceTypes_ThereAreNoChanges", "There are no changes.") });
				}

            	List<ProductFileType> oldTypes = ProductFileType.LoadAll();
                StringBuilder builder = new StringBuilder();
                builder.Append("Active");  
                foreach (var lockedType in oldTypes.Where(r => !r.Editable))
                {
                    bool activeStatus = false;

                    activeStatus = lockedType.Active == true ? true : false;

                    builder.Append("<li><input type=\"checkbox\"").Append(activeStatus == true ? " checked=\"checked\"" : " ")
                        .Append("\"  disabled=\"disabled\" /><span>&nbsp;&nbsp;&nbsp;&nbsp;</span><input type=\"text\" value=\"")
                        .Append(lockedType.GetTerm()).Append("\" disabled=\"disabled\" /><span class=\"UI-icon icon-lock\" title=\"locked\"></span></li>");
                    
                }
                foreach (ProductFileType resourceType in resourceTypes)
                {
                    ProductFileType productResourceType;
                    if (resourceType.ProductFileTypeID > 0)
                        productResourceType = oldTypes.First(l => l.ProductFileTypeID == resourceType.ProductFileTypeID);
                    else
                    {
                        productResourceType = new ProductFileType();
                        productResourceType.Editable = true;
                        productResourceType.TermName = resourceType.Name.ToPascalCase().RemoveSpaces();
                        productResourceType.Active = resourceType.Active;
                    }
                    if (productResourceType.Name != resourceType.Name && !string.IsNullOrEmpty(resourceType.Name) && CoreContext.CurrentLanguageID == Language.English.LanguageID)
                    {
                        productResourceType.Name = resourceType.Name;

                        productResourceType.Save();

                        oldTypes = ProductFileType.LoadAll();
                        productResourceType = oldTypes.First(l => l.Name == resourceType.Name);

                    }

                    if (productResourceType.Active != resourceType.Active && productResourceType.Name == resourceType.Name && !string.IsNullOrEmpty(resourceType.Name) && CoreContext.CurrentLanguageID == Language.English.LanguageID)
                    {
                        productResourceType.Active = resourceType.Active;
                        productResourceType.Save();
                    }

                    var term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(productResourceType.TermName, CoreContext.CurrentLanguageID);
                    if (term != null && term.Term != resourceType.Name)
                    {
                        term.Term = resourceType.Name;
                        term.Save();
                    }

                    bool activeStat = false;
                    activeStat = resourceType.Active == true ? true : false;

                    builder.Append("<li><input type=\"checkbox\" name=\"check").Append(productResourceType.ProductFileTypeID).Append(" \" ").Append(activeStat == true ? " checked=\"checked\"" : " ").Append("class=\"activeStatus\"/>")
                      .Append("<span>&nbsp;&nbsp;&nbsp;&nbsp;</span><input type=\"text\" name=\"value").Append(productResourceType.ProductFileTypeID).Append("\" value=\"").Append(term == null ? productResourceType.Name : term.Term)
                      .Append("\" class=\"resourceType\" maxlength=\"100\" /><a href=\"javascript:void(0);\" class=\"delete listValue\"><span class=\"UI-icon icon-x\" title=\"Delete\"></span></a></li>");
                   
                }
                return Json(new { result = true, resourceTypes = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Delete(int resourceTypeId)
        {
            try
            {
                ProductFileType.Delete(resourceTypeId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
