namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;
    using System;

    /// <summary>
    /// Activities Table on BelcorpCommissions
    /// </summary>
    [Table("Activities")]
    public class ActivityTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ActivityID"), Key]
        public long ActivityID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("ActivityStatusID")]
        public short ActivityStatusID { get; set; }

        [Column("PeriodID")]
        public int PeriodID { get; set; }

        [Column("IsQualified")]
        public bool IsQualified { get; set; }

        //INI - GR_Encore-07
        [Column("AccountConsistencyStatusID")]
        public Int16 AccountConsistencyStatusID { get; set; }

        [Column("HasContinuity")]
        public bool HasContinuity { get; set; }
        //FIN - GR_Encore-07
    }
}