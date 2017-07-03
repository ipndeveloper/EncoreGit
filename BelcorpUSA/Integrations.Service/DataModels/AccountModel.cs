using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract]
    class AccountModel
    {
        public int accountID { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string mainPhone { get; set; }
        [DataMember]
        public string cellPhone { get; set; }
        [DataMember]
        public string emailAddress { get; set; }
    }
}
