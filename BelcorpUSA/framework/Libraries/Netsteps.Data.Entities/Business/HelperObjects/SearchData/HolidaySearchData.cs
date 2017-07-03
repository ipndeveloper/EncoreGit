using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class HolidaySearchData
    {



        [TermName("HolidayID")]
        [Display(Name = "HolidayID")]
        public int HolidayID { get; set; }

        [TermName("CountryName")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [TermName("Status")]
        [Display(Name = "State Province Name")]
        public string StateProvinceName { get; set; }

        [TermName("DateHoliday")]
        [Display(Name = "Date Holiday")]
        public DateTime DateHoliday { get; set; }

        [TermName("IsIterative")]
        [Display(Name = "Is Iterative")]
        public bool IsIterative { get; set; }

        [TermName("Reason")]
        [Display(Name = "Reason")]
        public string Reason { get; set; }
    }
}
