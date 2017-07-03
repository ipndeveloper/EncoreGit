using nsDistributor.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Sponsor
{
    public class BrowseModel
    {
        public int? BusquedaAutomaticaBrasil { get; set; }
        public virtual AccountLocatorModel AccountLocator { get; set; }

        public BrowseModel()
        {
            AccountLocator = new AccountLocatorModel();
        }
    }
}