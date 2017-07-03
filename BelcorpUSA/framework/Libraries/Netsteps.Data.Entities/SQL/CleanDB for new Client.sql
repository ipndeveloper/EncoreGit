dba_SpaceUsed


BEGIN TRANSACTION

exec dba_SmartTruncateTable AutoResponses
exec dba_SmartTruncateTable LogItems

exec dba_SmartTruncateTable ForecastValues
exec dba_SmartTruncateTable Forecasts
exec dba_SmartTruncateTable ForecastColumns
exec dba_SmartTruncateTable ForecastTemplates

exec dba_SmartTruncateTable PromotionProducts
exec dba_SmartTruncateTable PromotionUsage
exec dba_SmartTruncateTable PromotionAccounts
exec dba_SmartTruncateTable PromotionMarkets
exec dba_SmartTruncateTable Promotions

-- TODO: Finish Clearing out Sites Info - JHE
exec dba_SmartTruncateTable ArchiveCategories
exec dba_SmartTruncateTable ArchiveTranslations
exec dba_SmartTruncateTable Archives
exec dba_SmartTruncateTable SiteArchives

exec dba_SmartTruncateTable CalendarEventAttributes
exec dba_SmartTruncateTable CalendarEvents
exec dba_SmartTruncateTable SiteCalendarEvents

exec dba_SmartTruncateTable ProxyLinks

exec dba_SmartTruncateTable SiteLanguages

exec dba_SmartTruncateTable SiteSettingValues
exec dba_SmartTruncateTable SiteSettings

exec dba_SmartTruncateTable SiteStatusChangeReasons

exec dba_SmartTruncateTable SiteNews
exec dba_SmartTruncateTable News

exec dba_SmartTruncateTable SiteUrls

exec dba_SmartTruncateTable NavigationTranslations
exec dba_SmartTruncateTable Navigations

exec dba_SmartTruncateTable PageHtmlSections
exec dba_SmartTruncateTable PageTranslations
exec dba_SmartTruncateTable Pages


exec dba_SmartTruncateTable SiteHtmlSections

-- Contacts Info
exec dba_SmartTruncateTable ContactPhones
exec dba_SmartTruncateTable ContactAddresses
exec dba_SmartTruncateTable Contacts

exec dba_SmartTruncateTable AutoshipLogs
exec dba_SmartTruncateTable AutoshipBatches
exec dba_SmartTruncateTable AutoshipScheduleProducts

exec dba_SmartTruncateTable OrderNotes
exec dba_SmartTruncateTable OrderLogs
exec dba_SmartTruncateTable OrderBookings
exec dba_SmartTruncateTable OrderItemReturns
exec dba_SmartTruncateTable OrderOverrides

exec dba_SmartTruncateTable OrderPaymentResults
exec dba_SmartTruncateTable OrderPayments

exec dba_SmartTruncateTable OrderItems

exec dba_SmartTruncateTable OrderShipmentsLogs
exec dba_SmartTruncateTable OrderShipmentItems
exec dba_SmartTruncateTable OrderShipments
exec dba_SmartTruncateTable OrderCustomers

exec dba_SmartTruncateTable PointOfSaleEventOrders
exec dba_SmartTruncateTable PointOfSaleEvents

-- TODO: Finish Clearing out Accounts Info - JHE
exec dba_SmartTruncateTable AccountNotes
exec dba_SmartTruncateTable AccountPolicies
exec dba_SmartTruncateTable EmailSignatures
exec dba_SmartTruncateTable AccountLanguages
exec dba_SmartTruncateTable AccountAddresses
exec dba_SmartTruncateTable AccountEmailLogs
exec dba_SmartTruncateTable AccountLogs
exec dba_SmartTruncateTable AccountEmailLogs
exec dba_SmartTruncateTable ApplicationAccountSettings
exec dba_SmartTruncateTable ApplicationRunningInstances
exec dba_SmartTruncateTable ApplicationSettings

exec dba_SmartTruncateTable AccountPaymentMethods
exec dba_SmartTruncateTable AccountPhones
exec dba_SmartTruncateTable AccountProperties
exec dba_SmartTruncateTable AccountPropertyTypes
exec dba_SmartTruncateTable AccountPropertyValues
exec dba_SmartTruncateTable AccountPublicContactInfo

