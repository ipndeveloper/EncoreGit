using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Services.ServiceModels
{
    public class OrderType : IOrderType
    {
        public string Name { get; set; }

        public int OrderTypeID { get; set; }
    }
}
