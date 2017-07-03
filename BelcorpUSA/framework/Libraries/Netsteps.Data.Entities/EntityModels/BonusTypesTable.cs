namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// BonusTypes Table on Belcorp Commissions
    /// </summary>
    [Table("BonusTypes")]
    public class BonusTypesTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("BonusTypeID"), Key]
        public int BonusTypeId { get; set; }

        [Column("BonusCode")]
        public string BonusCode { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }

        [Column("Enabled")]
        public bool Enabled { get; set; }

        [Column("Editable")]
        public bool Editable { get; set; }

        [Column("PlanID")]
        public int? PlanId { get; set; }

        [Column("EarningsTypeID")]
        public int? EarningsTypeId { get; set; }

        [Column("ClientName")]
        public string ClientName { get; set; }

        [Column("IsCommission")]
        public bool? IsCommission { get; set; }

        [Column("ClientCode")]
        public string ClientCode { get; set; }
    }
}
