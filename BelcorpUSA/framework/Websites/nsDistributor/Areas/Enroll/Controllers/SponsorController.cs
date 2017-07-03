using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.AccountLocatorService;
using NetSteps.AccountLocatorService.Common;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Extensions;
using nsDistributor.Areas.Enroll.Models.Sponsor;
using nsDistributor.Models.Shared;
using System.Configuration;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Common.Base;
using System.Collections.Generic;
using NetSteps.Data.Entities.Dto;
using System.Data;
using System.IO;
using NetSteps.Common.Globalization;

namespace nsDistributor.Areas.Enroll.Controllers
{
    public class SponsorController : EnrollStepBaseController
    {
        #region Actions

        public virtual ActionResult Index(int? sponsorID)
        {
			var cultura = CoreContext.CurrentCultureInfo.Name;
            int EnvironmentCountry = Convert.ToInt16(ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            int[] ListaValido = null;

            if (sponsorID == 1 | sponsorID == 3)
            {
                sponsorID = Convert.ToInt32(OrderExtensions.GetGeneralParametersByCode(56, "BEL").GeneralParameterVal);
            }
            if (sponsorID.HasValue)
            {
                //Validar solo Brazil
                if (EnvironmentCountry == (int)Constants.Country.Brazil)
                {
                    ListaValido = ValidarAccountIDs(null, sponsorID.Value) ?? new int[1] { 0 };
                    if (!ListaValido.Contains(sponsorID.Value))
                    {
                        return ValidAccounts(sponsorID.Value);
                    }
                }
                SetCookieSponsorId(sponsorID.Value);
            }
            Site oSite = CurrentSite; //Site.Repository.Where(x => x.SiteID == 3).FirstOrDefault();// ;
            if (!oSite.IsBase && !sponsorID.HasValue)
            {
                Account accountOwnerSite = SiteOwner;
                ListaValido = ValidarAccountIDs(null, accountOwnerSite != null ? (accountOwnerSite.AccountID) : 0) ?? new int[1] { 0 };
                if (!ListaValido.Contains(accountOwnerSite == null ? 0 : accountOwnerSite.AccountID))
                {
                    return ValidAccounts(sponsorID.Value);
                }
            }
            if (!sponsorID.HasValue)
            {
                if (oSite.IsBase && oSite.SiteTypeID == 3)//LBELUSA es pagina base 
                {
                    return RedirectToAction("Browse", new { BusquedaAutomaticaSponsorBrasil = 1 });
                }
            }
            if (_enrollmentContext.Sponsor == null)
            {
                return RedirectToAction("Browse");
            }
            var modelo = new IndexModel();
            modelo.IsValidSponsor = true;
            Index_LoadResources(modelo);
            return View(modelo);
        }
        public ActionResult ValidAccounts(int sponsorID)
        {
            var model = new IndexModel();
            Index_LoadResources(model);
            model.IsValidSponsor = false;
            DomainEventQueueItem.EnlistarCorreoNotificacionSponsorNoValido(sponsorID);
            return View(model);
        }
        public void SetCookieSponsorId(int sponsorID)
        {
            IndexGet_UpdateContext(sponsorID);
            if (AccountLocatorLocationSearchUsed)
            {
                this.SetAccountLocatorResultsUsedCookie(sponsorID);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Index(IndexModel model)
        {
            if (!ModelState.IsValid)
            {
                Index_LoadResources(model);
                return View(model);
            }

            try
            {
                // Apply updates
                var account = GetEnrollingAccount();
                IndexPost_UpdateAccount(model, account);

                // We can't save if the basic account info has not been entered yet.
                if (account.AccountID != 0) account.Save();
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log().PublicMessage);
                Index_LoadResources(model);
                return View(model);
            }
            return StepCompleted();
        }

        public virtual ActionResult Browse(IEnrollmentContext enrollmentContext, int BusquedaAutomaticaSponsorBrasil = 0)
        {
            if (_enrollmentContext.EnrollmentConfig.Sponsor.DenySponsorChange) return RedirectToAction("Index");
            var model = new BrowseModel() { BusquedaAutomaticaBrasil = BusquedaAutomaticaSponsorBrasil };
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            foreach (var item in PreOrderExtension.GetParameterCountriesByCountyIdStep(Country.Repository.Where(x => x.CountryID == countryId).FirstOrDefault().CountryID, 1))
            {
                model.AccountLocator.ParameterCountries.Add(new ParameterCountryModel() { Id = item.Id, CountryId = item.CountryId, Controls = item.Controls, Active = item.Active, Descriptions = item.Descriptions, Step = item.Step, Sites = item.Sites });
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Browse(AccountLocatorModel model, int? PageIndex1)
        {
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);

            if (!ModelState.IsValid)
            {
                return JsonError(ModelState.Values.First(x => x.Errors.Any()).Errors.First().ErrorMessage);
            }
            
            if (countryId == (int)Constants.Country.UnitedStates)
            {
                try
                {
                    if (model.PostalCode != null)
                    {
                        model.SearchType = AccountLocatorModel.AccountLocatorSearchType.Location;
                        if (model.Latitude == null || model.Longitude == null)
                        {
                            return JsonError(Translation.GetTerm("ErrorGettingLocationGeoCode", "Unable to lookup your location, please try again."));
                        }
                    }

                    var searchParameters = new AccountLocatorServiceSearchParameters
                    {
                        AccountTypeIDs = new[] { (short)Constants.AccountType.Distributor },
                        PageIndex = model.PageIndex
                    };
                    model.ApplyTo(searchParameters);

                    var accountLocatorService = Create.New<IAccountLocatorService>();
                    return getAccountBrasil(model, countryId, accountLocatorService, searchParameters);
                }
                catch (Exception ex)
                {
                    return JsonError(ex.Log().PublicMessage);
                }
            }
            else
            {
                var searchParameters = new AccountLocatorServiceSearchParameters
                {
                    AccountTypeIDs = new[] { (short)Constants.AccountType.Distributor },
                    PageIndex = model.PageIndex
                };

                model.ApplyTo(searchParameters);
                var accountLocatorService = Create.New<IAccountLocatorService>();
                return getAccountBrasil(model, countryId, accountLocatorService, searchParameters);
            }
        }


        public ActionResult getAccountBrasil(AccountLocatorModel model, int countryId, IAccountLocatorService accountLocatorService, AccountLocatorServiceSearchParameters searchParameters)
        {
            var SponsorRule = false;
            var UserExists = false;
            int[] AccountValidos;

            //if (model.BusquedaAutomaticaSponsorBrasil == 1 && (int)model.SearchType == 1 && countryId == Convert.ToInt32(Constants.Country.UnitedStates))
            //{
            //    Site oSite = CurrentSite;
            //    AccountLocator objAccountLocator = AccountSponsorBusinessLogic.Instance.SeleccionAutomaticaSponsor(model.PostalCode, oSite.MarketID);
            //    AccountLocatorResultModel objAccountLocatorResultModel = new AccountLocatorResultModel
            //    {
            //        AccountID = objAccountLocator.AccountID,
            //        FullName = string.Format("{0} {1}", objAccountLocator.FirstName, objAccountLocator.LastName),
            //        Location = string.Format("{0}, {1}", objAccountLocator.City, objAccountLocator.State),
            //        PwsUrl = objAccountLocator.PwsUrl ?? "",
            //        PhotoHtml = objAccountLocator.html != null ? objAccountLocator.html.ToString() : String.Empty,
            //        SelectUrl = Url.Action("Index", new { sponsorID = objAccountLocator.AccountID }),
            //        PhoneNumber = objAccountLocator.PhoneNumber,
            //        EmailAddress = objAccountLocator.EmailAddress
            //    };
            //    List<AccountLocatorResultModel> listModel = new List<AccountLocatorResultModel>() { objAccountLocatorResultModel };

            //    return Json(new
            //    {
            //        results = listModel,//model.SearchResults,
            //        showMoreButton = model.ShowMoreButton,
            //        CamposAdicionales = new
            //        {
            //            State = objAccountLocator.State,
            //            Street = objAccountLocator.Street,
            //            City = objAccountLocator.City,
            //            Lograduro = objAccountLocator.Lograduro,
            //            Country = objAccountLocator.Country
            //        }
            //    });
            //}

            if ((int)model.SearchType == 1)
            {
                var searchResults = accountLocatorService.Search(searchParameters);
                model.LoadResources(searchResults, x => Url.Action("Index", new { sponsorID = x.AccountId }));
                this.SetAccountLocatorLocationSearchUsedFlag(model);

                return Json(new
                {
                    results = model.SearchResults,
                    msg = Translation.GetTerm("AccountLocator_NoResults", "No matches found, please try a different search"),
                    showMoreButton = model.ShowMoreButton
                });
            }
            else
            {
                var searchResults = accountLocatorService.Search(searchParameters);
                UserExists = searchResults.Count > 0 ? true : false;
                AccountValidos = ValidarAccountIDs(searchResults, 0) ?? new int[1] { 0 };
                SponsorRule = AccountValidos[0] > 0 ? true : false;
                model.LoadResources(searchResults, x => Url.Action("Index", new { sponsorID = x.AccountId }));
                this.SetAccountLocatorLocationSearchUsedFlag(model);

                return Json(new
                {
                    //  se cruza el resultado con la lista de AccountID validos para mostrar solo los que cumplen con la regla  de validacion de patrocinio
                    results = model.SearchResults.Join(AccountValidos, (Sc) => Sc.AccountID, (AccountValido) => AccountValido, (i, d) => i),
                    msg = UserExists && !SponsorRule ? Translation.GetTerm("AccountLocator_SponsorRuleFalse", "The consultant entered is not fit to be a sponsor, please try another query or contact number 000000 for more information") : Translation.GetTerm("AccountLocator_NoResults", "No matches found, please try a different search"),
                    showMoreButton = model.ShowMoreButton
                });
            }
        }


        private static int[] ValidarAccountIDs(IPaginatedList<IAccountLocatorServiceResult> searchResults = null, int AccountID = 0)
        {
            int[] AccountValidos;
            DataTable dtAccountIds = new DataTable("dtAccountIds");
            dtAccountIds.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = "AccountID" });

            if (searchResults != null)
            {
                foreach (var AccountLocator in searchResults) dtAccountIds.Rows.Add(new Object[] { AccountLocator.AccountId });
            }
            else dtAccountIds.Rows.Add(new Object[] { AccountID });

            //lista de accountid validos
            AccountValidos = AccountSponsorBusinessLogic.Instance.AplicarreglacValiacionPatrocinio(dtAccountIds);

            return AccountValidos ?? new int[] { 0 };
        }

        #endregion

        #region Index Helpers

        protected virtual void IndexGet_UpdateContext(int sponsorID)
        {
            if (!_enrollmentContext.EnrollmentConfig.Sponsor.DenySponsorChange)
            {
                // Right now we don't have UI to select a placement so these are both SponsorID - JGL
                _enrollmentContext.SponsorID = sponsorID;
                _enrollmentContext.PlacementID = sponsorID;
            }
        }

        protected virtual void Index_LoadResources(IndexModel model)
        {
            // Get sponsor's photo
            MvcHtmlString sponsorPhotoHtml = null;
            try
            {
                var sponsorSite = Site.LoadByAccountID(_enrollmentContext.SponsorID.Value).FirstOrDefault();

                if (sponsorSite != null)
                {
                    var photoHtmlSection = sponsorSite.GetHtmlSectionByName("MyPhoto");
                    if (photoHtmlSection != null)
                    {
                        sponsorPhotoHtml = photoHtmlSection.ToDisplay(sponsorSite, Constants.ViewingMode.Production).ToMvcHtmlString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and continue without photo
                ex.Log();
            }

            model.LoadResources(
                sponsorPhotoHtml,
                ((Account)_enrollmentContext.Sponsor).FullName,
                !_enrollmentContext.EnrollmentConfig.Sponsor.DenySponsorChange
            );
        }

        protected virtual void IndexPost_UpdateAccount(IndexModel model, Account account)
        {
            // Right now we don't have UI to select an enroller so these are both SponsorID - JGL
            _enrollmentContext.SponsorID = _enrollmentContext.SponsorID ?? GetDefaultSponsorID();
            account.EnrollerID = _enrollmentContext.SponsorID;
            account.SponsorID = _enrollmentContext.SponsorID;
        }

        #endregion
    }
}
