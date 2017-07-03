using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

//@01 20150720 BR-CC-003 G&S LIB: Se agrego la clase con sus atributos respectivos

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class PaymentsZonesData
    {

        [Display(AutoGenerateField = false)]
        public int GeoScopeID { get; set; }

        [Display(AutoGenerateField = false)]
        public int PaymentsConfigurationID { get; set; }

        [Display(AutoGenerateField = false)]
        public int ScopeLevelID { get; set; }

        [Display(Name = "Area Level")]
        public string Name { get; set; }

        [Display(Name = "Name")]
        public string Value { get; set; }

        [Display(Name = "Except")]
        public bool Except { get; set; }

    }
}
