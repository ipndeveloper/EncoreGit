using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name="productModel", IsReference=true)]
    public class ProductModel
    {
        [DataMember]
        public string sku { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string sapCode { get; set; }

        [DataMember]
        public string bpcsCode { get; set; }

        [DataMember]
        public string materialGroup { get; set; }

        [DataMember]
        public int currentStock { get; set; }
    }
}
