using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;

namespace nsCore.Areas.Products.Controllers
{
	public class CustomerTypesController : BaseProductsController
	{
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Index()
		{
			return View();
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Get(int storeFrontId)
		{
			try
			{
				return Json(AccountPriceType.LoadAllByStoreFront(storeFrontId).Select(apt => new
				{
					accountPriceType = apt.AccountPriceTypeID,
					accountType = apt.AccountTypeID,
					relationshipType = apt.PriceRelationshipTypeID,
					priceType = apt.ProductPriceTypeID
				}));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Save(List<AccountPriceType> accountTypes)
		{
			try
			{
				foreach (AccountPriceType accountPriceType in accountTypes)
				{
					if (accountPriceType.AccountPriceTypeID == 0)
					{
						accountPriceType.Save();
					}
					else
					{
						AccountPriceType priceType = AccountPriceType.Load(accountPriceType.AccountPriceTypeID);
						priceType.ProductPriceTypeID = accountPriceType.ProductPriceTypeID;
						priceType.Save();
					}
				}

				SmallCollectionCache.Instance.AccountPriceTypes.ExpireCache();

				return Json(new
				{
					result = true,
					accountTypes = accountTypes.Select(apt => new
					{
						accountType = apt.AccountTypeID,
						relationshipType = apt.PriceRelationshipTypeID,
						accountPriceType = apt.AccountPriceTypeID
					})
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
