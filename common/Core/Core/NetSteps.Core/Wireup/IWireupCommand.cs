
namespace NetSteps.Encore.Core.Wireup
{
  /// <summary>
  /// Interface for commands executed at wireup time.
  /// </summary>
  public interface IWireupCommand
  {
    /// <summary>
    /// Executes the command.
    /// </summary>
		/// <param name="coordinator">the wireup coordinator</param>
    void Execute(IWireupCoordinator coordinator);
  }
}
