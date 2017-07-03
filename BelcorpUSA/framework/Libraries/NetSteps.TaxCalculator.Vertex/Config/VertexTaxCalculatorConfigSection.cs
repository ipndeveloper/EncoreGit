using System;
using System.Configuration;

namespace NetSteps.TaxCalculator.Vertex.Config
{
    public class VertexTaxCalculatorConfigSection : ConfigurationSection
    {
	    private const string ConfigSection = "vertexTaxCalculator";
	    private const string Property_trustedID = "trustedID";
        private const string Property_username = "username";
        private const string Property_password = "password";
		private const string Property_taxPayers = "taxPayers";
		private const string Property_shippingSku = "shippingSku";
		private const string Property_orderShippingSku = "orderShippingSku";

        [ConfigurationProperty(Property_trustedID)]
        public string TrustedID
        {
            get { return (string)this[Property_trustedID]; }
            set { this[Property_trustedID] = value; }
        }

        [ConfigurationProperty(Property_username)]
        public string UserName
        {
            get { return (string)this[Property_username]; }
            set { this[Property_username] = value; }
        }

        [ConfigurationProperty(Property_password)]
        public string Password
        {
            get { return (string)this[Property_password]; }
            set { this[Property_password] = value; }
        }

		[ConfigurationProperty(Property_shippingSku)]
		public string ShippingSku
		{
			get { return (string)this[Property_shippingSku]; }
			set { this[Property_shippingSku] = value; }
		}

		[ConfigurationProperty(Property_orderShippingSku)]
		public string OrderShippingSku
		{
			get { return (string)this[Property_orderShippingSku]; }
			set { this[Property_orderShippingSku] = value; }
		}

		[ConfigurationProperty("taxPayers", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(VertexTaxPayerCodeCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
		public VertexTaxPayerCodeCollection TaxPayerCodeCodes
		{
			get { return (VertexTaxPayerCodeCollection)base[Property_taxPayers]; }
			set { this[Property_taxPayers] = value; }
		}

        public static VertexTaxCalculatorConfigSection Current
        {
            get
            {
                var current = ConfigurationManager.GetSection(ConfigSection)
                    as VertexTaxCalculatorConfigSection;
                return current ?? new VertexTaxCalculatorConfigSection();
            }
        }
        
        internal CalculateTaxService60.LoginType FillLogin(CalculateTaxService60.LoginType login)
        {
            var trustedID = TrustedID;
            if (!String.IsNullOrWhiteSpace(trustedID))
            {
                login.TrustedId = trustedID;
            }
            else
            {
                login.UserName = UserName;
                login.Password = Password;
            }
            return login;
        }

        internal TaxAreaService60.LoginType FillLogin(TaxAreaService60.LoginType login)
        {
            var trustedID = TrustedID;
            if (!String.IsNullOrWhiteSpace(trustedID))
            {
                login.TrustedId = trustedID;
            }
            else
            {
                login.UserName = UserName;
                login.Password = Password;
            }
            return login;
        }
    }
}
