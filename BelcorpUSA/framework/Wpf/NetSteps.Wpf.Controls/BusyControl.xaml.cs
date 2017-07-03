using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NetSteps.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for BusyControl.xaml
	/// </summary>
	public partial class BusyControl : UserControl
	{
		public BusyControl()
		{
			InitializeComponent();
			this.Visibility = System.Windows.Visibility.Collapsed;
		}

		public void Start()
		{
			this.Visibility = System.Windows.Visibility.Visible;
			Storyboard BusyAnimation = (Storyboard)this.Resources["BusyAnimation"];
			BusyAnimation.Begin();
		}

		public void Stop()
		{
			this.Visibility = System.Windows.Visibility.Collapsed;
			Storyboard BusyAnimation = (Storyboard)this.Resources["BusyAnimation"];
			BusyAnimation.Stop();
		}
	}
}
