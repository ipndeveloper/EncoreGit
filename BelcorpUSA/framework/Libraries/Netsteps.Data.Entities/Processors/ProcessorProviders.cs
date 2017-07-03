using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Processors
{
    public static class ProcessorProviders
    {
        public static IAutoshipProcessor AutoshipProcessor { get { return Create.New<IAutoshipProcessor>(); } }
    }
}
