namespace NetSteps.Data.Entities.Dto
{
    public class TitleDto
    {
        /// <summary>
        /// Gets or sets Title Id
        /// </summary>
        public int TitleId { get; set; }

        /// <summary>
        /// Gets or sets Title Code
        /// </summary>
        public string TitleCode { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// Gets or sets Sort Order
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets Client Code
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets Client Name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets Report Visibility
        /// </summary>
        public bool ReportVisibility { get; set; }
    }
}
