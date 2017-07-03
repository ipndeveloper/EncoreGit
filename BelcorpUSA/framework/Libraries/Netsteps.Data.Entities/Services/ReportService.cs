using System.Linq;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories.Interfaces;

namespace NetSteps.Data.Entities.Services
{
    public class ReportService : IReportService
    {
        private readonly IDomainServicesFactory _dataAdapterFactory;

        public ReportService(IDomainServicesFactory dataAdapterFactory)
        {
            _dataAdapterFactory = dataAdapterFactory;
        }

        public void ResetCache()
        {
            SmallCollectionCache.ExpireReportsCategories();
        }

        public static ReportCategory GetCategory(int? id)
        {
            return SmallCollectionCache.ReportCategories.FirstOrDefault(m => m.ReportCategoryID == id);
        }
    }
}
