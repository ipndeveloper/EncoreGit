namespace NetSteps.Data.Entities.Globalization
{
	using System;

	using NetSteps.Common.Globalization;
	using NetSteps.Encore.Core.IoC;

	/// <summary>
	/// Author: John Egbert
	/// Description: Class to easily access all the Globalization 'providers' to make the various location lookups.
	/// Created: 03-24-2010
	/// </summary>
	public class LocationProviders
	{
		/// <summary>
		/// Gets the postal code lookup provider.
		/// </summary>
		[Obsolete("Use Create.New<IPostalCodeLookupProvider>() when and where you need one of these.")]
		public static IPostalCodeLookupProvider PostalCodeLookupProvider
		{
			get
			{
				return Create.New<IPostalCodeLookupProvider>();
			}
		}
	}
}