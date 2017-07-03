using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace NetSteps.Integrations.Service.DataModels
{
    [CollectionDataContract(Name = "inventoryItems", ItemName = "item")]
    public class GetInventoryItemModelCollection : Collection<GetInventoryItemModel>
    {
    }
}
