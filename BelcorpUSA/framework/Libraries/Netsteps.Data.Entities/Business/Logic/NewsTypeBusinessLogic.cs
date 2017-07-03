using System;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class NewsTypeBusinessLogic
    {
        public override void Delete(INewsTypeRepository repository, Int16 newsTypeID)
        {
            repository.Delete(newsTypeID);
        }
    }
}
