namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Used with Exceptions to mark the exception logged to the DB.
    /// Created: 02-05-2010
    /// </summary>
    public interface IHasBeenLogged
    {
        bool HasBeenLogged { get; set; }
    }
}
