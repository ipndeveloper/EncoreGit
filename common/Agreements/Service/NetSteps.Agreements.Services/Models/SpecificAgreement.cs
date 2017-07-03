namespace NetSteps.Agreements.Services.Models
{
	using NetSteps.Agreements.Common.Models;

	/// <summary>
	/// The specific agreement.
	/// </summary>
	public class SpecificAgreement : ISpecificAgreement
	{
		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		public string File { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the version number.
		/// </summary>
		public string VersionNumber { get; set; }

		/// <summary>
		/// Gets or sets the version id.
		/// </summary>
		public int VersionId { get; set; }

		/// <summary>
		/// Gets or sets the term name.
		/// </summary>
		public string TermName { get; set; }

		/// <summary>
		/// Gets or sets the release date UTC.
		/// </summary>
		public System.DateTime ReleaseDateUtc { get; set; }

		/// <summary>
		/// Gets or sets the acceptance log.
		/// </summary>
		public ISpecificAcceptanceLog AcceptanceLog { get; set; }

		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		public int AccountId { get; set; }
	}
}
