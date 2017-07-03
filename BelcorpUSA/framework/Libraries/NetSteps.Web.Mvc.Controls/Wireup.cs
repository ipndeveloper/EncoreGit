using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Web.Mvc.Controls;

[module: WireupDependency(typeof(ModuleWireup))]

namespace NetSteps.Web.Mvc.Controls
{
	using NetSteps.Common.Configuration;
	using NetSteps.Encore.Core.IoC;
	using NetSteps.Encore.Core.Wireup;

	// using StaticContent.Common;
	// using StaticContent.LocalFileSystemService;

	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// var fileUploadsPath = ConfigurationManager.GetAppSetting<string>("FileUploadAbsolutePath");

			// Container.Root.ForType<IStaticContentService>()
			// 		 .Register<LocalFileSystemService>(Param.Value(fileUploadsPath))
			// 		 .ResolveAnInstancePerRequest()
			//		 .End();
		}
	}
}