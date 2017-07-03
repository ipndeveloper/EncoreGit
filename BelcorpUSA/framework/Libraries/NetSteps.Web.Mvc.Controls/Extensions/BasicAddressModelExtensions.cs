using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Controls.Models.Shared;

namespace NetSteps.Web.Mvc.Controls
{
    public static class BasicAddressModelExtensions
    {
        public static BasicAddressModel  ToBasicAddressModel(this Address address)
        {
            var basicAddressModel = new BasicAddressModel();
            return basicAddressModel.LoadValues(address.CountryID, address);
        }
    }
}
