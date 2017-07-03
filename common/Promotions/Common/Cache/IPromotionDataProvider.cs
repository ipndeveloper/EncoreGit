using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common.Cache
{
	[ContractClass(typeof(PromotionDataProviderContract))]
	public interface IPromotionDataProvider
	{
		IPromotion FindPromotion(int promotionID, IUnitOfWork unitOfWork);

		IEnumerable<IPromotion> FindPromotions(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, Predicate<IPromotion> filter, IEnumerable<string> ofKinds);

		int FindPromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID);

		IPromotion AddPromotion(IPromotion promotion, IUnitOfWork unitOfWork);

		IPromotion UpdatePromotion(IPromotion promotion, IUnitOfWork unitOfWork);
	}

	public static class PromotionDataProviderExtensions
	{
		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, Create.New<IPromotionInterval>(), (x) => { return true; }, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, Create.New<IPromotionInterval>(), (x) => { return true; }, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, Predicate<IPromotion> filter)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, Create.New<IPromotionInterval>(), filter, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, Predicate<IPromotion> filter)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, Create.New<IPromotionInterval>(), filter, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, string ofKind)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, Create.New<IPromotionInterval>(), (x) => { return true; }, new string[] { ofKind });
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, string ofKind)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, Create.New<IPromotionInterval>(), (x) => { return true; }, new string[] { ofKind });
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, Predicate<IPromotion> filter, string ofKind)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, Create.New<IPromotionInterval>(), filter, new string[] { ofKind });
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IEnumerable<string> ofKinds)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, Create.New<IPromotionInterval>(), (x) => { return true; }, ofKinds);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IEnumerable<string> ofKinds)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, Create.New<IPromotionInterval>(), (x) => { return true; }, ofKinds);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, Predicate<IPromotion> filter, IEnumerable<string> ofKinds)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, Create.New<IPromotionInterval>(), filter, ofKinds);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IPromotionInterval searchInterval)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, searchInterval, (x) => { return true; }, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, searchInterval, (x) => { return true; }, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IPromotionInterval searchInterval, Predicate<IPromotion> filter)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, searchInterval, filter, new string[0]);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, Predicate<IPromotion> filter)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, searchInterval, filter, null);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IPromotionInterval searchInterval, string ofKind)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, searchInterval, (x) => { return true; }, new string[] { ofKind });
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, string ofKind)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, searchInterval, (x) => { return true; }, new string[] { ofKind });
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IPromotionInterval searchInterval, Predicate<IPromotion> filter, string ofKind)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, searchInterval, filter, new string[] { ofKind });
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IPromotionInterval searchInterval, IEnumerable<string> ofKinds)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, searchInterval, (x) => { return true; }, ofKinds);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, IEnumerable<string> ofKinds)
		{
			return repository.FindPromotions(unitOfWork, statusTypes, searchInterval, (x) => { return true; }, ofKinds);
		}

		public static IEnumerable<IPromotion> FindPromotions(this IPromotionDataProvider repository, IPromotionUnitOfWork unitOfWork, IPromotionInterval searchInterval, Predicate<IPromotion> filter, IEnumerable<string> ofKinds)
		{
			return repository.FindPromotions(unitOfWork, PromotionStatus.Enabled, searchInterval, filter, ofKinds);
		}
	}
}
