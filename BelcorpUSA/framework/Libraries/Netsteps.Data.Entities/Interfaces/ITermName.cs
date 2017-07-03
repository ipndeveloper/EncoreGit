
namespace NetSteps.Data.Entities.Business.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Interface for Entities with a TermName property.
    /// Used to more easily translate entity names.
    /// Created: 06-17-2010
    /// </summary>
    public interface ITermName
    {
        string Name { get; set; }
        string TermName { get; set; }
    }
}
