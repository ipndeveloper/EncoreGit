using System;

namespace NetSteps.Testing.Integration
{
    public static class PhoneType
    {
        public enum ID
        {
            Main,
            Cell,
            Fax,
            Work,
            Text,
            Home,
            Other,
            Pager
        }

        public static ID Parse(string phone)
        {
            ID phoneId = (ID)Enum.Parse(typeof(ID), phone);
            return phoneId;
        }
    }
}
