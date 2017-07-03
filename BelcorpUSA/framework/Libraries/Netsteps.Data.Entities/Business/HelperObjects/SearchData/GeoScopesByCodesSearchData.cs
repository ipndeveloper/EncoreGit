using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    public class GeoScopesByCodesSearchData
    {
        [TermName("PostalCodeFrom", "Postal Code From")]
        [Display(Name = "Postal Code From")]
        public string ValueFrom { get; set; }

        [TermName("PostalCodeTo", "Postal Code To")]
        [Display(Name = "Postal Code To")]
        public string ValueTo { get; set; }

        [TermName("Except", "Except")]
        [Display(Name = "Except")]
        public bool Except { get; set; }

        [Display(AutoGenerateField = false)]
        public int ShippingOrderTypeID { get; set; }

        [Display(AutoGenerateField = false)]
        public int ShippingOrderTypesGeoScopesByCodeID { get; set; }
    }
}
