﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Common
{
    public interface IObjectConverter<TObject>
    {
        IRecord GetRecord(TObject originalObject);
    }
}
