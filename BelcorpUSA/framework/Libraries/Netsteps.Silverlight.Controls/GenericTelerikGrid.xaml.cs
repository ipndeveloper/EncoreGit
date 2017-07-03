using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using NetSteps.Silverlight.Objects.DataSet;
using Telerik.Windows.Controls;
using ColumnDefinition = NetSteps.Silverlight.Objects.DataSet.ColumnDefinition;

namespace NetSteps.Silverlight.Controls
{
    public partial class GenericTelerikGrid : UserControl
    {
        public GenericTelerikGrid()
        {
            InitializeComponent();
            uniqueNames = new List<string>();
        }

        public object SelectedItem
        {
            get
            {
                return uxTelerikGrid.SelectedItem;
            }
        }

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(DataSet), typeof(GenericTelerikGrid), null);
        public DataSet DataSource
        {
            get
            {
                return (DataSet)GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
                updateColumnDefinitions();
            }
        }

        public static readonly DependencyProperty HeaderCellStyleProperty = DependencyProperty.Register("HeaderCellStyle", typeof(Style), typeof(GenericTelerikGrid), null);
        public Style HeaderCellStyle
        {
            get
            {
                return (Style)GetValue(HeaderCellStyleProperty);
            }
            set
            {
                SetValue(HeaderCellStyleProperty, value);

                foreach (GridViewColumn column in uxTelerikGrid.Columns)
                    column.HeaderCellStyle = HeaderCellStyle;
            }
        }

        private List<string> uniqueNames;

        private GridViewDataColumn convertToTelerikColumnDefinition(ColumnDefinition columnDefinition, int columnIndex)
        {
            GridViewDataColumn result = new GridViewDataColumn();
            result.IsReadOnly = columnDefinition.IsReadOnly;
            result.Header = columnDefinition.Name;

            string uniqueName = columnDefinition.Name.Trim();
            if (uniqueNames.Contains(uniqueName))
            {
                int uniqueNameID = 1;
                while (uniqueNames.Contains(uniqueName + uniqueNameID.ToString()))
                    ++uniqueNameID;

                uniqueName = uniqueName + uniqueNameID.ToString();
            }

            result.UniqueName = uniqueName;
            uniqueNames.Add(uniqueName);

            switch (columnDefinition.DataType)
            {
                case DataType.Bool:
                    result.DataMemberBinding = new Binding("Column" + columnIndex.ToString() + "AsBoolValue") { Mode = BindingMode.OneWay };
                    break;
                case DataType.Int:
                    result.DataMemberBinding = new Binding("Column" + columnIndex.ToString() + "AsIntValue") { Mode = BindingMode.OneWay };
                    break;
                case DataType.DateTime:
                    result.DataMemberBinding = new Binding("Column" + columnIndex.ToString() + "AsDateTimeValue") { Mode = BindingMode.OneWay };
                    break;
                case DataType.Double:
                    result.DataMemberBinding = new Binding("Column" + columnIndex.ToString() + "AsDoubleValue") { Mode = BindingMode.OneWay };
                    break;
                case DataType.String:
                    result.DataMemberBinding = new Binding("Column" + columnIndex.ToString() + "AsStringValue") { Mode = BindingMode.OneWay };
                    break;
            }

            result.HeaderCellStyle = HeaderCellStyle;

            return result;
        }

        private void updateColumnDefinitions()
        {
            uxTelerikGrid.Columns.Clear();
            uniqueNames.Clear();

            for (int i = 0; i < DataSource.GetColumnCount(); ++i)
            {
                ColumnDefinition columnDefinition = DataSource.GetColumnDefinitionAtIndex(i);
                if (columnDefinition.IsVisible)
                    uxTelerikGrid.Columns.Add(convertToTelerikColumnDefinition(columnDefinition, i));
            }

            uxTelerikGrid.ItemsSource = DataSource.BindableRows;
        }
    }
}
