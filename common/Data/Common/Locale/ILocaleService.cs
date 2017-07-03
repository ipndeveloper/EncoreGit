namespace NetSteps.Data.Common.Locale
{
	using System.Collections.Generic;
	using NetSteps.Data.Common.Entities;

	/// <summary>
	/// The LocaleService interface.
	/// </summary>
	public interface ILocaleService
	{
		/// <summary>
		/// Gets the states provinces.
		/// </summary>
		ICollection<IStateProvince> StatesProvinces { get; }

		/// <summary>
		/// The translate term.
		/// </summary>
		/// <param name="obj">
		/// The object implementing ITermName.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		string TranslateTerm(ITermName obj);
	}
}