exec dba_SmartTruncateTable AccountSponsors
exec dba_SmartTruncateTable AccountSponsorTypes

exec dba_SmartTruncateTable Notes
exec dba_SmartTruncateTable Policies

-- Finish Clearing out Hostess Info - JHE
exec dba_SmartTruncateTable HostessRewardCatalogs
exec dba_SmartTruncateTable HostessRewardRules
exec dba_SmartTruncateTable HostessRewardsRedeemable
exec dba_SmartTruncateTable HostessRewardCatalogs
--exec dba_SmartTruncateTable HostessRewardTypes

-- CampaignAlerts Tables
exec dba_SmartTruncateTable CampaignTrackerLogs
exec dba_SmartTruncateTable CampaignEmailLogs
exec dba_SmartTruncateTable CampaignAlertLogs
exec dba_SmartTruncateTable CampaignAlerts
exec dba_SmartTruncateTable CampaignEmails
exec dba_SmartTruncateTable Campaigns

-- Mail Tables
exec dba_SmartTruncateTable OptOut
exec dba_SmartTruncateTable OptOutTypes
exec dba_SmartTruncateTable MailAccounts
exec dba_SmartTruncateTable MailDomains

exec dba_SmartTruncateTable EmailTemplateAttributeConnections
exec dba_SmartTruncateTable EmailTemplateAttributes
exec dba_SmartTruncateTable EmailTemplates
exec dba_SmartTruncateTable MailDomains

-- DistributionList Tables
exec dba_SmartTruncateTable DistributionSubscribers
exec dba_SmartTruncateTable DistributionSubscriberAttributes
exec dba_SmartTruncateTable DistributionListAttributes
exec dba_SmartTruncateTable DistributionListHandlers
exec dba_SmartTruncateTable DistributionLists

-- Alert Tables
exec dba_SmartTruncateTable AlertTemplates
exec dba_SmartTruncateTable AlertTemplateAttributes
exec dba_SmartTruncateTable AlertTemplateAttributeConnections
exec dba_SmartTruncateTable AlertTriggerAttributes
exec dba_SmartTruncateTable AlertTriggerDistributionLists
exec dba_SmartTruncateTable AlertTriggers
exec dba_SmartTruncateTable AlertTriggerTypes

-- Wizards Tables
exec dba_SmartTruncateTable AccountWizards
exec dba_SmartTruncateTable WizardStepsComplete
exec dba_SmartTruncateTable WizardSteps
exec dba_SmartTruncateTable Wizards

--exec dba_SmartTruncateTable Applications
-- TODO: Recreate data - JHE

-- Processor Tables
exec dba_SmartTruncateTable ProcessorAttributes
exec dba_SmartTruncateTable ProcessorLogs
exec dba_SmartTruncateTable Processors
exec dba_SmartTruncateTable ProcessorSchedules

-- Saved Searches Tables
exec dba_SmartTruncateTable SearchParameters
exec dba_SmartTruncateTable SavedSearches

exec dba_SmartTruncateTable TaxCategories
exec dba_SmartTruncateTable TerminationReasons

exec dba_SmartTruncateTable Testimonials

exec dba_SmartTruncateTable CatalogItems
exec dba_SmartTruncateTable CatalogTranslations
exec dba_SmartTruncateTable CategoryTranslations

exec dba_SmartTruncateTable StoreFrontCatalogs

exec dba_SmartTruncateTable MarketStoreFronts
exec dba_SmartTruncateTable AccountPriceTypes
exec dba_SmartTruncateTable StoreFronts

exec dba_SmartTruncateTable HtmlSectionChoices
exec dba_SmartTruncateTable HtmlContentHistory
exec dba_SmartTruncateTable HtmlElements
exec dba_SmartTruncateTable HtmlContentWorkflow
exec dba_SmartTruncateTable HtmlSectionContent
exec dba_SmartTruncateTable HtmlContent
exec dba_SmartTruncateTable HtmlSections

UPDATE Sites SET AutoshipOrderID = null 
UPDATE Orders SET SiteID = null 
exec dba_SmartTruncateTable Sites
exec dba_SmartTruncateTable AutoshipOrders
exec dba_SmartTruncateTable Orders

