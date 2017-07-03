using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Common.Dynamic;
using NetSteps.Encore.Core.IoC;
using Newtonsoft.Json;

namespace NetSteps.Web.Mvc.Models
{
    /// <summary>
    /// A simple view model containing options and data - typically used with a client-side framework like Knockout.
    /// </summary>
    public interface IDynamicViewModel
    {
		/// <summary>
		/// Gets the options as a dynamic bag.
		/// </summary>
		dynamic OptionsBag { get; }


		/// <summary>
		/// Gets the data as a dynamic bag.
		/// </summary>
		dynamic DataBag { get; }

    	IDictionary<string, object> TempOptions { get; }
    	IDictionary<string, object> TempData { get; }
		
		/// <summary>
        /// Gets the options as a dynamic bag.
        /// </summary>
        dynamic Options { get; }

        /// <summary>
        /// Gets the data as a dynamic bag.
        /// </summary>
        dynamic Data { get; }
    }

    /// <summary>
    /// Extension methods for convenience when working with <see cref="IDynamicViewModel"/>.
    /// </summary>
    public static class IDynamicViewModelExtensions
    {
        /// <summary>
        /// Returns the options as a JSON string.
        /// </summary>
        public static MvcHtmlString OptionsJson(this IDynamicViewModel model)
        {
            var json = model == null
                ? "{}"
                : JsonConvert.SerializeObject(model.Options);

            return MvcHtmlString.Create(json);
        }

        /// <summary>
        /// Returns the options as a dictionary. Assumes the Options property is a <see cref="DynamicDictionary"/>.
        /// </summary>
        public static IDictionary<string, object> OptionsDictionary(this IDynamicViewModel model)
        {
            if (model == null
                || model.Options == null)
            {
                return new Dictionary<string, object>();
            }

            return model.Options.AsDictionary();
        }

        /// <summary>
        /// Returns the data as a JSON string.
        /// </summary>
        public static MvcHtmlString DataJson(this IDynamicViewModel model)
        {
            var json = model == null
                ? "{}"
                : JsonConvert.SerializeObject(model.Data);

            return MvcHtmlString.Create(json);
        }

        /// <summary>
        /// Returns the data as a dictionary. Assumes the Data property is a <see cref="DynamicDictionary"/>.
        /// </summary>
        public static IDictionary<string, object> DataDictionary(this IDynamicViewModel model)
        {
            if (model == null
                || model.Data == null)
            {
                return new Dictionary<string, object>();
            }

            return model.Data.AsDictionary();
        }
    }

    [ContainerRegister(typeof(IDynamicViewModel), RegistrationBehaviors.Default)]
    public class DynamicViewModel : IDynamicViewModel
    {
        public dynamic Options { get; protected set; }

        public dynamic Data { get; protected set; }
		
		public dynamic OptionsBag
		{
			get { return Options; }
		}

        public DynamicViewModel()
        {
            Options = new DynamicDictionary();
            Data = new DynamicDictionary();
        }


		public dynamic DataBag
		{
			get { return Data; }
		}

		public IDictionary<string, object> TempOptions
		{
			get { return OptionsBag.AsDictionary(); }
		}

		public IDictionary<string, object> TempData
		{
			get { return DataBag.AsDictionary(); }
		}
	}
}
