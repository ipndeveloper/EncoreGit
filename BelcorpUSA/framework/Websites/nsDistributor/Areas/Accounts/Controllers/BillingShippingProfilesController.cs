using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Controls.Models;
namespace nsDistributor.Areas.Accounts.Controllers
{
    public class BillingShippingProfilesController : BaseAccountsController
    {
       
       [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetAddressControl(int? countryId, int? addressId, string prefix, List<string> excludeFields,
            bool showCountrySelect = true)
        {
            try
            {
                Address address = addressId.HasValue && addressId.Value > 0 ? Address.Load(addressId.Value) : new Address();
                AddressModel model = new AddressModel()
                {
                    Address = address,
                    LanguageID = CoreContext.CurrentLanguageID,
                    ShowCountrySelect = showCountrySelect,
                    ChangeCountryURL = "~/Account/BillingShippingProfiles/GetAddressControl",
                    ExcludeFields = excludeFields,
                    Prefix = prefix
                };

                if (countryId != null)
                    model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId);
                else if (addressId != null)
                    model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == address.CountryID);

                return PartialView("Address", model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

    }
}