exec dba_SmartTruncateTable AutoshipScheduleAccountTypes
exec dba_SmartTruncateTable AutoshipScheduleDays
exec dba_SmartTruncateTable AutoshipSchedules

exec dba_SmartTruncateTable Accounts

-- TODO: Finish Clearing out Products Info - JHE
exec dba_SmartTruncateTable ProductBaseCategories
exec dba_SmartTruncateTable Catalogs
exec dba_SmartTruncateTable Categories

exec dba_SmartTruncateTable WarehouseProducts
exec dba_SmartTruncateTable MarketProducts

exec dba_SmartTruncateTable ProductBaseTestimonials
exec dba_SmartTruncateTable ProductBaseTranslations
exec dba_SmartTruncateTable ProductFiles
exec dba_SmartTruncateTable ProductPrices
exec dba_SmartTruncateTable ProductProperties
exec dba_SmartTruncateTable ProductPropertyValues
exec dba_SmartTruncateTable ProductRelations
exec dba_SmartTruncateTable ProductPropertyTypeRelations
exec dba_SmartTruncateTable ProductTranslations
exec dba_SmartTruncateTable Products
exec dba_SmartTruncateTable ProductTaxCategories
exec dba_SmartTruncateTable ProductTypeProperties
exec dba_SmartTruncateTable ProductBases

exec dba_SmartTruncateTable ProductTypes
exec dba_SmartTruncateTable ProductFileTypes
exec dba_SmartTruncateTable ProductPropertyTypes
exec dba_SmartTruncateTable ProductPriceTypes
exec dba_SmartTruncateTable ProductRelationsTypes

UPDATE Warehouses SET AddressID = null 
exec dba_SmartTruncateTable Addresses

exec dba_SmartTruncateTable DescriptionTranslations

exec dba_SmartTruncateTable NavigationTypes
exec dba_SmartTruncateTable OrderStatuses
--exec dba_SmartTruncateTable OrderTypes
exec dba_SmartTruncateTable SiteStatuses
exec dba_SmartTruncateTable SiteTypes
exec dba_SmartTruncateTable ArchiveTypes
exec dba_SmartTruncateTable NewsTypes
exec dba_SmartTruncateTable OrderItemTypes
exec dba_SmartTruncateTable OrderPaymentStatuses
exec dba_SmartTruncateTable OrderShipmentStatuses
exec dba_SmartTruncateTable NoteTypes
exec dba_SmartTruncateTable PhoneTypes
exec dba_SmartTruncateTable PaymentOrderTypes
exec dba_SmartTruncateTable PaymentTypes
exec dba_SmartTruncateTable PriceRelationshipTypes
exec dba_SmartTruncateTable Priorities
exec dba_SmartTruncateTable ReturnReasons
exec dba_SmartTruncateTable ReturnTypes
--exec dba_SmartTruncateTable RoleTypes
--exec dba_SmartTruncateTable ShippingMethods
exec dba_SmartTruncateTable AddressTypes
exec dba_SmartTruncateTable HtmlContentStatuses
exec dba_SmartTruncateTable HtmlSectionEditTypes
exec dba_SmartTruncateTable HtmlContentWorkflowTypes
exec dba_SmartTruncateTable PromotionTypes
exec dba_SmartTruncateTable AccountStatuses
exec dba_SmartTruncateTable AccountTypes
exec dba_SmartTruncateTable CampaignTypes
--exec dba_SmartTruncateTable Warehouses

INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Pending', 'Order has been saved, but no further processing has taken place.', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Pending Error', '', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Pending Cancelled', '', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Submitted', 'Payment on the order has been process successfully.', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Cancelled', '', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Partially Submitted', '1 or more of payments on the order containing multiple OrderCustomer processed successfully, but 1 ore more payments failed.', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Printed', '', 1)
INSERT INTO OrderStatuses (Name, Description, Active) VALUES ('Shipped', '', 1)

