using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;

namespace NetSteps.Web.Mvc.Controls.Models
{
    public class ProPayDisbursementViewModel
    {
        public int? PropayAccountNumber { get; set; }

        public ProPayDisbursementViewModel SetupProPayDisbursementModel(List<IDisbursementProfile> profiles)
        {
            this.PropayAccountNumber = null;
            var proPayProfile =
				profiles.Where(p => p.DisbursementMethod == DisbursementMethodKind.ProPay).FirstOrDefault();
            if (proPayProfile != null)
            {
                this.PropayAccountNumber = ((IPropayDisbursementProfile)proPayProfile).PropayAccountNumber;
            }
            return this;
        }
    }
}
