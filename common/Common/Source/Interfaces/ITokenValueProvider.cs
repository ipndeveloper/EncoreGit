using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Interfaces
{
    public interface ITokenValueProvider
    {
        IEnumerable<string> GetKnownTokens();
        string GetTokenValue(string token);
    }
}
