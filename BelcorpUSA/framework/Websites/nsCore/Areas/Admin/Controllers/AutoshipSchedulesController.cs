using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Controllers;

namespace nsCore.Areas.Admin.Controllers
{
	public class AutoshipSchedulesController : BaseController
	{
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult Index()
		{
			return View(AutoshipSchedule.LoadAllFull());
		}

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult Edit(int? id)
		{
			try
			{
				AutoshipSchedule schedule = null;
				if (id.HasValue)
					schedule = AutoshipSchedule.LoadFull(id.Value);
				if (schedule == null)
					schedule = new AutoshipSchedule();

				return View(schedule);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult GetProducts(int autoshipScheduleId, int page, int pageSize)
		{
			try
			{
				var builder = new StringBuilder();

				var schedule = AutoshipSchedule.LoadFull(autoshipScheduleId);

				if (!schedule.AutoshipScheduleProducts.Any())
				{
					builder.Append("<tr><td colspan=\"4\">" + Translation.GetTerm("ThereAreNoProducts", "There are no products.") + "</td></tr>");
				}
				else
				{
					var products = Inventory.GetProducts(schedule.AutoshipScheduleProducts.Select(a => a.ProductID));
					schedule.AutoshipScheduleProducts.Each(asp => asp.Product = products.First(p => p.ProductID == asp.ProductID));
					var count = 0;
					foreach (var asProduct in schedule.AutoshipScheduleProducts.OrderBy(p => p.Product.SKU).Skip(page * pageSize).Take(pageSize))
					{
						builder.Append("<tr class=\"GridRow").Append(count % 2 == 0 ? "" : " Alt").Append("\">")
							.AppendCheckBoxCell(cssClass: "productSelector", value: asProduct.AutoshipScheduleProductID.ToString())
							.AppendLinkCell(string.Format("~/Products/Products/Overview/{0}/{1}", asProduct.Product.ProductBaseID, asProduct.Product.ProductID), asProduct.Product.SKU)
							.AppendCell(asProduct.Product.Translations.Name())
							.AppendCell(asProduct.Quantity.ToString())
							.Append("</tr>");
						++count;
					}
				}

				return Json(new { result = true, totalPages = Math.Ceiling(schedule.AutoshipScheduleProducts.Count() / (double)pageSize), page = builder.ToString() });
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult AddProduct(int autoshipScheduleId, int productId, int quantity)
		{
			try
			{
				AutoshipSchedule schedule = AutoshipSchedule.LoadFull(autoshipScheduleId);
				if (!schedule.AutoshipScheduleProducts.Any(p => p.ProductID == productId))
					schedule.AutoshipScheduleProducts.Add(new AutoshipScheduleProduct()
					{
						ProductID = productId,
						Quantity = quantity
					});

				schedule.Save();

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult TryAddProduct(int autoshipScheduleId, string query, int quantity)
		{
			try
			{
				var products = Product.SlimSearch(query);
				if (products.Count > 0)
				{
					AutoshipSchedule schedule = AutoshipSchedule.LoadFull(autoshipScheduleId);

					if (!schedule.AutoshipScheduleProducts.Any(p => p.ProductID == products.First().ProductID))
						schedule.AutoshipScheduleProducts.Add(new AutoshipScheduleProduct()
						{
							ProductID = products.First().ProductID,
							Quantity = quantity
						});

					schedule.Save();

					return Json(new { result = true });
				}
				else
					return Json(new { result = false });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult RemoveProducts(List<int> products)
		{
			try
			{
				if (products != null)
				{
					foreach (int autoshipScheduleProductId in products)
					{
						AutoshipScheduleProduct.Delete(autoshipScheduleProductId);
					}
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		[FunctionFilter("Admin-Create and Edit Schedule", "~/Accounts")]
		public virtual ActionResult Save(
			int? scheduleId,
			string name,
			int scheduleTypeId,
			int? baseSiteId,
			int intervalTypeId,
			short orderTypeId,
			decimal? minimumCommissionableTotal,
			int? maxRetryCount,
			int? maxFailedIntervals,
			bool active,
			bool enrollable,
			bool editable,
			List<int> accountTypes,
			List<byte> runDays)
		{
			try
			{
				AutoshipSchedule schedule = null;
				if (scheduleId.HasValue)
					schedule = AutoshipSchedule.LoadFull(scheduleId.Value);
				else
					schedule = new AutoshipSchedule();

				schedule.Name = name;
				schedule.AutoshipScheduleTypeID = scheduleTypeId;
				schedule.BaseSiteID = baseSiteId;
				schedule.IntervalTypeID = intervalTypeId;
				schedule.OrderTypeID = orderTypeId;
				schedule.MinimumCommissionableTotal = minimumCommissionableTotal;

				schedule.MaximumAttemptsPerInterval = maxRetryCount.HasValue && maxRetryCount.Value == 0 ? null : maxRetryCount;
				schedule.MaximumFailedIntervals = maxFailedIntervals.HasValue && maxFailedIntervals.Value == 0 ? null : maxRetryCount;
				schedule.Active = active;
				schedule.IsEnrollable = enrollable;
				schedule.IsTemplateEditable = editable;

				if (accountTypes != null)
				{
					schedule.AccountTypes.SyncTo(accountTypes, at => at.AccountTypeID, id => AccountType.Load(id.ToShort()));
				}
				else
					schedule.AccountTypes.RemoveAll();

				if (runDays != null)
				{
					// Remove deselected days from database
					var daysToRemove = schedule.AutoshipScheduleDays.Where(s => !runDays.Contains(s.Day)).ToList();
					daysToRemove.ForEach(d => d.MarkAsDeleted());

					// Add new Days
					schedule.AutoshipScheduleDays.SyncTo(runDays, asd => asd.Day, day => new AutoshipScheduleDay() { Day = day });
				}
				else
					schedule.AutoshipScheduleDays.RemoveAllAndMarkAsDeleted();

				schedule.Save();

				return Json(new { result = true, scheduleId = schedule.AutoshipScheduleID });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
