using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class InventoryLevels
    {
        public long? QuantityAvailable { get; set; }

        public bool IsOutOfStock { get; set; }
    }
}
