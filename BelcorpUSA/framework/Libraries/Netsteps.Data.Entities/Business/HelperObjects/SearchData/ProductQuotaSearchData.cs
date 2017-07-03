using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Quota of Product, Entity to Search (Table)
    /// </summary>
    [Serializable]
    public class ProductQuotaSearchData
    {
        [Display(AutoGenerateField = false)]
        public int RestrictionID { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(AutoGenerateField = false)]
        public int ProductID { get; set; }

        [TermName("Product")]
        [Display(Name = "Product")]
        public string ProductName { get; set; }

        //[TermName("StartDate")]
        //[Display(AutoGenerateField = false)]
        //public DateTime? StartDate { get; set; }

        //[TermName("EndDate")]
        //[Display(AutoGenerateField = false)]
        //public DateTime? EndDate { get; set; }

        [TermName("Active")]
        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(AutoGenerateField = false)]
        public int Quantity { get; set; }

        [Display(AutoGenerateField = false)]
        public int RestricionType { get; set; }

        [Display(AutoGenerateField = false)]
        public string TermName { get; set; }

        [TermName("CUV")]
        [Display(Name = "CUV")]
        public string SKU { get; set; }

        [Display(AutoGenerateField = false)]
        public int StartPeriodID { get; set; }

        [Display(AutoGenerateField = false)]
        public int EndPeriodID { get; set; }

    }

    /// <summary>
    /// Restriction per Account Type
    /// </summary>
    [Serializable]
    public class RestrictionPerAccountType
    {
        public int RestrictionAccountID { get; set; }
        public int RestrictionID { get; set; }
        public int AccountTypeID { get; set; }
    }

    /// <summary>
    /// Restriction per Title
    /// </summary>
    [Serializable]
    public class RestrictionPerTitle
    {
        public int RestrictionTitleID { get; set; }
        public int RestrictionID { get; set; }
        public int TitleID { get; set; }
        public int TitleTypeID { get; set; }
    }


    /// <summary>
    /// Restriction per Person
    /// </summary>
    [Serializable]
    public class RestrictionsControl
    {
        public int RestrictionID { get; set; }
        public int AccountID { get; set; }
        public int Quantity { get; set; }
    }

    [Serializable]
    public class QuotaTypes
    {
        public int QuotaTypeID { get; set; }
        public string TermName { get; set; }
        public string Name { get; set; }
    }

    // <summary>
    /// Restriction per Product
    /// </summary>
    [Serializable]
    public class ProductsRestriction
    {
        public int RestrictionID { get; set; }
		public int AccountTypeID { get; set; }
		public int TitleID { get; set; }
		public int TitleTypeID { get; set; }
		public int TituloPago { get; set; }
		public int TituloCarrera { get; set; }
    }

    [Serializable]
    public class ListProducts
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int QuantityRes { get; set; }
    }
}
