using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderTackingsSearchData
    {        
        public string Pointer { get; set; }
        public int OrderStatusID { get; set; }
        public DateTime InitialTackingDateUTC { get; set; }
        public DateTime FinalTackingDateUTC { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int RowTotal { get; set; }
        public int Etapa { get; set; }
        public string ImagenStatus { get; set; }
    }
}