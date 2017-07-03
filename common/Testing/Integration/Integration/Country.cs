using System;

namespace NetSteps.Testing.Integration
{
    public static class Country
    {
        public enum ID
        {
            None = 0,
            Afghanistan,
            Albania,
            Algeria,
            Andorra,
            Angola,
            Anguilla,
            Antarctica,
            Argentina,
            Armenia,
            Aruba,
            Australia,
            Austria,
            Azerbaijan,
            Bahamas,
            Bahrain,
            Bangladesh,
            Barbados,
            Belarus,
            Belgium,
            Belize,
            Benin,
            Bermuda,
            Bhutan,
            Bolivia,
            Botswana,
            Brazil,
            Brunei,
            Bulgaria,
            BurkinaFaso,
            Burundi,
            Cambodia,
            Cameroon,
            Canada,
            CapeVerde,
            CaymanIslands,
            Chad,
            Chile,
            China,
            ChristmasIsland,
            CocosIslands,
            Colombia,
            Comoros,
            CookIslands,
            CostaRica,
            Croatia,
            Cuba,
            Cyprus,
            CzechRepublic,
            England,
            France,
            Germany,
            Ireland,
            Netherlands,
            NorthernIreland,
            PuertoRico,
            Scotland,
            Sweden,
            UnitedKingdom,
            UnitedStates,
            Wales,
            Deutschland,
            Luxemburg,
            Belgien,
            Niederlande
        }

        public static string ToPattern(this ID country)
        {
            string result;
            switch (country)
            {
                case ID.Ireland:
                    result = "Ireland";
                    break;
                case ID.NorthernIreland:
                    result = "Northern Ireland";
                    break;
                case ID.PuertoRico:
                    result = "Puerto Rico";
                    break;
                case ID.UnitedKingdom:
                    result = "United Kingdom";
                    break;
                case ID.UnitedStates:
                    result = "United States";
                    break;
                default:
                    result = country.ToString();
                    break;
            }
            return result;
        }

        public static ID Parse(string country)
        {            
            ID countryID;
            country = country.Trim();
            switch (country)
            {
                case "Northern Ireland":
                    countryID = ID.NorthernIreland;
                    break;
                case "Puerto Rico":
                    countryID = ID.PuertoRico;
                    break;
                case "United Kingdom":
                    countryID = ID.UnitedKingdom;
                    break;
                case "United States":
                    countryID = ID.UnitedStates;
                    break;
                default:
                    countryID = (ID)Enum.Parse(typeof(ID), country);
                    break;
            }
            return countryID;
        }

        public static string ToExpandedString(this ID country)
        {
            string result;
            switch (country)
            {
                case ID.Ireland:
                    result = "Ireland";
                    break;
                case ID.NorthernIreland:
                    result = "Northern Ireland";
                    break;
                case ID.PuertoRico:
                    result = "Puerto Rico";
                    break;
                case ID.UnitedKingdom:
                    result = "United Kingdom";
                    break;
                case ID.UnitedStates:
                    result = "United States";
                    break;
                default:
                    result = country.ToString();
                    break;
            }
            return result;
        }
    }
}
