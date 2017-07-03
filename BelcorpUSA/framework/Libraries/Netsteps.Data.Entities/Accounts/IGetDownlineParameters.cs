using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Common.Models
{
	[DTO]
	public interface IGetDownlineParameters
	{
		int RootAccountId { get; set; }
		int? MaxLevels { get; set; }
		IEnumerable<short> AccountStatusIds { get; set; }
		IEnumerable<short> AccountTypeIds { get; set; }
	}
}
