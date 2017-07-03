namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IApplicationRepository
    {
        Application GetByName(string key);
    }
}
