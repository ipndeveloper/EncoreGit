namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("Titles")]
    public class TitleTable
    {
        /// <summary>
        /// Gets or sets Title Id
        /// </summary>
        [Column("TitleID"), Key]
        public int TitleId { get; set; }

        /// <summary>
        /// Gets or sets Title Code
        /// </summary>
        [Column("TitleCode")]
        public string TitleCode { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        [Column("TermName")]
        public string TermName { get; set; }

        /// <summary>
        /// Gets or sets Sort Order
        /// </summary>
        [Column("SortOrder")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wheter is Active
        /// </summary>
        [Column("Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets Client Code
        /// </summary>
        [Column("ClientCode")]
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets Client Name
        /// </summary>
        [Column("ClientName")]
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has Report Visibility
        /// </summary>
        [Column("ReportVisibility")]
        public bool ReportVisibility { get; set; }
    }
}
