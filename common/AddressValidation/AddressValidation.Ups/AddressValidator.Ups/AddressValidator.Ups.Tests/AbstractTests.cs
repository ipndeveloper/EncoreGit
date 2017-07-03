using NetSteps.Encore.Core.IoC;

namespace AddressValidator.Ups.Tests
{
    public abstract class AbstractTests
    {
        public static IContainer Container
        {
            get { return Create.SharedOrNewContainer(); }
        }
    }
}
