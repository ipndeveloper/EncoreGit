using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NetSteps.Web.API.Base.Common
{
	public class BaseController : Controller
	{
        public virtual ActionResult Validate()
        {
            return this.Result_200_OK();
        }
	}
}
