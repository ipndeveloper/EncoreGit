
namespace NetSteps.Silverlight.DataFaker
{
    public static class DateTimeFaker
    {
        public static System.DateTime DateTime(System.DateTime from, System.DateTime to)
        {
            var timeSpan = to - from;
            return from.AddDays((double)Random.Next(1, (int)timeSpan.TotalDays - 1)).AddSeconds(Random.Next(1, 86400));
        }

        public static System.DateTime DateTime()
        {
            return DateTimeBetweenDays(5);
        }

        public static System.DateTime DateTimeBetweenDays(double fromDays, double toDays)
        {
            return DateTime(System.DateTime.Now.AddDays(-1 * fromDays), System.DateTime.Now.AddDays(toDays));
        }

        public static System.DateTime DateTimeBetweenDays(double days)
        {
            return DateTime(System.DateTime.Now.AddDays(-1 * days), System.DateTime.Now.AddDays(days));
        }

        public static System.DateTime DateTimeBetweenMonths(int fromMonths, int toMonths)
        {
            return DateTime(System.DateTime.Now.AddMonths(-1 * fromMonths), System.DateTime.Now.AddMonths(toMonths));
        }

        public static System.DateTime DateTimeBetweenMonths(int months)
        {
            return DateTime(System.DateTime.Now.AddMonths(-1 * months), System.DateTime.Now.AddMonths(months));
        }

        public static System.DateTime DateTimeBetweenYears(int fromYears, int toYears)
        {
            return DateTime(System.DateTime.Now.AddYears(-1 * fromYears), System.DateTime.Now.AddYears(toYears));
        }

        public static System.DateTime DateTimeBetweenYears(int years)
        {
            return DateTime(System.DateTime.Now.AddYears(-1 * years), System.DateTime.Now.AddDays(years));
        }

        public static System.DateTime BirthDay(int minAge, int maxAge)
        {
            return DateTimeBetweenYears(maxAge, minAge);
        }

        public static System.DateTime BirthDay(int minAge)
        {
            return DateTimeBetweenYears(100, minAge);
        }

        public static System.DateTime BirthDay()
        {
            return BirthDay(18);
        }
    }
}
