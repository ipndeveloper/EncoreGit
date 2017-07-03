using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class BalancesBillOrdersXmlDto
    {
        #region Constructor

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public BalancesBillOrdersXmlDto()
        {
            this.DetailList = new List<BalancesBillOrdersItemsXmlDto>();
        }
        
        #endregion

        #region Properties
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }        
        public string InvoiceSerie { get; set; }
        //public DateTime DateCreatedUTC { get; set; }
        public string DateInvoice { get; set; }
        //public string InvoiceStatus { get; set; }
        public string InvoiceType { get; set; }
        public string DistributionCenter { get; set; }
        public string ChaveNFe { get; set; }
        public string Boxes { get; set; }
        public string Weight { get; set; }
        public string QuantityOnHand { get; set; }
        public string InvoicePath { get; set; }
        
        /// Modificación: Campos nuevos 
        /// Fecha: 17/03/2016 
        /// Author: MAM - CSTI
        public string WeightLiq { get; set; }
        public string HeadICMSBase { get; set; }
        public string HeadICMSValue { get; set; }
        public string HeadICMSSTBase { get; set; }
        public string HeadICMSSTValue { get; set; }
        public string HeadIPIBase { get; set; }
        public string HeadIPIValue { get; set; }

        // Lista de Items

        public List<BalancesBillOrdersItemsXmlDto> DetailList { get; set; }

        /// Fin Modificación.
        /// 

        #endregion
    }
}
