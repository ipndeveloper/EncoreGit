using System;
using System.Linq.Expressions;
using NetSteps.Data.Entities.Business.Filters.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Filters
{
	[ContainerRegister(typeof(IUrlSelector), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class UrlSelector : IUrlSelector
	{
		public bool IsValid { get; set; }
		private string _url { get; set; }
		public string Url
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
				IsValid = _url != null;
			}
		}
		public Expression<Func<SiteUrl, bool>> Filter { get { return s => s.Url.StartsWith(Url); } }
	}
}
