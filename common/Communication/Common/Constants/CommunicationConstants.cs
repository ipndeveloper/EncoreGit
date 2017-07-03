using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common
{
    public static class CommunicationConstants
    {
        public static class AccountAlertProviderKey
        {
            public static readonly Guid
                Promotion = new Guid("621E92AF-E3F0-4445-87DB-1AA6A1263711");
            public static readonly Guid
                Message = new Guid("a1f81c02-d86c-41a5-a68a-63d2b0a0aea9");
        }

        public enum AccountAlertDisplayKind : int
        {
            NotSet = 0,
            Message = 1,
            Modal = 2
        }
    }
}
