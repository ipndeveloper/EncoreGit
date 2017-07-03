using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelPortIntegrationService
{
    public static class StringExtensions
    {
        public static string ToValidString(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }
    }
}
