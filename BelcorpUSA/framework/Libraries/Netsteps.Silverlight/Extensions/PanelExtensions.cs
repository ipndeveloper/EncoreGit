using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Extensions
{
    public static class PanelExtensions
    {
        public static void MoveChildDown(this Panel panel, string Name)
        {
            List<ControlDisplayIndex> overviewModules = new List<ControlDisplayIndex>();

            // Assign correct ZIndex - JHE
            int i = 0;
            foreach (UserControl userControl in panel.Children)
            {
                if (userControl.GetZIndex() == 0)
                    userControl.SetZIndex(i);
                i++;
            }

            foreach (UserControl userControl in panel.Children)
                overviewModules.Add(new ControlDisplayIndex() { UserControl = userControl, Name = userControl.Name, ZIndex = userControl.GetZIndex() });

            // Find Control to swap - JHE
            UserControl userControlToSwap = null;
            foreach (ControlDisplayIndex controlDisplayIndex in overviewModules)
                if (controlDisplayIndex.Name == Name)
                    userControlToSwap = controlDisplayIndex.UserControl;

            // Find Control to swap with - JHE
            UserControl userControlToSwapWith = null;
            foreach (ControlDisplayIndex controlDisplayIndex in overviewModules)
                if (controlDisplayIndex.ZIndex == userControlToSwap.GetZIndex() + 1)
                    userControlToSwapWith = controlDisplayIndex.UserControl;

            if (userControlToSwap != null && userControlToSwapWith != null)
            {
                userControlToSwap.SetZIndex(userControlToSwap.GetZIndex() + 1);
                userControlToSwapWith.SetZIndex(userControlToSwap.GetZIndex() - 1);
            }
        }

        private class ControlDisplayIndex
        {
            public UserControl UserControl { get; set; }
            public int ZIndex { get; set; }
            public string Name { get; set; }
        }

        public static void ShowIfHasChildren(this Panel panel)
        {
            panel.Visibility = (panel.Children.Count > 0).ToVisibility();
        }

        public static void ShowIfHasVisibleChildren(this Panel panel)
        {
            panel.Visibility = Visibility.Collapsed;
            for (int i = 0; i < panel.Children.Count; i++)
            {
                if (panel.Children[i].Visibility == Visibility.Visible)
                {
                    panel.Visibility = Visibility.Visible;
                    break;
                }
            }
        }
    }
}
