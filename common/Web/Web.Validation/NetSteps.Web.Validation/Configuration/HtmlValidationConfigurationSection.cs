using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NetSteps.Configuration;

namespace NetSteps.Web.Validation.Configuration
{
    public class HtmlValidationConfigurationSection : DeclaredMemberConfigurationSection
    {
        public static readonly ConfigurationProperty ValidateHtmlProperty = new ConfigurationProperty("validateHtml", typeof(bool), true);
        public bool ValidateHtml
        {
            get { return (bool)this[ValidateHtmlProperty]; }
            set { this[ValidateHtmlProperty] = value; }
        }

        public static readonly ConfigurationProperty ValidateScriptsProperty = new ConfigurationProperty("validateScripts", typeof(bool), true);
        public bool ValidateScripts
        {
            get { return (bool)this[ValidateScriptsProperty]; }
            set { this[ValidateScriptsProperty] = value; }
        }

        public static readonly ConfigurationProperty ScriptRegexProperty = new ConfigurationProperty("scriptRegex", typeof(string), string.Empty);
        public string ScriptRegex
        {
            get { return (string)this[ScriptRegexProperty]; }
            set { this[ScriptRegexProperty] = value; }
        }

    }
}
