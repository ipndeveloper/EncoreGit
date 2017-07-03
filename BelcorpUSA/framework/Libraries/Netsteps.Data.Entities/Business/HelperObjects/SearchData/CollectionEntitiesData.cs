using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;


namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class CollectionEntitiesData
    {

        [Display(Name = "Collection Entity ID")]
        public int? CollectionEntityID { get; set; }

        [Display(Name = "PaymentTypeID")]
        public int? PaymentTypeID { get; set; }

        [Display(Name = "PaymentGatewayID")]
        public int? PaymentGatewayID { get; set; }

        [Display(Name = "BankID")]
        public int? BankID { get; set; }

        [Display(Name = "BankAgencie")]
        public string BankAgencie { get; set; }

        [Display(Name = "BankAccountNumber")]
        public int? BankAccountNumber { get; set; }

        [Display(Name = "BankAccountType")]
        public int? BankAccountType { get; set; }

        [Display(Name = "CompanyID")]
        public int? CompanyID { get; set; }

        [Display(Name = "CollectionTypePerBankID")]
        public int? CollectionTypePerBankID { get; set; }

        [Display(Name = "CollectionDocumentTypePerBankID")]
        public int? CollectionDocumentTypePerBankID { get; set; }

        [Display(Name = "CollectionAgreement")]
        public string CollectionAgreement { get; set; }        

        [Display(Name = "Collection Entity Name")]
        public string CollectionEntityName { get; set; }

        [Display(Name = "Active")]
        public int? Active { get; set; }


        [Display(AutoGenerateField = false)]
        public string FileNameBankCollection { get; set; }

        [Display(AutoGenerateField = false)]
        public int? InitialPositionName { get; set; }

        [Display(AutoGenerateField = false)]
        public int? FinalPositionName { get; set; }

        [Display(AutoGenerateField = false)]
        public int? CodeDetail { get; set; }

        
    }
}
