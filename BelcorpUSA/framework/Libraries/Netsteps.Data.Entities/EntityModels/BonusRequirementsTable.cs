namespace NetSteps.Data.Entities.EntityModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Table BonusRequirements on BelcorpCommissions
    /// </summary>
    [Table("BonusRequirements")]
    public class BonusRequirementsTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("BonusRequirementsID"), Key]
        public int BonusRequirementsId { get; set; }

        [Column("BonusTypeID")]
        public int BonusTypeId { get; set; }

        //[Column("RequirementsTreeId")]
        //public Hierarchy RequirementsTreeId { get; set; }

        [Column("BonusAmount")]
        public decimal? BonusAmount { get; set; }

        [Column("BonusPercent")]
        public decimal? BonusPercent { get; set; }

        [Column("BonusMaxAmount")]
        public decimal? BonusMaxAmount { get; set; }

        [Column("BonusMaxPercent")]
        public decimal? BonusMaxPercent { get; set; }

        [Column("MinTitleID")]
        public int? MinTitleId { get; set; }

        [Column("MaxTitleID")]
        public int? MaxTitleId { get; set; }

        [Column("BonusMinAmount")]
        public int? BonusMinAmount { get; set; }

        [Column("PayMonth")]
        public int? PayMonth { get; set; }

        [Column("CountryID")]
        public int CountryId { get; set; }

        [Column("CurrencyTypeID")]
        public int CurrencyTypeId { get; set; }

        [Column("EffectiveDate")]
        public DateTime EffectiveDate { get; set; }
    }
}
