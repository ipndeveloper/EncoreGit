using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using System.Web;
using NetSteps.Encore.Core.Process;

namespace NetSteps.Web.API.Base.Common
{
	public enum ApiAccessKind
	{
		None = 0,
		Disabled = 1,
		Full = 2
	}

    [DTO]
	public interface IApiAccess
	{
		Guid AccessID { get; set; }
		ApiAccessKind Kind { get; set; }
		object Attachement { get; set; }
	}

	public interface IApiAccessKeyValidationService
	{
		IApiAccess VerifyAccess(Guid guid);
	}
}
