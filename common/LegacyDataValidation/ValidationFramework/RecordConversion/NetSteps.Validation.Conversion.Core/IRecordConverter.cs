using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Conversion.Core
{
    public interface IRecordConverter
    {
        IRecord Convert(object originalObject, IRecord parent);
    }

    public interface IRecordConverter<TObject> : IRecordConverter
    {
        IRecord Convert(TObject originalObject, IRecord parent);
    }
}
