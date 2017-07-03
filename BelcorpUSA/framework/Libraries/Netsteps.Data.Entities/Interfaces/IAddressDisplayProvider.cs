using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay;

namespace NetSteps.Data.Entities.Interfaces
{
    public interface IAddressDisplayProvider
    {
        StringBuilder CountryAddressDisplayFormat(StringBuilder builder, AddressDisplayModel addressDisplayModel);
    }
}
