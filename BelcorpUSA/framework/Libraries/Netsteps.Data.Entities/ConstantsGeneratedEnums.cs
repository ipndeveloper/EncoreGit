
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//  Enum Constants
//	Author: John Egbert
//  Generated: 12/10/2016 05:56:12
namespace NetSteps.Data.Entities.Generated
{
	public partial class ConstantsGenerated : NetSteps.Common.Constants
	{
		

		#region AccountPropertyTypes
		public enum AccountPropertyType : int
		{
			NotSet = 0,
		}
		#endregion
			

		#region AccountReportTypes
		public enum AccountReportType : short
		{
			NotSet = 0,
			ContactsReport = 1,
			DownlineReport = 4,
		}
		#endregion
			

		#region AccountSources
		public enum AccountSource : short
		{
			NotSet = 0,
			ConsultantLocator = 1,
			OnlineGuest = 2,
			PartyGuest = 3,
			ManuallyEntered = 4,
		}
		#endregion
			

		#region AccountSponsorTypes
		public enum AccountSponsorType : int
		{
			NotSet = 0,
			Enroller = 1,
			Sponsor = 2,
		}
		#endregion
			

		#region AccountStatusChangeReasons
		public enum AccountStatusChangeReason : short
		{
			NotSet = 0,
		}
		#endregion
			

		#region AccountStatuses
		public enum AccountStatus : short
		{
			NotSet = 0,
			Active = 1,
			Terminated = 2,
			BegunEnrollment = 3,
			Imported = 4,
		}
		#endregion
			

		#region AccountTypes
		public enum AccountType : short
		{
			NotSet = 0,
			Distributor = 1,
			PreferredCustomer = 2,
			RetailCustomer = 3,
			Employee = 4,
			Prospect = 5,
			Testing = 6,
			Temps = 7,
		}
		#endregion
			

		#region AddressPropertyTypes
		public enum AddressPropertyType : int
		{
			NotSet = 0,
			ServiceArea = 1,
			LocationDescription = 2,
			LocationUrl = 3,
			LocationLogoName = 4,
			LocationPhotoNames = 5,
			Keywords = 6,
			EmailAddress = 7,
		}
		#endregion
			

		#region AddressTypes
		public enum AddressType : short
		{
			NotSet = 0,
			Main = 1,
			Shipping = 2,
			Billing = 3,
			Autoship = 4,
			Disbursement = 5,
			Warehouse = 6,
			Event = 7,
			Party = 8,
			Hostess = 9,
			LogisticsProvider = 10,
			Location = 11,
			Archived = 12,
			PickupPoint = 13,
		}
		#endregion
			

		#region AlertPriorities
		public enum AlertPriority : short
		{
			NotSet = 0,
			Low = 1,
			Medium = 2,
			High = 3,
			Urgent = 4,
		}
		#endregion
			

		#region ArchiveTypes
		public enum ArchiveType : short
		{
			NotSet = 0,
			PDF = 1,
			Image = 3,
			Net = 4,
		}
		#endregion
			

		#region AuditChangeTypes
		public enum AuditChangeType : byte
		{
			NotSet = 0,
			Insert = 1,
			Update = 2,
			Delete = 3,
		}
		#endregion
			

		#region AutoresponderTypes
		public enum AutoresponderType : short
		{
			NotSet = 0,
			Autoresponder = 1,
			Notification = 2,
		}
		#endregion
			

		#region AutoshipScheduleTypes
		public enum AutoshipScheduleType : int
		{
			NotSet = 0,
			Normal = 1,
			Subscription = 2,
			SiteSubscription = 3,
		}
		#endregion
			

		#region CampaignActionTypes
		public enum CampaignActionType : short
		{
			NotSet = 0,
			Email = 1,
			Alert = 2,
		}
		#endregion
			

		#region CampaignTypes
		public enum CampaignType : short
		{
			NotSet = 0,
			MassEmails = 1,
			Campaigns = 2,
			EventBasedEmails = 3,
			MassAlerts = 4,
			Newsletters = 6,
		}
		#endregion
			

		#region CatalogTypes
		public enum CatalogType : short
		{
			NotSet = 0,
			Normal = 1,
			FeaturedItems = 2,
			EnrollmentKits = 3,
			AutoshipBundles = 4,
			AutoshipItems = 5,
			Fundraiser = 6,
			KitContents = 7,
			EnrollmentItems = 8,
		}
		#endregion
			

		#region CategoryTypes
		public enum CategoryType : short
		{
			NotSet = 0,
			Product = 1,
			Archive = 2,
		}
		#endregion
			

