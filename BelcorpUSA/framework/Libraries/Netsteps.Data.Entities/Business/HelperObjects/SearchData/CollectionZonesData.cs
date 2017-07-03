using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class CollectionZonesData
    {

        [Display(AutoGenerateField = false)]
        public int GeoScopeID { get; set; }

        [Display(AutoGenerateField = false)]
        public int CollectionEntityID { get; set; }

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
