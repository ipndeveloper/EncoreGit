using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities
{
	[Serializable]
	public class TemplatedMailMessage : System.Net.Mail.MailMessage
	{
		#region Fields

		private string _emailTemplate;
		private Dictionary<string, string> _emailValues;
		
		#endregion

		#region Properties

		public string EmailTemplate
		{
			get { return _emailTemplate; }
			set { _emailTemplate = value; }
		}

		public Dictionary<string, string> EmailValues
		{
			get { return _emailValues; }
			set { _emailValues = value; }
		}

		#endregion

		#region Constructors

		public TemplatedMailMessage(string emailTemplate, Dictionary<string, string> emailValues)
		{
			ReplaceTemplateValues(ref emailTemplate, emailValues);

			this.EmailTemplate = emailTemplate;
			this.EmailValues = emailValues;
			this.Body = emailTemplate; //Gets overrided if using RFMailMessage (specific for R+F)
		}

		#endregion

		#region Methods

		public void AppendOrderDetails(Order order)
		{
			this.Body += "<br /><br />";

			this.Body += "Order Number: " + order.OrderNumber + "<br />";
			this.Body += "Order Date: " + order.CompleteDate.ToShortDateString() + "<br />";

			this.Body += "<br />";

			this.Body += "<table cellspacing=\"6\">";

			// Header Row
			this.Body += "<tr>" +
				"<td>Qty</td>" +
				"<td>SKU</td>" +
				"<td>Item</td>" +
				"<td>Price</td>" +
				"<td>Total</td>" +
				"</tr>";

			// Order Items
			foreach (OrderItem item in order.OrderCustomers[0].OrderItems)
			{
				this.Body += "<tr>" +
				"<td>" + item.Quantity.ToString() + "</td>" +
				"<td>" + item.Product.SKU + "</td>" +
				"<td>" + item.Product.Name + "</td>" +
				"<td>" + item.GetAdjustedPrice().ToString("c") + "</td>" +
				"<td>" + (item.GetAdjustedPrice() * item.Quantity).ToString("c") + "</td>" +
				"</tr>";
			}

			this.Body += "</table>";

			this.Body += "<br /><br />";

			// Totals
			this.Body += "Subtotal: " + order.Subtotal.ToDecimal().ToString("c") + "<br />";
			this.Body += "Shipping: " + order.ShippingTotal.ToDecimal().ToString("c") + "<br />";
			this.Body += "Tax: " + order.TaxAmountTotal.ToDecimal().ToString("c") + "<br />";
			this.Body += "Grand Total: " + order.GrandTotal.ToDecimal().ToString("c") + "<br />";
		}

		protected void ReplaceTemplateValues(ref string emailTemplate, Dictionary<string, string> emailValues)
		{
			foreach (KeyValuePair<string, string> pair in emailValues)
			{
				emailTemplate = emailTemplate.Replace("{" + pair.Key + "}", pair.Value);
			}
		}

		#endregion
	}
}
