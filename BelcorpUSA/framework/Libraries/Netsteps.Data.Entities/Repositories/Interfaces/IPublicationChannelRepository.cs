
namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IPublicationChannelRepository
    {
        PublicationChannel Load(string number);
        void Delete(string number);
    }
}
