using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class CoApplicantSearchParameters
    {
        public int AccountID { get; set; }
        public int AccountAdditionalTitularID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }  
        public int Relationship { get; set; }
        public int Gender { get; set; }
        public List<AccountAdditionalPhonesParameters> Phones { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CPF { get; set; }
        public string PIS { get; set; }
        public string RG { get; set; }
        public string OrgExp { get; set; }
        public DateTime IssueDate { get; set; }

    }
}
