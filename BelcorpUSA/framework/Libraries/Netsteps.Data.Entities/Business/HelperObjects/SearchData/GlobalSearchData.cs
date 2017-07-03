using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class GlobalSearchData
	{
		public string ID { get; set; }

		public string DisplayText { get; set; }

		public string ExtraText { get; set; }

		public string Type { get; set; }
	}
}
