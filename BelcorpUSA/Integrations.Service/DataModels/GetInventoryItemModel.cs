using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name = "inventoryItemModel")]
    public class GetInventoryItemModel
    {
        [DataMember(IsRequired = true, Name = "sku")]
        [StringLength(50)]
        public string Sku { get; set; }
    }
}
