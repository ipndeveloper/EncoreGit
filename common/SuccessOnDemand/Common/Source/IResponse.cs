using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.SOD.Common
{
    [DTO]
    public interface IResponse
    {
        string Error { get; set; }
        string DistID { get; set; }
        string ID { get; set; }
    }
}
