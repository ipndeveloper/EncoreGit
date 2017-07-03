
using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Business;
namespace DistributorBackOffice.Areas.Account.Models.Edit
{
    public class AccountModel
    {
        public NetSteps.Data.Entities.Account Account { get; set; }

        public List<AccountPropertiesParameters> accountProperties { get; set; }

        public List<AccountSuppliedIDsParameters> accountSuplieds { get; set; }

        public string OrgExp { get; set; }

        public string IssueDate { get; set; }

        public string SpouseName { get; set; }

        public string SpeakWith { get; set; }

        public bool AutorizationNet { get; set; }

        public bool AutorizationEmails { get; set; }

        public bool AutorizationLocalization { get; set; }

        public List<AccountPropertiesParameters> listaPromerties = new List<AccountPropertiesParameters>();

        public dynamic listAdditonal = null;
    }
}