		#region Countries
		public enum Country : int
		{
			NotSet = 0,
			UnitedStates = 1,
			Canada = 2,
			Australia = 3,
			UK = 6,
			Ireland = 7,
			Sweden = 8,
			Netherlands = 9,
			Belgium = 10,
			Scotland = 11,
			Wales = 12,
			NorthernIreland = 13,
			France = 14,
			Austria = 37,
			CzechRepublic = 38,
			Denmark = 39,
			Finland = 40,
			Germany = 41,
			Italy = 42,
			Norway = 43,
			Poland = 44,
			Slovakia = 45,
			Afghanistan = 48,
			Albania = 49,
			Algeria = 50,
			AmericanSamoa = 51,
			Andorra = 52,
			Angola = 53,
			Anguilla = 54,
			Antarctica = 55,
			AntiguaAndBarbuda = 56,
			Argentina = 57,
			Armenia = 58,
			Aruba = 59,
			Azerbaijan = 60,
			Bahamas = 61,
			Bahrain = 62,
			Bangladesh = 63,
			Barbados = 64,
			Belarus = 65,
			Belize = 66,
			Benin = 67,
			Bermuda = 68,
			Bhutan = 69,
			Bolivia = 70,
			BosniaAndHerzegovina = 71,
			Botswana = 72,
			Brazil = 73,
			BritishIndianOceanTerritory = 74,
			BritishVirginIslands = 75,
			Brunei = 76,
			Bulgaria = 77,
			BurkinaFaso = 78,
			Myanmar = 79,
			Burundi = 80,
			Cambodia = 81,
			Cameroon = 82,
			CapeVerde = 83,
			CaymanIslands = 84,
			CentralAfricanRepublic = 85,
			Chad = 86,
			Chile = 87,
			China = 88,
			ChristmasIsland = 89,
			CocoskeelingIslands = 90,
			Colombia = 91,
			Comoros = 92,
			CookIslands = 93,
			CostaRica = 94,
			Croatia = 95,
			Cuba = 96,
			Cyprus = 97,
			DemocraticRepublicOfTheCongo = 98,
			Djibouti = 99,
			Dominica = 100,
			DominicanRepublic = 101,
			Ecuador = 102,
			Egypt = 103,
			ElSalvador = 104,
			EquatorialGuinea = 105,
			Eritrea = 106,
			Estonia = 107,
			Ethiopia = 108,
			FalklandIslands = 109,
			FaroeIslands = 110,
			Fiji = 111,
			FrenchPolynesia = 112,
			Gabon = 113,
			Gambia = 114,
			Georgia = 115,
			Ghana = 116,
			Gibraltar = 117,
			Greece = 118,
			Greenland = 119,
			Grenada = 120,
			Guam = 121,
			Guatemala = 122,
			Guinea = 123,
			GuineaBissau = 124,
			Guyana = 125,
			Haiti = 126,
			HolySeevaticanCity = 127,
			Honduras = 128,
			HongKong = 129,
			Hungary = 130,
			Iceland = 131,
			India = 132,
			Indonesia = 133,
			Iran = 134,
			Iraq = 135,
			Israel = 136,
			IvoryCoast = 137,
			Jamaica = 138,
			Japan = 139,
			Jordan = 140,
			Kazakhstan = 141,
			Kenya = 142,
			Kiribati = 143,
			Kuwait = 144,
			Kyrgyzstan = 145,
			Laos = 146,
			Latvia = 147,
			Lebanon = 148,
			Lesotho = 149,
			Liberia = 150,
			Libya = 151,
			Lithuania = 153,
			Luxembourg = 154,
			Macau = 155,
			Macedonia = 156,
			Madagascar = 157,
			Malawi = 158,
			Malaysia = 159,
			Maldives = 160,
			Mali = 161,
			Malta = 162,
			MarshallIslands = 163,
			Mauritania = 164,
			Mauritius = 165,
			Mayotte = 166,
			Mexico = 167,
			Micronesia = 168,
			Moldova = 169,
			Monaco = 170,
			Mongolia = 171,
			Montserrat = 172,
			Morocco = 173,
			Mozambique = 174,
			Namibia = 175,
			Nauru = 176,
			Nepal = 177,
			NetherlandsAntilles = 178,
			NewCaledonia = 179,
			NewZealand = 180,
			Nicaragua = 181,
			Niger = 182,
			Nigeria = 183,
			Niue = 184,
			NorfolkIsland = 185,
			NorthKorea = 186,
			NorthernMarianaIslands = 187,
			Oman = 188,
			Pakistan = 189,
			Palau = 190,
			Panama = 191,
			PapuaNewGuinea = 192,
			Paraguay = 193,
			Peru = 194,
			Philippines = 195,
			PitcairnIslands = 196,
			Portugal = 197,
			PuertoRico = 198,
			Qatar = 199,
			RepublicOfTheCongo = 200,
			Romania = 201,
			Russia = 202,
			Rwanda = 203,
			SaintHelena = 204,
			SaintKittsAndNevis = 205,
			SaintLucia = 206,
			SaintPierreAndMiquelon = 207,
			SaintVincentAndTheGrenadines = 208,
			Samoa = 209,
			SanMarino = 210,
			SaoTomeAndPrincipe = 211,
			SaudiArabia = 212,
			Senegal = 213,
			Serbia = 214,
			Seychelles = 215,
			SierraLeone = 216,
			Singapore = 217,
			Slovenia = 218,
			SolomonIslands = 219,
			Somalia = 220,
			SouthAfrica = 221,
			SouthKorea = 222,
			Spain = 223,
			SriLanka = 224,
			Sudan = 225,
			Suriname = 226,
			Svalbard = 227,
			Swaziland = 228,
			Syria = 229,
			Taiwan = 230,
			Tajikistan = 231,
			Tanzania = 232,
			Thailand = 233,
			TimorLeste = 234,
			Togo = 235,
			Tokelau = 236,
			Tonga = 237,
			TrinidadAndTobago = 238,
			Tunisia = 239,
			Turkey = 240,
			Turkmenistan = 241,
			TurksAndCaicosIslands = 242,
			Tuvalu = 243,
			Uganda = 244,
			Ukraine = 245,
			UnitedArabEmirates = 246,
			Uruguay = 247,
			UsVirginIslands = 248,
			Uzbekistan = 249,
			Vanuatu = 250,
			Venezuela = 251,
			Vietnam = 252,
			WallisAndFutuna = 253,
			WesternSahara = 254,
			Yemen = 255,
			Zambia = 256,
			Zimbabwe = 257,
			Martinique = 258,
			Guadeloupe = 259,
			PalestinianTerritoryOccupied = 260,
			Reunion = 261,
		}
		#endregion
			

