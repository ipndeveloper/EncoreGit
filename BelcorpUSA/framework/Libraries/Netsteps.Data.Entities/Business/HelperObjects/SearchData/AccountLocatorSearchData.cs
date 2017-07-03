using System.Web;
using NetSteps.Common.Globalization;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Entities.Business
{
    [DTO]
    public interface IAccountLocatorSearchData
    {
        int AccountID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string City { get; set; }
        string State { get; set; }
        int? CountryID { get; set; }
        double? Distance { get; set; }
        GeoLocation.DistanceType DistanceType { get; set; }
        string PwsUrl { get; set; }
        IHtmlString PhotoContent { get; set; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }

    }

    public class AccountLocatorSearchData : IAccountLocatorSearchData
    {
        public int AccountID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? CountryID { get; set; }
        public double? Distance { get; set; }
        public GeoLocation.DistanceType DistanceType { get; set; }
        public string PwsUrl { get; set; }
        public IHtmlString PhotoContent { get; set; }
        public string EmailAddress { get; set;}
        public string PhoneNumber { get; set; }
    }
}
