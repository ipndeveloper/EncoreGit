using System.Windows.Controls;

namespace NetSteps.Silverlight
{
    public static class TranslationExtensions
    {
        #region Extention Methods
        public static string GetText(this RadioButton radioButton)
        {
            return (radioButton.Content != null) ? radioButton.Content.ToString().Trim() : string.Empty;
        }

        public static string GetText(this CheckBox checkBox)
        {
            return (checkBox.Content != null) ? checkBox.Content.ToString().Trim() : string.Empty;
        }

        public static string GetText(this Button button)
        {
            return (button.Content != null) ? button.Content.ToString().Trim() : string.Empty;
        }
        #endregion
    }
}
