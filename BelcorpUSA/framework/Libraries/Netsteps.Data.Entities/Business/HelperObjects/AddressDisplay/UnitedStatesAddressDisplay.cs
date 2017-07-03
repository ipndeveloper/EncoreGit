using System;
using System.Text;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
    public class UnitedStatesAddressDisplay : BaseAddressDisplay
    {
        public override bool IsMatch(string cultureInfoCode)
        {
            return cultureInfoCode.EndsWith("US", StringComparison.OrdinalIgnoreCase);
        }

        public override void DisplayFormat(StringBuilder builder, AddressDisplayModel model)
        {
            if (model.ShowName && !model.Name.IsNullOrEmpty())
                builder.Append(model.Name).Append(model.Delimiter);

            if (!string.IsNullOrEmpty(model.Address.Address1))
                builder.Append(model.Address.Address1).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.Address2))
                builder.Append(model.Address.Address2).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.Address3))
                builder.Append(model.Address.Address3).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.City) || !string.IsNullOrEmpty(model.Address.State) || (model.Address.StateProvinceID.HasValue && model.Address.StateProvinceID > 0) || !string.IsNullOrEmpty(model.Address.PostalCode))
                builder.Append(model.Address.City).Append(!model.Address.City.IsNullOrEmpty() ? ", " : string.Empty).Append(model.State).Append(!model.Address.City.IsNullOrEmpty() ? " " : string.Empty).Append(model.Address.PostalCode != null ? model.Address.PostalCode.ZipCode() : "");
        }

    }

}
