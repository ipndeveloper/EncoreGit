using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class PublicationChannelRepository
    {
        public PublicationChannel Load(string number)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.PublicationChannels.
                        Where(x => x.PublicationChannelNumber == number).FirstOrDefault();
                }
            });
        }

        public void Delete(string number)
        {
            var item = Load(number);
            if (item != null)
            {
                Delete(item.PublicationChannelID);
            }
        }
    }
}
