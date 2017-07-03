using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Extensions
{
    public static class UserControlExtensions
    {
        public static void SetZIndex(this FrameworkElement frameworkElement, int value)
        {
            frameworkElement.SetValue(Canvas.ZIndexProperty, value);
        }

        public static int GetZIndex(this FrameworkElement frameworkElement)
        {
            return frameworkElement.GetValue(Canvas.ZIndexProperty).ToString().ToInt();
        }

        public static List<UserControl> SubUserControls(this UserControl userControl, UIElement rootElement)
        {
            List<UserControl> list = new List<UserControl>();
            IterateControls(rootElement, ref list);
            return list;
        }

        public static void IterateControls(UIElement element, ref List<UserControl> list)
        {
            if (element is UserControl)
            {
                //return this User Control
                list.Add((UserControl)element);
            }
            else if (element is Grid)
            {
                foreach (UIElement childElement in (element as Grid).Children)
                {
                    if (childElement is UserControl ||
                        childElement is Grid ||
                        childElement is Border ||
                        childElement is StackPanel ||
                        childElement is Button)
                    {
                        IterateControls(childElement, ref list);
                    }
                }
            }
            else if (element is Button)
            {
                if (((Button)element).Content is UserControl ||
                        ((Button)element).Content is Grid ||
                        ((Button)element).Content is Border ||
                        ((Button)element).Content is StackPanel ||
                        ((Button)element).Content is Button)
                {
                    IterateControls((UIElement)((Button)element).Content, ref list);
                }
            }
            else if (element is StackPanel)
            {
                foreach (UIElement childElement in (element as StackPanel).Children)
                {
                    if (childElement is UserControl ||
                        childElement is Grid ||
                        childElement is Border ||
                        childElement is StackPanel ||
                        childElement is Button)
                    {
                        IterateControls(childElement, ref list);
                    }
                }
            }
            else if (element is Border)
            {
                if (((Border)element).Child is UserControl ||
                    ((Border)element).Child is Grid ||
                    ((Border)element).Child is Border ||
                    ((Border)element).Child is StackPanel ||
                    ((Border)element).Child is Button)
                {
                    IterateControls(((Border)element).Child, ref list);
                }

            }
        }

        public static void Dispose(this UserControl userControl)
        {


        }

        public static void RemoveSelfFromParent(this UserControl userControl)
        {
            if (userControl.Parent != null)
            {
                if (userControl.Parent is Panel)
                    (userControl.Parent as Panel).Children.Remove(userControl);
                else if (userControl.Parent is Border)
                    (userControl.Parent as Border).Child = null;
            }
        }
    }
}
