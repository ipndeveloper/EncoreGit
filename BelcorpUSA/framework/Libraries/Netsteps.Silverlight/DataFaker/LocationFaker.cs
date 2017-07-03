using System.Collections.Generic;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.DataFaker
{
    public static class LocationFaker
    {
        private static List<string> _countries = "Afghanistan, Albania, Algeria, American Samoa, Andorra, Angola, Antigua and Barbuda, Argentina, Armenia, Aruba, Australia, Austria, Azerbaijan, Bahamas, The, Bahrain, Bangladesh, Barbados, Belarus, Belgium, Belize, Benin, Bermuda, Bhutan, Bolivia, Bosnia and Herzegovina, Botswana, Brazil, Brunei Darussalam, Bulgaria, Burkina Faso, Burundi, Cambodia, Cameroon, Canada, Cape Verde, Cayman Islands, Central African Republic, Chad, Channel Islands, Chile, China, Colombia, Comoros, Congo, Dem. Rep., Congo, Rep., Costa Rica, Côte d'Ivoire, Croatia, Cuba, Cyprus, Czech Republic, Denmark, Djibouti, Dominica, Dominican Republic, Ecuador, Egypt, Arab Rep., El Salvador, Equatorial Guinea, Eritrea, Estonia, Ethiopia, Faeroe Islands, Fiji, Finland, France, French Polynesia, Gabon, Gambia, The, Georgia, Germany, Ghana, Greece, Greenland, Grenada, Guam, Guatemala, Guinea, Guinea-Bissau, Guyana, Haiti, Honduras, Hong Kong, China, Hungary, Iceland, India, Indonesia, Iran, Islamic Rep., Iraq, Ireland, Isle of Man, Israel, Italy, Jamaica, Japan, Jordan, Kazakhstan, Kenya, Kiribati, Korea, Dem. Rep., Korea, Rep., Kuwait, Kyrgyz Republic, Lao PDR, Latvia, Lebanon, Lesotho, Liberia, Libya, Liechtenstein, Lithuania, Luxembourg, Macao, China, Macedonia, FYR, Madagascar, Malawi, Malaysia, Maldives, Mali, Malta, Marshall Islands, Mauritania, Mauritius, Mayotte, Mexico, Micronesia, Fed. Sts., Moldova, Monaco, Mongolia, Montenegro, Morocco, Mozambique, Myanmar, Namibia, Nepal, Netherlands, Netherlands Antilles, New Caledonia, New Zealand, Nicaragua, Niger, Nigeria, Northern Mariana Islands, Norway, Oman, Pakistan, Palau, Panama, Papua New Guinea, Paraguay, Peru, Philippines, Poland, Portugal, Puerto Rico, Qatar, Romania, Russian Federation, Rwanda, Samoa, San Marino, São Tomé and Principe, Saudi Arabia, Senegal, Serbia, Seychelles, Sierra Leone, Singapore, Slovak Republic, Slovenia, Solomon Islands, Somalia, South Africa, Spain, Sri Lanka, St. Kitts and Nevis, St. Lucia, St. Vincent and the Grenadines, Sudan, Suriname, Swaziland, Sweden, Switzerland, Syrian Arab Republic, Tajikistan, Tanzania, Thailand, Timor-Leste, Togo, Tonga, Trinidad and Tobago, Tunisia, Turkey, Turkmenistan, Uganda, Ukraine, United Arab Emirates, United Kingdom, United States, Uruguay, Uzbekistan, Vanuatu, Venezuela, RB, Vietnam, Virgin Islands (U.S.), West Bank and Gaza, Yemen, Rep., Zambia, Zimbabwe".ToStringList();
        private static List<string> _states = "AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY".ToStringList();
        private static List<string> _cities = "Midway, Mount, Pleasant, Greenwood, Franklin, Oak, Grove, Centerville, Salem, Georgetown, Fairview, Riverside, Rotorua, Tauranga, Whakatane, Taupo, Wanganui, Nababeep, Aggeneys, Pofadder, Polokwane, Bela, Bela, Goukamma, Karatara, Tswane, Prieska, Upington, Hoopstad, Bultfontein, Wesselsbron, Bothaville, Trompsburg, Henneman, Musina, Ogies, Kgatlahong, Tembisa, Tekoza, Sebokeng, Muntaung, Umnkomaaz".ToStringList();

        static public string Country()
        {
            return _countries.GetRandom();
        }

        static public string State()
        {
            return _states.GetRandom();
        }

        static public string City()
        {
            return _cities.GetRandom();
        }

        static public string ZipCode()
        {
            return StringFaker.Numeric(5);
        }

        static public string StreetName()
        {
            List<string> streetnames = "Highland, Hill, Park, Woodland, Sunset, Virginia".ToStringList();
            string place = streetnames.GetRandom();

            if (Random.GetBoolean())
            {
                List<string> trees = "Acacia, Beech, Birch, Cedar, Cherry, Chestnut, Elm, Larch, Laurel, Linden, Maple, Oak, Pine, Rose, Walnut, Willow".ToStringList();
                string tree = trees.GetRandom();

                return tree + " " + place;
            }
            else
            {
                List<string> people = "Adams, Franklin, Jackson, Jefferson, Lincoln, Madison, Washington, Wilson".ToStringList();
                string person = people.GetRandom();

                return person + " " + place;
            }
        }

        static public int StreetNumber()
        {
            return Random.Next(1, 50);
        }

        static public string Street()
        {
            return StreetNumber().ToString() + " " + StreetName();
        }

        static public string PostCode()
        {
            return StringFaker.Randomize("????? ##").ToUpper();
        }
    }
}
