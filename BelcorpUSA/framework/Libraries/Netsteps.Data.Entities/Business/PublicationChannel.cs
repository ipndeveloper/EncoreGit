
namespace NetSteps.Data.Entities
{
    public partial class PublicationChannel
    {

        public static PublicationChannel Load(string channelIdentifier)
        {
            return Repository.Load(channelIdentifier);
        }

        public static void Delete(string channelIdentifier)
        {
            Repository.Delete(channelIdentifier);
        }
    }
}
