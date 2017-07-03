using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Controls.Models;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public class AboutYouModel : SectionModel, IValidatableObject
    {
        public List<AccountPropertyModel> AccountPropertyList { get; set; }

        public AboutYouModel()
        {
            AccountPropertyList = new List<AccountPropertyModel>();
        }

        public AboutYouModel LoadValues(AccountPropertiesModel values)
        {
            foreach (AccountProperty property in values.AccountProperties)
            {
                AccountPropertyList.Add(new AccountPropertyModel().LoadValues(property));
            }

            return this;
        }

        public AboutYouModel LoadResources(AccountPropertiesModel values)
        {
            List<AccountPropertyModel> newTypes = new List<AccountPropertyModel>();

            foreach (AccountPropertyType types in values.AccountPropertyTypes)
            {
                var property = AccountPropertyList.FirstOrDefault(x => x.AccountPropertyTypeID == types.AccountPropertyTypeID);
                if (property != null)
                {
                    property.LoadResources(types);
                }
                else
                {
                    var accountPropertyModel = new AccountPropertyModel().LoadResources(types);
                    var value = values.AccountProperties.FirstOrDefault(x => x != null && x.AccountPropertyTypeID == types.AccountPropertyTypeID);
                    if (value != null && !value.PropertyValue.IsNullOrEmpty())
                        accountPropertyModel.PropertyValue = value.PropertyValue;
                    if (value != null && value.AccountPropertyValueID != null)
                        accountPropertyModel.AccountPropertyValueID = value.AccountPropertyValueID;
                    newTypes.Add(accountPropertyModel);
                }
            }

            AccountPropertyList.AddRange(newTypes);

            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int i = 0;
            if (AccountPropertyList != null)
            {
                foreach (AccountPropertyModel property in AccountPropertyList)
                {
                    var checkModel = new AccountPropertyModel();
                    checkModel.LoadResources(AccountPropertyType.Load(property.AccountPropertyTypeID));

                    if (checkModel.Required && String.IsNullOrEmpty(property.PropertyValue))
                    {
                        var errorLabel = String.Format("{0} {1}", checkModel.PropertyTypeLabel, Translation.GetTerm("IsARequiredField", "is a required field"));
                        yield return new ValidationResult(errorLabel, new string[] { String.Format("AccountPropertyList[{0}].PropertyValue", i) });
                    }

                    if ((!String.IsNullOrEmpty(property.PropertyValue) &&
                         checkModel.MinimumLength > 0 &&
                         property.PropertyValue.Length < checkModel.MinimumLength) ||
                        (String.IsNullOrEmpty(property.PropertyValue) && checkModel.MinimumLength > 0))
                    {
                        var errorLabel = String.Format("{0}: {1}", checkModel.PropertyTypeLabel, Translation.GetTerm("LessThanMinimumLength",
                            "The entered value is less than required minimum length of ({0}) characters", checkModel.MinimumLength));
                        yield return new ValidationResult(errorLabel, new string[] { String.Format("AccountPropertyList[{0}].PropertyValue", i) });
                    }

                    if (!String.IsNullOrEmpty(property.PropertyValue) &&
                        checkModel.MaximumLength > 0 &&
                        property.PropertyValue.Length > checkModel.MaximumLength)
                    {
                        var errorLabel = String.Format("{0}: {1}", checkModel.PropertyTypeLabel, Translation.GetTerm("GreaterThanMaximumLength",
                            "The entered value is greater than the required maximum length of ({0}) characters", checkModel.MaximumLength));
                        yield return new ValidationResult(errorLabel, new string[] { String.Format("AccountPropertyList[{0}].PropertyValue", i) });
                    }
                    i++;
                }
            }
        }
    }
}
