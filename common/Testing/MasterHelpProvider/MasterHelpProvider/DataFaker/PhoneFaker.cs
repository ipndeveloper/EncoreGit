
//using TestMasterHelpProvider.DataFaker;
namespace TestMasterHelpProvider.DataFaker
{
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
