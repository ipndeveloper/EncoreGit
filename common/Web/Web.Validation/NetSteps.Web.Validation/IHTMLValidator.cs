using System.Collections.Generic;

namespace NetSteps.Web.Validation
{
    public interface IHTMLValidator
    {
        bool IsValid(string html, out IEnumerable<IHTMLParseError> errors);

    }
}
