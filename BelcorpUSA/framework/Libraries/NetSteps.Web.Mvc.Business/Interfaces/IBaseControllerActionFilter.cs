using System.Web.Mvc;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Business
{
    public interface IBaseControllerActionFilter : IActionFilter
    {
        Controller BaseController { get; set; }
        Site CurrentSite { get; set; }
    }
}
