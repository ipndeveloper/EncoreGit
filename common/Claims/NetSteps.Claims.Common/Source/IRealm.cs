using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
    public interface IRealm
    {
        string Name { get; set; }

        IEnumerable<IApplication> Applications { get; set; }
    }
}
