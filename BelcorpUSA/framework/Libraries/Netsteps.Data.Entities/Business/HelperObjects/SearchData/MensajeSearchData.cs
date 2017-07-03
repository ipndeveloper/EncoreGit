using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class MensajeSearchData
    {
        public string Message { get; set; }
        public bool Estado { get; set; }
        public bool EstatusNewQuantity { get; set; }
        public int NewQuantity { get; set; }
    }
}
