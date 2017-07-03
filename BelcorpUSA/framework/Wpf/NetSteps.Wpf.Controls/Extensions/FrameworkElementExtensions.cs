using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NetSteps.Wpf.Controls.Extensions
{
	public static class FrameworkElementExtensions
	{
		#region Methods
		public static void SetSizeToParentsSize(this FrameworkElement userControl)
		{
			SetSizeToParentsSize(userControl, true);
		}
		public static void SetSizeToParentsSize(this FrameworkElement userControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();
				double width = (userControl.Parent as FrameworkElement).Width;
				double height = (userControl.Parent as FrameworkElement).Height;
				if (double.IsNaN(width))
					width = (userControl.Parent as FrameworkElement).ActualWidth;
				if (double.IsNaN(height))
					height = (userControl.Parent as FrameworkElement).ActualHeight;
				if (width != 0 && height != 0)
				{
					userControl.Width = width;
					userControl.Height = height;
				}
			}
		}

		public static void SetSizeToParentsActualSize(this FrameworkElement userControl)
		{
			SetSizeToParentsActualSize(userControl, true);
		}
		public static void SetSizeToParentsActualSize(this FrameworkElement userControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();
				double width = (userControl.Parent as FrameworkElement).ActualWidth;
				double height = (userControl.Parent as FrameworkElement).ActualHeight;
				if (width != 0 && height != 0)
				{
					userControl.Width = width;
					userControl.Height = height;
				}
			}
		}

		public static void SetSizeToParentsActualWidth(this FrameworkElement userControl)
		{
			SetSizeToParentsActualWidth(userControl, true);
		}
		public static void SetSizeToParentsActualWidth(this FrameworkElement userControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();
				double width = (userControl.Parent as FrameworkElement).ActualWidth;
				if (width != 0)
					userControl.Width = width;
			}
		}

		public static void SetSizeToParentsActualHeight(this FrameworkElement userControl)
		{
			SetSizeToParentsActualHeight(userControl, true);
		}
		public static void SetSizeToParentsActualHeight(this FrameworkElement userControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();
				double height = (userControl.Parent as FrameworkElement).ActualHeight;
				if (height != 0)
					userControl.Height = height;
			}
		}

		public static void SetSizeToControlsSize(this FrameworkElement userControl, FrameworkElement parentUserControl)
		{
			SetSizeToControlsSize(userControl, parentUserControl, true);
		}
		public static void SetSizeToControlsSize(this FrameworkElement userControl, FrameworkElement parentUserControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();
				double width = parentUserControl.Width;
				double height = parentUserControl.Height;
				if (double.IsNaN(width))
					width = parentUserControl.ActualWidth;
				if (double.IsNaN(height))
					height = parentUserControl.ActualHeight;
				if (width != 0 && height != 0)
				{
					userControl.Width = width;
					userControl.Height = height;
				}
			}
		}


		public static void SetSizeToParentsActualWidthCalcAttribs(this FrameworkElement userControl)
		{
			SetSizeToParentsActualWidthCalcAttribs(userControl, true);
		}
		public static void SetSizeToParentsActualWidthCalcAttribs(this FrameworkElement userControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();

				FrameworkElement frameworkElement = (userControl.Parent as FrameworkElement);
				double width = frameworkElement.ActualWidth;

				if (frameworkElement is Border)
				{
					Thickness border = (Thickness)frameworkElement.GetValue(Border.BorderThicknessProperty);
					Thickness padding = (Thickness)frameworkElement.GetValue(Border.PaddingProperty);

					width = width - border.Left - border.Right - padding.Left - padding.Right;
				}

				if (width != 0)
					userControl.Width = width;
			}
		}

		public static void SetSizeToParentsActualHeigthCalcAttribs(this FrameworkElement userControl)
		{
			SetSizeToParentsActualHeigthCalcAttribs(userControl, true);
		}
		public static void SetSizeToParentsActualHeigthCalcAttribs(this FrameworkElement userControl, bool updateLayoutFirst)
		{
			if (userControl.Parent != null)
			{
				if (updateLayoutFirst)
					userControl.UpdateLayout();

				FrameworkElement frameworkElement = (userControl.Parent as FrameworkElement);
				double height = frameworkElement.ActualHeight;

				if (frameworkElement is Border)
				{
					Thickness border = (Thickness)frameworkElement.GetValue(Border.BorderThicknessProperty);
					Thickness padding = (Thickness)frameworkElement.GetValue(Border.PaddingProperty);

					height = height - border.Top - border.Bottom;
				}

				if (height != 0)
					userControl.Height = height;
			}
		}
		#endregion

		public static void SetGrid(this FrameworkElement frameworkElement, int row, int column)
		{
			if (frameworkElement != null)
			{
				frameworkElement.SetValue(Grid.RowProperty, row);
				frameworkElement.SetValue(Grid.ColumnProperty, column);
			}
		}

		public static void SetCanvas(this FrameworkElement frameworkElement, double left, double top)
		{
			if (frameworkElement != null)
			{
				frameworkElement.SetValue(Canvas.LeftProperty, left);
				frameworkElement.SetValue(Canvas.TopProperty, top);
			}
		}

		/// <summary>
		/// Use this when binding to Properties or Entities that do not support IPropertyNotify... - JHE
		/// </summary>
		/// <param name="frameworkElement"></param>
		/// <param name="dp"></param>
		public static void UpdateBinding(this FrameworkElement frameworkElement, DependencyProperty dp)
		{
			if (frameworkElement != null)
			{
				BindingExpression bindExp = frameworkElement.GetBindingExpression(dp);
				Binding bind = bindExp.ParentBinding;
				frameworkElement.SetBinding(dp, bind);
			}
		}

	}
}
