using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities;
using System.Web.Mvc;
using nsDistributor.Areas.Enroll.Models.Shared;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;

using NetSteps.Web.Mvc.Extensions;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public class CheckPaymentMethodsModel
    {
        /*CS.21jun2016.Inicio*/
        #region Values
        public virtual bool IsSameShippingAddress { get; set; }
        #endregion

        #region Resources
        public virtual MvcHtmlString MainAddressHtml { get; set; }
        #endregion

        #region Models
        public virtual CheckPaymentMethodsAddressModel ShippingAddress { get; set; }
        #endregion

        #region Infrastructure
        public CheckPaymentMethodsModel()
        {
            this.ShippingAddress = new CheckPaymentMethodsAddressModel();
        }

        public virtual CheckPaymentMethodsAddressModel LoadValues(bool isSameShippingAddress, int countryID, IAddress mainAddress, IAddress shippingAddress)
        {
            this.IsSameShippingAddress = isSameShippingAddress;

            this.MainAddressHtml = mainAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();

            this.ShippingAddress.LoadValues(countryID, shippingAddress, !isSameShippingAddress);
            this.ShippingAddress.CountryID = countryID;
            return this.ShippingAddress;
        }

        public virtual MvcHtmlString LoadResources(IAddress mainAddress)
        {
            this.MainAddressHtml = mainAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();

            return this.MainAddressHtml;
        }

        #endregion

        public NetSteps.Data.Entities.Account Account { get; set; }

        public ICheckDisbursementProfile CheckProfile { get; set; }

        public Address CheckAddress { get; set; }

        public string PostalCodeLookupURL { get; set; }

        public string ChangeCountryURL { get; set; }

        public Address AddressOfRecord
        {
            get
            {
                if (Account.Addresses.Count > 0 && Account.Addresses.GetAllByTypeID(ConstantsGenerated.AddressType.Main).Count > 0)
                {
                    return Account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main);
                }

                return new Address();
            }
        }
        /*CS.21jun2016.Inicio*/
    }
}