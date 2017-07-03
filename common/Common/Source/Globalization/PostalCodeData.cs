using System;
using System.Runtime.Serialization;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to return basic postal code data from Provider
    /// Created: 8/18/2010
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostalCodeData
    {
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public int StateID { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string StateAbbreviation { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public bool EditaCounty { get; set; }
        [DataMember]
        public bool EditaStreet { get; set; }
    }
}
