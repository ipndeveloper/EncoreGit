using System;
using System.Text;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
    public class SwedenAddressDisplay : BaseAddressDisplay
    {
        public override bool IsMatch(string cultureInfoCode)
        {
            return cultureInfoCode.EndsWith("SE", StringComparison.OrdinalIgnoreCase);
        }

        public override void DisplayFormat(StringBuilder builder, AddressDisplayModel model)
        {
            if (model.ShowName && !model.Name.IsNullOrEmpty())
                builder.Append(model.Name).Append(model.Delimiter);

            if (!string.IsNullOrEmpty(model.Address.Address1))
                builder.Append(model.Address.Address1).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.Address2))
                builder.Append(model.Address.Address2).Append(model.Delimiter);
            if (!string.IsNullOrEmpty(model.Address.PostalCode) || !string.IsNullOrEmpty(model.Address.City))
                builder.Append(model.Address.PostalCode).Append("  ").Append(" ").Append(model.Address.City);
        }

    }
}
