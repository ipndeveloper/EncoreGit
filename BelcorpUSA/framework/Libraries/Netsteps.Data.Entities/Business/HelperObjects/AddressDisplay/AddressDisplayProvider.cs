using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
	public class AddressDisplayProvider : IAddressDisplayProvider
	{
		private readonly List<BaseAddressDisplay> _addressDisplays;

		public AddressDisplayProvider()
		{
			_addressDisplays = RegisteredAddressDisplays();
		}

		/// <summary>
		/// Register your new address display classes here and it will
		/// be picked up automatically.
		/// </summary>
		private static List<BaseAddressDisplay> RegisteredAddressDisplays()
		{
			return new List<BaseAddressDisplay>
            {
                new UnitedStatesAddressDisplay(),
                new CanadaAddressDisplay(),
                new GreatBritainAddressDisplay(),
                new AustraliaAddressDisplay(),
                new IrelandAddressDisplay(),
                new SwedenAddressDisplay(),
                new NetherLandsAddressDisplay(),
                new BelgiumAddressDisplay(),
                new JapanAddressDisplay()
            };

		}


		public StringBuilder CountryAddressDisplayFormat(StringBuilder builder, AddressDisplayModel addressDisplayModel)
		{
			string cultureInfo = addressDisplayModel.Country.CultureInfo ?? string.Empty;

			BaseAddressDisplay display = _addressDisplays.FirstOrDefault(a => a.IsMatch(cultureInfo));

			if (display == null) display = new DefaultAddressDisplay();

			return display.DisplayFormattedAddress(builder, addressDisplayModel);
		}

	}
}
