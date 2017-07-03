using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class CatalogSearchData
    {
        [Sortable(false)]
        [TermName("Market(s)")]
        [Display(Name = "Market(s)")]
        public string Markets { get; set; }

        [Sortable(false)]
        [TermName("StoreFront(s)", "Store Front(s)")]
        [Display(Name = "Store Front(s)")]
        public string StoreFronts { get; set; }

        [DefaultSort(Constants.SortDirection.Ascending)]
        public string Name { get; set; }

        [Display(AutoGenerateField = false)]
		public int CatalogID { get; set; }

        [TermName("Category")]
        [Display(Name = "Category")]
        [PropertyName("Category.Name")]
        public string CategoryTreeName { get; set; }

        [Display(AutoGenerateField = false)]
		public int CategoryTreeID { get; set; }

        [TermName("Status")]
        [Display(Name = "Status")]
        public bool Active { get; set; }

        [TermName("Schedule")]
        [Display(Name = "Schedule")]
        public DateTime? StartDate { get; set; }

        [TermName("EndDate", "End Date")]
        [Display(AutoGenerateField = false)]
        public DateTime? EndDate { get; set; }

		[Display(AutoGenerateField = false)]
		public bool Editable { get; set; }
    }
}
