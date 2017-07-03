namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Plans Table on BelcorpCommissions
    /// </summary>
    [Table("Plans")]
    public class PlansTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PlanID"), Key]
        public int PlanId { get; set; }

        [Column("PlanCode")]
        public string PlanCode { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Enabled")]
        public bool Enabled { get; set; }

        [Column("DefaultPlan")]
        public bool? DefaultPlan { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }
    }
}
