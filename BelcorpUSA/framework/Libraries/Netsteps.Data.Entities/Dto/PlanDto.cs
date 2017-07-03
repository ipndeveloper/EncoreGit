namespace NetSteps.Data.Entities.Dto
{
    /// <summary>
    /// Plan Data Transfer Object
    /// </summary>
    public class PlanDto
    {
        /// <summary>
        /// Gets or sets plan Id
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Gets or sets plan code
        /// </summary>
        public string PlanCode { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether plan is default
        /// </summary>
        public bool? DefaultPlan { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        public string TermName { get; set; }
    }
}
