
namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Enum of copy kinds.
	/// </summary>
	public enum CopyKind
	{
		/// <summary>
		/// Default. Indicates a loose copy; all properties of source do not have to 
		/// be present on target.
		/// </summary>
		Loose = 0,
		/// <summary>
		/// Indicates a strict copy; all properties of source must be present on target.
		/// </summary>
		Strict = 1
	}
}