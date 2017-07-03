namespace NetSteps.Data.Common.Entities
{
	using System.Collections.Generic;

	/// <summary>
	/// The Category interface.
	/// </summary>
	public interface ICategory
	{
		/// <summary>
		/// Gets or sets the translations.
		/// </summary>
		ICollection<ICategoryTranslation> AbstractedTranslations { get; set; }

		/// <summary>
		/// Gets or sets the category id.
		/// </summary>
		int CategoryID { get; set; }
	}
}