		#region CreditCardTypes
		public enum CreditCardType : short
		{
			NotSet = 0,
			Visa = 1,
			MasterCard = 2,
			AmericanExpress = 3,
			DinersClub = 4,
			Discover = 5,
			JCB = 6,
		}
		#endregion
			

		#region Currencies
		public enum Currency : int
		{
			NotSet = 0,
			UsDollar = 1,
			CanadianDollar = 2,
			JapaneseYen = 4,
			AustraliaDollar = 5,
			UnitedKingdomPounds = 7,
			FrenchEuro = 8,
			CzechKoruna = 9,
			DanishKrone = 10,
			NorwegianKrone = 11,
			PolishZłoty = 12,
			SwedishKrona = 13,
			Afghani = 15,
			Lek = 16,
			AlgerianDinar = 17,
			Kwanza = 18,
			EastCaribbeanDollar = 19,
			ArgentinePeso = 20,
			ArmenianDram = 21,
			ArubanGuilder = 22,
			AzerbaijanianManat = 23,
			BahamianDollar = 24,
			BahrainiDinar = 25,
			Taka = 26,
			BarbadosDollar = 27,
			BelarussianRuble = 28,
			BelizeDollar = 29,
			CfaFrancBceao = 30,
			BermudianDollar = 31,
			IndianRupee = 32,
			Boliviano = 33,
			ConvertibleMarks = 34,
			Pula = 35,
			BrazilianReal = 36,
			BruneiDollar = 37,
			BulgarianLev = 38,
			Kyat = 39,
			BurundiFranc = 40,
			Riel = 41,
			CfaFrancBeac = 42,
			CapeVerdeEscudo = 43,
			CaymanIslandsDollar = 44,
			ChileanPeso = 45,
			YuanRenminbi = 46,
			ColombianPeso = 47,
			ComoroFranc = 48,
			NewZealandDollar = 49,
			CostaRicanColon = 50,
			CroatianKuna = 51,
			CubanPeso = 52,
			CyprusPound = 53,
			FrancCongolais = 54,
			DjiboutiFranc = 55,
			DominicanPeso = 56,
			EgyptianPound = 57,
			ElSalvadorColon = 58,
			Nakfa = 59,
			Kroon = 60,
			EthiopianBirr = 61,
			FalklandIslandsPound = 62,
			FijiDollar = 63,
			CfpFranc = 64,
			Dalasi = 65,
			Lari = 66,
			Cedi = 67,
			GibraltarPound = 68,
			Quetzal = 69,
			GuineaFranc = 70,
			GuineabissauPeso = 71,
			GuyanaDollar = 72,
			Gourde = 73,
			Lempira = 74,
			HongKongDollar = 75,
			Forint = 76,
			IcelandKrona = 77,
			Rupiah = 78,
			IranianRial = 79,
			IraqiDinar = 80,
			NewIsraeliSheqel = 81,
			JamaicanDollar = 82,
			Yen = 83,
			JordanianDinar = 84,
			Tenge = 85,
			KenyanShilling = 86,
			KuwaitiDinar = 87,
			Som = 88,
			Kip = 89,
			LatvianLat = 90,
			LebanesePound = 91,
			Rand = 92,
			LiberianDollar = 93,
			LibyanDinar = 94,
			LithuanianLitas = 95,
			Pataca = 96,
			Denar = 97,
			Ariary = 98,
			MalawianKwacha = 99,
			MalaysianRinggit = 100,
			Rufiyaa = 101,
			MalteseLira = 102,
			Ouguiya = 103,
			MauritiusRupee = 104,
			MexicanPeso = 105,
			MoldovanLeu = 106,
			Tugrik = 107,
			MoroccanDirham = 108,
			Metical = 109,
			NepaleseRupee = 110,
			NetherlandsAntilleanGuilder = 111,
			CordobaOro = 112,
			Naira = 113,
			NorthKoreanWon = 114,
			RialOmani = 115,
			PakistanRupee = 116,
			Balboa = 117,
			Kina = 118,
			Guarani = 119,
			NuevoSol = 120,
			PhilippinePeso = 121,
			QatariRial = 122,
			Leu = 123,
			RussianRuble = 124,
			RwandaFranc = 125,
			SaintHelenaPound = 126,
			Tala = 127,
			Dobra = 128,
			SaudiRiyal = 129,
			SeychellesRupee = 130,
			Leone = 131,
			SingaporeDollar = 132,
			SlovakKoruna = 133,
			Tolar = 134,
			SolomonIslandsDollar = 135,
			SomaliShilling = 136,
			Won = 137,
			SriLankaRupee = 138,
			SudaneseDinar = 139,
			SurinameDollar = 140,
			Lilangeni = 141,
			SyrianPound = 142,
			NewTaiwanDollar = 143,
			Somoni = 144,
			TanzanianShilling = 145,
			Baht = 146,
			Paanga = 147,
			TrinidadAndTobagoDollar = 148,
			TunisianDinar = 149,
			TurkishLira = 150,
			Manat = 151,
			UgandaShilling = 152,
			Hryvnia = 153,
			UaeDirham = 154,
			PesoUruguayo = 155,
			UzbekistanSum = 156,
			Vatu = 157,
			Bolivar = 158,
			Dong = 159,
			YemeniRial = 160,
			Kwacha = 161,
			ZimbabweDollar = 162,
			GermanEuro = 163,
			AustrianEuro = 164,
			ItalianEuro = 165,
			SlovakianEuro = 166,
			FinnishEuro = 167,
		}
		#endregion
			

