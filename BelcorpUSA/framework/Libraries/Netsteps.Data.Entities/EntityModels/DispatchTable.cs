using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class DispatchTable
    {
        // Dispatch
        public Int32 DispatchID { get; set; }
        public Int32 DispatchTypeID { get; set; }
        public Int32 DispatchStatusType { get; set; }
        public String Description { get; set; }
        public Int32? PeriodStart { get; set; }
        public Int32? PeriodEnd { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Int32 OnlyTime { get; set; }
        public Int32? ListScope { get; set; }
        public Int32 Status { get; set; }
        public String Termname { get; set; }
        public Int32 SortIndex { get; set; }
        public bool Editable { get; set; }
        public List<DispatchItemsTable> Products { get; set; }
        public List<DispatchItemsQuery> ProductsQuery { get; set; }
    }

    [Serializable]
    public class listdispatchDisplay
    {
        public int DispatchListID { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class typedispatchDisplay
    {
        public int DispatchTypeID { get; set; }
        public string Name { get; set; }
        public int? SecondOwner { get; set; }
        public int Active { get; set; }
    }
}
