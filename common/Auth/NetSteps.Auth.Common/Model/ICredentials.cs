using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Model
{
    public interface ICredentials : IPartialCredentials
    {
        string Password { get; set; }
    }
}
