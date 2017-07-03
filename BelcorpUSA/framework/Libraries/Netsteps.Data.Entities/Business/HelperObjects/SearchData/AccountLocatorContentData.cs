using System.Web;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Entities.Business
{
    [DTO]
    public interface IAccountLocatorContentData
    {
        int AccountID { get; }
        string PwsUrl { get; }
        IHtmlString PhotoContent { get; }
    }

    public class AccountLocatorContentData : IAccountLocatorContentData
    {
        public int AccountID { get; set; }
        public string PwsUrl { get; set; }
        public IHtmlString PhotoContent { get; set; }
    }
}
