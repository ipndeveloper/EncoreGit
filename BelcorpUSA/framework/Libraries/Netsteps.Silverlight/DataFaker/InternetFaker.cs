using System.Collections.Generic;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.DataFaker
{
    public static class InternetFaker
    {
        private static List<string> _domains = "yahoo.com, gmail.com, privacy.net, webmail.com, sn.com, hotmail.com, example.com".ToStringList();

        public static string Domain()
        {
            return _domains.GetRandom();
        }

        public static string Email()
        {
            if (Random.Next(5) == 2)
                return NameFaker.FirstName().ToLower() + Random.Next(2) + "@" + Domain();
            else
                return NameFaker.FirstName().ToLower() + "@" + Domain();
        }

        public static string Url()
        {
            return "http://www." + Domain();
        }
    }
}
