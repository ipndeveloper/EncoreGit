using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{

    [Serializable]
    public class ProductMatrix
    {

        //[TermName("ID")]
        //[Display(AutoGenerateField = false)]
        public string CUV { get; set; }

        //[TermName("Period", "Order")]
        public int MaterialID { get; set; }

        //[TermName("CompleteDate", "Complete Date")]
        public string Descripcion { get; set; }

        //[TermName("ItemQuantity", "Item Quantity")]
        public string Mensaje { get; set; }
    }
}
