
namespace NetSteps.Common.DataFaker
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Methods to generate 'Fake' data for use in testing.
    /// Created: 03-18-2009
    /// </summary>
    public static class PhoneFaker
    {
        public static string Phone()
        {
            return StringFaker.Randomize("###-###-####");
        }

        public static string InternationalPhone()
        {
            return StringFaker.Randomize("+##-(0)####-####-####");
        }
    }
}
