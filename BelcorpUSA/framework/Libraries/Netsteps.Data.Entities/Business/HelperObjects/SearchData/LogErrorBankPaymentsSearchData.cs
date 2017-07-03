using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

//@01 20150720 BR-CC-002 G&S LIB: Se crea la clase con sus respectivos métodos

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class LogErrorBankPaymentsSearchData
    {
        [Sortable(false)]
        [TermName("Select", "Select")]
        public string Select { get; set; }

        [Display(Name = "LogErrorBankPaymentID")]
        public int LogErrorBankPaymentID { get; set; }

        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Display(Name = "TicketNumbers")]
        public int TicketNumber { get; set; }

        [Display(Name = "LogAmount")]
        public decimal LogAmount { get; set; }

        [Display(Name = "OrderNumber ")]
        public int OrderNumber { get; set; }

         [Display(Name = "DateBankLog")]
        public string DateBankLog { get; set; }

        [Display(Name = "Date ")]
        public string Date { get; set; }       

        [Display(Name = "FileSequenceLog")]
        public int FileSequenceLog { get; set; }

        [Display(Name = "logSequenceLog")]
        public int logSequenceLog { get; set; }

        [Display(Name = "StatusLog ")]
        public int StatusLog { get; set; }

        
    }
}