		#region DistributionListTypes
		public enum DistributionListType : short
		{
			NotSet = 0,
			Groups = 1,
		}
		#endregion
			

		#region DomainEventTypeCategories
		public enum DomainEventTypeCategory : int
		{
			NotSet = 0,
			Campaign = 1,
			DomainEventTask = 2,
		}
		#endregion
			

		#region DomainEventTypes
		public enum DomainEventType : short
		{
			NotSet = 0,
			OrderCompleted = 1,
			EnrollmentCompleted = 2,
			AutoshipCreditCardFailed = 3,
			AccountCanceled = 4,
			AutoshipCanceled = 5,
			SupportTicketInfoRequest = 6,
			OrderShipped = 7,
			DistributorEnrolled = 8,
			BreakingNews = 9,
			ReferAFriend = 10,
			ProspectCreated = 11,
			AutoshipReminder = 20,
			TitlePromotionRecognitionAlert = 21,
			PartyGuestReminder = 22,
			AutoshipCanceledBcc = 30,
			ExpiringPromotionNotification = 40,
			BirthdayAlert = 41,
			CollectionLetter = 42,
			InvalidNotificationSponsor = 43,
			PaymentTicket = 44,
			OrderStatus = 45,
		}
		#endregion
			

		#region DynamicKitPricingTypes
		public enum DynamicKitPricingType : int
		{
			NotSet = 0,
			FixedPricing = 1,
			DynamicPricing = 2,
		}
		#endregion
			

		#region EmailBodyTextTypes
		public enum EmailBodyTextType : short
		{
			NotSet = 0,
			Style = 1,
			Text = 2,
			Invitation = 3,
		}
		#endregion
			

		#region EmailTemplateTypes
		public enum EmailTemplateType : short
		{
			NotSet = 0,
			Standard = 1,
			Campaign = 2,
			Newsletter = 3,
			Autoresponder = 4,
			EvitesHostessInvite = 5,
			EvitesCustomerInvite = 6,
			EvitesYesConfirmation = 7,
			EvitesNoConfirmation = 8,
			EvitesThankYou = 9,
			OrderPlacementAutoresponder = 10,
			SupportTicketInfoRequestAutoresponder = 11,
			AutoshipCreditCardFailed = 12,
			OrderShipped = 13,
			ConsultantJoinsDownline = 14,
			EnrollmentCompleted = 15,
			ContactMe = 16,
			ReferAFriend = 17,
			ReferAMerchant = 18,
			AutoshipReminder = 19,
			ProspectCreated = 20,
			ForgotPassword = 21,
			AutoshipCanceled = 22,
			ExpiringPromotionNotification = 23,
			UserNotification = 24,
			Terms = 25,
			OrderPayment = 30,
			BirthdayAlert = 31,
			InvalidNotificationSponsor = 32,
			PaymentTicket = 33,
			OrderStatus = 34,
		}
		#endregion
			

		#region FileResourcePropertyTypes
		public enum FileResourcePropertyType : int
		{
			NotSet = 0,
			ImageThumbnail = 1,
		}
		#endregion
			

		#region FileResourceTypes
		public enum FileResourceType : int
		{
			NotSet = 0,
			Image = 1,
		}
		#endregion
			

		#region Genders
		public enum Gender : short
		{
			NotSet = 0,
			Male = 1,
			Female = 2,
		}
		#endregion
			

		#region HostessRewardRuleTypes
		public enum HostessRewardRuleType : int
		{
			NotSet = 0,
			OrderSubtotal = 1,
			CustomerCount = 2,
			ExclusiveProducts = 3,
		}
		#endregion
			

		#region HostessRewardTypes
		public enum HostessRewardType : int
		{
			NotSet = 0,
			HostCredit = 1,
			PercentOff = 2,
			ItemDiscount = 3,
			BookingCredit = 4,
			EnrollmentProductCredit = 5,
			ExclusiveProduct = 6,
			FreeItem = 7,
		}
		#endregion
			

		#region HtmlContentEditorTypes
		public enum HtmlContentEditorType : short
		{
			NotSet = 0,
			PlainText = 1,
			RichText = 2,
			Photo = 3,
			PhotoCropper = 4,
		}
		#endregion
			

