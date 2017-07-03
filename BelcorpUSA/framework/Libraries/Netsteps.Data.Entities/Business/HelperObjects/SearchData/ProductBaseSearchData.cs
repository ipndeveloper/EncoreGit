using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class ProductBaseSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int ProductBaseID { get; set; }

		[TermName("SKU")]
		[PropertyName("BaseSKU")]
		[DefaultSort(Constants.SortDirection.Ascending)]
		public string SKU { get; set; }

		[TermName("Name")]
		public string Name { get; set; }

		[Display(AutoGenerateField = false)]
		public int ProductTypeID { get; set; }

		[TermName("Type")]
		[PropertyName("ProductType.TermName")]
		public string ProductType { get; set; }

		[TermName("Categories")]
		[Sortable(false)]
		public string Categories { get; set; }

		[TermName("Catalogs")]
		[Sortable(false)]
		public string Catalogs { get; set; }

		[TermName("Status")]
		[Display(Name = "Status")]
		public bool Active { get; set; }

		[TermName("GMP_ProductBrowse_SKU Code", "SAP SKU")]
		[PropertyName("ProductType.TermName")]
        [Display(AutoGenerateField = false)]
		public string SAPSKU { get; set; }

		[TermName("GMP_ProductBrowse_BPCS Code")]
		[PropertyName("ProductType.TermName")]
        [Display(AutoGenerateField = false)]
		public string BPCS { get; set; }
	}
}
