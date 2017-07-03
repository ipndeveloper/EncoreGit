using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.EntityModels;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Business
{
    public class AccountInformacion
    {

        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstNameAddress { get; set; }
        public string LastNameAddress { get; set; }
        public string Attention { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string TaxNumber { get; set; }
        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string AccountType { get; set; }
        public DateTime Enrollment { get; set; }
        public Boolean IsLocked { get; set; }
        public string StatusName { get; set; }
        public string CareerAsTitle { get; set; }
        public string PaidAsTitle { get; set; }
        public string Sponsor { get; set; }

       
    }

}
