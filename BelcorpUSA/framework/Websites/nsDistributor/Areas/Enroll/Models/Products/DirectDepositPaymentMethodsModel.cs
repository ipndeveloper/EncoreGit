using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

/*CSTI(CS)-05/03/2016: Inicio*/
namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class DirectDepositPaymentMethodsModel
    {
        /*CSTI(CS)-05/03/2016: Inicio*/
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
        /*CSTI(CS)-05/03/2016: Fin*/
    }
}