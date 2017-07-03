using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public class DirectDepositPaymentMethodsModel
    {
        /*CS.21JUN2016.Inicio*/
        private List<IEFTDisbursementProfile> _eftProfiles;

        public IEnumerable<IEFTDisbursementProfile> EFTProfiles
        {
            get { return _eftProfiles; }
            set
            {
                _eftProfiles = value
                    .Where(p => p.DisbursementMethod == DisbursementMethodKind.EFT)
                    .OrderByDescending(x => x.IsEnabled)
                    .OrderBy(x => x.DisbursementProfileId)
                    .ToList();
            }
        }
        public string PostalCodeLookupURL { get; set; }

        public DirectDepositPaymentMethodsModel SetupEFTDisbursementModel(IEnumerable<IEFTDisbursementProfile> profiles)
        {
            var commissionService = Create.New<ICommissionsService>();

            this.EFTProfiles = profiles ?? new List<IEFTDisbursementProfile>();
            var profilesAllowed = commissionService.GetMaximumDisbursementProfiles(DisbursementMethodKind.EFT);

            return this;
        }
        /*CS.21JUN2016.Inicio*/
    }
}