		#region HtmlContentStatuses
		public enum HtmlContentStatus : int
		{
			NotSet = 0,
			Preview = 1,
			Draft = 2,
			Submitted = 3,
			Disapproved = 4,
			Production = 5,
			Archive = 6,
			Pushed = 7,
		}
		#endregion
			

		#region HtmlContentWorkflowTypes
		public enum HtmlContentWorkflowType : short
		{
			NotSet = 0,
			Created = 1,
			Edited = 2,
			Scheduled = 3,
			Archived = 4,
		}
		#endregion
			

		#region HtmlElementTypes
		public enum HtmlElementType : byte
		{
			NotSet = 0,
			Caption = 1,
			Image = 2,
			Link = 3,
			Body = 4,
			Title = 5,
			CategoryDescription = 6,
			CategoryLogoName = 7,
			Head = 8,
		}
		#endregion
			

		#region HtmlInputTypes
		public enum HtmlInputType : short
		{
			NotSet = 0,
			Dropdown = 1,
			Radio = 2,
			Checkbox = 3,
			Thumbnail = 4,
			Text = 5,
		}
		#endregion
			

		#region HtmlSectionEditTypes
		public enum HtmlSectionEditType : short
		{
			NotSet = 0,
			CorporateOnly = 1,
			Choices = 2,
			Override = 3,
			ConsultantList = 4,
			ChoicesPlus = 5,
		}
		#endregion
			

		#region IntervalTypes
		public enum IntervalType : int
		{
			NotSet = 0,
			Monthly = 1,
			BiMonthly = 2,
			Annual = 3,
		}
		#endregion
			

		#region Languages
		public enum Language : int
		{
			NotSet = 0,
			English = 1,
			Spanish = 4,
			Portugues = 5,
		}
		#endregion
			

		#region ListValueTypes
		public enum ListValueType : short
		{
			NotSet = 0,
			ContactCategory = 4,
			ContactStatus = 5,
			ContactType = 11,
			ContactMethod = 13,
			CommunicationPreference = 14,
			AccountStatusChangeReason = 15,
			ArchiveType = 16,
			NewsType = 17,
			ReturnReasons = 18,
			ReturnType = 19,
			SiteStatusChangeReason = 20,
			SupportTicketPriority = 21,
			SupportTicketCategory = 22,
			ReplacementReason = 23,
		}
		#endregion
			

		#region NavigationTypes
		public enum NavigationType : int
		{
			NotSet = 0,
			Header = 1,
			Footer = 2,
		}
		#endregion
			

		#region NewsTypes
		public enum NewsType : short
		{
			NotSet = 0,
		}
		#endregion
			

		#region NoteTypes
		public enum NoteType : int
		{
			NotSet = 0,
			AccountNotes = 1,
			CRMNotes = 2,
			OrderInvoiceNotes = 4,
			OrderInternalNotes = 5,
			SupportTicketNotes = 6,
			LogisticNotes = 7,
		}
		#endregion
			

		#region OptOutTypes
		public enum OptOutType : short
		{
			NotSet = 0,
			EndUser = 1,
			HardBounce = 2,
			SoftBounce = 3,
		}
		#endregion
			

		#region OrderCustomerTypes
		public enum OrderCustomerType : short
		{
			NotSet = 0,
			Normal = 1,
			Hostess = 2,
		}
		#endregion
			

		#region OrderItemParentTypes
		public enum OrderItemParentType : short
		{
			NotSet = 0,
			StaticKit = 1,
			DynamicKit = 2,
		}
		#endregion
			

		#region OrderItemPropertyTypes
		public enum OrderItemPropertyType : int
		{
			NotSet = 0,
			WasBackordered = 1,
		}
		#endregion
			

		#region OrderItemTypes
		public enum OrderItemType : short
		{
			NotSet = 0,
			Retail = 1,
			Fees = 2,
			HostCredit = 3,
			PercentOff = 4,
			ItemDiscount = 5,
			BookingCredit = 6,
			EnrollmentProductCredit = 7,
			ExclusiveProduct = 8,
			FreeItem = 15,
		}
		#endregion
			

		#region OrderPaymentStatuses
		public enum OrderPaymentStatus : short
		{
			NotSet = 0,
			Pending = 1,
			Completed = 2,
			Cancelled = 3,
			Declined = 4,
			ReNegociado = 5,
		}
		#endregion
			

