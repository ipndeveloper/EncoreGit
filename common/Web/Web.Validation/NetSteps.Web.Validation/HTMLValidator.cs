using HtmlAgilityPack;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Validation.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetSteps.Web.Validation
{
    [ContainerRegister(typeof(IHTMLValidator), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class HTMLValidator : IHTMLValidator
    {
        IValidationConfiguration Configuration = Create.New<IValidationConfiguration>();

        public bool IsValid(string html, out IEnumerable<IHTMLParseError> errors)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var localErrors = new List<IHTMLParseError>();

            if (this.Configuration.ValidateHtml)
            {
                this.ValidateHTML(htmlDoc, localErrors);
            }

            if (this.Configuration.ValidateScripts)
            {
                this.ValidateScripting(htmlDoc, localErrors);
            }

            errors = localErrors;
            return errors.Count() == 0;
        }

        void ValidateHTML(HtmlDocument htmlDoc, List<IHTMLParseError> errors)
        {
            var localErrors = htmlDoc.ParseErrors.Select(x => new HTMLParseError(x.Code.ToString(), x.Reason));
            errors.AddRange(localErrors);
        }

        List<string> GetScriptRegexPatterns()
        {
            if (!string.IsNullOrEmpty(this.Configuration.ScriptRegex))
            {
                return this.Configuration.ScriptRegex.Split(',')
                    .Select(x => x.Trim())
                    .ToList();
            }
            else
            {
                return new List<string> { string.Format("jquery(-\\d+{0}\\d+{0}\\d+)?({0}min)?{0}js", Regex.Escape(".")) };
            }
        }
        void ValidateScripting(HtmlDocument htmlDoc, List<IHTMLParseError> errors)
        {
            var regexPatterns = GetScriptRegexPatterns();
            var regexList = regexPatterns.Select(x => new Regex(x, RegexOptions.IgnoreCase));

        	var descendants = htmlDoc.DocumentNode.Descendants();
			if(descendants == null || !descendants.Any())
			{
				return;
			}

			var localErrors = descendants.Where(x => x.Name.ToLower() == "script")
				.Where(x =>
				       	{
				       		var sourceAttribute = x.Attributes.FirstOrDefault(attrib => attrib.Name == "src");
				       		return x.Attributes != null && x.Attributes.Any() &&
				            	       regexList.Select(r => sourceAttribute != null && r.IsMatch(sourceAttribute.Value)).Contains(true);
				       	})
				.Select(x => new HTMLParseError("INVALID_SCRIPT_INCLUDE"
					, string.Format("Invalid script include was detected: {0}", x.Attributes.Where(attrib => attrib.Name == "src").FirstOrDefault().Value)));

            if (localErrors != null && localErrors.Any())
            {
                errors.AddRange(localErrors);
            }
        }
    }
}
