using System.Linq;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Extensions
{
    public static class ItemsControlExtensions
    {
        public static void SetCurrent(this ItemsControl itemsControl, Button selectedButton)
        {
            itemsControl.UpdateLayout();
            var buttons = DependencyObjectExtensions.GetChildrenRecursive(itemsControl).OfType<Button>();
            foreach (Button button in buttons)
            {
                if (button == selectedButton)
                    button.IsEnabled = false;
                else
                    button.IsEnabled = true;
            }
        }
    }
}
