using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public class AccountPropertyModel // : IValidatableObject
    {
        public int? AccountPropertyValueID { get; set; }
        public int? AccountPropertyID { get; set; }
        public int AccountPropertyTypeID { get; set; }
        public string PropertyTypeLabel { get; set; }
        public string PropertyValue { get; set; }
        public bool Required { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public List<AccountPropertyValue> AccountPropertyValues { get; set; }
        public List<SelectListItem> PropertyValueListItems { get; set; }

        public AccountPropertyModel LoadValues(AccountProperty values)
        {
            AccountPropertyValueID = values.AccountPropertyValueID ?? null;
            AccountPropertyID = values.AccountPropertyID;
            AccountPropertyTypeID = values.AccountPropertyTypeID;
            PropertyValue = values.PropertyValue;
            return this;
        }

        public AccountPropertyModel LoadResources(AccountPropertyType types)
        {
            AccountPropertyTypeID = types.AccountPropertyTypeID;
            PropertyTypeLabel = types.GetTerm();
            Required = types.Required;
            MinimumLength = types.MinimumLength;
            MaximumLength = types.MaximumLength;
            AccountPropertyValues = types.AccountPropertyValues.ToList();
            PropertyValueListItems = GetPropertyDropdownOptions(types.AccountPropertyValues.ToList());
            return this;
        }

        public List<SelectListItem > GetPropertyDropdownOptions(List<AccountPropertyValue> accountPropertyValues)
        {
            var selectListItems = new List<SelectListItem>();

            foreach(var value in accountPropertyValues)
            {
                var item = new SelectListItem
                               {
                                   Text = value.Name,
                                   Value = value.AccountPropertyValueID.ToString()
                               };
                selectListItems.Add(item);
            }

            return selectListItems;

        }
        //public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Required && string.IsNullOrEmpty(PropertyValue))
        //    {
        //        yield return new ValidationResult("Question is required", new string[] {PropertyTypeLabel });
        //    }
        //}
    }
}