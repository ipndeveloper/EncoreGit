namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IDomainServicesFactory
    {
        IReportService ReportService { get; }
    }
}