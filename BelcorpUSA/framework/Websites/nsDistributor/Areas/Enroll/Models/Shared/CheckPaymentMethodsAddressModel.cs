using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Controls.Configuration;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities;
using System.Web.Mvc;

/*CSTI(CS)-05/03/2016: Inicio*/
namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class CheckPaymentMethodsAddressModel : IValidatableObject
    {
        /*CSTI(CS)-05/03/2016: Inicio*/
        #region Values
        public virtual int CountryID { get; set; }
        public virtual bool IsVisible { get; set; }

        //public virtual string Country { get; set; }

        [ISORequired("GetTag", "address1", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "address1", Condition = "EnableAddressValidation")]
        public virtual string Address1 { get; set; }

        [ISORequired("GetTag", "address2", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "address2", Condition = "EnableAddressValidation")]
        public virtual string Address2 { get; set; }

        [ISORequired("GetTag", "address3", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "address3", Condition = "EnableAddressValidation")]
        public virtual string Address3 { get; set; }

        [ISORequired("GetTag", "city", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "city", Condition = "EnableAddressValidation")]
        public virtual string City { get; set; }

        [ISORequired("GetTag", "county", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "county", Condition = "EnableAddressValidation")]
        public virtual string County { get; set; }

        [ISORequired("GetTag", "state", Condition = "EnableAddressValidation")]
        [ISORegularExpression("GetTag", "state", Condition = "EnableAddressValidation")]
        public virtual int? StateProvinceID { get; set; }

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
                this.CountryID = 1;//Borrar CMR
                return this._iso ?? (this._iso = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(this.CountryID).CountryCode));
            }
        }
        #endregion

        #region Validation
        public static readonly Predicate<CheckPaymentMethodsAddressModel>
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
        public CheckPaymentMethodsAddressModel()
        {
            this.IsVisible = true;
        }

        public virtual CheckPaymentMethodsAddressModel LoadValues(
            int countryID,
            IAddress address,
            bool isVisible = true)
        {
            this.CountryID = countryID;
            this.IsVisible = isVisible;

            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.Address3 = address.Address3;
            this.City = address.City;
            this.County = address.County;
            this.StateProvinceID = address.StateProvinceID;
            this.PostalCode = address.PostalCode;
            this.Latitude = address.Latitude;
            this.Longitude = address.Longitude;

            return this;
        }

        public virtual CheckPaymentMethodsAddressModel ApplyTo(Address address)
        {
            address.CountryID = this.CountryID;
            address.Address1 = this.Address1;
            address.Address2 = this.Address2;
            address.Address3 = this.Address3;
            address.City = this.City;
            address.County = this.County;
            address.StateProvinceID = this.StateProvinceID;
            address.PostalCode = this.PostalCode;
            address.Latitude = this.Latitude;
            address.Longitude = this.Longitude;

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

        public virtual MvcHtmlString ToDisplay(IAddressExtensions.AddressDisplayTypes type)
        {
            return this.ToAddress().ToDisplay(type, false).ToMvcHtmlString();
        }
        #endregion
    }
    /*CSTI(CS)-05/03/2016: Fin*/
}