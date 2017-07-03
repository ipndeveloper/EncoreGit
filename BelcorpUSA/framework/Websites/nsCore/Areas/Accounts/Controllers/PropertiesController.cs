using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Accounts.Controllers
{
    public class PropertiesController : BaseController
    {
        [FunctionFilter("Accounts-Edit Property Types", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
        {
            var properties = AccountPropertyType.Search(new FilterPaginatedListParameters<AccountPropertyType>()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection
            });

            AccountPropertyType newObject = new AccountPropertyType();
            
            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (var property in properties)
            {
                builder.Append("<tr>");
                if (property.Editable)
                    builder.AppendCheckBoxCell(value: property.AccountPropertyTypeID.ToString());
                else
                    builder.Append("<td></td>");
                builder.AppendLinkCell("~/Accounts/Properties/Edit/" + property.AccountPropertyTypeID, property.GetTerm())
                    .AppendCell(property.Required ? Translation.GetTerm("Yes") : Translation.GetTerm("No"))
                    .AppendCell(property.Active ? Translation.GetTerm("Yes") : Translation.GetTerm("No"))
                    .AppendCell(property.AccountPropertyValues.Select(v => v.Value).Join(", "))
                    .AppendCell(property.MinimumLength.ToString())
                    .AppendCell(property.MaximumLength.ToString())
                    .Append("</tr>");
                ++count;
            }
            return Json(new { result = true, totalPages = properties.TotalPages, page = builder.ToString() });
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Delete Property Type", "~/Accounts")]
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                foreach (int accountPropertyTypeId in items)
                {
                    AccountPropertyType.Delete(accountPropertyTypeId);
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Accounts-Edit Property Types", "~/Accounts")]
        public virtual ActionResult Edit(int? id)
        {
            AccountPropertyType property;
            if (id.HasValue && id.Value > 0)
            {
                property = AccountPropertyType.LoadFull(id.Value);
            }
            else
                property = new AccountPropertyType() { Editable = true };
            return View(property);
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Delete Property Type Value", "~/Accounts")]
        public virtual ActionResult DeleteValue(int valueId)
        {
            try
            {
                AccountPropertyValue.Delete(valueId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Edit Property Types", "~/Accounts")]
        public virtual ActionResult Save(int? propertyTypeId, string name, bool required, bool active, string dataType, List<AccountPropertyValue> values, string minLength, string maxLength)
        {
            try
            {
                AccountPropertyType property;
                if (propertyTypeId.HasValue && propertyTypeId.Value > 0)
                {
                    property = AccountPropertyType.LoadFull(propertyTypeId.Value);
                }
                else
                {
                    var existingPropertyWithSameName = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.AccountPropertyTypes.Where(p => p.Name == name.ToCleanString()).FirstOrDefault();
                    if (existingPropertyWithSameName != null)
                        return Json(new { result = false, message = Translation.TermTranslation.GetTerm("PropertyAlreadyExists", "A Property with the same name already exists") });

                    property = new AccountPropertyType();
                    property.Editable = true;
                }

                if (name.IsNullOrEmpty())
                    return Json(new { result = false, message = "Please enter a property name." });

                property.Required = required;
                property.Active = active;
                property.DataType = string.IsNullOrEmpty(dataType) ? "System.String" : dataType;

                if (property is ITermName)
                {
                    if (property.Name.IsNullOrEmpty())
                        property.Name = name.ToCleanString();

                    var termNameObj = property as ITermName;
                    if (termNameObj != null)
                    {
                        string typeName = termNameObj.GetType().Name;
                        if (termNameObj.TermName.IsNullOrEmpty() || !termNameObj.TermName.ToCleanString().StartsWith(typeName))
                            termNameObj.TermName = string.Format("{0}_{1}", typeName, property.Name);

                        TermTranslation term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(termNameObj.TermName, CoreContext.CurrentLanguageID); // Just edit the currently selected language term for now - JHE
                        if (term != null)
                        {
                            term.Term = name.ToCleanString();
                            term.Save();
                        }
                        else
                        {
                            if (CoreContext.CurrentLanguageID != Constants.Language.English.ToInt())
                            {
                                term = new TermTranslation();
                                term.LanguageID = Constants.Language.English.ToInt();
                                term.TermName = termNameObj.TermName;
                                term.Term = name.ToCleanString();
                                term.Active = true;
                                term.Save();
                            }
                            term = new TermTranslation();
                            term.LanguageID = CoreContext.CurrentLanguageID;
                            term.TermName = termNameObj.TermName;
                            term.Term = name.ToCleanString();
                            term.Active = true;
                            term.Save();
                        }
                    }
                }
                else
                    property.Name = name;

                if (values != null)
                {
                    foreach (var value in values)
                    {
                        if (value.AccountPropertyValueID > 0)
                            property.AccountPropertyValues.First(v => v.AccountPropertyValueID == value.AccountPropertyValueID).Value = value.Value;
                        else
                            property.AccountPropertyValues.Add(new AccountPropertyValue()
                            {
                                Value = value.Value,
                                Name = ""
                            });
                    }
                    List<AccountPropertyValue> removed = property.AccountPropertyValues.Where(v => !values.Any(val => val.AccountPropertyValueID == v.AccountPropertyValueID)).ToList();
                    foreach (AccountPropertyValue removedValue in removed)
                        property.AccountPropertyValues.RemoveAndMarkAsDeleted(removedValue);
                }

                int newInt = 0;
                if (Int32.TryParse(minLength, out newInt) || Convert.ToInt32(minLength) < 0)
                {
                    property.MinimumLength = Convert.ToInt32(minLength).Absolute();
                }
                else
                {
                    return Json(new { result = false, message = "Minimum length contains an invalid or negative value." });
                }

                if (Int32.TryParse(maxLength, out newInt) || Convert.ToInt32(maxLength) < 0)
                {
                    property.MaximumLength = Convert.ToInt32(maxLength).Absolute();
                }
                else
                {
                    return Json(new { result = false, message = "Maximum length contains an invalid or negative value." });
                }
                
                property.Save();

                return Json(new { result = true, propertyTypeId = property.AccountPropertyTypeID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
