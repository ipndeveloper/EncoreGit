using FubuTestingSupport;
using StructureMap.Pipeline;

namespace Testing.Repository
{
    /// <summary>
    /// Used with fubu testing support to mock contexts helps with the setting up of a mock
    ///specifically dealing with open generics
    ///if you are not dealing with an open generic class
    ///like our Specification<typeparam name="T">T</typeparam>
    ///and you are just testing a real class
    ///like ExampleController this helper is unecessary
    /// </summary>
    /// <typeparam name="T">ClassUnderTest</typeparam>
    public class GenericInteractionContext<T> : InteractionContext<T> where T : class
    {
        protected override void beforeEach()
        {
            Container.Configure(x =>
            {
                var instance = x.For<T>().Use<T>();
                ConstructUsing(instance);
            });
        }

        public virtual void ConstructUsing(SmartInstance<T> instance)
        {
        }
    }
}