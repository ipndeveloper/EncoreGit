using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

//@01 20150717 BR-CC-003 G&S LIB: Se crea la clase con sus respectivos métodos

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{

    [Serializable]
    public class RenegotiationDetSearchData
    {

        [Display(AutoGenerateField = false)]
        public int RenegotiationConfigurationDetailsID { get; set; }

        [Display(AutoGenerateField = false)]
        public int RenegotiationConfigurationID { get; set; }

        [Display(Name = "Opening Day")]
        public int OpeningDay { get; set; }

        [Display(Name = "Final Day")]
        public int FinalDay { get; set; }

        [Display(AutoGenerateField = false)]
        public int FineBaseAmountID { get; set; }

        [Display(Name = "Fine Base Amount")]
        public string FineBaseAmountDesc { get; set; }

        [Display(Name = "Discount")]
        public string Discount { get; set; }


    }
}
