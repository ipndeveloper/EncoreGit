using System;
using System.Collections;
using System.Collections.Generic;

namespace NetSteps.Common
{
    [Serializable]
    public class Constants
    {
        public enum ContentOwner
        {
            Site,
            Page
        }

        public enum ViewingMode
        {
            Production,
            Staging,
            Edit,
            Archive,
            Preview
        }

        /// <summary>
        /// Note: can have base replicated site id as value
        /// </summary>
        public enum Client
        {
            UnknownClient,
            Ppi,
            Neways,
            Ctmh = 420,
            TeamEverest,
            RodanAndFields,
            Zrii,
            Reliv,
            Scentsy,
            Synergy,
            Natura,
            Framework,
            ItWorks
        }

        public enum ServerEnviroment
        {
            Unknown,
            DeveloperMachine,
            Test,
            Staging,
            Live
        }

        public enum SortDirection
        {
            Ascending,
            Descending,
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum ControlInputType
        {
            TextBox,
            DropDown,
            DateTime,
            Birthdate,
            Gender
        }

        public enum ControlMode
        {
            Edit,
            Display
        }

        /// <summary>
        /// Key = abbreviation ; Value = full state name
        /// </summary>
        public static Hashtable States
        {
            get
            {
                Hashtable ht = new Hashtable();
                ht.Add("AL", "Alabama");
                ht.Add("AK", "Alaska");
                ht.Add("AZ", "Arizona");
                ht.Add("AR", "Arkansa");
                ht.Add("CA", "California");
                ht.Add("CO", "Colorado");
                ht.Add("CT", "Conneticut");
                ht.Add("DE", "Delaware");
                ht.Add("DC", "District of Columbia");
                ht.Add("FM", "Federal States of Micronesia");
                ht.Add("FL", "Florida");
                ht.Add("GA", "Georgia");
                ht.Add("HI", "Hawaii");
                ht.Add("ID", "Idaho");
                ht.Add("IL", "Illinois");
                ht.Add("IN", "Indiana");
                ht.Add("IA", "Iowa");
                ht.Add("KS", "Kansas");
                ht.Add("KY", "Kentucky");
                ht.Add("LA", "Louisiana");
                ht.Add("ME", "Maine");
                ht.Add("MD", "Maryland");
                ht.Add("MA", "Massachusetts");
                ht.Add("MI", "Michigan");
                ht.Add("MN", "Minnesota");
                ht.Add("MS", "Mississippi");
                ht.Add("MO", "Missouri");
                ht.Add("MT", "Montana");
                ht.Add("NE", "Nebraska");
                ht.Add("NV", "Nevada");
                ht.Add("NH", "New Hamsphire");
                ht.Add("NJ", "New Jersey");
                ht.Add("NM", "New Mexico");
                ht.Add("NY", "New York");
                ht.Add("NC", "North Carolina");
                ht.Add("ND", "North Dakota");
                ht.Add("OH", "Ohio");
                ht.Add("OK", "Oklahoma");
                ht.Add("OR", "Oregon");
                ht.Add("PW", "Palau");
                ht.Add("PA", "Pennsylvania");
                ht.Add("PR", "Puerto Rico");
                ht.Add("RI", "Rhode Island");
                ht.Add("SC", "South Carolina");
                ht.Add("SD", "South Dakota");
                ht.Add("TN", "Tennessee");
                ht.Add("TX", "Texas");
                ht.Add("UT", "Utah");
                ht.Add("VT", "Vermont");
                ht.Add("VA", "Virginia");
                ht.Add("WA", "Washington");
                ht.Add("WV", "West Virginia");
                ht.Add("WI", "Wisconsin");
                ht.Add("WY", "Wyoming");
                return ht;
            }
        }

