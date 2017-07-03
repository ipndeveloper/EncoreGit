using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nsCore.Areas.Reports.Controllers
{
    [Serializable]
    class ProductPriceSearchData
    {

        public string SKU { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Categories { get; set; }
        public string Catalogs { get; set; }
        public string Status { get; set; }
        public string SAPSKU { get; set; }
        public string BPCS { get; set; }
        public string Retail { get; set; }
        public string CV { get; set; }
        public string QV { get; set; }
        public string Handling { get; set; }


    }
}
