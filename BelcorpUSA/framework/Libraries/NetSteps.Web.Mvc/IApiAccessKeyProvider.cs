using System;

namespace NetSteps.Web.Mvc
{
    public interface IApiAccessKeyProvider
    {
        Guid GetApiAccessKey(int identifier);
    }
}
