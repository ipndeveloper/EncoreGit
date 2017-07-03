namespace NetSteps.Data.Entities.Dto
{
    using System;

    public class LogCommissionDto
    {
        /// <summary>
        /// Gets or sets Log Id
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Gets or sets Start Time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets End Time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets Result
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
    }
}