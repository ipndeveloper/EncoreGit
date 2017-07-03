using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class OferTypeRestrictionsTable
    {
        public int OferTypeRestrictionID { get; set; }
        public string OferTypeRestrictionValue { get; set; }
        public int ProductRelationTypeID { get; set; }
    }
}
