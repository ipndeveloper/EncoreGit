namespace NetSteps.Data.Common.Locale
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Interface for Entities with a TermName property.
	/// Used to more easily translate entity names.
	/// Created: 06-17-2010
	/// </summary>
	public interface ITermName
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the term name.
		/// </summary>
		string TermName { get; set; }
	}
}
