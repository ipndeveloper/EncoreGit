using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
    public partial class Market
    {
        #region Methods
        public static List<Market> LoadAllBySiteIDAndUserID(int siteID, int userID)
        {
            var list = Repository.LoadAllBySiteIDAndUserID(siteID, userID);
            foreach (var item in list)
            {
                item.StartTracking();
                item.IsLazyLoadingEnabled = true;
            }
            return list;
        }
        #endregion
    }
}
