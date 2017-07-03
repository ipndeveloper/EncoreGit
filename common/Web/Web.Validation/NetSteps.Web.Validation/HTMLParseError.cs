
namespace NetSteps.Web.Validation
{
    class HTMLParseError : IHTMLParseError
    {
        public HTMLParseError(string code, string reason)
        {
            this.Code = code;
            this.Reason = reason;
        }

        public string Code { get; private set; }
        public string Reason { get; private set; }
    }
}
