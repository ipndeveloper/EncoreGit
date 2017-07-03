using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class AccountSuppliedIDsTable
    {
        public Int32 AccountSuppliedID { get; set; }
        public Int32 IDTypeID { get; set; }
        public String Name { get; set; }
        public Int32 AccountID { get; set; }
        public String AccountSuppliedIDValue { get; set; }
        public Boolean IsPrimaryID { get; set; }
        public DateTime? IDExpeditionIDate { get; set; }
        public String ExpeditionEntity { get; set; } 

    }
}
