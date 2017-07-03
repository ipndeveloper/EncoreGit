namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Interface for named registrations.
	/// </summary>
	public interface INamedRegistration
	{
		/// <summary>
		/// Gets the registration's name.
		/// </summary>
		string Name { get; }
	}
}
