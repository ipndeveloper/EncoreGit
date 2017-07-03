namespace NetSteps.ShippingDataImporter.Helpers
{
    using System;

    public class ConsoleMessage
    {
        public void OutputHeader()
        {
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(" ShippingService Script Generator. Written by Spencer Killian ");
            Console.WriteLine("----------------------------------------------------------------");
        }

        public void OutputCompletedMessage(bool result, string errorMessage)
        {
            if (result)
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("Completed Successfuly.");
                Console.WriteLine("----------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("Completed with Errors.");
                Console.WriteLine(errorMessage);
                Console.WriteLine("----------------------------------------------------------------");
            }
        } 
    }
}