using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class ProductSearchData
    {
        public int ProductID { get; set; }
        public int SKU { get; set; }

        #region Capaign Information

        public int PeriodID { get; set; }
        public string CUV { get; set; }
        public string Name { get; set; }
        public int ExternalCode { get; set; }
        public int IsKit { get; set; }
        public string SAPSKU { get; set; }
        public int OfferType { get; set; }
        public double PrecioTabela { get; set; }  
        public double PrecioComision { get; set; }
        public double Puntos { get; set; }
        public double PrecioPracticado { get; set; }
        public string Tipo { get; set; }
        public string Categorias { get; set; } 
        public string Catalogos { get; set; } 
        public bool Situacion { get; set; }  

        #endregion
    }

    public class ProductTemporalSearchData
    {
        public int ProductID { get; set; }
        public int SKU { get; set; }
    }

    [Serializable]
    public class listdispatchGet
    {  
	    public int  DispatchID { get; set; }
        public int? PeriodStart { get; set; }
        public int? PeriodEnd { get; set; }
        public string Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string ListName { get; set; }
        public string Situacion { get; set; }
    }
    
    
}
