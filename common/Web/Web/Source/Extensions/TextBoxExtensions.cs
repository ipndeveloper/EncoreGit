using System;
using System.Web.UI.WebControls;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: TextBox Extensions
    /// Created: 03-18-2009
    /// </summary>
    public static class TextBoxExtensions
    {
        #region Validation Methods
        public static bool IsValidInt(this TextBox textBox)
        {
            return (textBox.Text.IsValidInt());
        }

        public static bool IsValidDateTime(this TextBox textBox)
        {
            return (textBox.Text.IsValidDateTime());
        }

        public static bool IsEmpty(this TextBox textBox)
        {
            return (textBox.Text.Trim() == string.Empty);
        }
        #endregion

        #region Conversion Methods
        public static int ToInt(this TextBox textBox)
        {
            return ToInt(textBox, 0);
        }
        public static int ToInt(this TextBox textBox, int defaultValue)
        {
            if (textBox.Text.IsValidInt())
                return Convert.ToInt32(textBox.Text);
            else
                return defaultValue;
        }

        public static DateTime ToDateTime(this TextBox textBox)
        {
            DateTime? returnValue = textBox.ToDateTime(DateTime.MinValue);
            return (returnValue == null) ? DateTime.MinValue : Convert.ToDateTime(returnValue);
        }
        public static DateTime? ToDateTime(this TextBox textBox, DateTime? defaultValue)
        {
            if (!textBox.IsValidDateTime())
                return defaultValue;

            DateTime date;
            if (DateTime.TryParse(textBox.Text, out date))
                return date;
            else
                return defaultValue;
        }

        public static string ToCleanString(this TextBox textBox)
        {
            return textBox.Text.Trim();
        }
        #endregion

        public static void SetPostBackControl(this TextBox textBox, LinkButton linkButton)
        {
            SetPostBackControl(textBox, linkButton.ClientID, true);
        }
        public static void SetPostBackControl(this TextBox textBox, Button button)
        {
            SetPostBackControl(textBox, button.ClientID, false);
        }
        public static void SetPostBackControl(this TextBox textBox, string controlClientId)
        {
            SetPostBackControl(textBox, controlClientId, false);
        }
        public static void SetPostBackControl(this TextBox textBox, string controlClientId, bool isLinkButton)
        {
            if (isLinkButton)
                textBox.Attributes.Add("onkeypress", "return clickLinkButton(event,'" + controlClientId + "')");
            else
                textBox.Attributes.Add("onkeypress", "return clickButton(event,'" + controlClientId + "')");
        }

        public static void AddTextWatermark(this TextBox textBox, string watermarkText)
        {
            textBox.Text = watermarkText.Trim();
            textBox.Attributes.Add("onfocus", "if(document.getElementById('" + textBox.ClientID + "').value=='" + watermarkText.Trim() + "'){this.select(value='')}");
            textBox.Attributes.Add("onBlur", "if(document.getElementById('" + textBox.ClientID + "').value==''){document.getElementById('" + textBox.ClientID + "').value='" + watermarkText.Trim() + "';}");
        }
    }
}
