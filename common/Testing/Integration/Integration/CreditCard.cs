using System;

namespace NetSteps.Testing.Integration
{
    public static class CreditCard
    {
        public enum ID
        {
            None = 0,
            CreditCard,
            AmericanExpress,
            Discover,
            MasterCard,
            Maestro,
            Visa
        }

        public static string ToPattern(this ID creditCard)
        {
            string result;
            switch (creditCard)
            {
                case ID.CreditCard:
                    result = "Credit Card";
                    break;
                case ID.AmericanExpress:
                    result = "American Extpress";
                    break;
                case ID.MasterCard:
                    result = "Master Card";
                    break;
                default:
                    result = creditCard.ToString();
                    break;
            }
            return result;
        }

        public static ID Parse(string creditCard)
        {
            ID result;
            creditCard = creditCard.Trim();
            switch (creditCard)
            {
                case "Master Card":
                    result = ID.MasterCard;
                    break;
                case "American Extpress":
                    result = ID.AmericanExpress;
                    break;
                default:
                    result = (ID)Enum.Parse(typeof(ID), creditCard);
                    break;
            }
            return result;
        }
    }
}
