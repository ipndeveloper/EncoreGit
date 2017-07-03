using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.SOD.Wrapper
{
    public class SODApiConnection
    {
        public static string BaseUri = "https://itworks.sd.success.com/";

        public static Uri UserUpdateMethod = new Uri(BaseUri + "api/userUpdate");
        public static Uri UserCreateMethod = new Uri(BaseUri + "api/userUpdate");
        public static Uri UserSsoMethod = new Uri(BaseUri + "account/login");
        public static string ContentType = "application/x-www-form-urlencoded";
        public static string PostParameterName = "xmldata";
        public static string DistNotFoundErrorMessage = "Distributor with DistID";
        public static string EmailAlreadyExistsErrorMessage = "That email is already associated with an existing distributor.";
    }
}
