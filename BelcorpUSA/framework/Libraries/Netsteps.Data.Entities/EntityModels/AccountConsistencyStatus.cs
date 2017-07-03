namespace NetSteps.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using NetSteps.Data.Entities.Business.Interfaces;
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel;
    using System.Collections.Specialized;
    
    /// <summary>
    /// AccountConsistencyStatuses Table on BelcorpCore
    /// </summary>
    [Table("AccountConsistencyStatuses")]
    public partial class AccountConsistencyStatus
    {
        [Column("AccountConsistencyStatusID"), Key]
        public Int16 AccountConsistencyStatusID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }

        [Column("SortIndex")]
        public int? SortIndex { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("MarketID")]
        public int MarketId { get; set; }
    }
}
