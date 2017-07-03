

namespace NetSteps.Data.Entities.Dto
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion

    public class BalancesBillOrdersItemsXmlDto
    {
        #region Properties

        public string SortLine { get; set; }
        public string Quantity { get; set; }
        public string Material { get; set; }
        public string QuantityPicked { get; set; }
        public string ICMS { get; set; }
        public string ICMS_ST { get; set; }
        public string IPI { get; set; }
        public string PIS { get; set; }
        public string COFINS { get; set; }
        public string InvoiceUnitValue { get; set; }
        public string ICMSAliq { get; set; }
        public string IPIAliq { get; set; }
        public string CFOP { get; set; }

        #endregion
        
    }
}
