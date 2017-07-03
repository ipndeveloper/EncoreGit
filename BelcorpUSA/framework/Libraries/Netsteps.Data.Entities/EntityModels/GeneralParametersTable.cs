using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class GeneralParametersTable
    {
        public Int32 GeneralParameterID { get; set; }
        public Int32 MarketID { get; set; }
        public String GeneralParameterCod { get; set; }
        public String GeneralParameterVal { get; set; }
        public String GeneralParameterDES { get; set; }
        public String GeneralParameterDESL { get; set; }
    }
}