		#region OrderShipmentStatuses
		public enum OrderShipmentStatus : short
		{
			NotSet = 0,
			Pending = 1,
			Cancelled = 2,
			Shipped = 3,
			PartiallyShipped = 4,
			processoDeTransporteJaIniciado = 5,
			EntregaRealizadaNormalmente = 6,
			EntregaForaDaDataProgramada = 7,
			RecusaPorFaltaDePedidoDeCompra = 8,
			RecusaPorPedidoDeCompraCancelado = 9,
			FaltaDeEspacoFisicoNoDepositoDoCliente = 10,
			EnderecoDoClienteDestinoNaoLocalizado = 11,
			DevolucaoNaoAutorizadaPeloCliente = 12,
			PrecoMercadoriaEmDesacordoComPedidoCompra = 13,
			MercadoriaEmDesacordoComOPedidoDeCompra = 14,
			ClienteSoRecebeMercadoriaComFretePago = 15,
			RecusaPorDeficienciaNaEmbalagem = 16,
			RedespachoNaoIndicado = 17,
			transportadoraNaoAtendeACidadeDestino = 18,
			MercadoriaSinistrada = 19,
			EmbalagemSinistrada = 20,
			PedidoDeComprasEmDuplicidade = 21,
			MercadoriaForaDaEmbalagemAtacadista = 22,
			MercadoriasTrocadas = 23,
			ReentregaSolicitadaPeloCliente = 24,
			EstabelecimentoFechado = 26,
			ReentregaSemCobrancaDoCliente = 27,
			extravioDeMercadoriaEmTransito = 28,
			mercadoriaReentregueAoClienteDestino = 29,
			MercadoriaDevolvidaAoClienteOrigem = 30,
			NfSendoAvaliadaPelaSefaz = 31,
			RouboDeCarga = 32,
			MercadoriaRetidaAteSegundaOrdem = 33,
			ClienteRetiraMercadoriaNaTransportadora = 34,
			ProblemaComADocumentacaodnectrc = 35,
			EntregaComIndenizacaoEfetuada = 36,
			FaltaComSolicitacaoDeReposicao = 37,
			FaltaComBuscareconferencia = 38,
			ClienteFechadoParaBalanco = 39,
			QuantidadeDeProdutoEmDesacordo = 40,
			ExtravioDeCargaPelaCiaAereaTrpAereo = 42,
			AvariaDeCargaPelaCiaAereaTrpAereo = 43,
			CorteDeCargaNaPistaTrpAereo = 44,
			AeroportoFechadoNaOrigemTrpAereo = 45,
			PedidoDeCompraIncompleto = 46,
			DneComProdutosDeSetoresDiferentes = 47,
			FeriadoLocalnacional = 48,
			ExcessoDeVeiculos = 49,
			ClienteDestinoEncerrouAtividades = 50,
			ResponsavelPorRecebimentoAusente = 51,
			ClienteDestinoEmGreve = 52,
			AeroportoFechadoNoDestinoTrpAereo = 53,
			VôoCanceladoTrpAereo = 54,
			GreveNacionalgreveGeral = 55,
			MercadoriaVencidadataDeValidadeExpirada = 56,
			MercadoriaNaoFoiEmbarcada = 58,
			TrpNaoAtendeACidadeDaTrpDeRedespacho = 62,
			QuebraDoVeiculoDeEntrega = 63,
			ClienteSemVerbaParaPagarOFrete = 64,
			EnderecoDeEntregaErrado = 65,
			ClienteSemVerbaParaReembolso = 66,
			RecusaDaCargaPorValorDeFreteErrado = 67,
			IdDoClienteNaoInformadenviadainsuficiente = 68,
			ClienteNaoIdentificadocadastrado = 69,
			EntrarEmContatoComOComprador = 70,
			TrocaNaoDisponivel = 71,
			FinsEstatisticos = 72,
			DataDeEntregaDiferenteDoPedido = 73,
			SubstituicaoTributaria = 74,
			SistemaForaDoAr = 75,
			ClienteDestinoNaoRecebePedidoParcial = 76,
			ClienteDestinoSoRecebePedidoParcial = 77,
			RedespachoSomenteComFretePago = 78,
			FuncionarioNaoAutorizadoAReceberMercadorias = 79,
			MercadoriaEmbarcadaParaRotaIndevida = 80,
			EstradaentradaDeAcessoInterditado = 81,
			ClienteDestinoMudouDeEndereco = 82,
			AvariaTotal = 83,
			AvariaParcial = 84,
			ExtravioTotal = 85,
			ExtravioParcial = 86,
			SobraDeMercadoriaSemDne = 87,
			MercadoriaEmPoderDaSuframaParaInternacao = 88,
			MercadoriaRetiradaParaConferencia = 89,
			ApreensaoFiscalDaMercadoria = 90,
			ExcessoDeCargapeso = 91,
			FeriasColetivas = 92,
			RECUSADO = 93,
			AguardandoRefaturamentoDasMercadorias = 94,
			RecusadoPeloRedespachante = 95,
			EntregaProgramada = 96,
			ProblemasFiscais = 97,
			AguardandoCartaDeCorrecao = 98,
			RecusaPorDivergenciaNasCondicoesDePgto = 99,
			CartaAguardandoVôoConexaoTrpAereo = 100,
			CargaSemEmbalagemPropriaTrpAereo = 101,
			DevoluçãoCobrada = 103,
			OutrosTiposDeOcorrenciasNaoEspecificados = 104,
		}
		#endregion
			

		#region OrderStatuses
		public enum OrderStatus : short
		{
			NotSet = 0,
			Pending = 1,
			PendingError = 2,
			Paid = 4,
			Cancelled = 5,
			PartiallyPaid = 6,
			Printed = 8,
			Shipped = 9,
			CreditCardDeclined = 11,
			CreditCardDeclinedRetry = 12,
			PartiallyShipped = 13,
			CancelledPaid = 14,
			DeferredOnlinePayment = 15,
			Suspended = 16,
			PartyOrderPending = 17,
			PendingPerPaidConfirmation = 18,
			PendingConfirm = 19,
			Invoiced = 20,
			Delivered = 21,
			Embarked = 22,
			BillingProcessing = 23,
		}
		#endregion
			

