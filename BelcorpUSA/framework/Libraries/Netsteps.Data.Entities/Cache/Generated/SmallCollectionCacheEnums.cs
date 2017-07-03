
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.IoC;


namespace NetSteps.Data.Entities.Cache
{
	public partial class SmallCollectionCache
	{ 
		public AccountPriceTypeCache AccountPriceTypes = new AccountPriceTypeCache();
		public sealed class AccountPriceTypeCache : SmallCollectionBase<AccountPriceType, Int32>
		{
			public AccountPriceTypeCache()
				: base("AccountPriceTypeListCache")
			{ }

			protected override Int32 PerformGetKey(AccountPriceType item)
			{
				return item.AccountPriceTypeID;
			}
			
			protected override List<AccountPriceType> PerformInitializeList()
			{
				var result = AccountPriceType.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountPropertyTypeCache AccountPropertyTypes = new AccountPropertyTypeCache();
		public sealed class AccountPropertyTypeCache : SmallCollectionBase<AccountPropertyType, Int32>
		{
			public AccountPropertyTypeCache()
				: base("AccountPropertyTypeListCache")
			{ }

			protected override Int32 PerformGetKey(AccountPropertyType item)
			{
				return item.AccountPropertyTypeID;
			}
			
			protected override List<AccountPropertyType> PerformInitializeList()
			{
				var result = AccountPropertyType.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountReportTypeCache AccountReportTypes = new AccountReportTypeCache();
		public sealed class AccountReportTypeCache : SmallCollectionBase<AccountReportType, Int16>
		{
			public AccountReportTypeCache()
				: base("AccountReportTypeListCache")
			{ }

			protected override Int16 PerformGetKey(AccountReportType item)
			{
				return item.AccountReportTypeID;
			}
			
			protected override List<AccountReportType> PerformInitializeList()
			{
				var result = AccountReportType.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountSourceCache AccountSources = new AccountSourceCache();
		public sealed class AccountSourceCache : SmallCollectionBase<AccountSource, Int16>
		{
			public AccountSourceCache()
				: base("AccountSourceListCache")
			{ }

			protected override Int16 PerformGetKey(AccountSource item)
			{
				return item.AccountSourceID;
			}
			
			protected override List<AccountSource> PerformInitializeList()
			{
				var result = AccountSource.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountSponsorTypeCache AccountSponsorTypes = new AccountSponsorTypeCache();
		public sealed class AccountSponsorTypeCache : SmallCollectionBase<AccountSponsorType, Int32>
		{
			public AccountSponsorTypeCache()
				: base("AccountSponsorTypeListCache")
			{ }

			protected override Int32 PerformGetKey(AccountSponsorType item)
			{
				return item.AccountSponsorTypeID;
			}
			
			protected override List<AccountSponsorType> PerformInitializeList()
			{
				var result = AccountSponsorType.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountStatusCache AccountStatuses = new AccountStatusCache();
		public sealed class AccountStatusCache : SmallCollectionBase<AccountStatus, Int16>
		{
			public AccountStatusCache()
				: base("AccountStatusListCache")
			{ }

			protected override Int16 PerformGetKey(AccountStatus item)
			{
				return item.AccountStatusID;
			}
			
			protected override List<AccountStatus> PerformInitializeList()
			{
				var result = AccountStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountStatusChangeReasonCache AccountStatusChangeReasons = new AccountStatusChangeReasonCache();
		public sealed class AccountStatusChangeReasonCache : SmallCollectionBase<AccountStatusChangeReason, Int16>
		{
			public AccountStatusChangeReasonCache()
				: base("AccountStatusChangeReasonListCache")
			{ }

			protected override Int16 PerformGetKey(AccountStatusChangeReason item)
			{
				return item.AccountStatusChangeReasonID;
			}
			
			protected override List<AccountStatusChangeReason> PerformInitializeList()
			{
				var result = AccountStatusChangeReason.LoadAllFull();
				return result.ToList();
			}
		}

		public AccountTypeCache AccountTypes = new AccountTypeCache();
		public sealed class AccountTypeCache : SmallCollectionBase<AccountType, Int16>
		{
			public AccountTypeCache()
				: base("AccountTypeListCache")
			{ }

			protected override Int16 PerformGetKey(AccountType item)
			{
				return item.AccountTypeID;
			}
			
			protected override List<AccountType> PerformInitializeList()
			{
				var result = AccountType.LoadAllFull();
				return result.ToList();
			}
		}

		public AddressPropertyTypeCache AddressPropertyTypes = new AddressPropertyTypeCache();
		public sealed class AddressPropertyTypeCache : SmallCollectionBase<AddressPropertyType, Int32>
		{
			public AddressPropertyTypeCache()
				: base("AddressPropertyTypeListCache")
			{ }

			protected override Int32 PerformGetKey(AddressPropertyType item)
			{
				return item.AddressPropertyTypeID;
			}
			
			protected override List<AddressPropertyType> PerformInitializeList()
			{
				var result = AddressPropertyType.LoadAllFull();
				return result.ToList();
			}
		}

		public AddressTypeCache AddressTypes = new AddressTypeCache();
		public sealed class AddressTypeCache : SmallCollectionBase<AddressType, Int16>
		{
			public AddressTypeCache()
				: base("AddressTypeListCache")
			{ }

			protected override Int16 PerformGetKey(AddressType item)
			{
				return item.AddressTypeID;
			}
			
			protected override List<AddressType> PerformInitializeList()
			{
				var result = AddressType.LoadAllFull();
				return result.ToList();
			}
		}

		public AlertPriorityCache AlertPriorities = new AlertPriorityCache();
		public sealed class AlertPriorityCache : SmallCollectionBase<AlertPriority, Int16>
		{
			public AlertPriorityCache()
				: base("AlertPriorityListCache")
			{ }

			protected override Int16 PerformGetKey(AlertPriority item)
			{
				return item.AlertPriorityID;
			}
			
			protected override List<AlertPriority> PerformInitializeList()
			{
				var result = AlertPriority.LoadAllFull();
				return result.ToList();
			}
		}

		public ApplicationCache Applications = new ApplicationCache();
		public sealed class ApplicationCache : SmallCollectionBase<Application, Int16>
		{
			public ApplicationCache()
				: base("ApplicationListCache")
			{ }

			protected override Int16 PerformGetKey(Application item)
			{
				return item.ApplicationID;
			}
			
			protected override List<Application> PerformInitializeList()
			{
				var result = Application.LoadAllFull();
				return result.ToList();
			}
		}

		public ArchiveTypeCache ArchiveTypes = new ArchiveTypeCache();
		public sealed class ArchiveTypeCache : SmallCollectionBase<ArchiveType, Int16>
		{
			public ArchiveTypeCache()
				: base("ArchiveTypeListCache")
			{ }

			protected override Int16 PerformGetKey(ArchiveType item)
			{
				return item.ArchiveTypeID;
			}
			
			protected override List<ArchiveType> PerformInitializeList()
			{
				var result = ArchiveType.LoadAllFull();
				return result.ToList();
			}
		}

		public AuditChangeTypeCache AuditChangeTypes = new AuditChangeTypeCache();
		public sealed class AuditChangeTypeCache : SmallCollectionBase<AuditChangeType, Byte>
		{
			public AuditChangeTypeCache()
				: base("AuditChangeTypeListCache")
			{ }

			protected override Byte PerformGetKey(AuditChangeType item)
			{
				return item.AuditChangeTypeID;
			}
			
			protected override List<AuditChangeType> PerformInitializeList()
			{
				var result = AuditChangeType.LoadAllFull();
				return result.ToList();
			}
		}

		public AutoresponderTypeCache AutoresponderTypes = new AutoresponderTypeCache();
		public sealed class AutoresponderTypeCache : SmallCollectionBase<AutoresponderType, Int16>
		{
			public AutoresponderTypeCache()
				: base("AutoresponderTypeListCache")
			{ }

			protected override Int16 PerformGetKey(AutoresponderType item)
			{
				return item.AutoresponderTypeID;
			}
			
			protected override List<AutoresponderType> PerformInitializeList()
			{
				var result = AutoresponderType.LoadAllFull();
				return result.ToList();
			}
		}

		public AutoshipScheduleCache AutoshipSchedules = new AutoshipScheduleCache();
		public sealed class AutoshipScheduleCache : SmallCollectionBase<AutoshipSchedule, Int32>
		{
			public AutoshipScheduleCache()
				: base("AutoshipScheduleListCache")
			{ }

			protected override Int32 PerformGetKey(AutoshipSchedule item)
			{
				return item.AutoshipScheduleID;
			}
			
			protected override List<AutoshipSchedule> PerformInitializeList()
			{
				var result = AutoshipSchedule.LoadAllFull();
				return result.ToList();
			}
		}

		public AutoshipScheduleDayCache AutoshipScheduleDays = new AutoshipScheduleDayCache();
		public sealed class AutoshipScheduleDayCache : SmallCollectionBase<AutoshipScheduleDay, Int32>
		{
			public AutoshipScheduleDayCache()
				: base("AutoshipScheduleDayListCache")
			{ }

			protected override Int32 PerformGetKey(AutoshipScheduleDay item)
			{
				return item.AutoshipScheduleDayID;
			}
			
			protected override List<AutoshipScheduleDay> PerformInitializeList()
			{
				var result = AutoshipScheduleDay.LoadAllFull();
				return result.ToList();
			}
		}

		public AutoshipScheduleProductCache AutoshipScheduleProducts = new AutoshipScheduleProductCache();
		public sealed class AutoshipScheduleProductCache : SmallCollectionBase<AutoshipScheduleProduct, Int32>
		{
			public AutoshipScheduleProductCache()
				: base("AutoshipScheduleProductListCache")
			{ }

			protected override Int32 PerformGetKey(AutoshipScheduleProduct item)
			{
				return item.AutoshipScheduleProductID;
			}
			
			protected override List<AutoshipScheduleProduct> PerformInitializeList()
			{
				var result = AutoshipScheduleProduct.LoadAllFull();
				return result.ToList();
			}
		}

		public AutoshipScheduleTypeCache AutoshipScheduleTypes = new AutoshipScheduleTypeCache();
		public sealed class AutoshipScheduleTypeCache : SmallCollectionBase<AutoshipScheduleType, Int32>
		{
			public AutoshipScheduleTypeCache()
				: base("AutoshipScheduleTypeListCache")
			{ }

			protected override Int32 PerformGetKey(AutoshipScheduleType item)
			{
				return item.AutoshipScheduleTypeID;
			}
			
			protected override List<AutoshipScheduleType> PerformInitializeList()
			{
				var result = AutoshipScheduleType.LoadAllFull();
				return result.ToList();
			}
		}

		public CampaignActionTypeCache CampaignActionTypes = new CampaignActionTypeCache();
		public sealed class CampaignActionTypeCache : SmallCollectionBase<CampaignActionType, Int16>
		{
			public CampaignActionTypeCache()
				: base("CampaignActionTypeListCache")
			{ }

			protected override Int16 PerformGetKey(CampaignActionType item)
			{
				return item.CampaignActionTypeID;
			}
			
			protected override List<CampaignActionType> PerformInitializeList()
			{
				var result = CampaignActionType.LoadAllFull();
				return result.ToList();
			}
		}

		public CampaignTypeCache CampaignTypes = new CampaignTypeCache();
		public sealed class CampaignTypeCache : SmallCollectionBase<CampaignType, Int16>
		{
			public CampaignTypeCache()
				: base("CampaignTypeListCache")
			{ }

			protected override Int16 PerformGetKey(CampaignType item)
			{
				return item.CampaignTypeID;
			}
			
			protected override List<CampaignType> PerformInitializeList()
			{
				var result = CampaignType.LoadAllFull();
				return result.ToList();
			}
		}

		public CatalogTypeCache CatalogTypes = new CatalogTypeCache();
		public sealed class CatalogTypeCache : SmallCollectionBase<CatalogType, Int16>
		{
			public CatalogTypeCache()
				: base("CatalogTypeListCache")
			{ }

			protected override Int16 PerformGetKey(CatalogType item)
			{
				return item.CatalogTypeID;
			}
			
			protected override List<CatalogType> PerformInitializeList()
			{
				var result = CatalogType.LoadAllFull();
				return result.ToList();
			}
		}

		public CategoryTypeCache CategoryTypes = new CategoryTypeCache();
		public sealed class CategoryTypeCache : SmallCollectionBase<CategoryType, Int16>
		{
			public CategoryTypeCache()
				: base("CategoryTypeListCache")
			{ }

			protected override Int16 PerformGetKey(CategoryType item)
			{
				return item.CategoryTypeID;
			}
			
			protected override List<CategoryType> PerformInitializeList()
			{
				var result = CategoryType.LoadAllFull();
				return result.ToList();
			}
		}

		public CountryCache Countries = new CountryCache();
		public sealed class CountryCache : SmallCollectionBase<Country, Int32>
		{
			public CountryCache()
				: base("CountryListCache")
			{ }

			protected override Int32 PerformGetKey(Country item)
			{
				return item.CountryID;
			}
			
			protected override List<Country> PerformInitializeList()
			{
				var result = Country.LoadAllFull();
				return result.ToList();
			}
		}

		public CreditCardTypeCache CreditCardTypes = new CreditCardTypeCache();
		public sealed class CreditCardTypeCache : SmallCollectionBase<CreditCardType, Int16>
		{
			public CreditCardTypeCache()
				: base("CreditCardTypeListCache")
			{ }

			protected override Int16 PerformGetKey(CreditCardType item)
			{
				return item.CreditCardTypeID;
			}
			
			protected override List<CreditCardType> PerformInitializeList()
			{
				var result = CreditCardType.LoadAllFull();
				return result.ToList();
			}
		}

		public CurrencyCache Currencies = new CurrencyCache();
		public sealed class CurrencyCache : SmallCollectionBase<Currency, Int32>
		{
			public CurrencyCache()
				: base("CurrencyListCache")
			{ }

			protected override Int32 PerformGetKey(Currency item)
			{
				return item.CurrencyID;
			}
			
			protected override List<Currency> PerformInitializeList()
			{
				var result = Currency.LoadAllFull();
				return result.ToList();
			}
		}

		public DistributionListTypeCache DistributionListTypes = new DistributionListTypeCache();
		public sealed class DistributionListTypeCache : SmallCollectionBase<DistributionListType, Int16>
		{
			public DistributionListTypeCache()
				: base("DistributionListTypeListCache")
			{ }

			protected override Int16 PerformGetKey(DistributionListType item)
			{
				return item.DistributionListTypeID;
			}
			
			protected override List<DistributionListType> PerformInitializeList()
			{
				var result = DistributionListType.LoadAllFull();
				return result.ToList();
			}
		}

		public DomainEventTypeCache DomainEventTypes = new DomainEventTypeCache();
		public sealed class DomainEventTypeCache : SmallCollectionBase<DomainEventType, Int16>
		{
			public DomainEventTypeCache()
				: base("DomainEventTypeListCache")
			{ }

			protected override Int16 PerformGetKey(DomainEventType item)
			{
				return item.DomainEventTypeID;
			}
			
			protected override List<DomainEventType> PerformInitializeList()
			{
				var result = DomainEventType.LoadAllFull();
				return result.ToList();
			}
		}

		public DomainEventTypeCategoryCache DomainEventTypeCategories = new DomainEventTypeCategoryCache();
		public sealed class DomainEventTypeCategoryCache : SmallCollectionBase<DomainEventTypeCategory, Int32>
		{
			public DomainEventTypeCategoryCache()
				: base("DomainEventTypeCategoryListCache")
			{ }

			protected override Int32 PerformGetKey(DomainEventTypeCategory item)
			{
				return item.DomainEventTypeCategoryID;
			}
			
			protected override List<DomainEventTypeCategory> PerformInitializeList()
			{
				var result = DomainEventTypeCategory.LoadAllFull();
				return result.ToList();
			}
		}

		public DynamicKitPricingTypeCache DynamicKitPricingTypes = new DynamicKitPricingTypeCache();
		public sealed class DynamicKitPricingTypeCache : SmallCollectionBase<DynamicKitPricingType, Int32>
		{
			public DynamicKitPricingTypeCache()
				: base("DynamicKitPricingTypeListCache")
			{ }

			protected override Int32 PerformGetKey(DynamicKitPricingType item)
			{
				return item.DynamicKitPricingTypeID;
			}
			
			protected override List<DynamicKitPricingType> PerformInitializeList()
			{
				var result = DynamicKitPricingType.LoadAllFull();
				return result.ToList();
			}
		}

		public EmailBodyTextTypeCache EmailBodyTextTypes = new EmailBodyTextTypeCache();
		public sealed class EmailBodyTextTypeCache : SmallCollectionBase<EmailBodyTextType, Int16>
		{
			public EmailBodyTextTypeCache()
				: base("EmailBodyTextTypeListCache")
			{ }

			protected override Int16 PerformGetKey(EmailBodyTextType item)
			{
				return item.EmailBodyTextTypeID;
			}
			
			protected override List<EmailBodyTextType> PerformInitializeList()
			{
				var result = EmailBodyTextType.LoadAllFull();
				return result.ToList();
			}
		}

		public EmailTemplateTypeCache EmailTemplateTypes = new EmailTemplateTypeCache();
		public sealed class EmailTemplateTypeCache : SmallCollectionBase<EmailTemplateType, Int16>
		{
			public EmailTemplateTypeCache()
				: base("EmailTemplateTypeListCache")
			{ }

			protected override Int16 PerformGetKey(EmailTemplateType item)
			{
				return item.EmailTemplateTypeID;
			}
			
			protected override List<EmailTemplateType> PerformInitializeList()
			{
				var result = EmailTemplateType.LoadAllFull();
				return result.ToList();
			}
		}

		public FileResourcePropertyTypeCache FileResourcePropertyTypes = new FileResourcePropertyTypeCache();
		public sealed class FileResourcePropertyTypeCache : SmallCollectionBase<FileResourcePropertyType, Int32>
		{
			public FileResourcePropertyTypeCache()
				: base("FileResourcePropertyTypeListCache")
			{ }

			protected override Int32 PerformGetKey(FileResourcePropertyType item)
			{
				return item.FileResourcePropertyTypeID;
			}
			
			protected override List<FileResourcePropertyType> PerformInitializeList()
			{
				var result = FileResourcePropertyType.LoadAllFull();
				return result.ToList();
			}
		}

		public FileResourceTypeCache FileResourceTypes = new FileResourceTypeCache();
		public sealed class FileResourceTypeCache : SmallCollectionBase<FileResourceType, Int32>
		{
			public FileResourceTypeCache()
				: base("FileResourceTypeListCache")
			{ }

			protected override Int32 PerformGetKey(FileResourceType item)
			{
				return item.FileResourceTypeID;
			}
			
			protected override List<FileResourceType> PerformInitializeList()
			{
				var result = FileResourceType.LoadAllFull();
				return result.ToList();
			}
		}

		public FunctionCache Functions = new FunctionCache();
		public sealed class FunctionCache : SmallCollectionBase<Function, Int32>
		{
			public FunctionCache()
				: base("FunctionListCache")
			{ }

			protected override Int32 PerformGetKey(Function item)
			{
				return item.FunctionID;
			}
			
			protected override List<Function> PerformInitializeList()
			{
				var result = Function.LoadAllFull();
				return result.ToList();
			}
		}

		public GenderCache Genders = new GenderCache();
		public sealed class GenderCache : SmallCollectionBase<Gender, Int16>
		{
			public GenderCache()
				: base("GenderListCache")
			{ }

			protected override Int16 PerformGetKey(Gender item)
			{
				return item.GenderID;
			}
			
			protected override List<Gender> PerformInitializeList()
			{
				var result = Gender.LoadAllFull();
				return result.ToList();
			}
		}

		public HostessRewardRuleCache HostessRewardRules = new HostessRewardRuleCache();
		public sealed class HostessRewardRuleCache : SmallCollectionBase<HostessRewardRule, Int32>
		{
			public HostessRewardRuleCache()
				: base("HostessRewardRuleListCache")
			{ }

			protected override Int32 PerformGetKey(HostessRewardRule item)
			{
				return item.HostessRewardRuleID;
			}
			
			protected override List<HostessRewardRule> PerformInitializeList()
			{
				var result = HostessRewardRule.LoadAllFull();
				return result.ToList();
			}
		}

		public HostessRewardRuleTypeCache HostessRewardRuleTypes = new HostessRewardRuleTypeCache();
		public sealed class HostessRewardRuleTypeCache : SmallCollectionBase<HostessRewardRuleType, Int32>
		{
			public HostessRewardRuleTypeCache()
				: base("HostessRewardRuleTypeListCache")
			{ }

			protected override Int32 PerformGetKey(HostessRewardRuleType item)
			{
				return item.HostessRewardRuleTypeID;
			}
			
			protected override List<HostessRewardRuleType> PerformInitializeList()
			{
				var result = HostessRewardRuleType.LoadAllFull();
				return result.ToList();
			}
		}

		public HostessRewardTypeCache HostessRewardTypes = new HostessRewardTypeCache();
		public sealed class HostessRewardTypeCache : SmallCollectionBase<HostessRewardType, Int32>
		{
			public HostessRewardTypeCache()
				: base("HostessRewardTypeListCache")
			{ }

			protected override Int32 PerformGetKey(HostessRewardType item)
			{
				return item.HostessRewardTypeID;
			}
			
			protected override List<HostessRewardType> PerformInitializeList()
			{
				var result = HostessRewardType.LoadAllFull();
				return result.ToList();
			}
		}

		public HtmlContentEditorTypeCache HtmlContentEditorTypes = new HtmlContentEditorTypeCache();
		public sealed class HtmlContentEditorTypeCache : SmallCollectionBase<HtmlContentEditorType, Int16>
		{
			public HtmlContentEditorTypeCache()
				: base("HtmlContentEditorTypeListCache")
			{ }

			protected override Int16 PerformGetKey(HtmlContentEditorType item)
			{
				return item.HtmlContentEditorTypeID;
			}
			
			protected override List<HtmlContentEditorType> PerformInitializeList()
			{
				var result = HtmlContentEditorType.LoadAllFull();
				return result.ToList();
			}
		}

		public HtmlContentStatusCache HtmlContentStatuses = new HtmlContentStatusCache();
		public sealed class HtmlContentStatusCache : SmallCollectionBase<HtmlContentStatus, Int32>
		{
			public HtmlContentStatusCache()
				: base("HtmlContentStatusListCache")
			{ }

			protected override Int32 PerformGetKey(HtmlContentStatus item)
			{
				return item.HtmlContentStatusID;
			}
			
			protected override List<HtmlContentStatus> PerformInitializeList()
			{
				var result = HtmlContentStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public HtmlContentWorkflowTypeCache HtmlContentWorkflowTypes = new HtmlContentWorkflowTypeCache();
		public sealed class HtmlContentWorkflowTypeCache : SmallCollectionBase<HtmlContentWorkflowType, Int16>
		{
			public HtmlContentWorkflowTypeCache()
				: base("HtmlContentWorkflowTypeListCache")
			{ }

			protected override Int16 PerformGetKey(HtmlContentWorkflowType item)
			{
				return item.HtmlContentWorkflowTypeID;
			}
			
			protected override List<HtmlContentWorkflowType> PerformInitializeList()
			{
				var result = HtmlContentWorkflowType.LoadAllFull();
				return result.ToList();
			}
		}

		public HtmlElementTypeCache HtmlElementTypes = new HtmlElementTypeCache();
		public sealed class HtmlElementTypeCache : SmallCollectionBase<HtmlElementType, Byte>
		{
			public HtmlElementTypeCache()
				: base("HtmlElementTypeListCache")
			{ }

			protected override Byte PerformGetKey(HtmlElementType item)
			{
				return item.HtmlElementTypeID;
			}
			
			protected override List<HtmlElementType> PerformInitializeList()
			{
				var result = HtmlElementType.LoadAllFull();
				return result.ToList();
			}
		}

		public HtmlInputTypeCache HtmlInputTypes = new HtmlInputTypeCache();
		public sealed class HtmlInputTypeCache : SmallCollectionBase<HtmlInputType, Int16>
		{
			public HtmlInputTypeCache()
				: base("HtmlInputTypeListCache")
			{ }

			protected override Int16 PerformGetKey(HtmlInputType item)
			{
				return item.HtmlInputTypeID;
			}
			
			protected override List<HtmlInputType> PerformInitializeList()
			{
				var result = HtmlInputType.LoadAllFull();
				return result.ToList();
			}
		}

		public HtmlSectionEditTypeCache HtmlSectionEditTypes = new HtmlSectionEditTypeCache();
		public sealed class HtmlSectionEditTypeCache : SmallCollectionBase<HtmlSectionEditType, Int16>
		{
			public HtmlSectionEditTypeCache()
				: base("HtmlSectionEditTypeListCache")
			{ }

			protected override Int16 PerformGetKey(HtmlSectionEditType item)
			{
				return item.HtmlSectionEditTypeID;
			}
			
			protected override List<HtmlSectionEditType> PerformInitializeList()
			{
				var result = HtmlSectionEditType.LoadAllFull();
				return result.ToList();
			}
		}

		public IntervalTypeCache IntervalTypes = new IntervalTypeCache();
		public sealed class IntervalTypeCache : SmallCollectionBase<IntervalType, Int32>
		{
			public IntervalTypeCache()
				: base("IntervalTypeListCache")
			{ }

			protected override Int32 PerformGetKey(IntervalType item)
			{
				return item.IntervalTypeID;
			}
			
			protected override List<IntervalType> PerformInitializeList()
			{
				var result = IntervalType.LoadAllFull();
				return result.ToList();
			}
		}

		public LanguageCache Languages = new LanguageCache();
		public sealed class LanguageCache : SmallCollectionBase<Language, Int32>
		{
			public LanguageCache()
				: base("LanguageListCache")
			{ }

			protected override Int32 PerformGetKey(Language item)
			{
				return item.LanguageID;
			}
			
			protected override List<Language> PerformInitializeList()
			{
				var result = Language.LoadAllFull();
				return result.ToList();
			}
		}

		public LayoutCache Layouts = new LayoutCache();
		public sealed class LayoutCache : SmallCollectionBase<Layout, Int32>
		{
			public LayoutCache()
				: base("LayoutListCache")
			{ }

			protected override Int32 PerformGetKey(Layout item)
			{
				return item.LayoutID;
			}
			
			protected override List<Layout> PerformInitializeList()
			{
				var result = Layout.LoadAllFull();
				return result.ToList();
			}
		}

		public ListValueTypeCache ListValueTypes = new ListValueTypeCache();
		public sealed class ListValueTypeCache : SmallCollectionBase<ListValueType, Int16>
		{
			public ListValueTypeCache()
				: base("ListValueTypeListCache")
			{ }

			protected override Int16 PerformGetKey(ListValueType item)
			{
				return item.ListValueTypeID;
			}
			
			protected override List<ListValueType> PerformInitializeList()
			{
				var result = ListValueType.LoadAllFull();
				return result.ToList();
			}
		}

		public MarketCache Markets = new MarketCache();
		public sealed class MarketCache : SmallCollectionBase<Market, Int32>
		{
			public MarketCache()
				: base("MarketListCache")
			{ }

			protected override Int32 PerformGetKey(Market item)
			{
				return item.MarketID;
			}
			
			protected override List<Market> PerformInitializeList()
			{
				var result = Market.LoadAllFull();
				return result.ToList();
			}
		}

		public MarketStoreFrontCache MarketStoreFronts = new MarketStoreFrontCache();
		public sealed class MarketStoreFrontCache : SmallCollectionBase<MarketStoreFront, Int32>
		{
			public MarketStoreFrontCache()
				: base("MarketStoreFrontListCache")
			{ }

			protected override Int32 PerformGetKey(MarketStoreFront item)
			{
				return item.MarketStoreFrontID;
			}
			
			protected override List<MarketStoreFront> PerformInitializeList()
			{
				var result = MarketStoreFront.LoadAllFull();
				return result.ToList();
			}
		}

		public NavigationTypeCache NavigationTypes = new NavigationTypeCache();
		public sealed class NavigationTypeCache : SmallCollectionBase<NavigationType, Int32>
		{
			public NavigationTypeCache()
				: base("NavigationTypeListCache")
			{ }

			protected override Int32 PerformGetKey(NavigationType item)
			{
				return item.NavigationTypeID;
			}
			
			protected override List<NavigationType> PerformInitializeList()
			{
				var result = NavigationType.LoadAllFull();
				return result.ToList();
			}
		}

		public NewsTypeCache NewsTypes = new NewsTypeCache();
		public sealed class NewsTypeCache : SmallCollectionBase<NewsType, Int16>
		{
			public NewsTypeCache()
				: base("NewsTypeListCache")
			{ }

			protected override Int16 PerformGetKey(NewsType item)
			{
				return item.NewsTypeID;
			}
			
			protected override List<NewsType> PerformInitializeList()
			{
				var result = NewsType.LoadAllFull();
				return result.ToList();
			}
		}

		public NoteTypeCache NoteTypes = new NoteTypeCache();
		public sealed class NoteTypeCache : SmallCollectionBase<NoteType, Int32>
		{
			public NoteTypeCache()
				: base("NoteTypeListCache")
			{ }

			protected override Int32 PerformGetKey(NoteType item)
			{
				return item.NoteTypeID;
			}
			
			protected override List<NoteType> PerformInitializeList()
			{
				var result = NoteType.LoadAllFull();
				return result.ToList();
			}
		}

		public OptOutTypeCache OptOutTypes = new OptOutTypeCache();
		public sealed class OptOutTypeCache : SmallCollectionBase<OptOutType, Int16>
		{
			public OptOutTypeCache()
				: base("OptOutTypeListCache")
			{ }

			protected override Int16 PerformGetKey(OptOutType item)
			{
				return item.OptOutTypeID;
			}
			
			protected override List<OptOutType> PerformInitializeList()
			{
				var result = OptOutType.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderCustomerTypeCache OrderCustomerTypes = new OrderCustomerTypeCache();
		public sealed class OrderCustomerTypeCache : SmallCollectionBase<OrderCustomerType, Int16>
		{
			public OrderCustomerTypeCache()
				: base("OrderCustomerTypeListCache")
			{ }

			protected override Int16 PerformGetKey(OrderCustomerType item)
			{
				return item.OrderCustomerTypeID;
			}
			
			protected override List<OrderCustomerType> PerformInitializeList()
			{
				var result = OrderCustomerType.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderItemParentTypeCache OrderItemParentTypes = new OrderItemParentTypeCache();
		public sealed class OrderItemParentTypeCache : SmallCollectionBase<OrderItemParentType, Int16>
		{
			public OrderItemParentTypeCache()
				: base("OrderItemParentTypeListCache")
			{ }

			protected override Int16 PerformGetKey(OrderItemParentType item)
			{
				return item.OrderItemParentTypeID;
			}
			
			protected override List<OrderItemParentType> PerformInitializeList()
			{
				var result = OrderItemParentType.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderItemPropertyTypeCache OrderItemPropertyTypes = new OrderItemPropertyTypeCache();
		public sealed class OrderItemPropertyTypeCache : SmallCollectionBase<OrderItemPropertyType, Int32>
		{
			public OrderItemPropertyTypeCache()
				: base("OrderItemPropertyTypeListCache")
			{ }

			protected override Int32 PerformGetKey(OrderItemPropertyType item)
			{
				return item.OrderItemPropertyTypeID;
			}
			
			protected override List<OrderItemPropertyType> PerformInitializeList()
			{
				var result = OrderItemPropertyType.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderItemTypeCache OrderItemTypes = new OrderItemTypeCache();
		public sealed class OrderItemTypeCache : SmallCollectionBase<OrderItemType, Int16>
		{
			public OrderItemTypeCache()
				: base("OrderItemTypeListCache")
			{ }

			protected override Int16 PerformGetKey(OrderItemType item)
			{
				return item.OrderItemTypeID;
			}
			
			protected override List<OrderItemType> PerformInitializeList()
			{
				var result = OrderItemType.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderPaymentStatusCache OrderPaymentStatuses = new OrderPaymentStatusCache();
		public sealed class OrderPaymentStatusCache : SmallCollectionBase<OrderPaymentStatus, Int16>
		{
			public OrderPaymentStatusCache()
				: base("OrderPaymentStatusListCache")
			{ }

			protected override Int16 PerformGetKey(OrderPaymentStatus item)
			{
				return item.OrderPaymentStatusID;
			}
			
			protected override List<OrderPaymentStatus> PerformInitializeList()
			{
				var result = OrderPaymentStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderShipmentStatusCache OrderShipmentStatuses = new OrderShipmentStatusCache();
		public sealed class OrderShipmentStatusCache : SmallCollectionBase<OrderShipmentStatus, Int16>
		{
			public OrderShipmentStatusCache()
				: base("OrderShipmentStatusListCache")
			{ }

			protected override Int16 PerformGetKey(OrderShipmentStatus item)
			{
				return item.OrderShipmentStatusID;
			}
			
			protected override List<OrderShipmentStatus> PerformInitializeList()
			{
				var result = OrderShipmentStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderStatusCache OrderStatuses = new OrderStatusCache();
		public sealed class OrderStatusCache : SmallCollectionBase<OrderStatus, Int16>
		{
			public OrderStatusCache()
				: base("OrderStatusListCache")
			{ }

			protected override Int16 PerformGetKey(OrderStatus item)
			{
				return item.OrderStatusID;
			}
			
			protected override List<OrderStatus> PerformInitializeList()
			{
				var result = OrderStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public OrderTypeCache OrderTypes = new OrderTypeCache();
		public sealed class OrderTypeCache : SmallCollectionBase<OrderType, Int16>
		{
			public OrderTypeCache()
				: base("OrderTypeListCache")
			{ }

			protected override Int16 PerformGetKey(OrderType item)
			{
				return item.OrderTypeID;
			}
			
			protected override List<OrderType> PerformInitializeList()
			{
				var result = OrderType.LoadAllFull();
				return result.ToList();
			}
		}

		public PageTypeCache PageTypes = new PageTypeCache();
		public sealed class PageTypeCache : SmallCollectionBase<PageType, Int16>
		{
			public PageTypeCache()
				: base("PageTypeListCache")
			{ }

			protected override Int16 PerformGetKey(PageType item)
			{
				return item.PageTypeID;
			}
			
			protected override List<PageType> PerformInitializeList()
			{
				var result = PageType.LoadAllFull();
				return result.ToList();
			}
		}

		public PaymentGatewayCache PaymentGateways = new PaymentGatewayCache();
		public sealed class PaymentGatewayCache : SmallCollectionBase<PaymentGateway, Int16>
		{
			public PaymentGatewayCache()
				: base("PaymentGatewayListCache")
			{ }

			protected override Int16 PerformGetKey(PaymentGateway item)
			{
				return item.PaymentGatewayID;
			}
			
			protected override List<PaymentGateway> PerformInitializeList()
			{
				var result = PaymentGateway.LoadAllFull();
				return result.ToList();
			}
		}

		public PaymentOrderTypeCache PaymentOrderTypes = new PaymentOrderTypeCache();
		public sealed class PaymentOrderTypeCache : SmallCollectionBase<PaymentOrderType, Int32>
		{
			public PaymentOrderTypeCache()
				: base("PaymentOrderTypeListCache")
			{ }

			protected override Int32 PerformGetKey(PaymentOrderType item)
			{
				return item.PaymentOrderTypeID;
			}
			
			protected override List<PaymentOrderType> PerformInitializeList()
			{
				var result = PaymentOrderType.LoadAllFull();
				return result.ToList();
			}
		}

		public PaymentTypeCache PaymentTypes = new PaymentTypeCache();
		public sealed class PaymentTypeCache : SmallCollectionBase<PaymentType, Int32>
		{
			public PaymentTypeCache()
				: base("PaymentTypeListCache")
			{ }

			protected override Int32 PerformGetKey(PaymentType item)
			{
				return item.PaymentTypeID;
			}
			
			protected override List<PaymentType> PerformInitializeList()
			{
				var result = PaymentType.LoadAllFull();
				return result.ToList();
			}
		}

		public PhoneTypeCache PhoneTypes = new PhoneTypeCache();
		public sealed class PhoneTypeCache : SmallCollectionBase<PhoneType, Int32>
		{
			public PhoneTypeCache()
				: base("PhoneTypeListCache")
			{ }

			protected override Int32 PerformGetKey(PhoneType item)
			{
				return item.PhoneTypeID;
			}
			
			protected override List<PhoneType> PerformInitializeList()
			{
				var result = PhoneType.LoadAllFull();
				return result.ToList();
			}
		}

		public PriceRelationshipTypeCache PriceRelationshipTypes = new PriceRelationshipTypeCache();
		public sealed class PriceRelationshipTypeCache : SmallCollectionBase<PriceRelationshipType, Int32>
		{
			public PriceRelationshipTypeCache()
				: base("PriceRelationshipTypeListCache")
			{ }

			protected override Int32 PerformGetKey(PriceRelationshipType item)
			{
				return item.PriceRelationshipTypeID;
			}
			
			protected override List<PriceRelationshipType> PerformInitializeList()
			{
				var result = PriceRelationshipType.LoadAllFull();
				return result.ToList();
			}
		}

		public ProductBackOrderBehaviorCache ProductBackOrderBehaviors = new ProductBackOrderBehaviorCache();
		public sealed class ProductBackOrderBehaviorCache : SmallCollectionBase<ProductBackOrderBehavior, Int16>
		{
			public ProductBackOrderBehaviorCache()
				: base("ProductBackOrderBehaviorListCache")
			{ }

			protected override Int16 PerformGetKey(ProductBackOrderBehavior item)
			{
				return item.ProductBackOrderBehaviorID;
			}
			
			protected override List<ProductBackOrderBehavior> PerformInitializeList()
			{
				var result = ProductBackOrderBehavior.LoadAllFull();
				return result.ToList();
			}
		}

		public ProductFileTypeCache ProductFileTypes = new ProductFileTypeCache();
		public sealed class ProductFileTypeCache : SmallCollectionBase<ProductFileType, Int32>
		{
			public ProductFileTypeCache()
				: base("ProductFileTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ProductFileType item)
			{
				return item.ProductFileTypeID;
			}
			
			protected override List<ProductFileType> PerformInitializeList()
			{
				var result = ProductFileType.LoadAllFull();
				return result.ToList();
			}
		}

		public ProductPriceTypeCache ProductPriceTypes = new ProductPriceTypeCache();
		public sealed class ProductPriceTypeCache : SmallCollectionBase<ProductPriceType, Int32>
		{
			public ProductPriceTypeCache()
				: base("ProductPriceTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ProductPriceType item)
			{
				return item.ProductPriceTypeID;
			}
			
			protected override List<ProductPriceType> PerformInitializeList()
			{
				var result = ProductPriceType.LoadAllFull();
				return result.ToList();
			}
		}

		public ProductPropertyTypeCache ProductPropertyTypes = new ProductPropertyTypeCache();
		public sealed class ProductPropertyTypeCache : SmallCollectionBase<ProductPropertyType, Int32>
		{
			public ProductPropertyTypeCache()
				: base("ProductPropertyTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ProductPropertyType item)
			{
				return item.ProductPropertyTypeID;
			}
			
			protected override List<ProductPropertyType> PerformInitializeList()
			{
				var result = ProductPropertyType.LoadAllFull();
				return result.ToList();
			}
		}

		public ProductRelationsTypeCache ProductRelationsTypes = new ProductRelationsTypeCache();
		public sealed class ProductRelationsTypeCache : SmallCollectionBase<ProductRelationsType, Int32>
		{
			public ProductRelationsTypeCache()
				: base("ProductRelationsTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ProductRelationsType item)
			{
				return item.ProductRelationTypeID;
			}
			
			protected override List<ProductRelationsType> PerformInitializeList()
			{
				var result = ProductRelationsType.LoadAllFull();
				return result.ToList();
			}
		}

		public ProductTypeCache ProductTypes = new ProductTypeCache();
		public sealed class ProductTypeCache : SmallCollectionBase<ProductType, Int32>
		{
			public ProductTypeCache()
				: base("ProductTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ProductType item)
			{
				return item.ProductTypeID;
			}
			
			protected override List<ProductType> PerformInitializeList()
			{
				var result = ProductType.LoadAllFull();
				return result.ToList();
			}
		}

		public ProxyLinkCache ProxyLinks = new ProxyLinkCache();
		public sealed class ProxyLinkCache : SmallCollectionBase<ProxyLink, Int32>
		{
			public ProxyLinkCache()
				: base("ProxyLinkListCache")
			{ }

			protected override Int32 PerformGetKey(ProxyLink item)
			{
				return item.ProxyLinkID;
			}
			
			protected override List<ProxyLink> PerformInitializeList()
			{
				var result = ProxyLink.LoadAllFull();
				return result.ToList();
			}
		}

		public PublicationChannelCache PublicationChannels = new PublicationChannelCache();
		public sealed class PublicationChannelCache : SmallCollectionBase<PublicationChannel, Int16>
		{
			public PublicationChannelCache()
				: base("PublicationChannelListCache")
			{ }

			protected override Int16 PerformGetKey(PublicationChannel item)
			{
				return item.PublicationChannelID;
			}
			
			protected override List<PublicationChannel> PerformInitializeList()
			{
				var result = PublicationChannel.LoadAllFull();
				return result.ToList();
			}
		}

		public QueueItemPriorityCache QueueItemPriorities = new QueueItemPriorityCache();
		public sealed class QueueItemPriorityCache : SmallCollectionBase<QueueItemPriority, Int16>
		{
			public QueueItemPriorityCache()
				: base("QueueItemPriorityListCache")
			{ }

			protected override Int16 PerformGetKey(QueueItemPriority item)
			{
				return item.QueueItemPriorityID;
			}
			
			protected override List<QueueItemPriority> PerformInitializeList()
			{
				var result = QueueItemPriority.LoadAllFull();
				return result.ToList();
			}
		}

		public QueueItemStatusCache QueueItemStatuses = new QueueItemStatusCache();
		public sealed class QueueItemStatusCache : SmallCollectionBase<QueueItemStatus, Int16>
		{
			public QueueItemStatusCache()
				: base("QueueItemStatusListCache")
			{ }

			protected override Int16 PerformGetKey(QueueItemStatus item)
			{
				return item.QueueItemStatusID;
			}
			
			protected override List<QueueItemStatus> PerformInitializeList()
			{
				var result = QueueItemStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public RedemptionMethodCache RedemptionMethods = new RedemptionMethodCache();
		public sealed class RedemptionMethodCache : SmallCollectionBase<RedemptionMethod, Int16>
		{
			public RedemptionMethodCache()
				: base("RedemptionMethodListCache")
			{ }

			protected override Int16 PerformGetKey(RedemptionMethod item)
			{
				return item.RedemptionMethodID;
			}
			
			protected override List<RedemptionMethod> PerformInitializeList()
			{
				var result = RedemptionMethod.LoadAllFull();
				return result.ToList();
			}
		}

		public ReplacementReasonCache ReplacementReasons = new ReplacementReasonCache();
		public sealed class ReplacementReasonCache : SmallCollectionBase<ReplacementReason, Int32>
		{
			public ReplacementReasonCache()
				: base("ReplacementReasonListCache")
			{ }

			protected override Int32 PerformGetKey(ReplacementReason item)
			{
				return item.ReplacementReasonID;
			}
			
			protected override List<ReplacementReason> PerformInitializeList()
			{
				var result = ReplacementReason.LoadAllFull();
				return result.ToList();
			}
		}

		public ReturnReasonCache ReturnReasons = new ReturnReasonCache();
		public sealed class ReturnReasonCache : SmallCollectionBase<ReturnReason, Int32>
		{
			public ReturnReasonCache()
				: base("ReturnReasonListCache")
			{ }

			protected override Int32 PerformGetKey(ReturnReason item)
			{
				return item.ReturnReasonID;
			}
			
			protected override List<ReturnReason> PerformInitializeList()
			{
				var result = ReturnReason.LoadAllFull();
				return result.ToList();
			}
		}

		public ReturnTypeCache ReturnTypes = new ReturnTypeCache();
		public sealed class ReturnTypeCache : SmallCollectionBase<ReturnType, Int32>
		{
			public ReturnTypeCache()
				: base("ReturnTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ReturnType item)
			{
				return item.ReturnTypeID;
			}
			
			protected override List<ReturnType> PerformInitializeList()
			{
				var result = ReturnType.LoadAllFull();
				return result.ToList();
			}
		}

		public RoleCache Roles = new RoleCache();
		public sealed class RoleCache : SmallCollectionBase<Role, Int32>
		{
			public RoleCache()
				: base("RoleListCache")
			{ }

			protected override Int32 PerformGetKey(Role item)
			{
				return item.RoleID;
			}
			
			protected override List<Role> PerformInitializeList()
			{
				var result = Role.LoadAllFull();
				return result.ToList();
			}
		}

		public RoleTypeCache RoleTypes = new RoleTypeCache();
		public sealed class RoleTypeCache : SmallCollectionBase<RoleType, Int16>
		{
			public RoleTypeCache()
				: base("RoleTypeListCache")
			{ }

			protected override Int16 PerformGetKey(RoleType item)
			{
				return item.RoleTypeID;
			}
			
			protected override List<RoleType> PerformInitializeList()
			{
				var result = RoleType.LoadAllFull();
				return result.ToList();
			}
		}

		public ShippingMethodCache ShippingMethods = new ShippingMethodCache();
		public sealed class ShippingMethodCache : SmallCollectionBase<ShippingMethod, Int32>
		{
			public ShippingMethodCache()
				: base("ShippingMethodListCache")
			{ }

			protected override Int32 PerformGetKey(ShippingMethod item)
			{
				return item.ShippingMethodID;
			}
			
			protected override List<ShippingMethod> PerformInitializeList()
			{
				var result = ShippingMethod.LoadAllFull();
				return result.ToList();
			}
		}

		public ShippingOrderTypeCache ShippingOrderTypes = new ShippingOrderTypeCache();
		public sealed class ShippingOrderTypeCache : SmallCollectionBase<ShippingOrderType, Int32>
		{
			public ShippingOrderTypeCache()
				: base("ShippingOrderTypeListCache")
			{ }

			protected override Int32 PerformGetKey(ShippingOrderType item)
			{
				return item.ShippingOrderTypeID;
			}
			
			protected override List<ShippingOrderType> PerformInitializeList()
			{
				var result = ShippingOrderType.LoadAllFull();
				return result.ToList();
			}
		}

		public ShippingRateCache ShippingRates = new ShippingRateCache();
		public sealed class ShippingRateCache : SmallCollectionBase<ShippingRate, Int32>
		{
			public ShippingRateCache()
				: base("ShippingRateListCache")
			{ }

			protected override Int32 PerformGetKey(ShippingRate item)
			{
				return item.ShippingRateID;
			}
			
			protected override List<ShippingRate> PerformInitializeList()
			{
				var result = ShippingRate.LoadAllFull();
				return result.ToList();
			}
		}

		public ShippingRateGroupCache ShippingRateGroups = new ShippingRateGroupCache();
		public sealed class ShippingRateGroupCache : SmallCollectionBase<ShippingRateGroup, Int32>
		{
			public ShippingRateGroupCache()
				: base("ShippingRateGroupListCache")
			{ }

			protected override Int32 PerformGetKey(ShippingRateGroup item)
			{
				return item.ShippingRateGroupID;
			}
			
			protected override List<ShippingRateGroup> PerformInitializeList()
			{
				var result = ShippingRateGroup.LoadAllFull();
				return result.ToList();
			}
		}

		public ShippingRateTypeCache ShippingRateTypes = new ShippingRateTypeCache();
		public sealed class ShippingRateTypeCache : SmallCollectionBase<ShippingRateType, Int16>
		{
			public ShippingRateTypeCache()
				: base("ShippingRateTypeListCache")
			{ }

			protected override Int16 PerformGetKey(ShippingRateType item)
			{
				return item.ShippingRateTypeID;
			}
			
			protected override List<ShippingRateType> PerformInitializeList()
			{
				var result = ShippingRateType.LoadAllFull();
				return result.ToList();
			}
		}

		public ShippingRegionCache ShippingRegions = new ShippingRegionCache();
		public sealed class ShippingRegionCache : SmallCollectionBase<ShippingRegion, Int32>
		{
			public ShippingRegionCache()
				: base("ShippingRegionListCache")
			{ }

			protected override Int32 PerformGetKey(ShippingRegion item)
			{
				return item.ShippingRegionID;
			}
			
			protected override List<ShippingRegion> PerformInitializeList()
			{
				var result = ShippingRegion.LoadAllFull();
				return result.ToList();
			}
		}

		public SiteStatusCache SiteStatuses = new SiteStatusCache();
		public sealed class SiteStatusCache : SmallCollectionBase<SiteStatus, Int16>
		{
			public SiteStatusCache()
				: base("SiteStatusListCache")
			{ }

			protected override Int16 PerformGetKey(SiteStatus item)
			{
				return item.SiteStatusID;
			}
			
			protected override List<SiteStatus> PerformInitializeList()
			{
				var result = SiteStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public SiteStatusChangeReasonCache SiteStatusChangeReasons = new SiteStatusChangeReasonCache();
		public sealed class SiteStatusChangeReasonCache : SmallCollectionBase<SiteStatusChangeReason, Int16>
		{
			public SiteStatusChangeReasonCache()
				: base("SiteStatusChangeReasonListCache")
			{ }

			protected override Int16 PerformGetKey(SiteStatusChangeReason item)
			{
				return item.SiteStatusChangeReasonID;
			}
			
			protected override List<SiteStatusChangeReason> PerformInitializeList()
			{
				var result = SiteStatusChangeReason.LoadAllFull();
				return result.ToList();
			}
		}

		public SiteTypeCache SiteTypes = new SiteTypeCache();
		public sealed class SiteTypeCache : SmallCollectionBase<SiteType, Int16>
		{
			public SiteTypeCache()
				: base("SiteTypeListCache")
			{ }

			protected override Int16 PerformGetKey(SiteType item)
			{
				return item.SiteTypeID;
			}
			
			protected override List<SiteType> PerformInitializeList()
			{
				var result = SiteType.LoadAllFull();
				return result.ToList();
			}
		}

		public StateProvinceCache StateProvinces = new StateProvinceCache();
		public sealed class StateProvinceCache : SmallCollectionBase<StateProvince, Int32>
		{
			public StateProvinceCache()
				: base("StateProvinceListCache")
			{ }

			protected override Int32 PerformGetKey(StateProvince item)
			{
				return item.StateProvinceID;
			}
			
			protected override List<StateProvince> PerformInitializeList()
			{
				var result = StateProvince.LoadAllFull();
				return result.ToList();
			}
		}

		public StatisticTypeCache StatisticTypes = new StatisticTypeCache();
		public sealed class StatisticTypeCache : SmallCollectionBase<StatisticType, Int16>
		{
			public StatisticTypeCache()
				: base("StatisticTypeListCache")
			{ }

			protected override Int16 PerformGetKey(StatisticType item)
			{
				return item.StatisticTypeID;
			}
			
			protected override List<StatisticType> PerformInitializeList()
			{
				var result = StatisticType.LoadAllFull();
				return result.ToList();
			}
		}

		public StatisticValueTypeCache StatisticValueTypes = new StatisticValueTypeCache();
		public sealed class StatisticValueTypeCache : SmallCollectionBase<StatisticValueType, Int16>
		{
			public StatisticValueTypeCache()
				: base("StatisticValueTypeListCache")
			{ }

			protected override Int16 PerformGetKey(StatisticValueType item)
			{
				return item.StatisticValueTypeID;
			}
			
			protected override List<StatisticValueType> PerformInitializeList()
			{
				var result = StatisticValueType.LoadAllFull();
				return result.ToList();
			}
		}

		public StoreFrontCache StoreFronts = new StoreFrontCache();
		public sealed class StoreFrontCache : SmallCollectionBase<StoreFront, Int32>
		{
			public StoreFrontCache()
				: base("StoreFrontListCache")
			{ }

			protected override Int32 PerformGetKey(StoreFront item)
			{
				return item.StoreFrontID;
			}
			
			protected override List<StoreFront> PerformInitializeList()
			{
				var result = StoreFront.LoadAllFull();
				return result.ToList();
			}
		}

		public SupportTicketCategoryCache SupportTicketCategories = new SupportTicketCategoryCache();
		public sealed class SupportTicketCategoryCache : SmallCollectionBase<SupportTicketCategory, Int16>
		{
			public SupportTicketCategoryCache()
				: base("SupportTicketCategoryListCache")
			{ }

			protected override Int16 PerformGetKey(SupportTicketCategory item)
			{
				return item.SupportTicketCategoryID;
			}
			
			protected override List<SupportTicketCategory> PerformInitializeList()
			{
				var result = SupportTicketCategory.LoadAllFull();
				return result.ToList();
			}
		}

		public SupportTicketPriorityCache SupportTicketPriorities = new SupportTicketPriorityCache();
		public sealed class SupportTicketPriorityCache : SmallCollectionBase<SupportTicketPriority, Int16>
		{
			public SupportTicketPriorityCache()
				: base("SupportTicketPriorityListCache")
			{ }

			protected override Int16 PerformGetKey(SupportTicketPriority item)
			{
				return item.SupportTicketPriorityID;
			}
			
			protected override List<SupportTicketPriority> PerformInitializeList()
			{
				var result = SupportTicketPriority.LoadAllFull();
				return result.ToList();
			}
		}

		public SupportTicketStatusCache SupportTicketStatuses = new SupportTicketStatusCache();
		public sealed class SupportTicketStatusCache : SmallCollectionBase<SupportTicketStatus, Int16>
		{
			public SupportTicketStatusCache()
				: base("SupportTicketStatusListCache")
			{ }

			protected override Int16 PerformGetKey(SupportTicketStatus item)
			{
				return item.SupportTicketStatusID;
			}
			
			protected override List<SupportTicketStatus> PerformInitializeList()
			{
				var result = SupportTicketStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public TaxCategoryCache TaxCategories = new TaxCategoryCache();
		public sealed class TaxCategoryCache : SmallCollectionBase<TaxCategory, Int32>
		{
			public TaxCategoryCache()
				: base("TaxCategoryListCache")
			{ }

			protected override Int32 PerformGetKey(TaxCategory item)
			{
				return item.TaxCategoryID;
			}
			
			protected override List<TaxCategory> PerformInitializeList()
			{
				var result = TaxCategory.LoadAllFull();
				return result.ToList();
			}
		}

		public TaxDataSourceCache TaxDataSources = new TaxDataSourceCache();
		public sealed class TaxDataSourceCache : SmallCollectionBase<TaxDataSource, Int16>
		{
			public TaxDataSourceCache()
				: base("TaxDataSourceListCache")
			{ }

			protected override Int16 PerformGetKey(TaxDataSource item)
			{
				return item.TaxDataSourceID;
			}
			
			protected override List<TaxDataSource> PerformInitializeList()
			{
				var result = TaxDataSource.LoadAllFull();
				return result.ToList();
			}
		}

		public TimeUnitTypeCache TimeUnitTypes = new TimeUnitTypeCache();
		public sealed class TimeUnitTypeCache : SmallCollectionBase<TimeUnitType, Int16>
		{
			public TimeUnitTypeCache()
				: base("TimeUnitTypeListCache")
			{ }

			protected override Int16 PerformGetKey(TimeUnitType item)
			{
				return item.TimeUnitTypeID;
			}
			
			protected override List<TimeUnitType> PerformInitializeList()
			{
				var result = TimeUnitType.LoadAllFull();
				return result.ToList();
			}
		}

		public TokenCache Tokens = new TokenCache();
		public sealed class TokenCache : SmallCollectionBase<Token, Int32>
		{
			public TokenCache()
				: base("TokenListCache")
			{ }

			protected override Int32 PerformGetKey(Token item)
			{
				return item.TokenID;
			}
			
			protected override List<Token> PerformInitializeList()
			{
				var result = Token.LoadAllFull();
				return result.ToList();
			}
		}

		public UserStatusCache UserStatuses = new UserStatusCache();
		public sealed class UserStatusCache : SmallCollectionBase<UserStatus, Int16>
		{
			public UserStatusCache()
				: base("UserStatusListCache")
			{ }

			protected override Int16 PerformGetKey(UserStatus item)
			{
				return item.UserStatusID;
			}
			
			protected override List<UserStatus> PerformInitializeList()
			{
				var result = UserStatus.LoadAllFull();
				return result.ToList();
			}
		}

		public UserTypeCache UserTypes = new UserTypeCache();
		public sealed class UserTypeCache : SmallCollectionBase<UserType, Int16>
		{
			public UserTypeCache()
				: base("UserTypeListCache")
			{ }

			protected override Int16 PerformGetKey(UserType item)
			{
				return item.UserTypeID;
			}
			
			protected override List<UserType> PerformInitializeList()
			{
				var result = UserType.LoadAllFull();
				return result.ToList();
			}
		}

		public WarehouseCache Warehouses = new WarehouseCache();
		public sealed class WarehouseCache : SmallCollectionBase<Warehouse, Int32>
		{
			public WarehouseCache()
				: base("WarehouseListCache")
			{ }

			protected override Int32 PerformGetKey(Warehouse item)
			{
				return item.WarehouseID;
			}
			
			protected override List<Warehouse> PerformInitializeList()
			{
				var result = Warehouse.LoadAllFull();
				return result.ToList();
			}
		}

		public WidgetCache Widgets = new WidgetCache();
		public sealed class WidgetCache : SmallCollectionBase<Widget, Int32>
		{
			public WidgetCache()
				: base("WidgetListCache")
			{ }

			protected override Int32 PerformGetKey(Widget item)
			{
				return item.WidgetID;
			}
			
			protected override List<Widget> PerformInitializeList()
			{
				var result = Widget.LoadAllFull();
				return result.ToList();
			}
		}

	}
}

