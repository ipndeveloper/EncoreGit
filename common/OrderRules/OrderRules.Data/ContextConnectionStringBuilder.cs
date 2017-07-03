﻿using System;
using System.Data;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Reflection;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;

namespace OrderRules.Core.Model
{
    public static class ContextConnectionStringBuilder
    {
        public static string GetEntityConnectionString(string ConnectionString,
                                                       Type ContextType)
        {
            string result = string.Empty;

            string prefix = ContextType.Namespace
              .Replace(ContextType.Assembly.GetName().Name, "");

            if (prefix.Length > 0
                && prefix.StartsWith("."))
            {
                prefix = prefix.Substring(1, prefix.Length - 1);
            }

            if (prefix.Length > 1
                && !prefix.EndsWith("."))
            {
                prefix += ".";
            }


            EntityConnectionStringBuilder csBuilder =
              new EntityConnectionStringBuilder();

            csBuilder.Provider = "System.Data.SqlClient";
            csBuilder.ProviderConnectionString = ConnectionString.ToString();
            csBuilder.Metadata = string.Format("res://{0}/{1}.csdl|"
                                                + "res://{0}/{1}.ssdl|"
                                                + "res://{0}/{1}.msl"
                                                , ContextType.Assembly.FullName
                                                , prefix + ContextType.Name);

            result = csBuilder.ToString();

            return result;
        }
    }
}
