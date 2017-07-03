using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Globalization;


namespace NetSteps.Web.Mvc.Extensions
{
	public static class SelectListExtensions
	{
		/// <summary>
		/// Workaround for a bug in MVC where Html.DropDownListFor() does not set the selected value in partial views.
		/// </summary>
		public static SelectList SetSelectedValue(this IEnumerable<SelectListItem> items, object selectedValue)
		{
			if (items == null) items = Enumerable.Empty<SelectListItem>();

			return selectedValue == null
				? new SelectList(items, "Value", "Text")
				: new SelectList(items, "Value", "Text", selectedValue.ToString());
		}


		public static IEnumerable<SelectListItem> ToSelectListItemsForProvince(
							this IEnumerable<StateProvince> stateProvinces, int countryId, bool isoUseName, int? selectedId = null)
		{
			return stateProvinces.Where(state => state.CountryID == countryId)
				.OrderBy(state => state.StateAbbreviation)
				.Select(state =>
						new SelectListItem
						{
							Text = isoUseName ? state.Name : state.StateAbbreviation,
							Value = state.StateProvinceID.ToString(),
							Selected = Selected(state.StateProvinceID, selectedId)
						});
		}


		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<UserStatus> userStatuses, int? selectedId = null)
		{
			return userStatuses.OrderBy(user => user.UserStatusID)
				.Select(user =>
						new SelectListItem
						{
							Text = user.GetTerm(),
							Value = user.UserStatusID.ToString(),
							Selected = Selected(user.UserStatusID, selectedId)
						});
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<AccountStatus> accountStatuses, int? selectedId = null)
		{
			return accountStatuses.OrderBy(a => a.AccountStatusID)
				.Select(a =>
						new SelectListItem
						{
							Text = a.GetTerm(),
							Value = a.AccountStatusID.ToString(),
							Selected = Selected(a.AccountStatusID, selectedId)
						});
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<AccountStatusChangeReason> accountStatuses, int? selectedId = null)
		{
			return accountStatuses.OrderBy(a => a.AccountStatusChangeReasonID)
				.Select(a =>
						new SelectListItem
						{
							Text = a.Name,
							Value = a.AccountStatusChangeReasonID.ToString(),
							Selected = Selected(a.AccountStatusChangeReasonID, selectedId)
						});
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<AutoshipScheduleDay> days, int? selectedId = null)
		{
			return days.OrderBy(d => d.Day)
				.Select(d =>
							new SelectListItem
							{
								Text = d.Day.ToString(),
								Value = d.Day.ToString(),
								Selected = Selected(d.Day, selectedId)
							});
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Category> categories, int? selectedId = null)
		{
			return categories
				.Select(c =>
						new SelectListItem
							{
								Text = c.Translations.GetByLanguageIdOrDefaultForDisplay().Name,
								Value = c.CategoryID.ToString(),
								Selected = Selected(c.CategoryID, selectedId)
							});
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Product> products, int? selectedId = null)
		{
			IEnumerable<SelectListItem> result = (new List<object>() { new object() })
				.Select(p => new SelectListItem
				{
					Text = Translation.GetTerm("SelectEnrollmentKitVariant", "Please Select Enrollment Kit"),
					Value = "0",
					Selected = !selectedId.HasValue || selectedId == 0
				})
				.Union(
				products.Select(p =>
					new SelectListItem
					{
						Text = p.Translations.GetByLanguageIdOrDefaultForDisplay().Name,
						Value = p.ProductID.ToString(),
						Selected = Selected(p.ProductID, selectedId)
					}));
			return result;
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<KeyValuePair<string, int>> pairs, int? selectedId = null)
		{
			return pairs
				.Select(p =>
					new SelectListItem
					{
						Text = p.Key,
						Value = p.Value.ToString(),
						Selected = Selected(p.Value, selectedId)
					});
		}

		private static bool Selected(int value, int? selectedId)
		{
			return selectedId.HasValue && value == selectedId.Value;
		}
	}
}
