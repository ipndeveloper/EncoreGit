using nsDistributor.Models.Shared;

namespace nsDistributor.Models.Locate
{
    public class IndexModel
    {
        public virtual AccountLocatorModel AccountLocator { get; set; }

        public IndexModel()
        {
            AccountLocator = new AccountLocatorModel();
        }
    }
}