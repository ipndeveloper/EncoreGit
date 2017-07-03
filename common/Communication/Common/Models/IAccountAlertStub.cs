using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Communication.Common
{
    [DTO]
    public interface IAccountAlertStub : IAccountAlertKey
    {
        Guid ProviderKey { get; set; }
    }
}
