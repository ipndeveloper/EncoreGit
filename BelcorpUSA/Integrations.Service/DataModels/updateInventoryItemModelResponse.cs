using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Integrations.Service.DataModels
{
	[DataContract(Name = "updateInventoryItemModelResponse")]
	public class UpdateInventoryItemModelResponse
	{
		[DataMember(IsRequired = true, Name = "id")]
		public string Id { get; set; }

		[DataMember(IsRequired = true, Name = "success")]
		public bool Success { get; set; }

		[DataMember(IsRequired = false, Name = "errorCode")]
		public int ErrorCode { get; set; }

		[DataMember(IsRequired = false, Name = "errorMessage")]
		public string ErrorMessage { get; set; }

		[DataMember(IsRequired = true, Name = "quantityOnHand")]
		public int QuantityOnHand { get; set; }

		[DataMember(IsRequired = true, Name = "quantityAvailable")]
		public int QuantityAvailable { get; set; }

		[DataMember(IsRequired = true, Name = "quantityAllocated")]
		public int QuantityAllocated { get; set; }

		[DataMember(IsRequired = true, Name = "sku")]
		[StringLength(50)]
		public string SKU { get; set; }

		[DataMember(IsRequired = true, Name = "sapCode")]
		public string SAPCode { get; set; }

		[DataMember(IsRequired = false, Name = "warehouseID")]
		public int WarehouseID { get; set; }
	}
}
