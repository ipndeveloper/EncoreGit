using System.ServiceModel;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service.Interfaces
{
    [ServiceContract(Name = "productAPI", Namespace="netSteps.products")]
    public interface IProductAPI
    {
        [OperationContract]
        [FaultContract(typeof(APIFault))]
        ProductModel addProduct(string userName, string password, ProductModel model);

        [OperationContract]
        [FaultContract(typeof(APIFault))]
        bool archiveProduct(string userName, string password, string sku);

        [OperationContract]
        [FaultContract(typeof(APIFault))]
        ProductModel updateProduct(string userName, string password, string sku, ProductModel model);
    }
}
