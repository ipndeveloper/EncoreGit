using System;

namespace NetSteps.Testing.Integration
{ 
    public static class Gender
    {
        public enum ID
        {
            None = 0,
            PreferNotToSay,
            Male,
            Female
        }

        public static string ToPattern(this ID gender)
        {
            string stg;
            switch (gender)
            {
                case ID.PreferNotToSay:
                    stg = "Prefer not to say";
                    break;
                default:
                    stg = gender.ToString();
                    break;
            }
            return stg;
        }

        public static ID Parse(string value)
        {
            ID gender;
            value = value.Trim();
            if (value == "Prefer Not To Say")
                gender = ID.PreferNotToSay;
            else if (value == string.Empty)
                gender = ID.PreferNotToSay;
            else
                gender = (ID)Enum.Parse(typeof(ID), value);
            return gender;
        }
    }
}
