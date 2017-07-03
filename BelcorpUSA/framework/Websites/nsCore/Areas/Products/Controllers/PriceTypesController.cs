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
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Products.Controllers
{
	public class PriceTypesController : BaseProductsController
	{
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Index()
		{
			return View(ProductPriceType.LoadAll());
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(List<ProductPriceType> priceTypes)
        {
            try
            {
                List<ProductPriceType> oldTypes = ProductPriceType.LoadAll();
                StringBuilder builder = new StringBuilder();
                builder.Append("<thead ><tr style=\"margin-bottom: 10px;\"> <th></th><th><span class=\"tableHeader\">").Append(Translation.GetTerm("PriceName", "Price Name")).Append("</span>")
                    .Append("</th><th><span class=\"tableHeader\">").Append(Translation.GetTerm("Mandatory", "Mandatory")).Append("</span></th></tr></thead>");

                foreach (var lockedType in oldTypes.Where(r => !r.Editable))
                {
                    builder.Append("<tr> <td><span class=\"UI-icon icon-lock\"").Append(" title=\"locked\"></span></td>")
                           .Append("<td><input type=\"text\" value=\"").Append(lockedType.GetTerm()).Append("\" disabled=\"disabled\" /></td>")
                           .Append("<td class=\"itemMandatory\"><input type=\"checkbox\" value=\"").Append(lockedType.ProductPriceTypeID).Append("\"").Append(lockedType.Mandatory? "checked=\"checked\"" : "").Append("disabled=\"disabled\"/> </td> </tr>");
                }

                foreach (ProductPriceType priceType in priceTypes)
                {
                    ProductPriceType productPriceType;
                    if (priceType.ProductPriceTypeID > 0)
                        productPriceType = oldTypes.First(l => l.ProductPriceTypeID == priceType.ProductPriceTypeID);
                    else
                    {
                        productPriceType = new ProductPriceType();
                        productPriceType.TermName = priceType.Name.ToPascalCase().RemoveSpaces();
                        productPriceType.Editable = true;
                    }

                    if (!string.IsNullOrEmpty(priceType.Name) && CoreContext.CurrentLanguageID == Language.English.LanguageID)
                    {
                        //KLC - CSTI
                        productPriceType = new ProductPriceType();
                        productPriceType.ProductPriceTypeID = priceType.ProductPriceTypeID;
                        productPriceType.Name = priceType.Name;
                        productPriceType.TermName = priceType.Name.ToPascalCase().RemoveSpaces();
                        productPriceType.Active = true;
                        productPriceType.Mandatory = priceType.Mandatory;
                        PriceTypeDataAcces.InsertProductPriceType(productPriceType);
                        SmallCollectionCache.Instance.ProductPriceTypes.ExpireCache();
                    }


                    var term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(productPriceType.TermName, CoreContext.CurrentLanguageID);
                    if (term != null && term.Term != priceType.Name)
                    {
                        term.Term = priceType.Name;
                        term.Save();
                    }

                    //bool activeStat = false;
                    //activeStat = priceType.Mandatory == true ? true : false;

                    builder.Append("<tr><td colspan='2'><input type=\"text\" name=\"value").Append(productPriceType.ProductPriceTypeID).Append("\" value=\"").Append(term == null ? productPriceType.Name : term.Term).Append("\" class=\"priceType\" maxlength=\"50\" />")
                           .Append("<span style=\"text-align:right;margin-left: 10px;\"> <input type=\"checkbox\" value=").Append(productPriceType.ProductPriceTypeID).Append("\"").Append(priceType.Mandatory ? " checked=\"checked\"" : "").Append("class=\"activeMandatory\"  /></span></td>")
                           .Append("<td class='itemMandatory'><a href=\"javascript:void(0);\" class=\"delete listValue\"><span class=\"UI-icon icon-x\" title=\"Delete\"></span></a></td></tr>");
                }
                return Json(new { result = true, priceTypes = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Delete(int priceTypeId)
		{
			try
			{
				ProductPriceType.Delete(priceTypeId);
				SmallCollectionCache.Instance.ProductPriceTypes.ExpireCache();
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
