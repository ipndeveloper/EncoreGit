using System;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.Process;

namespace NetSteps.Web.Mvc
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
		IApiAccess VerifyAccess(IProcessIdentity process, Guid guid);
	}
}
