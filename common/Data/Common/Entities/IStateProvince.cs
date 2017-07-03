namespace NetSteps.Data.Common.Entities
{
	/// <summary>
	/// The StateProvince interface.
	/// </summary>
	public interface IStateProvince
	{
		/// <summary>
		/// Gets or sets the country id.
		/// </summary>
		int CountryID { get; set; }

		/// <summary>
		/// Gets or sets the state abbreviation.
		/// </summary>
		string StateAbbreviation { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the state province id.
		/// </summary>
		int StateProvinceID { get; set; }
	}
}
