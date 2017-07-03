using System.Text;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
    public abstract class BaseAddressDisplay
    {
        public abstract bool IsMatch(string cultureInfoCode);

        public abstract void DisplayFormat(StringBuilder builder, AddressDisplayModel model);

        public StringBuilder DisplayFormattedAddress(StringBuilder builder, AddressDisplayModel displayModel)
        {
            ProfileName(builder, displayModel);

            DisplayFormat(builder, displayModel);

            CountryAndPhone(builder, displayModel);

            ShipToEmail(builder, displayModel);

            return builder;
        }

        public virtual void ProfileName(StringBuilder builder, AddressDisplayModel displayModel)
        {
            if (displayModel.ShowProfileName && !displayModel.Address.ProfileName.IsNullOrEmpty())
                builder.Append(displayModel.Address.ProfileName).Append(displayModel.Delimiter);
        }

        public virtual void CountryAndPhone(StringBuilder builder, AddressDisplayModel displayModel)
        {
            if (displayModel.ShowCountry)
                builder.Append(displayModel.Delimiter).Append(displayModel.Country.Name);

            if (displayModel.ShowPhone && !string.IsNullOrEmpty(displayModel.Address.PhoneNumber))
                builder.Append(displayModel.Delimiter).Append(Account.DisplayPhone(displayModel.Address.PhoneNumber, displayModel.Country.CultureInfo));
        }

        public virtual void ShipToEmail(StringBuilder builder, AddressDisplayModel displayModel)
        {
            var addressEntity = displayModel.Address as IShippingAddress;
            
            if (addressEntity != null && displayModel.ShowShipToEmail)
            {
                var addressEmail = addressEntity.ShipToEmailAddress;
                if (!string.IsNullOrEmpty(addressEmail))
                {
                    builder.Append(displayModel.Delimiter).Append(addressEmail);
                }
            }
        }




    }
}