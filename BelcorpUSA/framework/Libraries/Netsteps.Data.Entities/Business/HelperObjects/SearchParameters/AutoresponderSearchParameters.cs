using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class AutoresponderSearchParameters : FilterPaginatedListParameters<Autoresponder>
	{
		public bool? IsInternal { get; set; }

		public bool? IsExternal { get; set; }

		public bool? Active { get; set; }
	}
}
