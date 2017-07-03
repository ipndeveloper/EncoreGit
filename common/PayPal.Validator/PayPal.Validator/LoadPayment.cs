using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using Utilities;
using PayPal.Api;
using Newtonsoft.Json.Linq;

namespace PayPal.Validator
{
    public class LoadPayment : BaseSamplePage
    {
        //public APIContext apiContext;
        public override void RunSample() { }

        public  void chargePayment(ref PayPal_Tokens ppTokens)
        {
            APIContext ApiContext = Configuration.GetAPIContext();

            //Obtengo el Token
            ppTokens.Token_id = ApiContext.AccessToken;

            //Ajustando Dirección
            if (ppTokens.Line1.Length >100) ppTokens.Line1 = ppTokens.Line1.Substring(0, 100);
            if (ppTokens.Line2.Length > 100) ppTokens.Line2 = ppTokens.Line2.Substring(0, 100);
            
            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    total = ppTokens.Total,
                    currency = ppTokens.Currency
                },
                description = ppTokens.OrderID,
                item_list = new ItemList()
                {
                    shipping_address = new ShippingAddress
                    {
                        city = ppTokens.City,
                        country_code = ppTokens.Country_code,
                        line1 = ppTokens.Line1,
                        line2 = ppTokens.Line2,
                        postal_code = ppTokens.Postal_code,
                        state = ppTokens.State,
                        recipient_name = ppTokens.Recipient_name
                    }
                },
                payment_options = new PaymentOptions()
                {
                    allowed_payment_method = "IMMEDIATE_PAY"
                },
                custom = ppTokens.AccountID,
                invoice_number = ppTokens.OrderID
            };

            var redirect_urls = new RedirectUrls()
            {
                return_url = ppTokens.ReturnUrl,
                cancel_url = ppTokens.CancelUrl
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "sale",
                experience_profile_id = ppTokens.Experience_id,
                payer = payer,
                transactions = new List<Transaction>() { transaction },
                redirect_urls = redirect_urls
            };

            // Create a payment using a valid APIContext
            try
            {
                var createdPayment = payment.Create(ApiContext);
                for (int i = 0; i < createdPayment.links.Count; i++)
                {
                    if (createdPayment.links[i].rel.Equals("approval_url"))
                        ppTokens.Approval_url = Uri.EscapeDataString(createdPayment.links[i].href);

                    if (createdPayment.links[i].rel.Equals("execute"))
                        ppTokens.Execute_url = createdPayment.links[i].href;
                }
                ppTokens.Payment_Id = createdPayment.id;
                ppTokens.AccessToken = ApiContext.AccessToken;
                ppTokens.CreatedPayment = createdPayment.ConvertToJson();
            }
            catch (PaymentsException pe)
            {
                ppTokens.Approval_url = "{'name': '" + pe.Details.name + "', 'message': '" + CleanInput(pe.Details.message.ToString()) + "'}";
                ppTokens.CreatedPayment = "{'name': '" + pe.Details.name + "', 'message': '" + CleanInput(pe.Details.message.ToString()) + "'}";
            }
            catch (PayPalException ppe)
            {
                ppTokens.Approval_url = "{'name': 'PAYPAL_ERROR', 'message': '" + CleanInput(ppe.Message.ToString()) + "'}";
                ppTokens.CreatedPayment = "{'name': 'PAYPAL_ERROR', 'message': '" + CleanInput(ppe.Message.ToString()) + "'}";
            }
            catch (Exception e)
            {
                ppTokens.Approval_url = "{name: 'CHARGE_PAYMENT_ERROR', message: '" + CleanInput(e.Message.ToString()) + "'}";
                ppTokens.CreatedPayment = "{name: 'CHARGE_PAYMENT_ERROR', message: '" + CleanInput(e.Message.ToString()) + "'}";
            }

        }

        public string executePayment(PayPal_Tokens ppTokens)
        {
            try
            {

                // A resource representing a Payer that funds a payment.
                var payer = new Payer()
                {
                    payment_method = "paypal"
                };

                var transaction = new Transaction()
                {
                    amount = new Amount()
                    {
                        total = ppTokens.Total,
                        currency = ppTokens.Currency
                    },
                    description = ppTokens.OrderID,
                    payment_options = new PaymentOptions()
                    {
                        allowed_payment_method = "IMMEDIATE_PAY"
                    },
                    custom = ppTokens.AccountID,
                    invoice_number = ppTokens.OrderID
                };


                var redirect_urls = new RedirectUrls()
                {
                    return_url = ppTokens.ReturnUrl,
                    cancel_url = ppTokens.CancelUrl
                };

                // A Payment resource; create one using the above types and intent as `sale` or `authorize`
                var pymnt = new Payment()
                {
                    intent = "sale",
                    experience_profile_id = ppTokens.Experience_id,
                    payer = payer,
                    transactions = new List<Transaction>() { transaction },
                    redirect_urls = redirect_urls
                };

           
                Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
                sdkConfig.Add("mode", ppTokens.Mode);
                string accessToken = ppTokens.AccessToken;
                APIContext apiContext = new APIContext(accessToken);
                apiContext.Config = sdkConfig;

                PaymentExecution pymntExecution = new PaymentExecution();
                pymntExecution.payer_id = ppTokens.Payer_Id;
                
                pymnt.id = ppTokens.Payment_Id;

                Payment executedPayment = pymnt.Execute(apiContext, pymntExecution);

                return executedPayment.ConvertToJson();
            
            }
            catch (PaymentsException pe)
            {
                string error_Payment = (JObject.Parse("{name:'" + pe.Details.name + "', message: '" + CleanInput(pe.Details.message.ToString()) + "'}")).ToString();
                return error_Payment;
            }
            catch (PayPalException ppe)
            {
                string paypal = (JObject.Parse("{name: 'PAYPAL_ERROR', message: '" + CleanInput(ppe.Message.ToString()) + "'}")).ToString();
                return paypal;
            }
            catch (Exception e)
            {
                string error=(JObject.Parse("{name: 'EXECUTE_PAYMENT_ERROR', message: '" + e.Message.ToString() + "'}")).ToString();
                return error;
            }

            

        }

        public static string generateExperienceProfileID(APIContext apiContext)
        {
            // Create the web experience profile
            var profile = new WebProfile
            {
                name = "Belcorp",
                presentation = new Presentation
                {
                    brand_name = "Belcorp",
                    locale_code = "BR"
                },
                input_fields = new InputFields
                {
                    no_shipping = 1,
                    address_override = 1
                }
            };

            var createdProfile = profile.Create(apiContext);

            return createdProfile.id;
        }

        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\s\.@-]", "",RegexOptions.None);
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (Exception)
            {
                return String.Empty;
            }
        }


    }
}
