using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Web.Mvc.Extensions;

namespace nsDistributor.Models.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SubstringGroupModel
    {
        public virtual List<SubstringModel> Substrings { get; set; }

        public virtual string Value
        {
            get
            {
                return IsValid ? string.Join(string.Empty, Substrings.Select(x => x.Text)) : null;
            }
        }

        public virtual bool IsValid
        {
            get
            {
                if (IsBlank)
                {
                    return false;
                }

                if (_substringConfigs != null)
                {
                    return ValidateSubstrings(Substrings.Select(x => x.Text).ToArray());
                }

                return true;
            }
        }

        public virtual bool IsBlank
        {
            get
            {
                return Substrings == null || !Substrings.Any() || Substrings.All(x => string.IsNullOrEmpty(x.Text));
            }
        }

        protected abstract SubstringConfig[] _substringConfigs { get; }

        protected virtual void LoadSubstringValues(
            string value)
        {
            Substrings = new List<SubstringModel>();

            if (_substringConfigs == null || !_substringConfigs.Any())
            {
                Substrings.Add(new SubstringModel { Text = value });
            }
            else
            {
                int startIndex = 0;
                for (int i = 0; i < _substringConfigs.Count(); i++)
                {
                    Substrings.Add(new SubstringModel
                    {
                        Text = value.SubstringSafe(startIndex, _substringConfigs[i].Length)
                    });

                    startIndex += _substringConfigs[i].Length;
                }
            }
        }

        protected virtual void LoadSubstringResources()
        {
            if (Substrings == null || !Substrings.Any())
            {
                return;
            }

            // This should never happen, but just in case...
            if (_substringConfigs == null || !_substringConfigs.Any())
            {
                Substrings.ForEach(x => x.LoadResources(
                    new Dictionary<string, object>(),
                    null,
                    null
                ));

                return;
            }

            for (int i = 0; i < Substrings.Count() && i < _substringConfigs.Count(); i++)
            {
                var htmlAttributes = new Dictionary<string, object>();

                if (_substringConfigs[i].Length > 0)
                {
                    htmlAttributes["maxlength"] = htmlAttributes["size"] = _substringConfigs[i].Length.ToString();
                }

                if (_substringConfigs[i].IsRequired)
                {
                    htmlAttributes["data-val"] = "true";
                    htmlAttributes["data-val-requireany"] = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
                    htmlAttributes["data-val-requireany-group"] = "{0}";
                }

                if (!string.IsNullOrEmpty(_substringConfigs[i].InputRegex))
                {
                    htmlAttributes["data-inputfilter"] = _substringConfigs[i].InputRegex;
                }

                if (i < (Substrings.Count() - 1))
                {
                    htmlAttributes["data-autotab"] = "next";
                }
                if (i > 0)
                {
                    htmlAttributes["data-autotab-prev"] = "prev";
                }

                Substrings[i].LoadResources(
                    htmlAttributes,
                    _substringConfigs[i].BeforeHtml.ToMvcHtmlString(),
                    _substringConfigs[i].AfterHtml.ToMvcHtmlString()
                );
            }
        }

        protected virtual bool ValidateSubstrings(string[] substrings)
        {
            if (substrings == null || _substringConfigs == null || substrings.Count() < _substringConfigs.Count(x => x.IsRequired))
            {
                return false;
            }

            for (int i = 0; i < substrings.Count(); i++)
            {
                if (string.IsNullOrEmpty(substrings[i]) && _substringConfigs[i].IsRequired)
                {
                    return false;
                }

                if (substrings[i] != null && _substringConfigs[i].ValidRegex != null && !Regex.IsMatch(substrings[i], _substringConfigs[i].ValidRegex))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class SubstringModel
    {
        public virtual string Text { get; set; }

        public virtual IDictionary<string, object> HtmlAttributes { get; set; }
        public virtual MvcHtmlString BeforeHtml { get; set; }
        public virtual MvcHtmlString AfterHtml { get; set; }

        public virtual SubstringModel LoadValues(
            string text)
        {
            Text = text;

            return this;
        }

        public virtual SubstringModel LoadResources(
            IDictionary<string, object> htmlAttributes,
            MvcHtmlString beforeHtml,
            MvcHtmlString afterHtml)
        {
            HtmlAttributes = htmlAttributes;
            BeforeHtml = beforeHtml;
            AfterHtml = afterHtml;

            return this;
        }
    }

    public class SubstringConfig
    {
        public virtual int Length { get; set; }
        public virtual string InputRegex { get; set; }
        public virtual string ValidRegex { get; set; }
        public virtual bool IsRequired { get; set; }
        public virtual string BeforeHtml { get; set; }
        public virtual string AfterHtml { get; set; }
    }
}