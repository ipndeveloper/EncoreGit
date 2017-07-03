using System;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Events
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Event and Args to relay updated progress of a process.
	/// Created: 08-13-2010
	/// </summary>
	public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

	public class ProgressEventArgs : EventArgs
	{
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public int TotalItems { get; set; }
		public int ItemsProcessed { get; set; }
		public string CurrentItemDescription { get; set; }

		public bool IsComplete
		{
			get
			{
				return (ItemsProcessed == TotalItems);
			}
		}

		public int RemainingItemsCount
		{
			get
			{
				return (TotalItems - ItemsProcessed);
			}
		}

		public double PercentComplete
		{
			get
			{
				if (TotalItems == ItemsProcessed)
					return 100;
				else
					return (Convert.ToDouble(100) / Convert.ToDouble(TotalItems) * Convert.ToDouble(ItemsProcessed));
			}
		}

		public TimeSpan? EstimatedTimeLeft
		{
			get
			{
				if (StartDate == null)
					return null;
				else if (ItemsProcessed > 0)
				{
					var millisecondsLeft = (DateTime.Now.ApplicationNow() - StartDate.ToDateTime()).TotalMilliseconds / ItemsProcessed * RemainingItemsCount;
					return TimeSpan.FromMilliseconds(millisecondsLeft);
				}
				else
					return null;
			}
		}

		public TimeSpan? AverageTimePerItem
		{
			get
			{
				if (StartDate == null || EndDate == null)
					return null;
				else if (ItemsProcessed > 0)
				{
					var millisecondsTotal = (EndDate.ToDateTime() - StartDate.ToDateTime()).TotalMilliseconds / ItemsProcessed;
					return TimeSpan.FromMilliseconds(millisecondsTotal);
				}
				else
					return null;
			}
		}

		public ProgressEventArgs(int totalItems, int itemsProcessed, string currentItemDescription)
		{
			TotalItems = totalItems;
			ItemsProcessed = itemsProcessed;
			CurrentItemDescription = currentItemDescription;
		}

		public ProgressEventArgs(DateTime? startDate, int totalItems, int itemsProcessed, string currentItemDescription)
		{
			StartDate = startDate;
			TotalItems = totalItems;
			ItemsProcessed = itemsProcessed;
			CurrentItemDescription = currentItemDescription;

			if (totalItems == itemsProcessed)
				EndDate = DateTime.Now.ApplicationNow();
		}
	}
}
