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
    public class BankConsolidateApplicationSearchData
    {
        [Display(Name = "BankConsolidateSec")]
        public int BankConsolidateSec { get; set; }

        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Display(Name = "BankConsolidatePro")]
        public int BankConsolidatePro { get; set; }

        [Display(Name = "BankConsolidateVal")]
        public decimal BankConsolidateVal { get; set; }

        [Display(Name = "BankConsolidateErr")]
        public int BankConsolidateErr { get; set; }

        [Display(Name = "BankConsolidateFile")]
        public string BankConsolidateFile { get; set; }

        [Display(Name = "BankConsolidateDateFile")]
        public string BankConsolidateDateFile { get; set; }

        [Display(Name = "BankConsolidateDatePro")]
        public string BankConsolidateDatePro { get; set; }

   

        [Display(AutoGenerateField = false)]
        public string BankID { get; set; }


        ////----------

        //public string AccountCode { get; set; }
        //public string AccountName { get; set; }
        //public string BankName { get; set; }
        //public string TicketNumber { get; set; }
        //public string OrderNumber { get; set; }
        //public decimal Amount { get; set; }
        //public string DateReceivedBank { get; set; }
        //public DateTime DateApplied { get; set; }
        //public string FileNameBank { get; set; }
        //public string FileSequence { get; set; }
        //public string logSequence { get; set; }
        //public string Applied { get; set; }

    }
}
