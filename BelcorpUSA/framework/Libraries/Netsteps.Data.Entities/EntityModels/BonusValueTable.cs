namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;
    using System;

    /// <summary>
    /// BonusValues Table on BelcorpCommissions
    /// </summary>
    [Table("BonusValues")]
    public class BonusValueTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("BonusValueID"), Key]
        public int BonusValueID { get; set; }

        [Column("BonusTypeID")]
        public int BonusTypeID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("BonusAmount")]
        public decimal? BonusAmount { get; set; }

        [Column("CurrencyTypeID")]
        public int CurrencyTypeID { get; set; }

        [Column("CorpBonusAmount")]
        public decimal? CorpBonusAmount { get; set; }

        [Column("CorpCurrencyTypeID")]
        public int? CorpCurrencyTypeID { get; set; }

        [Column("PeriodID")]
        public int PeriodID { get; set; }

        [Column("CountryID")]
        public int? CountryID { get; set; }

        [Column("DateModified")]
        public DateTime DateModified { get; set; }
    }
}