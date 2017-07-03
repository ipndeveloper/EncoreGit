namespace NetSteps.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using NetSteps.Data.Entities.Business.Interfaces;
    using System.Runtime.Serialization;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// ActivityStatuses Table on BelcorpCore
    /// </summary>
    [Table("ActivityStatuses")]
    public partial class ActivityStatus
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ActivityStatusID"), Key]
        public Int16 ActivityStatusId { get; set; }

        [Column("InternalName")]
        public string InternalName { get; set; }

        [Column("ExternalName")]
        public string Name { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }

        [Column("CampaignsWithoutOrder")]
        public string CampaignsWithoutOrder { get; set; }

        [Column("AccountStatusID")]
        public Int16 AccountStatusId { get; set; }

        [Column("ActivityDescripcion")]
        public string ActivityDescription { get; set; }

        [Column("SortIndex")]
        public int? SortIndex { get; set; }

        [Column("MarketID")]
        public int MarketId { get; set; }

        [Column("IsActive")]
        public bool? IsActive { get; set; }
    }
}
