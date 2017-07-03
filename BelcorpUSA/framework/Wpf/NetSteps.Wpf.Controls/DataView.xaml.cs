using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Wpf.Controls.Extensions;

namespace NetSteps.Wpf.Controls
{
	public partial class DataView : UserControl, INotifyPropertyChanged
	{
		#region Members
		private List<TextBlock> _labels = new List<TextBlock>();
		private List<TextBlock> _values = new List<TextBlock>();
		private List<Border> _alternateRows = new List<Border>();
		#endregion

		#region Properties
		public new static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), typeof(DataView), null);
		public new object DataContext
		{
			get
			{
				return (object)GetValue(DataContextProperty);
			}
			set
			{
				SetValue(DataContextProperty, value);
				NotifyPropertyChanged("DataContext");

				List<NameValue<string, string>> values = new List<NameValue<string, string>>();
				if (value is List<NameValue<string, string>>)
					values = (List<NameValue<string, string>>)value;
				else
				{
					Type type = value.GetType();
					var props = type.GetPropertiesCached();
					props.RemoveAll((pi) => !Reflection.SystemCoreTypes.Contains(pi.PropertyType));

					// Filter out properties we shouldn't show - JHE
					List<string> businessLogicProperties = new List<string> { "SuppressEntityEvents", "Error", "IsValid", "Item", "String" };
					foreach (var item in businessLogicProperties)
						props.RemoveWhere((n) => n.Name == item);

					foreach (PropertyInfo pi in props)
					{
						NameValue<string, string> nameValue = new NameValue<string, string>();
						nameValue.Name = pi.Name;
						object propertyValue = pi.GetValue(value, null);

						if (propertyValue is DateTime && Convert.ToDateTime(propertyValue).IsNullOrEmpty())
							nameValue.Value = "N/A";
						else
						{
							object val = pi.GetValue(value, null);
							nameValue.Value = (val == null) ? string.Empty : val.ToString().ToCleanString();
						}

						values.Add(nameValue);
					}
				}

				// Change Form - JHE
				FillView(values);
			}
		}

		public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register("LabelStyle", typeof(Style), typeof(DataView), null);
		public Style LabelStyle
		{
			get
			{
				return (Style)GetValue(LabelStyleProperty);
			}
			set
			{
				SetValue(LabelStyleProperty, value);
				NotifyPropertyChanged("LabelStyle");

				foreach (var label in _labels)
					label.Style = value;
			}
		}

		public static readonly DependencyProperty ValueStyleProperty = DependencyProperty.Register("ValueStyle", typeof(Style), typeof(DataView), null);
		public Style ValueStyle
		{
			get
			{
				return (Style)GetValue(ValueStyleProperty);
			}
			set
			{
				SetValue(ValueStyleProperty, value);
				NotifyPropertyChanged("ValueStyle");

				foreach (var eachValue in _values)
					eachValue.Style = value;
			}
		}

		public static readonly DependencyProperty AlternatingRowStyleProperty = DependencyProperty.Register("AlternatingRowStyle", typeof(Style), typeof(DataView), null);
		public Style AlternatingRowStyle
		{
			get
			{
				return (Style)GetValue(AlternatingRowStyleProperty);
			}
			set
			{
				SetValue(AlternatingRowStyleProperty, value);
				NotifyPropertyChanged("AlternatingRowStyle");

				foreach (var eachValue in _alternateRows)
					eachValue.Style = value;
			}
		}

		public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(GridLength), typeof(DataView), null);
		public GridLength RowHeight
		{
			get
			{
				return (GridLength)GetValue(RowHeightProperty);
			}
			set
			{
				SetValue(RowHeightProperty, value);
				NotifyPropertyChanged("RowHeight");

				foreach (var row in LayoutRoot.RowDefinitions)
					row.Height = value;
			}
		}
		#endregion

		#region Initialize
		public DataView()
		{
			InitializeComponent();
			Loaded += new RoutedEventHandler(UserControl_Loaded);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{

		}
		#endregion

		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		#region Methods
		private void FillView(List<NameValue<string, string>> values)
		{
			LayoutRoot.Children.Clear();
			_labels.Clear();
			_values.Clear();

			int row = 0;
			foreach (var item in values)
			{
				TextBlock label = new TextBlock() { Text = item.Name, VerticalAlignment = VerticalAlignment.Center };
				TextBlock value = new TextBlock() { Text = item.Value, VerticalAlignment = VerticalAlignment.Center };
				label.SetGrid(row, 0);
				value.SetGrid(row, 1);

				if (!row.IsEven())
				{
					Border border = new Border();
					if (AlternatingRowStyle != null)
						border.Style = AlternatingRowStyle;
					_alternateRows.Add(border);
					border.SetGrid(row, 0);
					border.SetValue(Grid.ColumnSpanProperty, 2);
					LayoutRoot.Children.Add(border);
				}

				LayoutRoot.Children.Add(label);
				LayoutRoot.Children.Add(value);

				if (LabelStyle != null)
					label.Style = LabelStyle;
				if (ValueStyle != null)
					value.Style = ValueStyle;

				_labels.Add(label);
				_values.Add(value);

				row++;

				RowDefinition rowDefinition = new RowDefinition();
				rowDefinition.Height = (RowHeight != null) ? RowHeight : new GridLength(20);

				LayoutRoot.RowDefinitions.Add(rowDefinition);
			}
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
