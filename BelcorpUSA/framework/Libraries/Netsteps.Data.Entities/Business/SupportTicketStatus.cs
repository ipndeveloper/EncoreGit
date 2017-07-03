using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities
{
    public partial class SupportTicketStatus : ISortIndex
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
