using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
    public class ProductSlimSearchData
    {
        [TermName("ID")]
        public int ProductID { get; set; }

        [TermName("SKU")]
        public string SKU { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("IsVariant")]
        public bool IsVariant { get; set; }

        [TermName("ProductBaseID")]
        public int ProductBaseID { get; set; }

        [TermName("IsVariantTemplate")]
        public bool IsVariantTemplate { get; set; }

        [TermName("ProductBaseHasProperties")]
        public bool ProductBaseHasProperties { get; set; }
    }
}
