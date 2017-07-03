using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Configuration
{
    [ConfigurationCollection(typeof(AdminSetting), AddItemName = "addSetting", RemoveItemName = "removeSetting", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AdminSettings : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AdminSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AdminSetting)element).Name;
        }

        public bool GetValue(string settingName)
        {
            var setting = base.BaseGet(settingName);
            if (setting == null)
                return false;
            
            return ((AdminSetting)setting).Value;
        }

        public IDictionary<string, bool> GetSettings()
        {
            var dictionary = new Dictionary<string, bool>();
            foreach (string key in base.BaseGetAllKeys())
            {
                dictionary.Add(key, GetValue(key));
            }
            return dictionary;
        }
    }
}
