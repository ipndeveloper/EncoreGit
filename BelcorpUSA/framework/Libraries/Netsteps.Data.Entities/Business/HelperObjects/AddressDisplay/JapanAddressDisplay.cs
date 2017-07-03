using System;
using System.Text;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
    public class JapanAddressDisplay : BaseAddressDisplay
    {
        public override bool IsMatch(string cultureInfoCode)
        {
            return cultureInfoCode.EndsWith("JP", StringComparison.OrdinalIgnoreCase);
        }

        public override void DisplayFormat(StringBuilder builder, AddressDisplayModel model)
        {
            if (!string.IsNullOrEmpty(model.Address.PostalCode))
                builder.Append(model.Address.PostalCode).Append(model.Delimiter);

            if (!string.IsNullOrEmpty(model.Address.State) || (model.Address.StateProvinceID.HasValue && model.Address.StateProvinceID > 0))
                builder.Append(model.State);

            if (!string.IsNullOrEmpty(model.Address.City))
                builder.Append(model.Address.City).Append(model.Delimiter);

            if (!string.IsNullOrEmpty(model.Address.Address1))
                builder.Append(model.Address.Address1).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.Address2))
                builder.Append(model.Address.Address2).Append(model.Delimiter);

            if (model.ShowName && !model.Name.IsNullOrEmpty())
                builder.Append(model.Name).Append(model.Delimiter);
        }

    }
}
