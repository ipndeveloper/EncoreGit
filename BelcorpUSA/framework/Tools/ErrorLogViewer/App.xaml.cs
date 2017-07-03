using System.Windows;
using NetSteps.Common.DataAccess;

namespace ErrorLogViewer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnExit(ExitEventArgs e)
		{
			DataAccess.CloseAllSqlDependencies();
			base.OnExit(e);
		}
	}
}
