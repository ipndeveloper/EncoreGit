using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Authorization.Common.Configuration
{
    [ConfigurationCollection(typeof(FunctionElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class FunctionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FunctionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FunctionElement)element).Name;
        }

        public FunctionElement this[int index]
        {
            get
            {
                return (FunctionElement)base.BaseGet(index);
            }
        }
    }
}
