using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class DispatchListItems
    {
        public Int32 DispatchListItemID { get; set; }
        public Int32 DispatchListID { get; set; }
        public Int32 AccountID { get; set; }
        public String AccountNumber { get; set; }
        public String AccountName { get; set; }
        public DateTime DateCreated { get; set; } 
    }
}
