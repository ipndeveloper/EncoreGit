using System;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Services;

namespace NetSteps.Data.Entities.Factories
{
    class DomainServicesFactory : IDomainServicesFactory
    {
        readonly IDomainServicesFactory _dataAdapterFactory;

        readonly Lazy<IReportService> _reportService;

        public DomainServicesFactory(IDomainServicesFactory dataAdapterFactory)
        {
            _dataAdapterFactory = dataAdapterFactory;
            _reportService = new Lazy<IReportService>(CreateReportService);
        }

        public IReportService ReportService
        {
            get { return _reportService.Value; }
        }

        private IReportService CreateReportService()
        {
            return new ReportService(_dataAdapterFactory);
        }

    }
}
