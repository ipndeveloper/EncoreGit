
namespace NetSteps.Common.SSL
{
    public class AwsSslPolicy : ISslPolicy
    {
        #region ISslPolicy Members

        public Constants.IsHttpsReturnStatus IsHttps(System.Web.HttpRequestBase request)
        {
            Constants.IsHttpsReturnStatus result = Constants.IsHttpsReturnStatus.Unknown;

            string sslProtocolName = "https";
            string headerName = "X-Forwarded-Proto";

            string headerValue = request.Headers[headerName];

            if (headerValue != null)
            {
                result = headerValue.ToLower() == sslProtocolName ? Constants.IsHttpsReturnStatus.IsHttps : Constants.IsHttpsReturnStatus.IsNotHttps;
            }

            return result;
        }

        #endregion
    }
}
