namespace NetSteps.Agreements.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	/// <summary>
	/// The SpecificAgreement interface.
	/// </summary>
	[DTO]
	public interface ISpecificAgreement
	{
		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		string File { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		/// Gets or sets the version number.
		/// </summary>
		string VersionNumber { get; set; }

		/// <summary>
		/// Gets or sets the version id.
		/// </summary>
		int VersionId { get; set; }

		/// <summary>
		/// Gets or sets the term name.
		/// </summary>
		string TermName { get; set; }

		/// <summary>
		/// Gets or sets the release date UTC.
		/// </summary>
		System.DateTime ReleaseDateUtc { get; set; }

		/// <summary>
		/// Gets or sets the acceptance log.
		/// </summary>
		ISpecificAcceptanceLog AcceptanceLog { get; set; }

		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		int AccountId { get; set; }
	}
}
