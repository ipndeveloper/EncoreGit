using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class CompanySearchData 
    {

        [Display(Name = "Company ID")]
        public int CompanyID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

       
    }
}
