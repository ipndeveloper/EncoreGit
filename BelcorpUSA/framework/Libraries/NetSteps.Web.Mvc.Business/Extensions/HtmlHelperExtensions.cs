using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Web.Mvc.Extensions;

namespace NetSteps.Web.Mvc.Business.Extensions
{
	public static class HtmlHelperExtensions
	{
		private static string GetFormat(CultureInfo culture, bool isPositiveValue)
		{
			string format = "";
			if (isPositiveValue)
			{
				switch (culture.NumberFormat.CurrencyPositivePattern)
				{
					case 0: format = "{0}{1}"; break;
					case 1: format = "{1}{0}"; break;
					case 2: format = "{0} {1}"; break;
					case 3: format = "{1} {0}"; break;
				}
			}
			else
			{
				switch (culture.NumberFormat.CurrencyNegativePattern)
				{
					case 0: format = "({0}{1})"; break;
					case 1: format = "-{0}{1}"; break;
					case 2: format = "{0}-{1}"; break;
					case 3: format = "{0}{1}-"; break;
					case 4: format = "({1}{0})"; break;
					case 5: format = "-{1}{0}"; break;
					case 6: format = "{1}-{0}"; break;
					case 7: format = "{1}{0}-"; break;
					case 8: format = "-{1} {0}"; break;
					case 9: format = "-{0} {1}"; break;
					case 10: format = "{1} {0}-"; break;
					case 11: format = "{0} {1}-"; break;
					case 12: format = "{0} -{1}"; break;
					case 13: format = "{1}- {0}"; break;
					case 14: format = "({0} {1})"; break;
					case 15: format = "({1} {0}) "; break;
				}
			}
			return format.Replace("{0}", "<span class=\"currencyLabel\">{0}</span>");
		}

		public static string TextBox(this HtmlHelper helper, Currency currency, string name, bool isPositiveValue = true)
		{
			CultureInfo culture = new CultureInfo(currency.CultureInfo);
			return string.Format(GetFormat(culture, isPositiveValue), culture.NumberFormat.CurrencySymbol, helper.TextBox(name).ToHtmlString());
		}

		public static string TextBox(this HtmlHelper helper, Currency currency, string name, object value, bool isPositiveValue = true)
		{
			CultureInfo culture = new CultureInfo(currency.CultureInfo);
			return string.Format(GetFormat(culture, isPositiveValue), culture.NumberFormat.CurrencySymbol, helper.TextBox(name, value).ToHtmlString());
		}

		public static string TextBox(this HtmlHelper helper, Currency currency, string name, object value, IDictionary<string, object> htmlAttributes, bool isPositiveValue = true)
		{
			CultureInfo culture = new CultureInfo(currency.CultureInfo);
			return string.Format(GetFormat(culture, isPositiveValue), culture.NumberFormat.CurrencySymbol, helper.TextBox(name, value, htmlAttributes).ToHtmlString());
		}

		public static string TextBox(this HtmlHelper helper, Currency currency, string name, object value, object htmlAttributes, bool isPositiveValue = true)
		{
			if (!currency.CultureInfo.IsNullOrEmpty())
			{
				CultureInfo culture = new CultureInfo(currency.CultureInfo);
				return string.Format(GetFormat(culture, isPositiveValue), culture.NumberFormat.CurrencySymbol, helper.TextBox(name, value, htmlAttributes).ToHtmlString());
			}
			else
			{
				return string.Format("{0}{1}", string.Empty, helper.TextBox(name, value, htmlAttributes).ToHtmlString());
			}
		}

		private static string GetTitle(this HtmlHelper helper, Product product)
		{
			var applicableProperties = product.Properties.Where(p => p.ProductPropertyValue != null && !p.ProductPropertyValue.Value.IsNullOrEmpty()).ToList();
			int propertyCount = applicableProperties.Count();
			string toolTip = product.SKU + " (";
			for (int i = 0; i < propertyCount; i++)
			{
				ProductProperty property = applicableProperties[i];
				if (i != 0)
				{
					toolTip += ", " + property.ProductPropertyValue.Value;
				}
				else
				{
					toolTip += property.ProductPropertyValue.Value;
				}

			}
			toolTip += ")";
			return toolTip;
		}

