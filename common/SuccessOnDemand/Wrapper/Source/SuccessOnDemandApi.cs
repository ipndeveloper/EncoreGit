using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Xml;
using NetSteps.SOD.Common;
using IoC = NetSteps.Encore.Core.IoC;
using System.Diagnostics.Contracts;
using NetSteps.SOD.Common.Exceptions;

namespace NetSteps.SOD.Wrapper
{
    [ContainerRegister(typeof(ISuccessOnDemandApi), RegistrationBehaviors.Default)]
    public class SuccessOnDemandApi : ISuccessOnDemandApi
    {
        static bool IsXmlContentType(string contentType)
        {
            if (contentType == null || contentType.Length < 8) return false;

            return contentType.StartsWith("text/xml")
                || contentType.StartsWith("application/xml");
        }

        void CallSuccessOnDemandApiAtUrl(Uri url, string postbody, string contentType, Action<HttpWebResponse> responseHandler)
        {
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentNullException>(postbody != null);
            Contract.Requires<ArgumentNullException>(contentType != null);
            Contract.Requires<ArgumentNullException>(responseHandler != null);


            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.KeepAlive = false;
            byte[] byteData = Encoding.UTF8.GetBytes(postbody);
            req.ContentType = contentType;
            req.ContentLength = byteData.Length;
            using (var stream = req.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
            }
            try
            {
                using (var resp = req.GetResponse() as HttpWebResponse)
                {
                    responseHandler(resp);
                }
            }
            catch (WebException wex)
            {
                var resp = wex.Response as HttpWebResponse;
                if (resp != null)
                {
                    responseHandler(resp);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Decodes the response xml from the Success On Demand api call to a response object.
        /// </summary>
        /// <param name="response">The response from the api request.</param>
        /// <returns>The result of the api request in the form of an IResponse object.</returns>
        IResponse DecodeMessageBodyAsIResponse(HttpWebResponse response)
        {
            Contract.Requires<ArgumentNullException>(response != null);

            IResponse result = default(IResponse);
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                if (IsXmlContentType(response.ContentType))
                {
                    var xml = XElement.Load(stream);
                    using (var create = IoC.Create.SharedOrNewContainer())
                    {
                        result = create.Mutation<IResponse>(create.New<IResponse>(), r =>
                        {
                            r.Error = xml.ReadStringOrDefault("error");
                            r.DistID = xml.ReadStringOrDefault("DistID");
                            r.ID = xml.ReadStringOrDefault("ID");
                        });
                    }
                }
                stream.Close();
                reader.Close();
                response.Close();
            }
            return result;
        }

        /// <summary>
        /// Attempts to create a Success On Demand account.
        /// </summary>
        /// <param name="acct">The info of the account to create.</param>
        /// <returns>The response object from the create request.</returns>
        public IResponse CreateAccount(IAccountInfo acct)
        {
            IResponse result = default(IResponse);
            CallSuccessOnDemandApiAtUrl(SODApiConnection.UserCreateMethod, acct.EncodeAsPostBody(true), SODApiConnection.ContentType,
                resp =>
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        result = DecodeMessageBodyAsIResponse(resp);
                        if (!String.IsNullOrWhiteSpace(result.Error))
                        {
                            if (result.Error.Equals(SODApiConnection.EmailAlreadyExistsErrorMessage))
                                throw new SODEmailExistsException(result.Error);
                            if (String.IsNullOrWhiteSpace(result.DistID))
                                throw new SODException("The call was successful, but the rep failed to be created");
                            throw new SODException(result.Error);
                        }
                    }
                    else
                    {
                        throw new HttpListenerException((int)resp.StatusCode);
                    }
                }
                );
            return result;
        }

        /// <summary>
        /// Attempts to update the Success On Demand account.
        /// </summary>
        /// <param name="acct">Info of the account to update.</param>
        /// <returns>Response object returned from the update request.</returns>
        public IResponse UpdateAccount(IAccountInfo acct)
        {
            IResponse result = default(IResponse);
            CallSuccessOnDemandApiAtUrl(SODApiConnection.UserUpdateMethod, acct.EncodeAsPostBody(false), SODApiConnection.ContentType,
                resp =>
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        result = DecodeMessageBodyAsIResponse(resp);
                        if (!String.IsNullOrWhiteSpace(result.Error))
                        {
                            if (result.Error.Equals(SODApiConnection.DistNotFoundErrorMessage))
                                throw new SODDistIDNotFoundException(result.Error);
                            if (String.IsNullOrWhiteSpace(result.DistID))
                                throw new SODException("The call was successful, but the rep failed to be updated");
                            throw new SODException(result.Error);
                        }
                    }
                    else
                    {
                        throw new HttpListenerException((int)resp.StatusCode);
                    }
                }
                );
            return result;
        }

        /// <summary>
        /// Attempts to log in the Success On Demand user.
        /// </summary>
        /// <param name="login">The info to use to attempt the log in.</param>
        /// <returns>True or False depending on the success of the log in.</returns>
        public bool Login(ILoginInfo login)
        {
            Contract.Assert(login != null);
            bool result = false;
            CallSuccessOnDemandApiAtUrl(SODApiConnection.UserSsoMethod, login.EncodeAsFormBody(), SODApiConnection.ContentType,
                resp =>
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                        result = true;
                    else
                        result = false;
                });
            return result;
        }
    }
}
