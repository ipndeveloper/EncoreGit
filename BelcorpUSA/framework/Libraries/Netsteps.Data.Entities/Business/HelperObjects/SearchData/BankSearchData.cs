using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class BankSearchData
    {

        [Display(Name = "BankID")]
        public int BankID { get; set; }

        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Display(Name = "BankCode")]
        public int BankCode { get; set; }

        [Display(Name = "TermName")]
        public string TermName { get; set; }

        [Display(Name = "SortIndex")]
        public int SortIndex { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(Name = "MarketID")]
        public int MarketID { get; set; }
    }
}
