using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Security.Authentication
{
	[DTO]
	public interface IAuthorizationResult
	{
		bool IsAuthorzied { get; set; }
		string Message { get; set; }
	}
}
