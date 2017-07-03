// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisbursementProfilesController.cs" company="Net">
//   
// </copyright>
// <summary>
//   Controller for Disbursement Profiles
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Linq;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles;
using NetSteps.Data.Entities.Extensions;

namespace DistributorBackOffice.Areas.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using NetSteps.Common.Extensions;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Data.Entities.Generated;
    using NetSteps.Data.Entities.Business.HelperObjects;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Web.Mvc.Attributes;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Commissions.Common;
    using NetSteps.Web.Mvc.Controls.Models;
    using NetSteps.Data.Entities.Business.Logic;

    /// <summary>
    /// Controller for Disbursement Profiles
    /// </summary>
    public class DisbursementProfilesController : BaseAccountsController
    {
        /// <summary>
        /// The Index view
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// returns an ActionResults with a DisbursementProfileViewModel model
        /// </returns>
        [FunctionFilter("Accounts-Disbursement Profiles", "~/Account", ConstantsGenerated.SiteType.BackOffice)]
        public virtual ActionResult Index(string id)
        {
            var viewModel = new DisbursementProfileViewModel();
            viewModel.CreateModel(CurrentAccount);
            viewModel.PostalCodeLookupURL = "BillingShippingProfiles/LookupZip";

            //Shows or Hides Second Disbursement Profile Account
            ViewBag.DPA = OrderExtensions.GeneralParameterVal(56, "DPA");

            return View(viewModel);
        }

        /// <summary>
        /// Saves the disbursement profile
        /// </summary>
        /// <param name="profileID">
        /// The profile id.
        /// </param>
        /// <param name="preference">
        /// The preference.
        /// </param>
        /// <param name="useAddressOfRecord">
        /// The use address of record.
        /// </param>
        /// <param name="isActive">
        /// </param>
        /// <param name="profileName">
        /// The profile Name.
        /// </param>
        /// <param name="payableTo">
        /// The payable To.
        /// </param>
        /// <param name="address1">
        /// The address 1.
        /// </param>
        /// <param name="address2">
        /// The address 2.
        /// </param>
        /// <param name="address3">
        /// The address 3.
        /// </param>
        /// <param name="city">
        /// The city.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="agreementOnFile">
        /// The agreement on file.
        /// </param>
        /// <param name="accounts">
        /// The accounts.
        /// </param>
        /// <returns>
        /// returns a view
        /// </returns>
        [FunctionFilter("Accounts-Disbursement Profiles", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult Save(
            int id, DisbursementMethodKind preference
            , bool? useAddressOfRecord
            , bool? isActive
            , string profileName
            , string payableTo
            , string address1
            , string address2
            , string address3
            , string city
            , string county
            , string street
            , string state
            , string zip
            , int? country
            , bool? agreementOnFile
            , List<EFTAccountModel> accounts)
        {
            try
            {
                var stateCode = GetStateCode(state);
                int stateId;
                Int32.TryParse(state, out stateId);

                var service = Create.New<ICommissionsService>();
                var viewAddress = new Address
                {
                    ProfileName = profileName,
                    Attention = payableTo,
                    AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    County = county,
                    Street = street,
                    State = stateCode,
                    StateProvinceID = stateId,
                    PostalCode = zip,
                    CountryID = country ?? (int)ConstantsGenerated.Country.Brazil
                };

                if (accounts == null)
                {
                    accounts = new List<EFTAccountModel>();
                }

                useAddressOfRecord = useAddressOfRecord ?? false;
                agreementOnFile = agreementOnFile ?? false;
                service.SaveDisbursementProfile(
                                id,
                                CurrentAccount,
                                viewAddress,
                                preference,
                                (bool)useAddressOfRecord,
                                accounts.Select(x => x.Convert()),
                                (bool)agreementOnFile);

                var profiles = service.GetDisbursementProfilesByAccountId(CurrentAccount.AccountID);
                List<IEFTDisbursementProfile> disburEFT = profiles.Where(p => p.DisbursementMethod == DisbursementMethodKind.EFT).Cast<IEFTDisbursementProfile>().ToList();

                var eftId1 = disburEFT != null && disburEFT.Count > 0 ? disburEFT[0].DisbursementProfileId : 0;
                var eftId2 = disburEFT != null && disburEFT.Count > 1 ? disburEFT[1].DisbursementProfileId : 0;

                return Json(new { result = true, id, eftId1, eftId2 });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateAddressStreet(Account account, string street, int addressID)
        {
            Address direccion = new Address();
            direccion = account.Addresses.Where(donde => donde.AddressID == addressID).FirstOrDefault();
            if (direccion != null)
            {
                direccion.Street = street;
                AddressBusinessLogic business = new AddressBusinessLogic();
                business.UpdateAddressStreet(direccion);
            }
        }

        /// <summary>
        /// Takes in a state value and will return the state code if it is an ID
        /// </summary>
        /// <param name="state">state value</param>
        /// <returns>state abbreviation</returns>
        private string GetStateCode(string state)
        {
            int stateId = 0;

            //set statecode to whatever is currently in state
            string stateCode = state;
            var stateCodes = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.StateProvinces;

            //If state is an int, get the ID from cache
            if (Int32.TryParse(state, out stateId))
            {
                stateCode = stateCodes.GetById(stateId).StateAbbreviation;
            }
            return stateCode;
        }
    }
}
