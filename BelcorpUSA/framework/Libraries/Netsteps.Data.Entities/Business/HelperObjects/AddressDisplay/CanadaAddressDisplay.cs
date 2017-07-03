using System;
using System.Text;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
    public class CanadaAddressDisplay : BaseAddressDisplay
    {
        public override bool IsMatch(string cultureInfoCode)
        {
            return cultureInfoCode.EndsWith("CA", StringComparison.OrdinalIgnoreCase);
        }

        public override void DisplayFormat(StringBuilder builder, AddressDisplayModel model)
        {
            if (model.ShowName && !model.Name.IsNullOrEmpty())
                builder.Append(model.Name).Append(model.Delimiter);

            builder.Append(model.Address.Address1).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.Address2))
                builder.Append(model.Address.Address2).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.Address3))
                builder.Append(model.Address.Address3).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.City) || !string.IsNullOrEmpty(model.Address.State) || (model.Address.StateProvinceID.HasValue && model.Address.StateProvinceID > 0) || !string.IsNullOrEmpty(model.Address.PostalCode))
                builder.Append(model.Address.City).Append(" ").Append(model.State).Append(" ").Append(model.Address.PostalCode);
        }
    }

}
