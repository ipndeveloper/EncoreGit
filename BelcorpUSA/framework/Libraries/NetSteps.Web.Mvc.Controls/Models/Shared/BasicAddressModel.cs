using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Controls.Configuration;
using NetSteps.Web.Mvc.Extensions;

namespace NetSteps.Web.Mvc.Controls.Models.Shared
{
    public abstract class ISOValidationAttribute : NSValidationAttribute
    {
        public string GetTagMethod { get; set; }

        public string TagID { get; set; }

        protected Tag _tag;

        protected abstract Predicate<Tag> IsTagValidationEnabled { get; }

        public ISOValidationAttribute(string getTagMethod, string tagID)
        {
            this.GetTagMethod = getTagMethod;
            this.TagID = tagID;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            this.LoadTag(validationContext);

            return base.IsValid(value, validationContext);
        }

        protected override ModelClientValidationRule GetDefaultClientValidationRule(string validationType, ModelMetadata metadata, ControllerContext controllerContext)
        {
            this.LoadTag(controllerContext);

            if (this._tag == null || this.IsTagValidationEnabled == null || !this.IsTagValidationEnabled(_tag))
            {
                return null;
            }

            return base.GetDefaultClientValidationRule(validationType, metadata, controllerContext);
        }

        protected override bool IsEnabled(ValidationContext validationContext)
        {
            if (this._tag == null || this.IsTagValidationEnabled == null || !this.IsTagValidationEnabled(_tag))
            {
                return false;
            }

            return base.IsEnabled(validationContext);
        }

        private void LoadTag(ValidationContext validationContext)
        {
            if (validationContext != null
                && validationContext.ObjectInstance != null
                && !string.IsNullOrWhiteSpace(this.GetTagMethod)
                && !string.IsNullOrWhiteSpace(this.TagID))
            {
                this._tag = this.InvokeGetTagMethod(
                    validationContext.ObjectType,
                    validationContext.ObjectInstance);
            }
        }

        private void LoadTag(ControllerContext controllerContext)
        {
            if (controllerContext != null
                && !string.IsNullOrWhiteSpace(this.GetTagMethod)
                && !string.IsNullOrWhiteSpace(this.TagID))
            {
                var viewContext = controllerContext as ViewContext;
                if (viewContext != null
                    && viewContext.ViewData != null
                    && viewContext.ViewData.Model != null)
                {
                    this._tag = this.InvokeGetTagMethod(
                        viewContext.ViewData.Model.GetType(),
                        viewContext.ViewData.Model);
                }
            }
        }

        private Tag InvokeGetTagMethod(Type objectType, object objectInstance)
        {
            var getTagMethod = objectType.GetMethod(this.GetTagMethod);

            if (getTagMethod == null)
            {
                return null;
            }

            return getTagMethod.Invoke(objectInstance, new[] { this.TagID }) as Tag;
        }
    }

    public class ISORequiredAttribute : ISOValidationAttribute, IClientValidatable
    {
        protected override Predicate<Tag> IsTagValidationEnabled { get { return x => x != null && x.IsRequired; } }
        protected RequiredAttribute _innerAttribute = new RequiredAttribute();

        public ISORequiredAttribute(string getTagMethod, string tagID)
            : base(getTagMethod, tagID)
        {
            this.TermName = "ErrorFieldRequired";
            this.ErrorMessage = "{0} is required.";
        }

