using System.Collections.Generic;
using PayPal.Api;
using System;

namespace Utilities
{ 
    public static class Configuration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static Configuration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window  
            string accessToken = "";
            try
            {
                accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            }
            catch(ArgumentException ae)
            {
                return "{'name':"+ae.Message+"}";
            }
            catch (Exception e)
            {
                return "{'name':" + e.Message + "}";
            }

            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string accessToken = "")
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext=new APIContext();

            try
            {
                apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
            }
            catch (ArgumentException ae)
            {
                
            }

            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }

    }
}
 