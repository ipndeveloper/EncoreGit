using System;
using System.IO;

using NetSteps.ShippingDataImporter.Models;

using Newtonsoft.Json;

namespace NetSteps.ShippingDataImporter.Services
{
    public class IOService
    {
        private string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        public string GetOutputDirectory()
        {
            var foo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"output\\");
            if (!Directory.Exists(foo))
            {
                Directory.CreateDirectory(foo);
            }

            return foo;
        }

        public void WriteInterfaceSettings(ServiceRunnerModel model)
        {
            if (model != null)
            {
                var json = JsonConvert.SerializeObject(model, Formatting.Indented);
                File.WriteAllText(settingsFilePath, json);
            }
        }

        public ServiceRunnerModel LoadInterfaceSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                var json = File.ReadAllText(settingsFilePath);
                var model = JsonConvert.DeserializeObject<ServiceRunnerModel>(json);
                return model;
            }

            return null;
        }
    }
}