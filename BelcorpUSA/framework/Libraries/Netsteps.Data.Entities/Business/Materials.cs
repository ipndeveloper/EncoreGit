﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class Materials : DynamicModel
    {
        public Materials() : base("Core", "ProductRelations", "ProductRelationID") { }
    }
}
