namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    using System.Web.Mvc;
    using NetSteps.Addresses.Common.Models;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Extensions;
    using NetSteps.Enrollment.Common.Models.Context;
    using NetSteps.Web.Mvc.Extensions;
    using nsDistributor.Areas.Enroll.Models.Shared;

    public class ShippingModel : SectionModel
    {
        #region Values
        public virtual bool IsSameShippingAddress { get; set; }
        #endregion

        #region Resources
        public virtual MvcHtmlString MainAddressHtml { get; set; }
        #endregion

        #region Models
        public virtual BasicAddressModel ShippingAddress { get; set; }
        #endregion

        #region Infrastructure
        public ShippingModel()
        {
            this.ShippingAddress = new BasicAddressModel();
        }

        public virtual ShippingModel LoadValues(
            bool isSameShippingAddress,
            int countryID,
            IAddress shippingAddress)
        {
            this.IsSameShippingAddress = isSameShippingAddress;

            this.ShippingAddress.LoadValues(
                countryID,
                shippingAddress,
                !isSameShippingAddress
            );

            return this;
        }

        public virtual ShippingModel LoadResources(
            IAddress mainAddress)
        {
            this.MainAddressHtml = mainAddress
                .ToDisplay(IAddressExtensions.AddressDisplayTypes.Web)
                .ToMvcHtmlString();

            return this;
        }

        public virtual ShippingModel ApplyTo(IEnrollmentContext enrollmentContext)
        {
            enrollmentContext.IsSameShippingAddress = this.IsSameShippingAddress;

            return this;
        }

        public virtual ShippingModel ApplyTo(
            Address address,
            Address mainAddress)
        {
            if (this.IsSameShippingAddress)
            {
                new BasicAddressModel()
                    .LoadValues(
                        mainAddress.CountryID,
                        mainAddress)
                    .ApplyTo(address);
            }
            else
            {
                this.ShippingAddress.ApplyTo(address);
            }

            return this;
        }
        #endregion
    }
}