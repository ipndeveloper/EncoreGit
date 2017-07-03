using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class ZonesData
    {
        [Display(AutoGenerateField = false)]
        public int GeoScopeID { get; set; }

        [Display(AutoGenerateField = false)]
        public int RouteID { get; set; }

        [Display(AutoGenerateField = false)]
        public int ScopeLevelID { get; set; }

        [TermName("AreaLevel", "Area Level")]
        [Display(Name = "Area Level")]
        public string Name { get; set; }

        [TermName("Name", "Name")]
        [Display(Name = "Name")]
        public string Value { get; set; }

        [TermName("Except", "Except")]
        [Display(Name = "Except")]
        public bool  Except  { get; set; }

        [Display(AutoGenerateField = false)]
        public int ShippingOrderTypeID { get; set; }

    }
}
