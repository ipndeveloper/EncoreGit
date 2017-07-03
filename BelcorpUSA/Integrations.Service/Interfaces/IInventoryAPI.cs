using System.ServiceModel;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service.Interfaces
{
	[ServiceContract(Name = "inventoryAPI", Namespace = "netSteps.inventory")]
	public interface IInventoryAPI
	{
		[OperationContract]
		[FaultContract(typeof(APIFault))]
		UpdateInventoryItemModelResponseCollection UpdateInventory(string username, string password, UpdateInventoryItemModelCollection items);

		[OperationContract]
		[FaultContract(typeof(APIFault))]
		GetInventoryItemModelResponseCollection GetInventory(string username, string password, GetInventoryItemModelCollection items);
	}
}
