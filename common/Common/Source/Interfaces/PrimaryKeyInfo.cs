
namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 08/18/2010
    /// </summary>
    public class PrimaryKeyInfo<T, TKeyType> : IPrimaryKey<TKeyType>
    {
        public virtual T ParentEntity { get; protected set; }

        public virtual TKeyType PrimaryKey { get { return default(TKeyType); } }

        public virtual string ColumnName { get { return default(string); } }

        public PrimaryKeyInfo(T obj)
        {
            ParentEntity = obj;
        }
    }
}