        /// <summary>
        /// Key = abbreviation ; Value = full state name
        /// </summary>
        public static Dictionary<string, string> StatesAndTerritories_USA_CAN
        {
            get
            {
                Dictionary<string, string> statesAndTerritories = new Dictionary<string, string>();
                statesAndTerritories.Add("AL", "Alabama");
                statesAndTerritories.Add("AK", "Alaska");
                statesAndTerritories.Add("AZ", "Arizona");
                statesAndTerritories.Add("AR", "Arkansa");
                statesAndTerritories.Add("CA", "California");
                statesAndTerritories.Add("CO", "Colorado");
                statesAndTerritories.Add("CT", "Conneticut");
                statesAndTerritories.Add("DE", "Delaware");
                statesAndTerritories.Add("DC", "District of Columbia");
                statesAndTerritories.Add("FL", "Florida");
                statesAndTerritories.Add("GA", "Georgia");
                statesAndTerritories.Add("HI", "Hawaii");
                statesAndTerritories.Add("ID", "Idaho");
                statesAndTerritories.Add("IL", "Illinois");
                statesAndTerritories.Add("IN", "Indiana");
                statesAndTerritories.Add("IA", "Iowa");
                statesAndTerritories.Add("KS", "Kansas");
                statesAndTerritories.Add("KY", "Kentucky");
                statesAndTerritories.Add("LA", "Louisiana");
                statesAndTerritories.Add("ME", "Maine");
                statesAndTerritories.Add("MD", "Maryland");
                statesAndTerritories.Add("MA", "Massachusetts");
                statesAndTerritories.Add("MI", "Michigan");
                statesAndTerritories.Add("MN", "Minnesota");
                statesAndTerritories.Add("MS", "Mississippi");
                statesAndTerritories.Add("MO", "Missouri");
                statesAndTerritories.Add("MT", "Montana");
                statesAndTerritories.Add("NE", "Nebraska");
                statesAndTerritories.Add("NV", "Nevada");
                statesAndTerritories.Add("NH", "New Hamsphire");
                statesAndTerritories.Add("NJ", "New Jersey");
                statesAndTerritories.Add("NM", "New Mexico");
                statesAndTerritories.Add("NY", "New York");
                statesAndTerritories.Add("NC", "North Carolina");
                statesAndTerritories.Add("ND", "North Dakota");
                statesAndTerritories.Add("OH", "Ohio");
                statesAndTerritories.Add("OK", "Oklahoma");
                statesAndTerritories.Add("OR", "Oregon");
                statesAndTerritories.Add("PA", "Pennsylvania");
                statesAndTerritories.Add("RI", "Rhode Island");
                statesAndTerritories.Add("SC", "South Carolina");
                statesAndTerritories.Add("SD", "South Dakota");
                statesAndTerritories.Add("TN", "Tennessee");
                statesAndTerritories.Add("TX", "Texas");
                statesAndTerritories.Add("UT", "Utah");
                statesAndTerritories.Add("VT", "Vermont");
                statesAndTerritories.Add("VA", "Virginia");
                statesAndTerritories.Add("WA", "Washington");
                statesAndTerritories.Add("WV", "West Virginia");
                statesAndTerritories.Add("WI", "Wisconsin");
                statesAndTerritories.Add("WY", "Wyoming");
                statesAndTerritories.Add("AS", "American Samoa");
                statesAndTerritories.Add("GU", "Guam");
                statesAndTerritories.Add("MP", "Northern Mariana Islands");
                statesAndTerritories.Add("PR", "Puerto Rico");
                statesAndTerritories.Add("VI", "Virgin Islands");
                statesAndTerritories.Add("FM", "Federal States of Micronesia");
                statesAndTerritories.Add("MH", "Marshall Islands");
                statesAndTerritories.Add("PW", "Palau");
                statesAndTerritories.Add("AA", "Armed Forces Americas");
                statesAndTerritories.Add("AE", "Armed Forces");
                statesAndTerritories.Add("AP", "Armed Forces Pacific");
                statesAndTerritories.Add("AB", "Alberta");
                statesAndTerritories.Add("BC", "British Columbia");
                statesAndTerritories.Add("MB", "Manitoba");
                statesAndTerritories.Add("NB", "New Brunswick");
                statesAndTerritories.Add("NL", "Newfoundland");
                statesAndTerritories.Add("NS", "Nova Scotia");
                statesAndTerritories.Add("NT", "Northwest Territories");
                statesAndTerritories.Add("NU", "Nunavut");
                statesAndTerritories.Add("ON", "Ontario");
                statesAndTerritories.Add("PE", "Prince Edward Island");
                statesAndTerritories.Add("QC", "Quebec");
                statesAndTerritories.Add("SK", "Saskatchewan");
                statesAndTerritories.Add("YT", "Yukon");
                return statesAndTerritories;
            }
        }


        public enum MonthDisplayType
        {
            NumberOnly = 1,
            Acronym,
            FullName
        }

        public enum ApplicationMessageType
        {
            Standard,
            Warning,
            Error,
            Successful
        }

        public enum ImageFolder
        {
            Categories,
            Products,
            MyPhotos, //For the My Photo section of PWS
            CorporatePhotos //For corporate uploads from PWS through ckfinder
        }

        public enum FileType
        {
            PDF,
            Audio,
            Flash,
            Image,
            Video,
            Powerpoint,
            Word,
            Excel,
            Archive,
            Unknown
        }

        public enum ShippingCalculationMode
        {
            Weight,
            Cost
        }

        public const string BEGIN_TOKEN_DELIMITER = "{{";
        public const string END_TOKEN_DELIMITER = "}}";

        public enum IsHttpsReturnStatus
        {
            Unknown = 0,
            IsHttps = 1,
            IsNotHttps = 2,
        }

		public enum TokenValueProviderType
		{
			Cms = 0,
		}
    }
}
