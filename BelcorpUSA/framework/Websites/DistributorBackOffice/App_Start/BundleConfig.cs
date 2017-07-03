using System.Web.Optimization;

namespace DistributorBackOffice
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;

			// Style bundles should be at the same folder depth as their CSS files.
			// Otherwise relative paths in the CSS content will break. That is why
			// the virtual paths are ~/Content/stylebundles/*.
			var jQueryUITheme = NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting("jQueryUITheme", "smoothness");
			bundles.Add(new StyleBundle("~/Content/stylebundles/jqueryui", "//ajax.googleapis.com/ajax/libs/jqueryui/1.8.21/themes/" + jQueryUITheme + "/jquery-ui.css")
				.Include("~/Content/CSS/jquery-ui-" + jQueryUITheme + ".1.8.21.css")
			);

            bundles.Add(new ScriptBundle("~/scriptbundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js")
				{
					CdnFallbackExpression = "window.jQuery"
				}
				.Include(
                    "~/Scripts/jquery-1.7.2.js"
				)
			);

			bundles.Add(new ScriptBundle("~/scriptbundles/jqueryui", "//ajax.googleapis.com/ajax/libs/jqueryui/1.8.21/jquery-ui.min.js")
				{
					CdnFallbackExpression = "window.jQuery.ui"
				}
				.Include(
					"~/Scripts/jquery-ui.1.8.21.min.js"
				)
			);

			bundles.Add(new ScriptBundle("~/scriptbundles/jqueryval", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.11.1/jquery.validate.min.js")
				{
					CdnFallbackExpression = "window.jQuery.validator"
				}
				.Include(
					"~/Scripts/jquery.validate.js"
				)
			);

			bundles.Add(new ScriptBundle("~/scriptbundles/knockout", "//ajax.aspnetcdn.com/ajax/knockout/knockout-2.2.1.js")
				{
					CdnFallbackExpression = "window.ko"
				}
				.Include(
					"~/Scripts/knockout-2.2.1.js"
				)
			);

			bundles.Add(new ScriptBundle("~/scriptbundles/site")
				// jquery.watermark.js must come before utilities.js
				.Include("~/Scripts/jquery.watermark.js")
				// jquery.watermark.unobtrusive.js must come before jquery.validate.unobtrusive.js
				.Include("~/Scripts/jquery.watermark.unobtrusive.js")
				.Include("~/Scripts/jquery.validate.unobtrusive.netsteps.js")
				.Include("~/Scripts/utilities.js")
				.Include("~/Scripts/jqModal.js")
				.Include("~/Scripts/jsonSuggest.js")
				.Include("~/Scripts/numeric.js")
				.Include("~/Scripts/jquery.hoverIntent.js")
				.Include("~/Scripts/jquery.inputfilter.js")
				.Include("~/Scripts/knockout.mapping-latest.js")
				.Include("~/Scripts/knockout.extensions.netsteps.js")
				.Include("~/Scripts/NS.js")
				.Include("~/Scripts/netsteps.unobtrusive.js")
				.Include("~/Scripts/jquery.lazyload.js")
				.Include("~/Scripts/modernizr.custom.js")
				.Include("~/Scripts/ns.common.ux.min.js")
			);

			// This is used on Login, ForgotPassword, and ResetPassword because they don't need
			// the full JS stack. - Lundy
			bundles.Add(new ScriptBundle("~/scriptbundles/utilities")
				.Include("~/Scripts/utilities.js")
			);
		}
	}
}