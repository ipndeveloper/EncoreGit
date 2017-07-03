using System.Collections.Generic;
using System;

namespace NetSteps.Data.Entities.Business
{
    public class CreditRequirementSearchData
    {
        public decimal CreditRequirementID { get; set; }

        public int AccountID { get; set; }

        public int RequirementTypeID { get; set; }

        public string RequirementTypeName { get; set; }

        public int RequirementStatusID { get; set; }

        public string RequirementStatusName { get; set; }

        public string Observations { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string LastUserModifiedName { get; set; }

        public int LastUserModifiedID { get; set; }

        public int UserCreatedID { get; set; }

        public bool IsModified { get; set; }
    }
}
