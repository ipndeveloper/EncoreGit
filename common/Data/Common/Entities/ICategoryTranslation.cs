namespace NetSteps.Data.Common.Entities
{
	using NetSteps.Data.Common.Locale;

	/// <summary>
	/// The CategoryTranslation interface.
	/// </summary>
	public interface ICategoryTranslation : ILanguageID
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }
	}
}