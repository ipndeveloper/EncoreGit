using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name = "disbursementCheckItem")]
    public class DisbursementDetailCheckModel : DisbursementDetailBaseModel
    {
        [DataMember]
        public int referenceID { get; set; }

        [DataMember]
        public decimal targetPercentage { get; set; }

        [DataMember]
        public decimal amount { get; set; }

        [DataMember]
        public uint routingNumber { get; set; }

        [DataMember]
        public uint accountNumber { get; set; }

        [DataMember]
        public string accountType { get; set; }

        [DataMember]
        public string bankName { get; set; }

        [DataMember]
        public string bankPhone { get; set; }

        [DataMember]
        public DisbursementAddressModel address { get; set; }

    }
}
