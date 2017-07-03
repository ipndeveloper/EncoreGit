using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Integrations.Service.DataModels
{
	[DataContract(Name = "inventoryItemModelResponse")]
	public class GetInventoryItemModelResponse
	{
		[DataMember(IsRequired = true, Name = "quantityOnHand")]
		public int QuantityOnHand { get; set; }

		[DataMember(IsRequired = true, Name = "quantityAvailable")]
		public int QuantityAvailable { get; set; }

		[DataMember(IsRequired = true, Name = "quantityAllocated")]
		public int QuantityAllocated { get; set; }

		[DataMember(IsRequired = true, Name = "sku")]
		[StringLength(50)]
		public string Sku { get; set; }

		[DataMember(IsRequired = true, Name = "sapCode")]
		public string SAPCode { get; set; }

		[DataMember(IsRequired = false, Name = "warehouseID")]
		public int WarehouseId { get; set; }
	}
}
