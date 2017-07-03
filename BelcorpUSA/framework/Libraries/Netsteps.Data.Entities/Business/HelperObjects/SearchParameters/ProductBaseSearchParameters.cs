using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class ProductBaseSearchParameters : FilterPaginatedListParameters<ProductBase>
	{
		public string Query { get; set; }

		public int? ProductTypeID { get; set; }

		public bool? Active { get; set; }

		public bool? IsShippable { get; set; }

		public bool? ChargeShipping { get; set; }

		public string BPCS { get; set; }

		public string SAPSKU { get; set; }

        public int PeriodID { get; set; } //recien agregado

        public int LanguageID { get; set; } //recien agregado
	}
}
