using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Means of Collections Business Entity to Search
    /// </summary>
    [Serializable]
    public class MeansOfCollectionSearchData
    {
        [Display(Name = "Collection Entity ID")]
        public int CollectionEntityID { get; set; }

        [TermName("CollectionEntityName", "Collection Entity Name")]
        [Display(Name = "Collection Entity Name")]
        public string CollectionEntityName { get; set; }

        [Display(AutoGenerateField = false)]
        public int LocationID { get; set; }

        [TermName("Location")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(AutoGenerateField = false)]
        public int PaymentTypeID { get; set; }

        [TermName("PaymentType", "Payment Type")]
        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }

        [TermName("Status", "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

    }
}
