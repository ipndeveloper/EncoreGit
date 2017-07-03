using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using TestMasterHelpProvider.Extensions;

namespace TestMasterHelpProvider.DataFaker
{
	public class LocationFaker
	{
		#region Fields

		private static readonly IList<string> __countryNamesWhichCaseShouldntChange = new List<string>
		{
			"USA"
		};

        private const string ConnectionStringSettingName = "CoreConnectionString";
		private const int UsaCountryId = 1;

		private static SqlQueryManager __queryManager;

		private int _countryId;
		private string _country;
		private string _province;
		private string _provinceCode;
		private string _county;
		private string _postalCode;
		private IList<string> _city;
		private IList<string> _possiblePostalCodes;

		#endregion

		#region Properties

		/// <summary>
		/// Gets a randome street name.
		/// </summary>
		public string StreetName
		{
			get
			{
				string result = String.Empty;

				List<string> streetnames = "Highland, Hill, Park, Woodland, Sunset, Virginia".ToStringList();
				string place = streetnames.GetRandom();

				if (Random.GetBoolean())
				{
					List<string> trees = "Acacia, Beech, Birch, Cedar, Cherry, Chestnut, Elm, Larch, Laurel, Linden, Maple, Oak, Pine, Rose, Walnut, Willow".ToStringList();
					string tree = trees.GetRandom();

					result = tree + " " + place;
				}
				else
				{
					List<string> people = "Adams, Franklin, Jackson, Jefferson, Lincoln, Madison, Washington, Wilson".ToStringList();
					string person = people.GetRandom();

					result = person + " " + place;
				}

				List<string> streettypes = "Way, Drive, Circle, Avenue, Street, Road, Lane".ToStringList();
				result = result + " " + streettypes.GetRandom();

				return result;
			}
		}

		/// <summary>
		/// Gets a random street number.
		/// </summary>
		public int StreetNumber
		{
			get { return Random.Next(1, 50); }
		}

		/// <summary>
		/// Gets a full street address.
		/// </summary>
		public string Street
		{
			get { return String.Format("{0} {1}", StreetNumber, StreetName); }
		}

		/// <summary>
		/// Gets this instance's postal code.
		/// </summary>
		public string PostalCode
		{
			get { return _postalCode; }
		}

		/// <summary>
		/// Gets this instance's country.
		/// </summary>
		public string Country
		{
			get { return _country; }
		}

		/// <summary>
		/// Gets this instance's country ID.
		/// </summary>
		public int CountryId
		{
			get { return _countryId; }
		}

		/// <summary>
		/// Gets this instance's province (called a state in U.S.).
		/// </summary>
		public string Provice
		{
			get { return _province; }
		}

		/// <summary>
		/// Gets the instance's province code - for U.S. this is a two-letter code.
		/// </summary>
		public string ProvinceCode
		{
			get { return _provinceCode; }
		}

		/// <summary>
		/// Gets this instance's county.
		/// </summary>
		public string County
		{
			get { return _county; }
		}

		/// <summary>
		/// Gets this instance's city list. Multiple cities can belong to a single postal code.
		/// </summary>
		public IList<string> City
		{
			get { return _city; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of LocationFaker that is inherently U.S.-based.
		/// </summary>
		public LocationFaker()
		{
			if (LocationFaker.__queryManager == null)
			{
				string connectionStringName = ConfigurationManager.AppSettings[ConnectionStringSettingName];
				ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];

				LocationFaker.__queryManager = SqlQueryManager.GetCoreInstance();
			}

			_possiblePostalCodes = new List<string>();

			InstantiateFakerForCountry(UsaCountryId);
		}

		/// <summary>
		/// Creates an instance of LocationFinder using a specified country.
		/// </summary>
		/// <param name="countryId"></param>
		public LocationFaker(int countryId)
		{
			_countryId = countryId;

			if (LocationFaker.__queryManager == null)
			{
				string connectionStringName = ConfigurationManager.AppSettings[ConnectionStringSettingName];
				ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];

				LocationFaker.__queryManager = SqlQueryManager.GetCoreInstance();
			}

			_possiblePostalCodes = new List<string>();

			InstantiateFakerForCountry(countryId);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Takes care of overall instantiation for this class.
		/// </summary>
		/// <param name="country"></param>
		private void InstantiateFakerForCountry(int countryId)
		{
			string postalCodeQuery = String.Format(@"select pcl.PostalCode from dbo.PostalCodeLookup pcl, dbo.Countries c where pcl.CountryId = c.CountryID	and c.CountryID = {0};", countryId);
			string locationQuery = String.Empty;

			using (DataTable results = LocationFaker.__queryManager.ExecuteQuery(postalCodeQuery))
			{
				foreach (DataRow nextRow in results.Rows)
				{
					string nextZipCode = nextRow["PostalCode"] as string;

					if (countryId == UsaCountryId)
					{
						if (!TestMasterHelpProviderConstants.MilitaryPostalCodeRegex.IsMatch(nextZipCode))
						{
							_possiblePostalCodes.Add(nextZipCode);
						}
					}
					else 
					{
						_possiblePostalCodes.Add(nextZipCode);
					}
				}

				_postalCode = _possiblePostalCodes[(0).GetRandom(_possiblePostalCodes.Count)];

				locationQuery = String.Format(@"select pcl.CityName, pcl.CountyName, pcl.ProvinceName, pcl.ProvinceCode, c.CountryName from dbo.PostalCodeLookup pcl, dbo.Countries c where pcl.CountryId = c.CountryID	and c.CountryID = {0} and pcl.PostalCode = '{1}'", countryId, _postalCode);
			}

			using (DataTable results = LocationFaker.__queryManager.ExecuteQuery(locationQuery))
			{
				int rowCount = results.Rows.Count;

				_country = results.Rows[0]["CountryName"] as string;

				if (!LocationFaker.__countryNamesWhichCaseShouldntChange.Contains(_country))
				{
					_country = _country.CapitalizeAndProperCaseInvariant();
				}
				
				_province = results.Rows[0]["ProvinceName"] as string;
				_provinceCode = results.Rows[0]["ProvinceCode"] as string;
				_county = results.Rows[0]["CountyName"] as string;

				_city = new List<string>();

				foreach (DataRow nextRow in results.Rows)
				{
					_city.Add(nextRow["CityName"] as string);
				}
			}
		}

		#endregion
	}
}