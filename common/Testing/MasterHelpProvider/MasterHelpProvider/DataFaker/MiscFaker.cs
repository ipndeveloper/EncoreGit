using System.Collections.Generic;
using TestMasterHelpProvider.Extensions;

namespace TestMasterHelpProvider.DataFaker
{
    public static class MiscFaker
    {
        private static List<string> _eventName = "Annual Convention, Business Meeting, Board Meeting, Training Meeting, Regional Meeting".ToStringList();
        private static List<string> _personImage = "billherf.jpg, domingo.jpg, napoleon.jpg, Seth.JPG".ToStringList();
        private static List<string> _randomImage = "j0409255.jpg, j0410185.jpg, j0411768.jpg, j0426646.JPG, j0433093.jpg, j0438548.jpg".ToStringList();
        private static List<string> _randomProduct = "Scrub, Anti-Age Regimen, Night Pills Sooth, Essentials, Enhancements, Night Serum, Whitening, Release, Greens, Argi9".ToStringList();
        private static List<string> _randomProductLine = "Skin Care, Hair Care, Weight Loss, Daily Essentials, Fitness".ToStringList();
        private static List<string> _randomNewsType = "Corporate Announcement, Regional News, International News, Newsletter".ToStringList();
        private static List<string> _randomFileType = "PDF, MOV, PNG, BMP, JPG, SWF, DOC".ToStringList();
        private static List<string> _randomArchiveType = "PDF, Movie, Image, Conference Call, Form, Application".ToStringList();

        public static string EventName()
        {
            return (Random.GetBoolean()) ? _eventName.GetRandom() : "Meeting with " + DataFaker.NameFaker.Name();
        }

        public static string PersonImage()
        {
            return _personImage.GetRandom();
        }

        public static string GetRandomImage()
        {
            return _randomImage.GetRandom();
        }

        public static double GetPrice(int min, int max)
        {
            double number = double.Parse(string.Format("{0}.{1}", Random.Next(min, max), Random.Next(1, 99)));
            if (number > max)
                number -= 1;
            return number;
        }

        public static string Status()
        {
            return (Random.GetBoolean()) ? "Active" : "Inactive";
        }

        public static string ProductName()
        {
            return _randomProduct.GetRandom();
        }

        public static string ProductLineName()
        {
            return _randomProductLine.GetRandom();
        }

        public static string NewsType()
        {
            return _randomNewsType.GetRandom();
        }

        public static string FileType()
        {
            return _randomFileType.GetRandom();
        }

        public static string ArchiveType()
        {
            return _randomArchiveType.GetRandom();
        }
    }
}
