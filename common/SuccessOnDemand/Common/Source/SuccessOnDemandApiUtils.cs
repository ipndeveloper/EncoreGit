// -----------------------------------------------------------------------
// <copyright file="SuccessOnDemandApiUtils.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.SOD.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuccessOnDemandApiUtils
    {
        public static string PreparePasswordForApi(string password)
        {
            Contract.Requires<ArgumentNullException>(password != null);
            Contract.Requires<ArgumentException>(password.Length > 0);



            return new string(password.Where(p => Char.IsLetterOrDigit(p)).Take(20).ToArray());
        }
    }
}