/*
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Retail', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('PC', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Distributor', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('PC Auto-ship Template', 1, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Distributor Auto-ship Template', 1, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('PC Auto-ship', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Distributor Auto-ship', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Override', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Return', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('POS', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Comp Order', 0, 1)
INSERT INTO OrderTypes (Name, IsTemplate, Active) VALUES ('Replacement Order', 0, 1)

INSERT INTO RoleTypes (Name, Active) VALUES ('nsCore Role', 1)
INSERT INTO RoleTypes (Name, Active) VALUES ('Distributor Role', 1)

INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('FedEx 1 day', 'FED1', 1, 0, 1)
INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('FedEx 2 day', 'FED2', 1, 0, 2)
INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('FedEx Ground', 'FEDG', 1, 0, 3)
INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('USPS 1 day', '1STP', 1, 0, 4)
INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('USPS 2 day', '2STP', 1, 0, 5)
INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('USPS Ground', 'GSTP', 1, 0, 6)
INSERT INTO ShippingMethods (Name, ShortName, Active, IsWillCall, Sort) VALUES ('Point of Sale', 'POS', 1, 0, 7)

INSERT INTO Warehouses (Name, Active) VALUES ('Salt Lake City', 1)
*/

INSERT INTO SiteStatuses (Name, Active) VALUES ('Active', 1)
INSERT INTO SiteStatuses (Name, Active) VALUES ('InActive', 1)
INSERT INTO SiteStatuses (Name, Active) VALUES ('Pending Approval', 1)
INSERT INTO SiteStatuses (Name, Active) VALUES ('On Hold', 1)
INSERT INTO SiteStatuses (Name, Active) VALUES ('Disabled for Non Payment', 1)

INSERT INTO ArchiveTypes (Name, Active) VALUES ('PDF', 1)
INSERT INTO ArchiveTypes (Name, Active) VALUES ('Image', 1)

INSERT INTO NewsTypes (Name, Active) VALUES ('Announcement', 1)
INSERT INTO NewsTypes (Name, Active) VALUES ('Press Release', 1)
INSERT INTO NewsTypes (Name, Active) VALUES ('Corporate News', 1)
INSERT INTO NewsTypes (Name, Active) VALUES ('Product News', 1)
INSERT INTO NewsTypes (Name, Active) VALUES ('Industry News', 1)

INSERT INTO OrderItemTypes (Name, Active) VALUES ('Retail', 1)
INSERT INTO OrderItemTypes (Name, Active) VALUES ('Fees', 1)

INSERT INTO OrderPaymentStatuses (Name, Active) VALUES ('Pending', 1)
INSERT INTO OrderPaymentStatuses (Name, Active) VALUES ('Completed', 1)
INSERT INTO OrderPaymentStatuses (Name, Active) VALUES ('Cancelled', 1)

INSERT INTO OrderShipmentStatuses (Name, Active) VALUES ('Pending', 1)
INSERT INTO OrderShipmentStatuses (Name, Active) VALUES ('Cancelled', 1)
INSERT INTO OrderShipmentStatuses (Name, Active) VALUES ('Shipped', 1)

INSERT INTO NoteTypes (Name, Active) VALUES ('AccountNotes', 1)
INSERT INTO NoteTypes (Name, Active) VALUES ('Distributor BackOffice Notes', 1)
INSERT INTO NoteTypes (Name, Active) VALUES ('Order Invoice Notes', 1)
INSERT INTO NoteTypes (Name, Active) VALUES ('Order Internal Notes', 1)

INSERT INTO PhoneTypes (Name, Active) VALUES ('Main', 1)
INSERT INTO PhoneTypes (Name, Active) VALUES ('Cell', 1)
INSERT INTO PhoneTypes (Name, Active) VALUES ('Fax', 1)
INSERT INTO PhoneTypes (Name, Active) VALUES ('Work', 1)
INSERT INTO PhoneTypes (Name, Active) VALUES ('Text', 1)
INSERT INTO PhoneTypes (Name, Active) VALUES ('Home', 1)
INSERT INTO PhoneTypes (Name, Active) VALUES ('Other', 1)

