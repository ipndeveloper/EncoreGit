using System;

namespace NetSteps.Testing.Integration
{
    public static class Language
    {

        public enum ID
        {
            None,
            Czech,
            Danish,
            Dutch,
            DutchGermany,
            English,
            EnglishIreland,
            EnglishOnly,
            EnglishUnitedKingdom,
            Finnish,
            French,
            FrenchGermany,
            FrenchSwitzerland,
            German,
            GermanAustria,
            GermanSwitzerland,
            Italian,
            ItalianSwitzerland,
            Japanese,
            Norwegian,
            Polish,
            Slovak,
            Spanish,
            Swedish,
            SwedishFinland
        }

        public static ID Parse(string language)
        {
            ID lng;
            language = language.Trim();
            switch(language)
            {
                case "Dutch (Germany)":
                    lng = ID.DutchGermany;
                    break;
                case "English (Ireland)":
                    lng = ID.EnglishIreland;
                    break;
                case "English (United Kingdom)":
                    lng = ID.EnglishUnitedKingdom;
                    break;
                case "English (Please select English ONLY)":
                    lng = ID.EnglishOnly;
                    break;
                case "French (Germany)":
                    lng = ID.FrenchGermany;
                    break;
                case "French (Switzerland)":
                    lng = ID.FrenchSwitzerland;
                    break;
                case "German (Austria)":
                    lng = ID.GermanAustria;
                    break;
                case "German (Switzerland)":
                    lng = ID.GermanSwitzerland;
                    break;
                case "Italian (Switzerland)":
                    lng = ID.ItalianSwitzerland;
                    break;
                case "Swedish (Finland)":
                    lng = ID.SwedishFinland;
                    break;
                default:
                    lng = (ID)Enum.Parse(typeof(ID), language);
                    break;
            }
            return lng;
        }

        public static string ToPattern(this ID language)
        {
            string lng;
            switch (language)
            {
                case ID.DutchGermany:
                    lng = "Dutch \\(Germany\\)";
                    break;
                case ID.EnglishIreland:
                    lng = "English \\(Ireland\\)";
                    break;
                case ID.EnglishOnly:
                    lng = "English \\(Please select English ONLY\\)";
                    break;
                case ID.EnglishUnitedKingdom:
                    lng = "English \\(United Kingdom\\)";
                    break;
                case ID.FrenchGermany:
                    lng = "French \\(Germany\\)";
                    break;
                case ID.FrenchSwitzerland:
                    lng = "French \\(Switzerland\\)";
                    break;
                case ID.GermanAustria:
                    lng = "German \\(Austria\\)";
                    break;
                case ID.GermanSwitzerland:
                    lng = "German \\(Switzerland\\)";
                    break;
                case ID.ItalianSwitzerland:
                    lng = "Italian \\(Switzerland\\)";
                    break;
                case ID.SwedishFinland:
                    lng = "Swedish \\(Finland\\)";
                    break;
                default:
                    lng = language.ToString() + "$";
                    break;
            }
            return lng;
        }

        public static string ToExpandedString(this ID language)
        {
            string lng;
            switch (language)
            {
                case ID.DutchGermany:
                    lng = "Dutch (Germany)";
                    break;
                case ID.EnglishIreland:
                    lng = "English (Ireland)";
                    break;
                case ID.EnglishOnly:
                    lng = "English (Please select English ONLY)";
                    break;
                case ID.EnglishUnitedKingdom:
                    lng = "English (United Kingdom)";
                    break;
                case ID.FrenchGermany:
                    lng = "French (Germany)";
                    break;
                case ID.FrenchSwitzerland:
                    lng = "French (Switzerland)";
                    break;
                case ID.GermanAustria:
                    lng = "German (Austria)";
                    break;
                case ID.GermanSwitzerland:
                    lng = "German (Switzerland)";
                    break;
                case ID.ItalianSwitzerland:
                    lng = "Italian (Switzerland)";
                    break;
                case ID.SwedishFinland:
                    lng = "Swedish (Finland)";
                    break;
                default:
                    lng = language.ToString();
                    break;
            }
            return lng;
        }
    }
}
