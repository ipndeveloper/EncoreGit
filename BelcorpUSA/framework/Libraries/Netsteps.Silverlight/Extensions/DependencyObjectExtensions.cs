using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetSteps.Silverlight.Extensions
{
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/questions/693848/is-there-a-way-to-iterate-in-a-listbox-items-templates - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            // Search immediate children    
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static void RemoveChild(this DependencyObject obj, UIElement uIElement)
        {
            if (obj is Panel)
                (obj as Panel).Children.Remove(uIElement);
            else if (obj is Border)
                (obj as Border).Child = null;
        }

        /// <summary>
        /// http://www.silverlightissues.com/silverlight-framework/get-all-child-controls-recursively.html - JHE
        /// Example:
        /// var buttons = DependencyObjectExtensions.GetChildrenRecursive(itemsControl).OfType<Button>();
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetChildrenRecursive(DependencyObject root)
        {
            List<DependencyObject> elts = new List<DependencyObject>();
            elts.Add(root);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                elts.AddRange(GetChildrenRecursive(VisualTreeHelper.GetChild(root, i)));

            return elts;
        }

        public static T GetChildByNameRecursive<T>(this DependencyObject root, string name) where T : class
        {
            var elements = DependencyObjectExtensions.GetChildrenRecursive(root).OfType<T>();
            foreach (T element in elements)
                if (element is FrameworkElement && !(element as FrameworkElement).Name.IsNullOrEmpty() && (element as FrameworkElement).Name == name)
                    return element;

            return null;
        }

    }
}
