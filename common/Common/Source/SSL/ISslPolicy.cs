
namespace NetSteps.Common.SSL
{
    public interface ISslPolicy
    {
        Constants.IsHttpsReturnStatus IsHttps(System.Web.HttpRequestBase request);
    }
}
