using System.Collections.Generic;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.DataFaker
{
    public static class MiscFaker
    {
        private static List<string> _eventName = "Annual Convention, Business Meeting, Board Meeting, Training Meeting, Regional Meeting".ToStringList();
        private static List<string> _personImage = "billherf.jpg, domingo.jpg, napoleon.jpg, Seth.JPG".ToStringList();
        private static List<string> _randomImage = "j0409255.jpg, j0410185.jpg, j0411768.jpg, j0426646.JPG, j0433093.jpg, j0438548.jpg".ToStringList();

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
    }
}
