using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using NetSteps.Common.Extensions;

namespace NetSteps.Wpf.Controls.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Wpf Chart Extension methods
    /// Created: 08-31-2010
    /// </summary>
    public static class ChartExtensions
    {
        public static void FillWithDailyStatsForCurrentMonth<T>(this Chart chart, IEnumerable<T> data, Func<T, DateTime> dateTimeSelector, string chartTitle = "")
        {
            chart.FillWithDailyStatsForMonth(data, dateTimeSelector, DateTime.Now.Month, chartTitle);
        }

        public static void FillWithDailyStatsForMonth<T>(this Chart chart, IEnumerable<T> data, Func<T, DateTime> dateTimeSelector, int month, string chartTitle = "")
        {
            if (chart != null)
            {
                var dailyStats = data.GroupBy(d => dateTimeSelector(d).Midnight()).Select(g => new
                {
                    Date = g.Key,
                    Total = g.Count()
                });

                // Filter down to just this month - JHE
                dailyStats = dailyStats.WithInMonth(s => s.Date, month).ToList();

                chart.AddColumnSeriesDataForDailyStats(dailyStats);
                chart.SetDateTimeAxisLabelForMonthlyDay();

                chart.Title = chartTitle;
            }
        }

        public static void FillWithDailyStatsWithInDayRange<T>(this Chart chart, IEnumerable<T> data, Func<T, DateTime> dateTimeSelector, DateTime startDate, DateTime endDate, string chartTitle = "")
        {
            if (chart != null)
            {
                var dailyStats = data.GroupBy(d => dateTimeSelector(d).Midnight()).Select(g => new
                {
                    Date = g.Key,
                    Total = g.Count()
                });

                // Filter down to date range - JHE
                dailyStats = dailyStats.WithInDayRange(s => s.Date, startDate, endDate).ToList();

                chart.AddColumnSeriesDataForDailyStats(dailyStats);
                chart.SetDateTimeAxisLabelForMonthlyDay();

                chart.Title = chartTitle;
            }
        }

        public static void AddColumnSeriesDataForDailyStats<T>(this Chart chart, IEnumerable<T> dailyStats)
        {
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.IndependentValueBinding = new System.Windows.Data.Binding("Date");
            columnSeries.DependentValueBinding = new System.Windows.Data.Binding("Total");
            columnSeries.ItemsSource = dailyStats;
            chart.Series.Clear();
            chart.Series.Add(columnSeries);
        }

        public static void SetDateTimeAxisLabelForMonthlyDay(this Chart chart)
        {
            Style style = new Style(typeof(DateTimeAxisLabel));
            style.Setters.Add(new Setter(System.Windows.Controls.ContentControl.ContentStringFormatProperty, "{}{0:MMM d}"));
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            dateTimeAxis.AxisLabelStyle = style;
            dateTimeAxis.Orientation = AxisOrientation.X;
            chart.Axes.Clear();
            chart.Axes.Add(dateTimeAxis);
        }



        public static void AddLineSeriesDataToChart(this Chart chart, List<KeyValuePair<DateTime, int>> data, DateTime startDate, DateTime endDate, string seriesTitle = "")
        {
            if (chart != null)
            {
                var dailyStats = data;
                // Filter down to date range - JHE
                dailyStats = dailyStats.WithInDayRange(s => s.Key, startDate, endDate).ToList();

                chart.AddLineSeriesDataForDailyStats(dailyStats, seriesTitle);
                //chart.SetDateTimeAxisLabelForMonthlyDay();

                //chart.Title = chartTitle;
            }
        }

        public static void AddLineSeriesDataForDailyStats<T>(this Chart chart, IEnumerable<T> dailyStats, string title)
        {
            LineSeries lineSeries = new LineSeries();
            lineSeries.IndependentValueBinding = new System.Windows.Data.Binding("Key");
            lineSeries.DependentValueBinding = new System.Windows.Data.Binding("Value");
            //lineSeries.IndependentValuePath = "Name";
            //lineSeries.DependentValuePath = "Value";
            lineSeries.ItemsSource = dailyStats;
            lineSeries.Title = title;
            chart.Series.Add(lineSeries);
        }




        public static void AddPieSeriesDataToChart(this Chart chart, List<KeyValuePair<string, int>> data, DateTime startDate, DateTime endDate, string chartTitle = "")
        {
            if (chart != null)
            {
                var dailyStats = data;

                chart.AddPieSeriesDataForDailyStats(dailyStats, chartTitle);
                //chart.SetDateTimeAxisLabelForMonthlyDay();

                chart.Title = chartTitle;
            }
        }
        public static void AddPieSeriesDataForDailyStats<T>(this Chart chart, IEnumerable<T> dailyStats, string title)
        {
            if (dailyStats != null && dailyStats.Count() > 0)
            {
                PieSeries pieSeries = new PieSeries();
                pieSeries.IndependentValueBinding = new System.Windows.Data.Binding("Key");
                pieSeries.DependentValueBinding = new System.Windows.Data.Binding("Value");
                //lineSeries.IndependentValuePath = "Name";
                //lineSeries.DependentValuePath = "Value";
                pieSeries.ItemsSource = dailyStats;
                pieSeries.Title = title;
                chart.Series.Add(pieSeries);
            }
        }
    }
}
