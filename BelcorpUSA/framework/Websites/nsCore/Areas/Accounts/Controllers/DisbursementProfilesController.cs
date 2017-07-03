using NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Accounts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Globalization;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Data.Entities.Generated;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Web.Mvc.Attributes;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Web.Mvc.Controls.Models;
    using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Web.Mvc.Helpers;

    public class DisbursementProfilesController : BaseAccountsController
    {
        [FunctionFilter("Accounts-Disbursement Profiles", "~/Accounts/Overview")]
        public virtual ActionResult Index(string id)
        {
            AccountNum = id;


            var account = CurrentAccount;
            if (account == null)
            {
                return Redirect("~/Accounts");
            }
            var viewModel = new DisbursementProfileViewModel();
            viewModel.CreateModel(account);
            viewModel.ChangeCountryURL = "/Accounts/BillingShippingProfiles/GetAddressControl";
            viewModel.PostalCodeLookupURL = "/Accounts/DisbursementProfiles/LookupZip";

            obfuscateEFTAccountNumbers(viewModel);
            ViewData["EftMarketID"] = viewModel.Account.MarketID;

            viewModel.EFTProfiles = viewModel.EFTProfiles.OrderBy(x => x.DisbursementProfileId).ToList();
            LoadBankIDEdit();

            //Shows or Hides Second Disbursement Profile Account
            ViewBag.DPA = OrderExtensions.GeneralParameterVal(56, "DPA");

            return View(viewModel);
        }
        private void LoadBankIDEdit()
        {
            var listdist = AccountExtensions.GetDisbursementProfileIDs(CurrentAccount.AccountID);
            int i = 1;
            foreach (var item in listdist)
            {
                var regBak = AccountExtensions.GetBanksID(item.ID);

                if (i == 1)
                    ViewBag.bankID1 = regBak;

                if (i == 2)
                    ViewBag.bankID2 = regBak;
                i++;
            }
        }
        [FunctionFilter("Accounts-Disbursement Profiles", "~/Accounts/Overview")]
        public virtual ActionResult Save(
            //bool paymentPreference,
             int id,
             DisbursementMethodKind preference,
             bool? useAddressOfRecord,
             bool? isActive,
             string profileName,
             string payableTo,
             string address1,
             string address2,
             string address3,
             string city,
             string county,
             string state,
             string street,
             string zip,
             int? country,
             bool? agreementOnFile,
             List<EFTAccountModel> accounts,
            IEnumerable<int> bankID)
        {
            try
            {
                //var account = CurrentAccount;
                Account account = Account.LoadForSession(CurrentAccount.AccountID);

                if (account == null)
                {
                    return Redirect("~/Accounts");
                }

                var stateCode = GetStateCode(state);
                int stateId;
                Int32.TryParse(state, out stateId);
                var viewAddress = new Address
                {
                    ProfileName = profileName,
                    Attention = payableTo,
                    AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = (city == null ? "" : city),
                    State = stateCode,
                    County = county,
                    StateProvinceID = stateId,
                    PostalCode = zip,
                    Street = street,
                    CountryID = country ?? (int)ConstantsGenerated.Country.Brazil
                };

                useAddressOfRecord = useAddressOfRecord ?? false;
                agreementOnFile = agreementOnFile ?? false;

                var ieftAccounts = Enumerable.Empty<IEFTAccount>();
                if (accounts != null && accounts.Any())
                {
                    ieftAccounts = accounts.Select(x => x.Convert());
                }

                CommissionsService.SaveDisbursementProfile(
                                     id,
                                     account,
                                     viewAddress,
                                     preference,
                                     (bool)useAddressOfRecord,
                                     ieftAccounts,
                                     (bool)agreementOnFile);

                CoreContext.CurrentAccount = account;

                var profiles = AccountExtensions.GetFullDisbursementProfileByAccount(CurrentAccount.AccountID);
                List<DisbursementProfilesSearchData> disburEFT = profiles.Where(p => Convert.ToInt32(p.FormaTratamiento) == DisbursementMethodKind.EFT.GetHashCode()).ToList();

                var eftId1 = disburEFT != null && disburEFT.Count > 0 ? disburEFT[0].DisbursementeProfileId.ToInt() : 0;
                var eftId2 = disburEFT != null && disburEFT.Count > 1 ? disburEFT[1].DisbursementeProfileId.ToInt() : 0;

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

        private void UpdateDisbursementProfiles(List<MinimumSearchData> ieftAccounts, IEnumerable<int> bankID)
        {

            if (ieftAccounts.First().ID != null && bankID.First() != null)
            {
                int bankID01 = Convert.ToInt32(bankID.First());
                int DisbursementProfileId01 = Convert.ToInt32(ieftAccounts.First().ID);
                AccountExtensions.UpdateDisbursementProfileBank(DisbursementProfileId01, bankID01);
            }

            if (ieftAccounts.Last().ID != null && bankID.Last() != null && bankID.Last() != bankID.First())
            {
                int DisbursementProfileId02 = Convert.ToInt32(ieftAccounts.Last().ID);
                int bankID02 = Convert.ToInt32(bankID.Last());
                AccountExtensions.UpdateDisbursementProfileBank(DisbursementProfileId02, bankID02);
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult LookupZip(int countryId, string zip)
        {
            try
            {
                switch (zip.Length)
                {
                    case 5:
                        return Json(Create.New<IPostalCodeLookupProvider>().LookupPostalCodeByAccount(countryId, zip, (CurrentAccount == null ? 0 : CurrentAccount.AccountID), 0).Select(r => new { city = r.City.ToTitleCase().Trim(), county = r.County.ToTitleCase().Trim(), stateId = r.StateID, state = r.StateAbbreviation.Trim() }).Distinct());
                    case 9:
                        string zipPlusFour = zip.Substring(5);
                        zip = zip.Substring(0, 5);
                        return Json(Create.New<IPostalCodeLookupProvider>().LookupPostalCodeByAccount(countryId, string.Format("{0}-{1}", zip, zipPlusFour), (CurrentAccount == null ? 0 : CurrentAccount.AccountID), 0).Select(r => new { city = r.City.ToTitleCase().Trim(), county = r.County.ToTitleCase().Trim(), stateId = r.StateID, state = r.StateAbbreviation.Trim() }).Distinct());
                }

                return Json(new List<PostalCodeData>());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Takes in a state value and will return the state code if it is an ID
        /// </summary>
        /// <param name="state">state value</param>
        /// <returns>state abbreviation</returns>
        private string GetStateCode(string state)
        {
            //set statecode to whatever is currently in state
            var stateCode = state;
            var stateCodes = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.StateProvinces;

            //If state is an int, get the ID from cache
            int stateId;
            if (Int32.TryParse(state, out stateId))
            {
                stateCode = stateCodes.GetById(stateId).StateAbbreviation;
            }
            return stateCode;
        }

        protected virtual void obfuscateEFTAccountNumbers(DisbursementProfileViewModel viewModel)
        {
            // Allows for GoldCanyon override
        }
    }
}
