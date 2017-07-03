
namespace NetSteps.Data.Entities
{
    public interface INetStepsInformer<T>
    {
        void InformObservers(T observable);
        void AddObserver<U>();
        void RemoveObserver<U>();
    }

		public class NullInformer<T> : INetStepsInformer<T>
		{
			public void InformObservers(T observable)
			{				
			}

			public void AddObserver<U>()
			{				
			}

			public void RemoveObserver<U>()
			{				
			}
		}
}
