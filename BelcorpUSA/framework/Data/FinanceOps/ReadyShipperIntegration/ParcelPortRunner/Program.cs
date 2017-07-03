using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ParcelPortIntegrationService;



namespace ParcelPortRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Be sure to update reference to DLL before building
             */
            ParcelPortIntegrationService.ParcelPortIntegrationService client = new ParcelPortIntegrationService.ParcelPortIntegrationService();
            String message = "";

            try
            {
                client.SendOrders(ConfigurationManager.AppSettings["ParcelPortAPIKey"]);
            }
            catch (Exception e)
            {
                message = e.Message;
                
                
            }
            Console.Out.WriteLine("ParcelPort push complete!");
        }
    }
}
