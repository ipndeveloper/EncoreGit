using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class ScopeLevelSearchData
    {

        [Display(Name = "Scope Level ID")]
        public int? scopeLevelID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

    }
}
