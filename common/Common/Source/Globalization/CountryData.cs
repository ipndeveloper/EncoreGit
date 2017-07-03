using System;
using System.Runtime.Serialization;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to return basic country data from Provider
    /// Created: 8/18/2010
    /// </summary>
    [DataContract]
    [Serializable]
    public class CountryData
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Abbreviation { get; set; }
        [DataMember]
        public string CountryIso2 { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CultureCode { get; set; }
        [DataMember]
        public bool IsAvailableForRegistration { get; set; }
    }
}
