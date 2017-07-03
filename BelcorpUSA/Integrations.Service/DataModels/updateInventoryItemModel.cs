using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Integrations.Service.DataModels
{
	[DataContract(Name = "updateInventoryItemModel")]
	public class UpdateInventoryItemModel
	{
		[DataMember(IsRequired = true, Name = "id")]
		public string Id { get; set; }

		[DataMember(IsRequired = true, Name = "sku")]
		[StringLength(50)]
		public string Sku { get; set; }

		[DataMember(IsRequired = true, Name = "sapCode")]
		public string SAPCode { get; set; }

		[DataMember(IsRequired = true, Name = "updateQuantityOnHand")]
		public int UpdateQuantityOnHand { get; set; }

		[DataMember(IsRequired = true, Name = "targetWarehouseID")]
		public int TargetWarehouseId { get; set; }
	}
}
