using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class CorporateUserSearchParameters : FilterPaginatedListParameters<CorporateUser>
	{
		public int? Status { get; set; }

		public int? Role { get; set; }

		public string Username { get; set; }
	}
}
