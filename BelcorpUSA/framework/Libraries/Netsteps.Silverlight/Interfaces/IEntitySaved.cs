
namespace NetSteps.Silverlight
{
    public interface IEntitySaved<T>
    {
        event EntityEventHandler<T> Saved;
    }
}
