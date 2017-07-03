using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name="disbursement")]
    public class DisbursementModel
    {
        [DataMember(Name = "periodID")]
        int PeriodID { get; set; }

        [DataMember(Name = "effectiveDate")]
        DateTime EffectiveDate { get; set; }

        [DataMember(Name = "disbursementID")]
        int DisbursementID { get; set; }

        [DataMember(Name="accounts")]
        DisbursementAccountModelCollection Accounts { get; set; }
    }
}
