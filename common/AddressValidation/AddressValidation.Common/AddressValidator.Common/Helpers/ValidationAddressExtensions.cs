using AddressValidator.Common;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Common.Data
{
    public static class IValidationAddressExtensions
    {
        public static IValidationAddress ToValidationAddress(this IAddress address)
        {
            using (var container = Create.SharedOrNewContainer())
            {
                var validationAddress = Copy<IValidationAddress>.From(container, address, CopyKind.Loose);
                validationAddress.MainDivision = address.State;
                validationAddress.SubDivision = address.City;
                return validationAddress;
            }
        }
    }
}