INSERT INTO PaymentTypes (Name, PaymentTypeCode, IsCreditCard, IsCash, IsCheck, IsEFT, IsGiftCard, Active) VALUES ('Credit Card', 'CreditCard', 1, 0, 0, 0, 0, 1)
INSERT INTO PaymentTypes (Name, PaymentTypeCode, IsCreditCard, IsCash, IsCheck, IsEFT, IsGiftCard, Active) VALUES ('Check', 'Check', 0, 0, 1, 0, 0, 1)
INSERT INTO PaymentTypes (Name, PaymentTypeCode, IsCreditCard, IsCash, IsCheck, IsEFT, IsGiftCard, Active) VALUES ('Cash', 'Cash', 0, 1, 0, 0, 0, 1)
INSERT INTO PaymentTypes (Name, PaymentTypeCode, IsCreditCard, IsCash, IsCheck, IsEFT, IsGiftCard, Active) VALUES ('EFT', 'EFT', 0, 0, 0, 1, 0, 1)
INSERT INTO PaymentTypes (Name, PaymentTypeCode, IsCreditCard, IsCash, IsCheck, IsEFT, IsGiftCard, Active) VALUES ('Gift Card', 'GiftCard', 0, 0, 0, 0, 1, 1)

INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (1, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (2, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (3, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (4, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (5, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (6, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (7, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (8, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (9, 1, 1, 1)
INSERT INTO PaymentOrderTypes (OrderTypeID, PaymentTypeID, CountryID, PaymentGatewayID) VALUES (10, 1, 1, 1)

INSERT INTO PriceRelationshipTypes (Name, Active) VALUES ('Products', 1)
INSERT INTO PriceRelationshipTypes (Name, Active) VALUES ('Taxes', 1)
INSERT INTO PriceRelationshipTypes (Name, Active) VALUES ('Commissions', 1)

INSERT INTO Priorities (Name, Active) VALUES ('Lowest', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Low', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Normal', 1)
INSERT INTO Priorities (Name, Active) VALUES ('High', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Highest', 1)

INSERT INTO ReturnReasons (Name, Active) VALUES ('Inventory Return / Terminating Account', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Unsatisfactory Results', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Damaged In Shipment', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Arrived To Late', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Order Error (Consultant/Customer)', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Wrong Merchandise Ordered', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Wrong Merchandise Sent', 1)
INSERT INTO ReturnReasons (Name, Active) VALUES ('Adverse Reaction', 1)

INSERT INTO ReturnTypes (Name, Active) VALUES ('Customer Return', 1)
INSERT INTO ReturnTypes (Name, Active) VALUES ('Call Tag', 1)
INSERT INTO ReturnTypes (Name, Active) VALUES ('Refused', 1)

INSERT INTO Priorities (Name, Active) VALUES ('Main', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Shipping', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Billing', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Autoship', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Disbursement', 1)
INSERT INTO Priorities (Name, Active) VALUES ('Warehouse Address', 1)

INSERT INTO HtmlContentStatuses (Name, Active) VALUES ('Preview', 1)
INSERT INTO HtmlContentStatuses (Name, Active) VALUES ('Draft', 1)
INSERT INTO HtmlContentStatuses (Name, Active) VALUES ('Submitted', 1)
INSERT INTO HtmlContentStatuses (Name, Active) VALUES ('Disapproved', 1)
INSERT INTO HtmlContentStatuses (Name, Active) VALUES ('Production', 1)
INSERT INTO HtmlContentStatuses (Name, Active) VALUES ('Archive', 1)

INSERT INTO HtmlSectionEditTypes (Name, Active) VALUES ('Corporate Only', 1)
INSERT INTO HtmlSectionEditTypes (Name, Active) VALUES ('Choices', 1)
INSERT INTO HtmlSectionEditTypes (Name, Active) VALUES ('Override', 1)
INSERT INTO HtmlSectionEditTypes (Name, Active) VALUES ('Consultant List', 1)
INSERT INTO HtmlSectionEditTypes (Name, Active) VALUES ('ChoicesPlus', 1)

INSERT INTO HtmlContentWorkflowTypes (Name, Active) VALUES ('Created', 1)
INSERT INTO HtmlContentWorkflowTypes (Name, Active) VALUES ('Edited', 1)
INSERT INTO HtmlContentWorkflowTypes (Name, Active) VALUES ('Scheduled', 1)
INSERT INTO HtmlContentWorkflowTypes (Name, Active) VALUES ('Archived', 1)

INSERT INTO CampaignTypes (Name, Active) VALUES ('Percent Off', 1)
INSERT INTO CampaignTypes (Name, Active) VALUES ('Flat Discount', 1)

INSERT INTO AccountStatuses (Name, Active) VALUES ('Active', 1)
INSERT INTO AccountStatuses (Name, Active) VALUES ('Terminated', 1)
INSERT INTO AccountStatuses (Name, Active) VALUES ('Begun Enrollment', 1)

INSERT INTO AccountTypes (Name, Active) VALUES ('Distributor', 1)
INSERT INTO AccountTypes (Name, Active) VALUES ('Preferred Customer', 1)
INSERT INTO AccountTypes (Name, Active) VALUES ('Retail Customer', 1)
INSERT INTO AccountTypes (Name, Active) VALUES ('Employee', 1)

INSERT INTO CampaignTypes (Name, Active) VALUES ('Mass Emails', 1)
INSERT INTO CampaignTypes (Name, Active) VALUES ('Campaigns', 1)
INSERT INTO CampaignTypes (Name, Active) VALUES ('Event Based Emails', 1)
INSERT INTO CampaignTypes (Name, Active) VALUES ('Mass Alerts', 1)

INSERT INTO ProductTypes (Name, Active) VALUES ('Default', 1)
INSERT INTO ProductTypes (Name, Active) VALUES ('Kits', 1)

INSERT INTO ProductFileTypes (Name, Active) VALUES ('Large Image', 1)
INSERT INTO ProductFileTypes (Name, Active) VALUES ('Small Image', 1)

INSERT INTO ProductPropertyTypes (Name, DataType, Required) VALUES ('Ingredients', 'Text', 0)
INSERT INTO ProductPropertyTypes (Name, DataType, Required) VALUES ('Usage', 'System.String', 0)
INSERT INTO ProductPropertyTypes (Name, DataType, Required) VALUES ('Discontinued', 'Bit', 0)
INSERT INTO ProductPropertyTypes (Name, DataType, Required) VALUES ('Discontinued Message', 'System.String', 1)

INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Retail', 1)
INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Preferred Customer', 1)
INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Consultant', 1)
INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Commissionable', 1)
INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Tax Base', 1)
INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Cost at Standard', 1)
INSERT INTO ProductPriceTypes (Name, Active) VALUES ('Volume', 1)

INSERT INTO ProductRelationsTypes (Name, Active) VALUES ('Adjunct', 1)
INSERT INTO ProductRelationsTypes (Name, Active) VALUES ('Refill', 1)
INSERT INTO ProductRelationsTypes (Name, Active) VALUES ('Kit', 1)

INSERT INTO StoreFronts (Name, TermName, Active) VALUES ('nsCore', 'nsCore', 1)

INSERT INTO MarketStoreFronts (MarketID, StoreFrontID) VALUES (1, 1)
INSERT INTO MarketStoreFronts (MarketID, StoreFrontID) VALUES (2, 1)

-- TODO: Wipe and re-create AccountPriceTypes before StoreFronts
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (5, 1, 2, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (3, 1, 1, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (3, 1, 3, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (2, 2, 1, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (5, 2, 2, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (2, 2, 3, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (1, 3, 1, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (5, 3, 2, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (1, 3, 3, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (6, 4, 1, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (5, 4, 2, 1)
INSERT INTO AccountPriceTypes (ProductPriceTypeID, AccountTypeID, PriceRelationshipTypeID, StoreFrontID) VALUES (6, 4, 3, 1)

INSERT INTO SiteTypes (Name, Active) VALUES ('Corporate', 1)
INSERT INTO SiteTypes (Name, Active) VALUES ('Distributor BackOffice', 1)
INSERT INTO SiteTypes (Name, Active) VALUES ('Replicated', 1)

INSERT INTO NavigationTypes (SiteTypeID, Name, Active) VALUES (1, 'Main Corporate Navigation', 1)

INSERT INTO Categories (CategoryTypeID, SortIndex) VALUES (1, 1)
INSERT INTO Catalogs (CategoryID, Active) VALUES (1, 1)

INSERT INTO AutoshipSchedules (AutoshipScheduleTypeID, OrderTypeID, CatalogID, Name, IntervalTypeID, Active) VALUES (1, 5, 1, 'Consultants Monthly Replenishment', 1, 1)
INSERT INTO AutoshipSchedules (AutoshipScheduleTypeID, OrderTypeID, CatalogID, Name, IntervalTypeID, Active) VALUES (1, 4, 1, 'PC Bi-Monthly Replenishment', 2, 1)
INSERT INTO AutoshipSchedules (AutoshipScheduleTypeID, OrderTypeID, CatalogID, Name, IntervalTypeID, Active) VALUES (3, 5, 1, 'PWS Monthly Subscription', 1, 1)

INSERT INTO AutoshipScheduleAccountTypes (AutoshipScheduleID, AccountTypeID) VALUES (1, 1) -- TODO: Change this to look up the ID by Name first for accurate INSERTS - JHE
INSERT INTO AutoshipScheduleAccountTypes (AutoshipScheduleID, AccountTypeID) VALUES (2, 2)
INSERT INTO AutoshipScheduleAccountTypes (AutoshipScheduleID, AccountTypeID) VALUES (3, 1)

INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (1, 5)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (1, 10)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (1, 15)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (1, 20)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (1, 25)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (1, 30)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (2, 15)
INSERT INTO AutoshipScheduleDays (AutoshipScheduleID, Day) VALUES (3, 5)




-- TODO: Wipe out users and CorpUsers & recreate Admin

UPDATE AccountListValues SET ModifiedByUserID = null 
exec dba_SmartTruncateTable CorporateUsersSites
exec dba_SmartTruncateTable UserFunctionOverrides
exec dba_SmartTruncateTable UserRoles

--INSERT INTO Users (Username, UserTypeID, UserStatusID, PasswordOLD) VALUES ('Admin', 1)
--INSERT INTO CorporateUsers (UserID, FirstName, LastName, HasAccessToAllSites) VALUES (@@IDENTITY, 'Admin', 'User', 1)

DELETE CorporateUsers WHERE FirstName <> 'Admin'
DELETE Users WHERE Username <> 'admin'
INSERT INTO UserRoles (UserID, RoleID) VALUES ((SELECT UserID FROM Users WHERE Username = 'admin'), (SELECT RoleID FROM Roles WHERE Name = 'Power User'))
INSERT INTO UserRoles (UserID, RoleID) VALUES ((SELECT UserID FROM Users WHERE Username = 'nsadmin'), (SELECT RoleID FROM Roles WHERE Name = 'Administrator'))

UPDATE Users set PasswordOLD = 'Password0', PasswordHash = 'kMOt2U31oBdxIDbvlj9fg8M6iCqXNUEFV5DIedJJ3q9WQFYHXKS2B6emnreb/dX9LvpSiWPZkjExtBvOwyN977yMTfc=' where Username = 'admin'
UPDATE Users set PasswordOLD = 'n3tst3psd3m0', PasswordHash = 'XY8AHfyfcWlosCjl8e5TrVbtgmdDomufEMyrxDSoMcoClt2KhMuzDZpfuh45EMfoCD924l742Cb6eg8gSAx3fzrC/QYlFA==' where Username = 'nsadmin'



INSERT INTO Sites (SiteTypeID, SiteStatusID, Name, DisplayName, MarketID, IsBase, DefaultLanguageID, DateSignedUpUTC) VALUES (1, 1, 'Corporate Base Site', 'Corporate Site', 1, 1, 1, GETUTCDATE())
INSERT INTO Sites (SiteTypeID, SiteStatusID, Name, DisplayName, MarketID, IsBase, DefaultLanguageID, DateSignedUpUTC) VALUES (2, 1, 'Distributor BackOffice Base Site', 'Distributor BackOffice', 1, 1, 1, GETUTCDATE())
INSERT INTO Sites (SiteTypeID, SiteStatusID, Name, DisplayName, MarketID, IsBase, DefaultLanguageID, DateSignedUpUTC) VALUES (3, 1, 'PWS Base Site', 'PWS Site', 1, 1, 1, GETUTCDATE())

INSERT INTO SiteLanguages (SiteID, LanguageID) VALUES (1, 1)
INSERT INTO SiteLanguages (SiteID, LanguageID) VALUES (2, 1)
INSERT INTO SiteLanguages (SiteID, LanguageID) VALUES (3, 1)



-- Logs Tables
exec dba_SmartTruncateTable ErrorLogs
exec dba_SmartTruncateTable DebugLogs
exec dba_SmartTruncateTable ApplicationUsageLogs
exec dba_SmartTruncateTable UsageLogs

-- Audit Tables
exec dba_SmartTruncateTable AuditMachineNames
exec dba_SmartTruncateTable AuditSqlUserNames
exec dba_SmartTruncateTable AuditTableColumns
exec dba_SmartTruncateTable AuditTableIgnoredColumns
exec dba_SmartTruncateTable AuditTables
exec dba_SmartTruncateTable AuditLogs
















-- Insert AuditTableIgnoredColumns records for all tables with 'ModifiedByUserID' column - JHE
	DECLARE @NewLineChar AS VARCHAR(MAX) = CHAR(13) + CHAR(10)
	DECLARE @columnName sysname, @tableName sysname, @createScript AS VARCHAR(MAX)
	DECLARE @insertIgnoreRowsSQL AS VARCHAR(MAX) = '', @insertAuditTableRowsSQL AS VARCHAR(MAX) = ''

	DECLARE rename_cursor CURSOR FOR 
	WITH AllPKs AS (
		SELECT SO.NAME AS 'TableName', SC.NAME AS 'ColumnName'
		FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id 
		LEFT JOIN dbo.syscomments SM ON SC.cdefault = SM.id,
		information_schema.columns i 
		WHERE SO.xtype = 'U'  
		AND	(SC.NAME = 'ModifiedByUserID' OR SC.NAME = 'DateCreatedUTC' OR SC.NAME = 'CreatedByUserID')
		AND i.TABLE_NAME = SO.NAME
		AND i.COLUMN_NAME = SC.NAME
	)
	SELECT * FROM AllPKs

	OPEN rename_cursor

	FETCH NEXT FROM rename_cursor 
	INTO @tableName, @columnName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @insertAuditTableRowsSQL = @insertAuditTableRowsSQL + ' IF NOT EXISTS (SELECT * FROM AuditTables WHERE Name = ''' + @tableName + ''') INSERT INTO AuditTables (Name) VALUES (''' + @tableName + ''') ' + @NewLineChar		
		SELECT @insertIgnoreRowsSQL = @insertIgnoreRowsSQL + ' IF NOT EXISTS (SELECT * FROM AuditTableIgnoredColumns WHERE AuditTableID = (SELECT AuditTableID FROM AuditTables WHERE Name = ''' + @tableName + ''') AND ColumnName = ''' + @columnName + ''') INSERT INTO AuditTableIgnoredColumns (AuditTableID, ColumnName) VALUES ((SELECT AuditTableID FROM AuditTables WHERE Name = ''' + @tableName + '''), ''' + @columnName + ''') ' + @NewLineChar
			
	FETCH NEXT FROM rename_cursor 
	INTO @tableName, @columnName
	END
	CLOSE rename_cursor
	DEALLOCATE rename_cursor
				
	-- STEP 1: Start the transaction
	BEGIN TRANSACTION
	
	DECLARE @SQL AS NVARCHAR(MAX)
	DECLARE @SQLScript AS VARCHAR(MAX) -- Just for printing more chars - JHE
	DECLARE @intTableCount INT
	SELECT @SQL = @insertAuditTableRowsSQL + @NewLineChar + @insertIgnoreRowsSQL
	SELECT @SQLScript = @SQL
	--SELECT @SQL
	PRINT '----- Running Script -----' + @NewLineChar + @NewLineChar
	PRINT @SQLScript + @NewLineChar
	PRINT '----- End of Script -----' + @NewLineChar + @NewLineChar
		
	EXEC sp_executesql @SQL, N'@intTableCount INT OUTPUT', @intTableCount OUTPUT
	
	IF (@intTableCount <> 0)
	BEGIN
		PRINT 'Rolling Back the Transaction.'
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		COMMIT TRANSACTION
		PRINT 'Inserted Successfully.'
	END






COMMIT TRANSACTION

GO
DBCC SHRINKDATABASE(N'NSFramework_Test', 2 )
GO