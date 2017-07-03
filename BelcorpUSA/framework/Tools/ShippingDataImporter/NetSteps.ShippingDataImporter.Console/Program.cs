namespace NetSteps.ShippingDataImporter.Console
{
    using NetSteps.ShippingDataImporter.Services;

    public class Program
	{
		static void Main(string[] args)
		{
		    var service = new Service();
		    service.RunConsole(args);

		}
	}
}