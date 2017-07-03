using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayPal.Api;

namespace PayPal.Validator
{
    [Serializable]
    public class PayPal_Tokens
    {
        private string orderID;
        private string accountID;
        private string experience_id;
        private string currency;
        private string total;
        private string approval_url;
        private string returnUrl;
        private string cancelUrl;
        private string execute_url;
        private string token_id;
        private string payment_Id;
        private string payer_Id;
        private string accessToken;
        private string mode;
        private string createdPayment;

        private string city;
        private string country_code;
        private string line1;
        private string line2;
        private string postal_code;
        private string state;
        private string recipient_name;

        public PayPal_Tokens() { }

        public PayPal_Tokens(string orderID, string accountID, string experience_id, string currency, string total, 
                             string approval_url, string returnUrl, string cancelUrl, string execute_url, string token_id, string payment_Id) 
        {
            this.orderID = orderID;
            this.accountID = accountID;
            this.experience_id = experience_id;
            this.currency = currency;
            this.total = total;
            this.approval_url = approval_url;
            this.returnUrl = returnUrl;
            this.cancelUrl = cancelUrl;
            this.execute_url = execute_url;
            this.token_id = token_id;
            this.payment_Id = payment_Id;
            
        }

        public string OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
        
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        
        public string Experience_id
        {
            get { return experience_id; }
            set { experience_id = value; }
        }
        
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public string Total
        {
            get { return total; }
            set { total = value; }
        }
        
        public string Approval_url
        {
            get { return approval_url; }
            set { approval_url = value; }
        }

        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }
        
        public string CancelUrl
        {
            get { return cancelUrl; }
            set { cancelUrl = value; }
        }
        
        public string Execute_url
        {
            get { return execute_url; }
            set { execute_url = value; }
        }
        
        public string Token_id
        {
            get { return token_id; }
            set { token_id = value; }
        }
        
        public string Payment_Id
        {
            get { return payment_Id; }
            set { payment_Id = value; }
        }

        public string Payer_Id
        {
            get { return payer_Id; }
            set { payer_Id = value; }
        }

        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }

        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public string CreatedPayment
        {
            get { return createdPayment; }
            set { createdPayment = value; }
        }


        public string City
        {
            get { return city; }
            set { city = value; }
        }


        public string Country_code
        {
            get { return country_code; }
            set { country_code = value; }
        }

        public string Line1
        {
            get { return line1; }
            set { line1 = value; }
        }

        public string Line2
        {
            get { return line2; }
            set { line2 = value; }
        }

        public string Postal_code
        {
            get { return postal_code; }
            set { postal_code = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Recipient_name
        {
            get { return recipient_name; }
            set { recipient_name = value; }
        }
        
    }
}
