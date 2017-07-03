namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data.Objects;
    using System.Linq;

    public partial class ApplicationRepository
    {
        protected Func<NetStepsEntities, string, Application> CompiledGetByName
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, string, Application>(
                   (context, name) => (from app in context.Applications
                                       where app.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                       select app).FirstOrDefault()
                );
            }
        }

        public Application GetByName(string key)
        {
            return CompiledGetByName(this.DataContext, key);
        }
    }
}