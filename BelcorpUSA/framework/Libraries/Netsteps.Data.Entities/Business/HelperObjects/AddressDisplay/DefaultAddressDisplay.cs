using System;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
	public sealed class DefaultAddressDisplay : BaseAddressDisplay
	{
		public override bool IsMatch(string cultureInfoCode)
		{
			return true;
		}

		public override void DisplayFormat(StringBuilder builder, AddressDisplayModel model)
		{
			if (model.ShowName && !String.IsNullOrWhiteSpace(model.Name))
				builder.Append(model.Name).Append(model.Delimiter);

            if (!String.IsNullOrWhiteSpace(model.Address.Name))
                builder.Append(model.Address.Name).Append(model.Delimiter);
			if (!String.IsNullOrEmpty(model.Address.Address1))
				builder.Append(model.Address.Address1).Append(model.Delimiter);
			if (!String.IsNullOrEmpty(model.Address.Address2))
				builder.Append(model.Address.Address2).Append(model.Delimiter);
			if (!String.IsNullOrEmpty(model.Address.Address3))
                builder.Append(model.Address.Address3).Append(model.Delimiter);
            if (!String.IsNullOrWhiteSpace(model.Address.County))
                builder.Append(" ").Append(model.Address.County).Append(model.Delimiter);
			if (!String.IsNullOrWhiteSpace(model.Address.City))
                builder.Append(" ").Append(model.Address.City).Append(model.Delimiter);
			if(!String.IsNullOrWhiteSpace(model.State))
				builder.Append(" ").Append(model.Address.State);
			if (!String.IsNullOrEmpty(model.Address.PostalCode))
				builder.Append("  ").Append(model.Address.PostalCode);
		}
	}
}