		#region OrderTypes
		public enum OrderType : short
		{
			NotSet = 0,
			OnlineOrder = 1,
			WorkstationOrder = 2,
			PartyOrder = 3,
			PortalOrder = 4,
			AutoshipTemplate = 5,
			AutoshipOrder = 6,
			OverrideOrder = 7,
			ReturnOrder = 8,
			CompOrder = 9,
			ReplacementOrder = 10,
			EnrollmentOrder = 11,
			EmployeeOrder = 12,
			BusinessMaterialsOrder = 13,
			HostessRewardsOrder = 14,
			FundraiserOrder = 15,
			OnlinePartyOrder = 16,
			Employee = 17,
			ClaimOrder = 18,
		}
		#endregion
			

		#region PageTypes
		public enum PageType : short
		{
			NotSet = 0,
			User = 1,
			Home = 2,
			Shop = 3,
			Contact = 4,
			Enrollment = 5,
			Hostess = 6,
			External = 7,
		}
		#endregion
			

		#region PaymentTypes
		public enum PaymentType : int
		{
			NotSet = 0,
			CreditCard = 1,
			Check = 2,
			Cash = 3,
			EFT = 4,
			GiftCard = 5,
			ProductCredit = 6,
			EnrollmentCredit = 7,
			InstantCommission = 8,
			TestPayment = 10,
			PaymentTicket = 11,
		}
		#endregion
			

		#region PhoneTypes
		public enum PhoneType : int
		{
			NotSet = 0,
			Main = 1,
			Cell = 2,
			Fax = 3,
			Work = 4,
			Text = 5,
			Home = 6,
			Other = 7,
			Pager = 8,
		}
		#endregion
			

		#region PriceRelationshipTypes
		public enum PriceRelationshipType : int
		{
			NotSet = 0,
			Products = 1,
			Taxes = 2,
			Commissions = 3,
		}
		#endregion
			

		#region ProductBackOrderBehaviors
		public enum ProductBackOrderBehavior : short
		{
			NotSet = 0,
			Hide = 1,
			ShowOutOfStockMessage = 2,
			AllowBackorderInformCustomer = 3,
			AllowBackorder = 4,
		}
		#endregion
			

		#region ProductFileTypes
		public enum ProductFileType : int
		{
			NotSet = 0,
			Image = 1,
		}
		#endregion
			

		#region ProductPriceTypes
		public enum ProductPriceType : int
		{
			NotSet = 0,
			Retail = 1,
			PreferredCustomer = 2,
			ShippingFee = 10,
			HandlingFee = 11,
			CV = 18,
			HostBase = 20,
			QV = 21,
			Wholesale = 22,
		}
		#endregion
			

		#region ProductPropertyTypes
		public enum ProductPropertyType : int
		{
			NotSet = 0,
		}
		#endregion
			

		#region ProductRelationsTypes
		public enum ProductRelationsType : int
		{
			NotSet = 0,
			RelatedItem = 1,
			Kit = 2,
		}
		#endregion
			

		#region ProductTypes
		public enum ProductType : int
		{
			NotSet = 0,
			Fees = 38,
			GiftCard = 100,
		}
		#endregion
							

		#region QueueItemPriorities
		public enum QueueItemPriority : short
		{
			NotSet = 0,
			Lowest = 1,
			Low = 2,
			Normal = 3,
			High = 4,
			Highest = 5,
		}
		#endregion
			

		#region QueueItemStatuses
		public enum QueueItemStatus : short
		{
			NotSet = 0,
			Queued = 1,
			Running = 2,
			Failed = 3,
			Completed = 4,
		}
		#endregion
			

		#region ReturnTypes
		public enum ReturnType : int
		{
			NotSet = 0,
		}
		#endregion
			

		#region Roles
		public enum Role : int
		{
			NotSet = 0,
			LimitedUser = 1,
			WorkstationUser = 32,
			Anonymous = 33,
			LogisticsManager = 44,
		}
		#endregion
			

		#region RoleTypes
		public enum RoleType : short
		{
			NotSet = 0,
			NscoreRole = 1,
			DistributorRole = 2,
			CorporateUserRole = 3,
			FieldUserRole = 4,
		}
		#endregion
			

		#region ShippingRateTypes
		public enum ShippingRateType : short
		{
			NotSet = 0,
			TotalOrderCost = 1,
			TotalShippmentWeight = 2,
		}
		#endregion
			

		#region SiteStatusChangeReasons
		public enum SiteStatusChangeReason : short
		{
			NotSet = 0,
		}
		#endregion
			

		#region SiteStatuses
		public enum SiteStatus : short
		{
			NotSet = 0,
			Active = 1,
			InActive = 2,
			PendingApproval = 3,
			OnHold = 4,
			DisabledForNonPayment = 5,
		}
		#endregion
			

		#region SiteTypes
		public enum SiteType : short
		{
			NotSet = 0,
			Corporate = 1,
			Replicated = 3,
			BackOffice = 6,
			GlobalManagementPortal = 7,
		}
		#endregion
			

		#region StatisticTypes
		public enum StatisticType : short
		{
			NotSet = 0,
			Impression = 1,
			Redeem = 2,
			Search = 3,
		}
		#endregion
			

