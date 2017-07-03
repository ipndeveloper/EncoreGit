using System;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class HtmlSectionChoiceRepository
	{
		protected override Func<NetStepsEntities, IQueryable<HtmlSectionChoice>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<HtmlSectionChoice>>(
				 (context) => context.HtmlSectionChoices.Include("HtmlContent").Include("HtmlContent.HtmlElements").Include("HtmlContent.HtmlSectionContents"));
			}
		}
	}
}
