using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Integrations.Service.Interfaces;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, Name="productAPI")]
    public class ProductAPI : IProductAPI
    {
        public ProductModel addProduct(string userName, string password, ProductModel model)
        {
            throw new NotImplementedException();
        }

        public bool archiveProduct(string userName, string password, string sku)
        {
            throw new NotImplementedException();
        }

        public ProductModel updateProduct(string userName, string password, string sku, ProductModel model)
        {
            throw new NotImplementedException();
        }
    }
}