        public override bool IsValid(object value)
        {
            return this._innerAttribute.IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            if (_tag != null)
            {
                if (!string.IsNullOrWhiteSpace(_tag.RequiredMessageTermName))
                {
                    this.TermName = _tag.RequiredMessageTermName;
                }
                if (!string.IsNullOrWhiteSpace(_tag.DefaultRequiredMessage))
                {
                    this.ErrorMessage = _tag.DefaultRequiredMessage;
                }
            }

            return base.FormatErrorMessage(name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
        {
            var rule = this.GetDefaultClientValidationRule("required", metadata, controllerContext);
            if (rule != null) yield return rule;
        }
    }

    public class ISORegularExpressionAttribute : ISOValidationAttribute, IClientValidatable
    {
        protected override Predicate<Tag> IsTagValidationEnabled { get { return x => x != null && !string.IsNullOrWhiteSpace(_tag.Regex); } }

        public ISORegularExpressionAttribute(string getTagMethod, string tagID)
            : base(getTagMethod, tagID)
        {
            this.TermName = "ErrorFieldDoesNotMatchPattern";
            this.ErrorMessage = "{0} is invalid.";
        }

        public override bool IsValid(object value)
        {
            if (!this.IsTagValidationEnabled(_tag))
            {
                return true;
            }

            return new RegularExpressionAttribute(_tag.Regex).IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            if (_tag != null)
            {
                if (!string.IsNullOrWhiteSpace(_tag.RegexFailMessageTermName))
                {
                    this.TermName = _tag.RegexFailMessageTermName;
                }
                if (!string.IsNullOrWhiteSpace(_tag.DefaultRegexFailMessage))
                {
                    this.ErrorMessage = _tag.DefaultRegexFailMessage;
                }
            }

            return base.FormatErrorMessage(name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
        {
            var rule = this.GetDefaultClientValidationRule("regex", metadata, controllerContext);
            if (rule != null && this.IsTagValidationEnabled(_tag))
            {
                rule.ValidationParameters["pattern"] = _tag.Regex;

                yield return rule;
            }
        }
    }

    public class BasicAddressModel : IValidatableObject
    {
        #region Values
        public virtual int CountryID { get; set; }
        public virtual bool IsVisible { get; set; }

        [ISORequired("GetTag", "address1", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "address1", Condition = "EnableAddressValidation")]
        public virtual string Address1 { get; set; }

        [ISORequired("GetTag", "address2", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "address2", Condition = "EnableAddressValidation")]
        public virtual string Address2 { get; set; }

        [ISORequired("GetTag", "city", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "city", Condition = "EnableAddressValidation")]
        public virtual string City { get; set; }

        [ISORequired("GetTag", "county", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "county", Condition = "EnableAddressValidation")]
        public virtual string County { get; set; }

        [ISORequired("GetTag", "state", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "state", Condition = "EnableAddressValidation")]
        public virtual int? StateProvinceID { get; set; }

        [ISORequired("GetTag", "street", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "street", Condition = "EnableAddressValidation")]
        public virtual string Street { get; set; }

        [ISORequired("GetTag", "postalCode", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "postalCode", Condition = "EnableAddressValidation")]
        public virtual string PostalCode
        {
            get
            {
                return this.PostalCode1 + this.PostalCode2;
            }
            set
            {
                if (value == null)
                {
                    this.PostalCode1 = this.PostalCode2 = null;
                    return;
                }

                var tag1 = GetTag("zip");
                if (tag1 == null
                    || !tag1.MaxLengthSpecified
                    || Convert.ToInt32(tag1.MaxLength) >= value.Length)
                {
                    this.PostalCode1 = value;
                    this.PostalCode2 = null;
                    return;
                }

                int maxLength1 = Convert.ToInt32(tag1.MaxLength);
                this.PostalCode1 = value.Substring(0, maxLength1);

                var tag2 = GetTag("zipPlusFour");
                if (tag2 == null
                    || !tag2.MaxLengthSpecified
                    || Convert.ToInt32(tag2.MaxLength) >= (value.Length - maxLength1))
                {
                    this.PostalCode2 = value.Substring(maxLength1);
                    return;
                }

                int maxLength2 = Convert.ToInt32(tag2.MaxLength);
                this.PostalCode2 = value.Substring(maxLength1, maxLength2);
            }
        }

        [ISORequired("GetTag", "zip", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "zip", Condition = "EnableAddressValidation")]
        public virtual string PostalCode1 { get; set; }

        [ISORequired("GetTag", "zipPlusFour", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "zipPlusFour", Condition = "EnableAddressValidation")]
        public virtual string PostalCode2 { get; set; }

        public virtual double? Latitude { get; set; }

        public virtual double? Longitude { get; set; }
        
        #endregion

        #region Resources
        protected ISO _iso;
        public virtual ISO ISO
        {
            get
            {
                return this._iso ?? (this._iso = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(this.CountryID).CountryCode));
            }
        }
        #endregion

        #region Validation
        public static readonly Predicate<BasicAddressModel>
            EnableAddressValidation = x => x.IsVisible;

        public virtual Tag GetTag(string tagID)
        {
            return this.ISO.Tags.FirstOrDefault(x => x.Id.EqualsIgnoreCase(tagID));
        }

        // Ensures that class validation only runs once
        protected bool _validated = false;
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!_validated && EnableAddressValidation(this))
            {
                this._validated = true;
                var result = this.ToAddress().ValidateAddressAccuracy();
                if (!result.Success)
                {
                    yield return new ValidationResult(result.Message, new[] { "Address1" });
                }
            }
        }
        #endregion

        #region Infrastructure
        public BasicAddressModel()
        {
            this.IsVisible = true;
        }

        public virtual BasicAddressModel LoadValues(
            int countryID,
            IAddress address,
            bool isVisible = true)
        {
            this.CountryID = countryID;
            this.IsVisible = isVisible;

            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.City = address.City;
            this.County = address.County;
            this.StateProvinceID = address.StateProvinceID;
            this.PostalCode = address.PostalCode;
            this.Latitude = address.Latitude;
            this.Longitude = address.Longitude;
            //this.Street = address.Street;
            return this;
        }

        public virtual BasicAddressModel ApplyTo(Address address)
        {
            address.CountryID = this.CountryID;
            address.Address1 = this.Address1;
            address.Address2 = this.Address2;
            address.City = this.City;
            address.County = this.County;
            address.StateProvinceID = this.StateProvinceID;
            address.PostalCode = this.PostalCode;
            address.Latitude = this.Latitude;
            address.Longitude = this.Longitude;
            //address.Street = this.Street;

            if (this.StateProvinceID != null)
            {
                var state = SmallCollectionCache.Instance.StateProvinces.GetById(this.StateProvinceID.Value);
                if (state != null)
                {
                    address.State = state.StateAbbreviation;
                }
            }

            return this;
        }

        

        public virtual Address ToAddress()
        {
            var address = new Address();
            this.ApplyTo(address);
            return address;
        }

        public virtual MvcHtmlString ToDisplay(NetSteps.Data.Entities.Extensions.IAddressExtensions.AddressDisplayTypes type)
        {
            return this.ToAddress().ToDisplay(type, false).ToMvcHtmlString();
        }
        #endregion
    }
}