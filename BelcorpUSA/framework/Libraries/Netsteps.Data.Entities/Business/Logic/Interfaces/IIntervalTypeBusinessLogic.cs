using System;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IIntervalTypeBusinessLogic
    {
        DateTime GetStartOfInterval(DateTime date, IntervalType intervalType);
        DateTime GetStartOfNextInterval(DateTime date, IntervalType intervalType);
    }
}
