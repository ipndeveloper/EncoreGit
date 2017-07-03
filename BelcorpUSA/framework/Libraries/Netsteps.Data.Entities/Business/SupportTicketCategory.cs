using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities
{
    public partial class SupportTicketCategory : ISortIndex
    {
        #region ISortIndex Members

        int ISortIndex.SortIndex
        {
            get
            {
                return SortIndex;
            }
            set
            {
                SortIndex = value.ToByte();
            }
        }

        #endregion
    }
}
