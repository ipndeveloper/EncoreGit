using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Web;
using NetSteps.Addresses.Common;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Globalization
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Default implementation of IGeoCodeProvider to lookup Geo Codes.
	/// Created: 06-15-2010
	/// </summary>
	[Serializable]
	[ContainerRegister(typeof(IGeoCodeProvider), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class GoogleGeoCodeProvider : IGeoCodeProvider, IDefaultImplementation
	{
		ICache<string, IGeoCodeData> _cache;

		class GeoCodeDataResolver : DemuxCacheItemResolver<string, IGeoCodeData>
		{
			GoogleGeoCodeProvider _owner;
			public GeoCodeDataResolver(GoogleGeoCodeProvider owner)
			{
				_owner = owner;
			}
			protected override bool DemultiplexedTryResolve(string addrString, out IGeoCodeData value)
			{
				value = _owner.PerformGetGeoCode(addrString);
				return value != null;
			}
		}

		public GoogleGeoCodeProvider()
		{
			_cache = new ActiveMruLocalMemoryCache<string, IGeoCodeData>("GoogleGeoCode", 
				new GeoCodeDataResolver(this), Create.New<ICacheEvictionManager>());
		}
				
		private static int sleepinterval = 200;

		/// <summary>
		/// Basic Implementation of Google Geocoding API - JHE
		/// http://code.google.com/apis/maps/documentation/geocoding/
		/// http://friism.com/c-and-google-geocoding-web-service-v3
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public IGeoCodeData GetGeoCode(IAddressBasic address)
		{
			Contract.Requires<ArgumentNullException>(address != null);

			string addressString = string.Format("{0},{1},{2},{3},{4},{5}", address.Address1, address.Address2, address.Address3, address.City, address.State, address.PostalCode);

			IGeoCodeData result;
			
			// Return if address is empty - JHE
			if (addressString.Replace(",", "").Replace(" ", "").ToCleanString().Trim() == string.Empty)
			{
				result = Create.New<IGeoCodeData>();
				result.Latitude = 0;
				result.Longitude = 0;
				return result;
			}
			
			_cache.TryGet(addressString, out result);
			return result;
		}

		internal IGeoCodeData PerformGetGeoCode(string addressString)
		{			
			Thread.Sleep(sleepinterval);
			GoogleGeoResponse res;
			try
			{
				res = CallGeoWS(addressString);
			}
			catch (Exception e)
			{
				Console.WriteLine("Caught exception: " + e);
				res = null;
			}
			if (res == null || res.Status == "ZERO_RESULTS" || res.Status == "OVER_QUERY_LIMIT")
			{
				// we're hitting Google too fast, increase interval           
				//sleepinterval = Math.Min(sleepinterval + badtries * 1000, 60000);
				//Console.WriteLine("Interval:" + sleepinterval + "\r");
				//return CallWSCount(address, badtries);
			}
			else if (res.Results.CountSafe() > 0)
			{
				// no throttling, go a little bit faster        
				if (sleepinterval > 10000)
					sleepinterval = 200;
				else
					sleepinterval = Math.Max(sleepinterval / 2, 50);

				var geoCodeData = Create.New<IGeoCodeData>();
				geoCodeData.Latitude = res.Results[0].Geometry.Location.Lat;
				geoCodeData.Longitude = res.Results[0].Geometry.Location.Lng;
				return geoCodeData;
			}
			return null;
		}

		private GoogleGeoResponse CallGeoWS(string addressString)
		{
			string url = string.Format("http://maps.google.com/maps/api/geocode/json?address={0}&sensor=false", HttpUtility.UrlEncode(addressString));
			var request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GoogleGeoResponse));
			using (var response = request.GetResponse())
			using (var stream = response.GetResponseStream())
			{
				return (GoogleGeoResponse)serializer.ReadObject(stream);
			}
		}
	}
}
