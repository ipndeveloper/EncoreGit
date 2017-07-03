using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name = "disbursementACHItem")]
    public class DisbursementDetailACHModel : DisbursementDetailBaseModel
    {
        

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
