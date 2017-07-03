
namespace NetSteps.Silverlight
{
    public interface IEntityDeleted<T>
    {
        event EntityEventHandler<T> Deleted;
    }
}
