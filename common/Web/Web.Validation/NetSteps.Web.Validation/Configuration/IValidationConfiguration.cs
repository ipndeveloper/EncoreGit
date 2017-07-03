using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.Validation.Configuration
{
    public interface IValidationConfiguration
    {
        bool ValidateHtml { get; }
        bool ValidateScripts { get; }

        string ScriptRegex { get; }
    }
}
