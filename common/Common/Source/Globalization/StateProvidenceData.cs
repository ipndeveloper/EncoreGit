using System;
using System.Runtime.Serialization;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to return basic state data from Provider
    /// Created: 8/18/2010
    /// </summary>
    [DataContract]
    [Serializable]
    public class StateProvidenceData
    {
        [DataMember]
        public bool UseAbbreviation { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string StateAbbr { get; set; }
        [DataMember]
        public int StateID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
    }
}
