using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 11/10/2009
    /// </summary>
    public class GridElementVisibilityToggler
    {
        private Grid _grid = null;
        private Dictionary<int, RowHeight> _rowHeights = new Dictionary<int, RowHeight>();
        private Dictionary<int, ColumnWidth> _columnWidths = new Dictionary<int, ColumnWidth>();

        public GridElementVisibilityToggler(Grid grid)
        {
            _grid = grid;

            for (int i = 0; i < grid.RowDefinitions.Count; i++)
                _rowHeights.Add(i, new RowHeight()
                {
                    Height = _grid.RowDefinitions[i].Height,
                    MinHeight = _grid.RowDefinitions[i].MinHeight,
                    MaxHeight = _grid.RowDefinitions[i].MaxHeight
                });

            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
                _columnWidths.Add(i, new ColumnWidth()
                {
                    Width = _grid.ColumnDefinitions[i].Width,
                    MinWidth = _grid.ColumnDefinitions[i].MinWidth,
                    MaxWidth = _grid.ColumnDefinitions[i].MaxWidth
                });
        }

        /// <summary>
        /// For this to work the row Heights must start out visible (height greater than 0 before calling this) - JHE
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="visibility"></param>
        public void SetRowVisibility(int rowIndex, Visibility visibility)
        {
            if (_grid != null)
            {
                if (_grid.RowDefinitions.Count - 1 >= rowIndex)
                {
                    if (visibility == Visibility.Visible)
                    {
                        _grid.RowDefinitions[rowIndex].Height = _rowHeights[rowIndex].Height;
                        _grid.RowDefinitions[rowIndex].MinHeight = _rowHeights[rowIndex].MinHeight;
                        _grid.RowDefinitions[rowIndex].MaxHeight = _rowHeights[rowIndex].MaxHeight;
                    }
                    else
                    {
                        _grid.RowDefinitions[rowIndex].Height = new GridLength(0);
                        _grid.RowDefinitions[rowIndex].MinHeight = 0;
                        _grid.RowDefinitions[rowIndex].MaxHeight = 0;
                    }
                }
            }
        }
        public void SetColumnVisibility(int columnIndex, Visibility visibility)
        {
            if (_grid != null)
            {
                if (_grid.ColumnDefinitions.Count - 1 >= columnIndex)
                {
                    if (visibility == Visibility.Visible)
                    {
                        _grid.ColumnDefinitions[columnIndex].Width = _columnWidths[columnIndex].Width;
                        _grid.ColumnDefinitions[columnIndex].MinWidth = _columnWidths[columnIndex].MinWidth;
                        _grid.ColumnDefinitions[columnIndex].MaxWidth = _columnWidths[columnIndex].MaxWidth;
                    }
                    else
                    {
                        _grid.ColumnDefinitions[columnIndex].Width = new GridLength(0);
                        _grid.ColumnDefinitions[columnIndex].MinWidth = 0;
                        _grid.ColumnDefinitions[columnIndex].MaxWidth = 0;
                    }
                }
            }
        }

        public void ToggleRowVisibility(int rowIndex)
        {
            if (_grid != null)
            {
                if (_grid.RowDefinitions.Count - 1 >= rowIndex)
                {
                    if (_grid.RowDefinitions[rowIndex].Height.Value == 0)
                    {
                        _grid.RowDefinitions[rowIndex].Height = _rowHeights[rowIndex].Height;
                        _grid.RowDefinitions[rowIndex].MinHeight = _rowHeights[rowIndex].MinHeight;
                        _grid.RowDefinitions[rowIndex].MaxHeight = _rowHeights[rowIndex].MaxHeight;
                    }
                    else
                    {
                        _grid.RowDefinitions[rowIndex].Height = new GridLength(0);
                        _grid.RowDefinitions[rowIndex].MinHeight = 0;
                        _grid.RowDefinitions[rowIndex].MaxHeight = 0;
                    }
                }
            }
        }
        public void ToggleColumnVisibility(int columnIndex)
        {
            if (_grid != null)
            {
                if (_grid.ColumnDefinitions.Count - 1 >= columnIndex)
                {
                    if (_grid.ColumnDefinitions[columnIndex].Width.Value == 0)
                    {
                        _grid.ColumnDefinitions[columnIndex].Width = _columnWidths[columnIndex].Width;
                        _grid.ColumnDefinitions[columnIndex].MinWidth = _columnWidths[columnIndex].MinWidth;
                        _grid.ColumnDefinitions[columnIndex].MaxWidth = _columnWidths[columnIndex].MaxWidth;
                    }
                    else
                    {
                        _grid.ColumnDefinitions[columnIndex].Width = new GridLength(0);
                        _grid.ColumnDefinitions[columnIndex].MinWidth = 0;
                        _grid.ColumnDefinitions[columnIndex].MaxWidth = 0;
                    }
                }
            }
        }
    }

    public class RowHeight
    {
        public GridLength Height { get; set; }
        public double MinHeight { get; set; }
        public double MaxHeight { get; set; }
    }

    public class ColumnWidth
    {
        public GridLength Width { get; set; }
        public double MinWidth { get; set; }
        public double MaxWidth { get; set; }
    }
}
