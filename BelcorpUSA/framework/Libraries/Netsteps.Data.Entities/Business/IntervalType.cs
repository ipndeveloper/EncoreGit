using System;

namespace NetSteps.Data.Entities
{
    public partial class IntervalType
    {
        public DateTime GetStartOfInterval(DateTime date)
        {
            return BusinessLogic.GetStartOfInterval(date, this);
        }

        public DateTime GetStartOfNextInterval(DateTime date)
        {
            return BusinessLogic.GetStartOfNextInterval(date, this);
        }
    }
}
