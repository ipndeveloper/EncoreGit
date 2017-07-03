using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name="disbursementAccount")]
    public class DisbursementAccountModel
    {
        [DataMember(Name="accountNumber")]
        public int AccountNumber { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "entityType")]
        public string EntityType { get; set; }

        [DataMember(Name = "disbursementDetails")]
        DisbursementDetailModelCollection DisbursementDetails { get; set; }

    }
}
