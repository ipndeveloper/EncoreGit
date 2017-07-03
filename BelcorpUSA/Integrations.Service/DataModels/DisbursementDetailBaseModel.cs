using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name="disbursementDetailBaseModel")]
    public abstract class DisbursementDetailBaseModel
    {
        [DataMember]
        public int referenceID { get; set; }

        [DataMember]
        public decimal targetPercentage { get; set; }

        [DataMember]
        public decimal amount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enrollment form received].  This seems like a horrible place for this.  Is this correct?
        /// </summary>
        /// <value>
        /// <c>true</c> if [enrollment form received]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool enrollmentFormReceived { get; set; }
    }
}
