using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using NetSteps.ShippingDataImporter.Helpers;
using NetSteps.ShippingDataImporter.Models;

namespace NetSteps.ShippingDataImporter.Services
{
    
    public class Service
    {
    	public ShippingService ShippingService;

        public void UpdateServiceProgress()
        {
            ShippingService = new ShippingService();
        }

        #region ConsoleApp

        public void RunConsole(string[] args)
        {
            var console = new ConsoleMessage();

            var filePath = GetStringValueFromArguments(args, "-filename");
            var generatedFilePath = GetStringValueFromArguments(args, "-generatedfilename");
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

            console.OutputHeader();

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Constants.ImportFilePath;
            }

            if (string.IsNullOrEmpty(generatedFilePath))
            {
                generatedFilePath = Constants.OutputFilePrefix;
            }

            try
            {
                ShippingService.CreateSQL(Path.Combine(appPath, filePath), Path.Combine(appPath, generatedFilePath));
                console.OutputCompletedMessage(true, "Success");
            }
            catch (Exception ex)
            {
                console.OutputCompletedMessage(false, ex.Message);
            }

            Console.WriteLine("Press enter to close.>");
            Console.ReadLine();
        }

        private static string GetArgumentValueString(string p, List<string> commandArguments)
        {
            Contract.Requires<ArgumentNullException>(p != null);
            Contract.Requires<ArgumentNullException>(commandArguments != null);

            var i = commandArguments.IndexOf(p);
            return (i >= 0) ? commandArguments[i + 1] : string.Empty;
        }

        private static string GetStringValueFromArguments(string[] args, string switchName)
        {
            var commandArguments = args.ToList();

            return GetArgumentValueString(switchName, commandArguments);
        }

        #endregion
    }
}