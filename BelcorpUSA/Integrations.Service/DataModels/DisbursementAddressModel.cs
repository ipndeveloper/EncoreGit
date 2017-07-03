using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    /// <summary>
    /// This is NOT a globalized address.
    /// </summary>
    [DataContract(Name = "disbursementAddress")]
    public class DisbursementAddressModel
    {
        [DataMember]
        public string address1 { get; set; }

        [DataMember]
        public string address2 { get; set; }

        [DataMember]
        public string city { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string zip { get; set; }

        [DataMember]
        public string countryISOCode { get; set; }
    }
}
