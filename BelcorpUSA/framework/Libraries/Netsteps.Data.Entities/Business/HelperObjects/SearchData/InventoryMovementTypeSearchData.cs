using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class InventoryMovementTypeSearchData
    {
        public int InventoryMovementTypeID { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public bool Active { get; set; }
        public bool PositiveMovement { get; set; }
    }

    public class InventoryMovementTypeID
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

}
