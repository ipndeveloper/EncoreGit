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
    public class CollectionEntitySearchData
    {
        [Display(Name = "Collection Entity ID")]
        public int CollectionEntityID { get; set; }

        [Display(Name = "Collection Entity Name")]
        public string CollectionEntityName { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "PaymentType")]
        public string PaymentType { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
