using System;

namespace NetSteps.Testing.Integration
{


    public static class AccountStatus
    {
        /// <summary>
        /// Account Status ID
        /// </summary>
        public enum ID
        {
            Active,
            BegunEnrollment,
            Canceled,
            Imported,
            Inactive,
            Suspended,
            Terminated
        }

        public static string ToPattern(this ID accountStatus)
        {
            string result;

            if (accountStatus.Equals(ID.BegunEnrollment))
                result = "Begun Enrollment";
            else
                result = accountStatus.ToString();
            return result;
        }

        public static ID Parse(string value)
        {
            ID result;
            value = value.Trim();
            if (value.Equals("Begun Enrollment"))
                result = ID.BegunEnrollment;
            else
                result = (ID) Enum.Parse(typeof(ID), value);
            return result;
        }
    }
}
