using NetSteps.Common.Attributes;
using System;


namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class DisbursmentProfilesSearchData
    {
        [TermName("AccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("DisburmentType")]
        public String DisburmentType { get; set; }

        [TermName("Address1")]
        public String Address1 { get; set; }

        [TermName("Address2")]
        public String Address2 { get; set; }

        [TermName("City")]
        public String City { get; set; }

        [TermName("State")]
        public String State { get; set; }

        [TermName("PostalCode")]
        public String PostalCode { get; set; }

    }
}
