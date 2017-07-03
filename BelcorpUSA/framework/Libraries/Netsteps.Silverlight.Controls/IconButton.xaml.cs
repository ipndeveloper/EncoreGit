using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace NetSteps.Silverlight.Controls
{
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	public partial class IconButton : UserControl
	{
		private bool isMouseOver;

		public IconButton()
		{
			InitializeComponent();

			this.MouseEnter += new MouseEventHandler(IconButton_MouseEnter);
			this.MouseLeave += new MouseEventHandler(IconButton_MouseLeave);
			this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(IconButton_IsEnabledChanged);

			GoToState(false);
		}

		private void IconButton_MouseEnter(object sender, MouseEventArgs e)
		{
			isMouseOver = true;
			GoToState(true);
		}

		private void IconButton_MouseLeave(object sender, MouseEventArgs e)
		{
			isMouseOver = false;
			GoToState(true);
		}

		private void IconButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			GoToState(true);
		}

		public static readonly DependencyProperty NormalImageSourceProperty = DependencyProperty.Register("NormalImageSource", typeof(BitmapImage), typeof(IconButton), new PropertyMetadata(NormalImageSourceChanged));

		public BitmapImage NormalImageSource
		{
			get
			{
				return (BitmapImage)GetValue(NormalImageSourceProperty);
			}

			set
			{
				SetValue(NormalImageSourceProperty, value);
			}
		}

		private static void NormalImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IconButton myControl = d as IconButton;

			if (myControl != null)
			{
				if (e.NewValue != null)
				{
					BitmapImage source = e.NewValue as BitmapImage;
					myControl.normalImage.Source = source;
				}
			}
			else
			{
				myControl.normalImage.Source = null;
			}
		}

		public static readonly DependencyProperty DisabledImageSourceProperty = DependencyProperty.Register("DisabledImageSource", typeof(BitmapImage), typeof(IconButton), new PropertyMetadata(DisabledImageSourceChanged));

		public BitmapImage DisabledImageSource
		{
			get
			{
				return (BitmapImage)GetValue(DisabledImageSourceProperty);
			}

			set
			{
				SetValue(DisabledImageSourceProperty, value);
			}
		}

		private static void DisabledImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IconButton myControl = d as IconButton;

			if (myControl != null)
			{
				if (e.NewValue != null)
				{
					BitmapImage source = e.NewValue as BitmapImage;
					myControl.disabledImage.Source = source;
				}
			}
			else
			{
				myControl.disabledImage.Source = null;
			}
		}

		public static readonly DependencyProperty MouseOverImageSourceProperty = DependencyProperty.Register("MouseOverImageSource", typeof(BitmapImage), typeof(IconButton), new PropertyMetadata(MouseOverImageSourceChanged));

		public BitmapImage MouseOverImageSource
		{
			get
			{
				return (BitmapImage)GetValue(MouseOverImageSourceProperty);
			}

			set
			{
				SetValue(MouseOverImageSourceProperty, value);
			}
		}

		private static void MouseOverImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IconButton myControl = d as IconButton;

			if (myControl != null)
			{
				if (e.NewValue != null)
				{
					BitmapImage source = e.NewValue as BitmapImage;
					myControl.mouseOverImage.Source = source;
				}
			}
			else
			{
				myControl.mouseOverImage.Source = null;
			}
		}

		private void GoToState(bool useTransitions)
		{
			if (!IsEnabled)
			{
				if (DisabledImageSource != null)
					VisualStateManager.GoToState(this, "Disabled", useTransitions);
				else
					VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			else if (isMouseOver)
			{
				if (MouseOverImageSource != null)
					VisualStateManager.GoToState(this, "MouseOver", useTransitions);
				else
					VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
		}
	}
}

