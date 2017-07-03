using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Controls.Controllers.Enrollment;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Enrollment.Controllers
{
    public class DisbursementProfilesStep : BaseEnrollmentStep
    {
        private static bool _enableCheckProfile = true;
        private static bool _enableEFTProfile = true;

        private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

        static DisbursementProfilesStep()
        {
            IEnumerable<dynamic> disbProfilesProperties = EnrollmentConfigHandler.GetProperties("DisbursementProfiles");
            _enableCheckProfile = bool.Parse((string)disbProfilesProperties.First(p => p.Name == "EnableCheckProfile"));
            _enableEFTProfile = bool.Parse((string)disbProfilesProperties.First(p => p.Name == "EnableEFTProfile"));
        }

        public DisbursementProfilesStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
            : base(stepConfig, controller, enrollmentContext) { }

        public ActionResult Index()
        {
            var service = Create.New<ICommissionsService>();
            var checkProfile =
                service.GetDisbursementProfilesByAccountAndDisbursementMethod(_enrollmentContext.EnrollingAccount.AccountID, DisbursementMethodKind.Check).FirstOrDefault();
            _controller.ViewData["CheckProfileID"] = checkProfile != null ? checkProfile.DisbursementProfileId : 0;
            
            _controller.ViewData["EnableCheckProfile"] = _enableCheckProfile;
            _controller.ViewData["EnableEFTProfile"] = _enableEFTProfile;
            _controller.ViewData["IsSkippable"] = this.IsSkippable;
            _controller.ViewData["StepCounter"] = _enrollmentContext.StepCounter;
            return PartialView();
        }

        public ActionResult SubmitStep(int id, DisbursementMethodKind preference, bool? useAddressOfRecord, bool? isActive, string profileName, string payableTo, string address1, string address2, string address3, string city, string state, string zip, bool? agreementOnFile, List<EFTAccountModel> accounts)
        {
            try
            {
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
                    City = city,
                    State = stateCode,
                    StateProvinceID = stateId,
                    PostalCode = zip,
                    CountryID = (int)ConstantsGenerated.Country.UnitedStates
                };

                useAddressOfRecord = useAddressOfRecord ?? false;
                agreementOnFile = agreementOnFile ?? false;

                _commissionsService.SaveDisbursementProfile(id, EnrollingAccount, viewAddress,
                    preference, (bool)useAddressOfRecord, accounts.Select(x => x.Convert()), (bool)agreementOnFile);

                return NextStep();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (EnrollingAccount != null) ? EnrollingAccount.AccountID.ToIntNullable() : null);
                return JsonError(exception.PublicMessage);
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

        public ActionResult SkipStep()
        {
            return NextStep();
        }
    }
}
