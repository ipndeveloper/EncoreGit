using NetSteps.Configuration;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.Validation.Configuration
{
    [ContainerRegister(typeof(IValidationConfiguration), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class ValidationConfigurationDefault : IValidationConfiguration
    {
        HtmlValidationConfigurationSection Section { get; set; }

        public ValidationConfigurationDefault()
        {
            var section = ConfigurationUtility.GetSection<HtmlValidationConfigurationSection>();
            this.Section = (section != null) ? section : new HtmlValidationConfigurationSection();
        }

        public bool ValidateHtml
        {
            get { return this.Section.ValidateHtml; }
        }

        public bool ValidateScripts
        {
            get { return this.Section.ValidateScripts; }
        }

        public string ScriptRegex
        {
            get { return this.Section.ScriptRegex; }
        }
    }
}
