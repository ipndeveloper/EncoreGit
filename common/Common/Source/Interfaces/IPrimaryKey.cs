namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: For use with Business Objects to read an objects single primary key.
    /// Created: 03-11-2010
    /// </summary>
    public interface IPrimaryKey<TKeyType>
    {
        TKeyType PrimaryKey { get; }
    }
}
