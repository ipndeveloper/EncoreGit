using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Communication.UI.Common
{
    [DTO]
    public interface IAccountAlertMessageModel : IAccountAlertKey
    {
        string Message { get; set; }
        string ActionLinkUrl { get; set; }
        bool OpenActionLinkInNewWindow { get; set; }
        bool IsDismissable { get; set; }
    }
}
