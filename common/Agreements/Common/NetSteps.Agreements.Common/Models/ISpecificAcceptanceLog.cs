namespace NetSteps.Agreements.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	/// <summary>
	/// The SpecificAcceptanceLog interface.
	/// </summary>
	[DTO]
	public interface ISpecificAcceptanceLog
	{
		/// <summary>
		/// Gets or sets the date accepted UTC.
		/// </summary>
		System.DateTime DateAcceptedUtc { get; set; }

        /// <summary>
        /// Gets or sets if accepted.
        /// </summary>
        bool Accepted { get; set; }
	}
}
