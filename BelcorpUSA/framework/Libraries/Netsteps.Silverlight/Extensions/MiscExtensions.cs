using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NetSteps.Silverlight.Base;

namespace NetSteps.Silverlight.Extensions
{
    public static class MiscExtensions
    {
        public static bool ContainsIgnoreCase(this ThreadSafeList<string> list, string stringValue)
        {
            foreach (string entry in list)
                if (entry.ToString().ToUpper() == stringValue.ToString().ToUpper())
                    return true;
            return false;
        }

        public static bool ToBool(this Visibility value)
        {
            return (value == Visibility.Visible) ? true : false;
        }

        public static Visibility ToOpposite(this Visibility value)
        {
            return (value == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        public static bool ContainsChildControls(this Grid grid, Type type)
        {
            for (int i = grid.Children.Count - 1; i >= 0; i--)
                if (grid.Children[i].GetType() == type)
                    return true;
            return false;
        }

        public static void RemoveChildControls(this Grid grid, Type type)
        {
            try
            {
                for (int i = grid.Children.Count - 1; i >= 0; i--)
                    if (grid.Children[i].GetType() == type)
                        grid.Children.RemoveAt(i);
            }
            catch (Exception ex)
            {
                AppFactory.ExceptionManager.HandleError(ex);
            }
        }

        public static void RemoveChildOfChildTypeControls(this Grid grid, Type type, bool matchingType)
        {
            try
            {
                for (int i = grid.Children.Count - 1; i >= 0; i--)
                {
                    Type childType = null;
                    UIElement control = null;
                    if (grid.Children[i] is IChild && (grid.Children[i] as IChild).Child != null)
                    {
                        childType = (grid.Children[i] as IChild).Child.GetType();
                        control = (grid.Children[i] as IChild).Child;
                    }
                    else if (grid.Children[i] is Border && (grid.Children[i] as Border).Child != null)
                    {
                        childType = (grid.Children[i] as Border).Child.GetType();
                        control = (grid.Children[i] as Border).Child;
                    }

                    bool removeItem = false;
                    if (matchingType)
                    {
                        if (childType == type)
                            removeItem = true;
                        else
                            removeItem = false;
                    }
                    else
                    {
                        if (childType != type)
                            removeItem = true;
                        else
                            removeItem = false;
                    }


                    if (removeItem && childType != null)
                    {
                        TimerHelper.SetTimeout(2000, () =>
                        {
                            if (control is IDisposable)
                                (control as IDisposable).Dispose();
                        });

                        grid.Children.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFactory.ExceptionManager.HandleError(ex);
            }
        }

        public static List<UIElement> GetChildControls(this Grid grid, Type type)
        {
            try
            {
                List<UIElement> result = new List<UIElement>();

                for (int i = grid.Children.Count - 1; i >= 0; i--)
                    if (grid.Children[i].GetType() == type)
                        result.Add(grid.Children[i]);

                return result;
            }
            catch (Exception ex)
            {
                AppFactory.ExceptionManager.HandleError(ex);
                return null;
            }
        }

        public static void MakeColorsTransparent(this Border border)
        {
            border.BorderBrush = CommonResources.SolidBrushes.Transparent;
            border.Background = CommonResources.SolidBrushes.Transparent;
        }

        public static void SetAllColorBrushes(this Border border, Color color)
        {
            border.BorderBrush = color.ToSolidColorBrush();
            border.Background = color.ToSolidColorBrush();
        }


        public static void WireupErrorEventHandler(this IErrorMessage userControl, ErrorEventHandler errorEventHandler)
        {
            if (userControl != null)
            {
                userControl.ErrorOccured -= errorEventHandler;
                userControl.ErrorOccured += errorEventHandler;
            }
        }

        public static void WireupStateChangedEventHandler(this IStateChanged userControl, StateEventHandler stateEventHandler)
        {
            if (userControl != null)
            {
                userControl.StateChanged -= stateEventHandler;
                userControl.StateChanged += stateEventHandler;
            }
        }

        public static void WireupCloseWindowEventHandler(this ICloseWindow userControl, EventHandler eventHandler)
        {
            if (userControl != null)
            {
                userControl.CloseWindow -= eventHandler;
                userControl.CloseWindow += eventHandler;
            }
        }
    }
}
