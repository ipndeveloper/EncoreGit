namespace NetSteps.Data.Entities.EntityModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Table("LogCommissions")]
    public class LogCommissionTable
    {
        /// <summary>
        /// Gets or sets Log Id
        /// </summary>
        [Column("LogId"), Key]
        public int LogId { get; set; }

        /// <summary>
        /// Gets or sets Start Time
        /// </summary>
        [Column("StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets End Time
        /// </summary>
        [Column("EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets Result
        /// </summary>
        [Column("Result")]
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }
    }
}