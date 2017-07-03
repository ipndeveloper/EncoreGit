using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//@01 20150722 BR-CC-019 G&S LIB: Se crea la clase con sus respectivos métodos

namespace NetSteps.Data.Entities.Business
{
    public class OrderPaymentModificationReasons
    {
        public int OrderPaymentsModificationReasonID { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }

    }
}