		#region StatisticValueTypes
		public enum StatisticValueType : short
		{
			NotSet = 0,
			BrandIdentifier = 1,
			OfferIdentifier = 2,
			OfferDataIdentifier = 3,
			SearchDescription = 4,
			LocationIdentifier = 5,
			MemberIdentifier = 6,
		}
		#endregion
			

		#region SupportTicketPriorities
		public enum SupportTicketPriority : short
		{
			NotSet = 0,
			Urgent = 1,
			High = 2,
			Medium = 4,
			InfoRequest = 8,
			Low = 9,
		}
		#endregion
			

		#region SupportTicketStatuses
		public enum SupportTicketStatus : short
		{
			NotSet = 0,
			Entered = 1,
			Assigned = 2,
			Escalated = 3,
			Resolved = 4,
			NeedsInput = 6,
		}
		#endregion
			

		#region TaxCategories
		public enum TaxCategory : int
		{
			NotSet = 0,
			NotTaxable = 1,
			Product = 2,
			VIT = 3,
			SRV = 4,
		}
		#endregion
			

		#region TaxDataSources
		public enum TaxDataSource : short
		{
			NotSet = 0,
			Simpova = 1,
			ZipSales = 2,
		}
		#endregion
				

		#region TimeUnitTypes
		public enum TimeUnitType : short
		{
			NotSet = 0,
			Seconds = 1,
			Minutes = 2,
			Hours = 3,
			Days = 4,
			Weeks = 5,
			Months = 6,
			Years = 7,
		}
		#endregion
			

		#region Tokens
		public enum Token : int
		{
			NotSet = 0,
			OrderNumber = 1,
			OrderTotal = 2,
			OrderDatePlaced = 3,
			DistributorFullName = 4,
			DistributorImage = 5,
			DistributorContent = 6,
			RecipientFirstName = 7,
			RecipientLastName = 8,
			RecipientFullName = 9,
			SponsorFirstName = 10,
			SponsorLastName = 11,
			SponsorFullName = 12,
			SponsorPhone = 13,
			SponsorEmail = 14,
			CustomerName = 15,
			CustomerSubtotal = 16,
			CustomerTotal = 17,
			CustomerTaxTotal = 18,
			CustomerShippingTotal = 19,
			CustomerHandlingTotal = 20,
			CustomerShippingAndHandlingTotal = 21,
			GuestId = 22,
			GuestName = 23,
			GuestEmail = 24,
			HostName = 25,
			HostSso = 26,
			PartyId = 27,
			PartyName = 28,
			PartyDate = 29,
			PartyAddress = 30,
			DistributorName = 31,
			DistributorPwsUrl = 32,
			SupportTicketUrl = 33,
			DistributorWorkstationUrl = 36,
			OrderPaymentCreditCardLastFour = 38,
			OrderDateShipped = 39,
			NewConsultantAccountNumber = 40,
			NewConsultantFirstName = 41,
			NewConsultantLastName = 42,
			OrderShippingAddress = 43,
			OrderBillingAddress = 44,
			OrderShippingMethod = 45,
			OrderItems = 46,
			OrderTotals = 47,
			NewDistributorFirstName = 48,
			NewDistributorLastName = 49,
			NewDistributorFullName = 50,
			NewDistributorAccountNumber = 51,
			NewDistirbutorInitialOrderNumber = 52,
			NewDistributorAutoshipOrderNumber = 53,
			ContactMeName = 54,
			ContactMeEmail = 55,
			ContactMePhone = 56,
			ContactMeMessage = 57,
			ReferAFriendUrl = 58,
			ReferrerId = 59,
			ReferralAccountTypeId = 60,
			BusinessName = 61,
			ContactName = 62,
			MerchantAddress = 63,
			ContactPhone = 64,
			BusinessCategory = 65,
			MerchantReferrerId = 66,
			MerchantReferrerName = 67,
			CustomBody = 68,
			AutoshipRunDate = 69,
			AccountNumber = 70,
			AutoshipOrderUrl = 71,
			CustomerFirstName = 72,
			CustomerLastName = 73,
			ConsultantFirstName = 74,
			ForgotPasswordUrl = 75,
			ForgotPasswordAccountName = 76,
			NewDistributorEnrollmentDate = 77,
			NewDistributorQuickstartDate = 78,
			TitlePromoted = 79,
			SponsorPwsUrl = 80,
			PromotionName = 81,
			PromotionStartDate = 82,
			PromotionEndDate = 83,
			AccountFirstname = 84,
			AccountLastname = 85,
			AccountFullname = 86,
			CustomerAddress = 87,
			OrderTrackingNumber = 88,
			UserName = 90,
			UserNotificationBeautyAdvisor = 91,
			UserNotificationOrderNumber = 92,
			UserNotificationSubTotal = 93,
			UserNotificationTax = 94,
			UserNotificationShippingAndHandling = 95,
			UserNotificationTotal = 96,
			OrderStatus = 97,
			NewConsultantPhone = 98,
		}
		#endregion
			

		#region UserStatuses
		public enum UserStatus : short
		{
			NotSet = 0,
			Active = 1,
			Inactive = 2,
			LockedOut = 3,
		}
		#endregion
			

		#region UserTypes
		public enum UserType : short
		{
			NotSet = 0,
			Corporate = 1,
			Distributor = 2,
			Customer = 3,
		}
		#endregion
		}
}

