using System.Collections.Generic;
using TestMasterHelpProvider.Extensions;

namespace TestMasterHelpProvider.DataFaker
{
    public static class InternetFaker
    {
        private static List<string> _domains = "yahoo.com, gmail.com, privacy.net, webmail.com, sn.com, hotmail.com, example.com".ToStringList();
        private static List<string> _pageName = "About Us, Company, News, Meetings, Announcements, Products, Events, Calendar Events, Help, Technical Support, Contact Us, Q & A, Home, Product Line, Products Landing Page, Help, Terms & Conditions, Archives, Document Library, Privacy Policy".ToStringList();

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

        public static string PageName()
        {
            return _pageName.GetRandom();
        }
    }
}
