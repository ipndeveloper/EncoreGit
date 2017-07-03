using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Products.Controllers
{
	public class ProductTypesController : BaseProductsController
	{
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Index()
		{
			return View(ProductType.LoadAll());
		}

		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Edit(int? id)
		{
			ProductType type;
			if (id.HasValue && id.Value > 0)
			{
				type = ProductType.LoadFull(id.Value);
			}
			else
				type = new ProductType();
			return View(type);
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Save(int? productTypeId, string name, bool active, List<int> properties)
		{
			try
			{
				ProductType type;
				if (productTypeId.HasValue && productTypeId.Value > 0)
					type = ProductType.LoadFull(productTypeId.Value);
				else
				{
					type = new ProductType();
					type.TermName = name.ToPascalCase().RemoveSpaces();
					if (TermTranslation.LoadTermByTermNameAndLanguageID(type.TermName, CoreContext.CurrentLanguageID) != null)
					{
						throw new Exception(Translation.GetTerm("ProductType_TermInUse", "TermName '{0}' in use", type.TermName));
					}
					type.Editable = true;
				}

				if (CoreContext.CurrentLanguageID == Language.English.LanguageID)
				{
					type.Name = name;
				}
				type.Active = active;

				var term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(type.TermName, CoreContext.CurrentLanguageID);
				if (term != null && term.Term != name)
				{
					term.Term = name;
					term.Save();
				}

				if (properties != null)
					type.ProductPropertyTypes.SyncTo(properties, p => p.ProductPropertyTypeID, id => ProductPropertyType.LoadFull(id));
				else
					type.ProductPropertyTypes.RemoveAll();

				type.Save();

				return Json(new { result = true, productTypeId = type.ProductTypeID });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Delete(int productTypeId)
		{
			try
			{
				var productType = ProductType.LoadFull(productTypeId);
				if (productType != null)
				{
					productType.ProductPropertyTypes.RemoveAll();
					productType.Save();
				}
				ProductType.Delete(productTypeId);
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
