using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.EntityModel;

namespace NetSteps.Promotions.Plugins.Qualifications
{
	[ContainerRegister(typeof(IAccountHasTitleQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AccountHasTitleQualificationRepository : BasePromotionQualificationRepository<IAccountHasTitleQualificationExtension, PromotionQualificationAccountHasTitle>, IAccountHasTitleQualificationRepository
	{
		protected override PromotionQualificationAccountHasTitle Convert(IAccountHasTitleQualificationExtension dto)
		{
			PromotionQualificationAccountHasTitle AccountHasTitle = base.Convert(dto);
			if (dto.AllowedTitles != null)
			{
				foreach (var dtoTitle in dto.AllowedTitles)
				{
					AccountHasTitle.PromotionQualificationAccountHasTitleItems.Add(new PromotionQualificationAccountHasTitleItem() { TitleID = dtoTitle.TitleID, TitleTypeID = dtoTitle.TitleTypeID });
				}
			}
			return AccountHasTitle;
		}

		protected override IAccountHasTitleQualificationExtension Convert(PromotionQualificationAccountHasTitle entity)
		{

			IAccountHasTitleQualificationExtension extension = base.Convert(entity);
			foreach (var item in entity.PromotionQualificationAccountHasTitleItems)
			{
				var newOption = Create.New<IAccountTitleOption>();
				newOption.TitleID = item.TitleID;
				newOption.TitleTypeID = item.TitleTypeID;
				extension.AllowedTitles.Add(newOption);
			}
			return extension;
		}
	}
}
