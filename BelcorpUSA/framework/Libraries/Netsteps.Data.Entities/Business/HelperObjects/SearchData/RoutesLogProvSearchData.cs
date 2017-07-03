using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class RoutesLogProvSearchData
    {
        [Display(AutoGenerateField = false)]
        public int LogisticsProviderID { get; set; }

        [TermName("Code")]
        [Display(Name = "Code")]
        public int RouteID { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("Monday")]
        [Display(Name = "Monday")]
        public bool Monday { get; set; }

        [TermName("Tuesday")]
        [Display(Name = "Tuesday")]
        public bool Tuesday { get; set; }

        [TermName("Wednesday")]
        [Display(Name = "Wednesday")]
        public bool Wednesday { get; set; }

        [TermName("Thursday")]
        [Display(Name = "Thursday")]
        public bool Thursday { get; set; }

        [TermName("Friday")]
        [Display(Name = "Friday")]
        public bool Friday { get; set; }

        [TermName("Saturday")]
        [Display(Name = "Saturday")]
        public bool Saturday { get; set; }

        [TermName("Sunday")]
        [Display(Name = "Sunday")]
        public bool Sunday { get; set; }
    }
}
