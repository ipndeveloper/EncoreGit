using NetSteps.Communication.UI.Common;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    public interface IContentService<T>
    {
        T SetDisplayValues(T model);
    }

    public class PromotionStuff : IContentService<IAccountAlertMessageModel>
    {
        public IAlertInfo SetDisplayValues(IAlertInfo model)
        {
            throw new System.NotImplementedException();
        }

        public IAccountAlertMessageModel SetDisplayValues(IAccountAlertMessageModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}