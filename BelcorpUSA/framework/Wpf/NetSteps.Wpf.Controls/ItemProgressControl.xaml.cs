using System;
using System.Windows.Controls;
using NetSteps.Common.Events;
using NetSteps.Common.Extensions;

namespace NetSteps.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for ItemProgressControl.xaml
	/// </summary>
	public partial class ItemProgressControl : UserControl
	{
		#region Properties
		public bool IsComplete { get; set; }

		public string ProcessTitle
		{
			get
			{
				return uxProcessTitle.Text;
			}
			set
			{
				uxProcessTitle.Text = value;
			}
		}
		#endregion

		public ItemProgressControl()
		{
			InitializeComponent();
			this.Visibility = System.Windows.Visibility.Collapsed;
		}

		#region Events
		public event EventHandler CancelProcess;
		protected virtual void OnCancelProcess(object sender, EventArgs e)
		{
			if (CancelProcess != null)
				CancelProcess(this, e);
		}
		#endregion

		public void UpdateProgress(ProgressEventArgs e)
		{
			this.Visibility = System.Windows.Visibility.Visible;
			progressBar.Value = e.PercentComplete;
			uxProgressText.Text = string.Format("{0}/{1} ({2}%) complete", e.ItemsProcessed, e.TotalItems, e.PercentComplete.TruncateDoubleInsertCommas(2));
			uxCurrentItemText.Text = e.CurrentItemDescription;
			uxTimeLeft.Text = string.Format("{0} left", e.EstimatedTimeLeft.HasValue ? e.EstimatedTimeLeft.Value.ToFriendlyString(true) : string.Empty);
			IsComplete = e.IsComplete;

			if (IsComplete)
				uxTimeLeft.Text = string.Format("1 per {0} average", e.AverageTimePerItem.HasValue ? e.AverageTimePerItem.Value.ToFriendlyString(true) : string.Empty);
		}

		private void uxClose_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (IsComplete)
				this.Visibility = System.Windows.Visibility.Collapsed;
			else
				OnCancelProcess(this, null);
		}
	}
}
