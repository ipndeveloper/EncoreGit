using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Communication.UI.Common
{
    [DTO]
    public interface IMessageAccountAlertModalModel : IAccountAlertModalModel
    {
        string Message { get; set; }
    }
}