		public static MvcHtmlString DropDownVariantProducts(this HtmlHelper helper, ProductBase productBase, string errorMessage = "VariantProductsDropDown", object htmlAttributes = null, int? selectedProductID = null, string selectTextTermName = null, bool includeVariantTemplate = true)
		{
			Dictionary<int, string> list;
			if (includeVariantTemplate)
			{
				list = productBase.Products.ToDictionary(p => p.ProductID,
					p => p.IsVariantTemplate ? Translation.GetTerm("Master") + "-" + p.SKU + "-" + p.Translations.Name() + "," + GetTitle(helper, p) : p.SKU + "-" + p.Translations.Name() + "," + GetTitle(helper, p));
			}
			else
			{
				list = productBase.Products.Where(p => !p.IsVariantTemplate).ToDictionary(p => p.ProductID,
					p => p.SKU + " - " + p.Translations.Name() + ", " + GetTitle(helper, p));
			}
			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedProductID, selectTextTermName);
		}

		public static MvcHtmlString DropDownLanguages(this HtmlHelper helper, string errorMessage = "LanguagesDropDown", object htmlAttributes = null, int? selectedLanguageID = null, string selectTextTermName = null)
		{
			var list = TermTranslation.GetTranslatedLanguages();
			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedLanguageID, selectTextTermName);
		}

		public static MvcHtmlString DropDownCountries(this HtmlHelper helper, string errorMessage = "CountriesDropDown", object htmlAttributes = null, int? selectedCountryID = null, string selectTextTermName = null)
		{
			var list = TermTranslation.GetTranslatedCountries();
			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedCountryID, selectTextTermName);
		}

		public static MvcHtmlString DropDownMarkets(this HtmlHelper helper, string errorMessage = "MarketsDropDown", object htmlAttributes = null, int? selectedMarketID = null, string selectTextTermName = null)
		{
			var list = TermTranslation.GetTranslatedMarkets();
			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedMarketID, selectTextTermName);
		}

		public static MvcHtmlString DropDownBaseSites(this HtmlHelper helper, string errorMessage = "DropDownBaseSites", List<Site> baseSites = null, object htmlAttributes = null, int? selectedMarketID = null, string selectTextTermName = null)
		{
			var list = baseSites.ToDictionary(s => s.SiteID, s => s.Name);

			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedMarketID, selectTextTermName);
		}

		public static MvcHtmlString DropDownAccountStatus(this HtmlHelper helper, string errorMessage = "AccountStatusDropDown", object htmlAttributes = null,
															short? selectedStatusID = null, string selectTextTermName = null, List<short> excludedStatuses = null)
		{
			var list = TermTranslation.GetTranslatedAccountStatuses();
			if (excludedStatuses != null && excludedStatuses.Any())
			{
				excludedStatuses.Each(es => list.Remove(es));
			}
			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedStatusID, selectTextTermName);
		}

		public static MvcHtmlString DropDownAccountType(this HtmlHelper helper, string errorMessage = "AccountTypeDropDown", object htmlAttributes = null,
															short? selectedTypeID = null, string selectTextTermName = null)
		{
			var list = TermTranslation.GetTranslatedAccountTypes();
			return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedTypeID, selectTextTermName);
		}



		private static MvcHtmlString DropDownHelper<TKey, TValue>(HtmlHelper helper, Dictionary<TKey, TValue> values, string errorMessage,
																	 object htmlAttributes = null, int? selectedID = null, string selectTextTermName = null)
		{
			var list = values
				.Select(s => new ExtendedSelectListItem<TKey, TValue>
						{
							SelectedID = selectedID,
							SelectItem = s
						}).ToList();

			if (selectTextTermName != null)
			{
				list = list
						   .Prepend(new ExtendedSelectListItem<TKey, TValue>
						   {
							   Text = Translation.GetTerm(selectTextTermName),
							   Selected = !(list.Any(s => s.Selected))
						   }).ToList();
			}

			return helper.DropDownList(errorMessage, list, htmlAttributes ?? new { });
		}

		public static string GetEmailValidaionRegex(this HtmlHelper helper)
		{
			return Constants.HTMLHelperEmailValidaionRegex;
		}

        //@01 AINI
         public static MvcHtmlString DropDownRelationShip(this HtmlHelper helper, string errorMessage = "RelationShipDropDown", object htmlAttributes = null, int? selectedRelationShipID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetRelationShip();
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedRelationShipID, selectTextTermName);
        }

        public static MvcHtmlString DropDownSchoolineLevel(this HtmlHelper helper, string errorMessage = "SchoolineLevelDropDown", object htmlAttributes = null, int? selectedSchoolineLevelID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetAccountPropertiesValuesByTermName("SchoolingLevel");
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedSchoolineLevelID, selectTextTermName);
        }

        public static MvcHtmlString DropDownNationality(this HtmlHelper helper, string errorMessage = "NationalityDropDown", object htmlAttributes = null, int? selectedNationalityID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetAccountPropertiesValuesByTermName("Nationality");
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedNationalityID, selectTextTermName);
        }

        public static MvcHtmlString DropDownMaritalStatus(this HtmlHelper helper, string errorMessage = "MaritalStatusDropDown", object htmlAttributes = null, int? selectedMaritalStatusID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetAccountPropertiesValuesByTermName("MaritalStatus");
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedMaritalStatusID, selectTextTermName);
        }

        public static MvcHtmlString DropDownOccupation(this HtmlHelper helper, string errorMessage = "OccupationDropDown", object htmlAttributes = null, int? selectedOccupationID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetAccountPropertiesValuesByTermName("Occupation");
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedOccupationID, selectTextTermName);
        }

        public static MvcHtmlString DropDownInternetUse(this HtmlHelper helper, string errorMessage = "InternetUseDropDown", object htmlAttributes = null, int? selectedInternetUseID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetAccountPropertiesValuesByTermName("InternetUseFrecuency");
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedInternetUseID, selectTextTermName);
        }

        public static MvcHtmlString DropDownSupportTicketStatuses(this HtmlHelper helper, string errorMessage = "SupportTicketStatusDropDown", object htmlAttributes = null, int? selectedStatusID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetSupportTicketStatus();
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedStatusID, selectTextTermName);
        }

        public static MvcHtmlString DropDownSupportTicketPriority(this HtmlHelper helper, string errorMessage = "SupportTicketPriorityDropDown", object htmlAttributes = null, int? selectedPriorityID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetSupportTicketPriority();
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedPriorityID, selectTextTermName);
        }
        //@01 AFIN

        public static MvcHtmlString DropDownReentryRulesValuesByType(this HtmlHelper helper, string errorMessage = "ReentryRuleValueDropDown", object htmlAttributes = null, int? selectedReentryRuleValueID = null, string selectTextTermName = null, string ReentryRuleTypeID = null)
        {
            var list = ReEntryRulesBusinessLogic.GetReentryRulesValuesByType(ReentryRuleTypeID);
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedReentryRuleValueID, selectTextTermName);
        }

        public static MvcHtmlString DropDownSupportLevel(this HtmlHelper helper, string errorMessage = "SupportLevelDropDown", object htmlAttributes = null, int? selectedSupportLevelID = null, string selectTextTermName = null, string SupportLevelID = null)
        {
            var list = SupportLevels.GetLevels();
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedSupportLevelID, selectTextTermName);
        }

        /*Antonio Campos Santos: 2015/12/18*/
        public static MvcHtmlString DropDownOpcionsDisbursementsManagment(this HtmlHelper helper, string errorMessage = "OpcionsDisbursementsManagmentDropDown", object htmlAttributes = null,short? selectedOptionID = null, string selectTextTermName = null, List<short> excludedStatuses = null)
        {
            Dictionary<int, string> list = new Dictionary<int, string>();


            list.Add(1, Translation.GetTerm("ApprovalCommissionsAndBonus", "Approval Commissions and Bonus").ToString());
            list.Add(2, Translation.GetTerm("ApprovalDisbursements").ToString());
            list.Add(3, Translation.GetTerm("SendToTheBank").ToString());
            
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedOptionID, selectTextTermName);
        }


        public static MvcHtmlString DropDownFineBaseAmounts(this HtmlHelper helper, string errorMessage = "FineBaseAmountsDropDown", object htmlAttributes = null, int? selectedFineBaseAmountsID = null, string selectTextTermName = null)
        {
            var list = AccountPropertiesBusinessLogic.GetFineBaseAmounts();
            return DropDownHelper(helper, list, errorMessage, htmlAttributes, selectedFineBaseAmountsID, selectTextTermName);
        }
	}
}
