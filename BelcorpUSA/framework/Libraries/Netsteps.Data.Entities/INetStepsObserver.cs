
namespace NetSteps.Data.Entities
{
    public interface INetStepsObserver<T>
    {
        void Inform(T obj);
    }
}
