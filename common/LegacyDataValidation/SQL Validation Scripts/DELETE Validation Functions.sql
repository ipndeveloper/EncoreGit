-----------------------------------------------------------------------------------------------------------------
---------------------------------------CLIENT SETTINGS FUNCTIONS ------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'validation.validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity...')
	DROP FUNCTION [validation].[validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity]
END

GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'validation.validationudf_ClientSettings_GetClientCurrencyPriceTypeArray') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_ClientSettings_GetClientCurrencyPriceTypeArray...')
	DROP FUNCTION [validation].[validationudf_ClientSettings_GetClientCurrencyPriceTypeArray]
END

GO 

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'validation.validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID...')
	DROP FUNCTION [validation].[validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID]
END

GO

-----------------------------------------------------------------------------------------------------------------
----------------------------------------------- FUNCTIONS -------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'validation.validationudf_CalculateOrderPrimaryCommissionableUnitPrice') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_CalculateOrderPrimaryCommissionableUnitPrice...')
	DROP FUNCTION [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice]
END

GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'validation.validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice...')
	DROP FUNCTION [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice]
END

GO 

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'validation.validationudf_CalculateOrderItemModificationAdjustmentAmount') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_CalculateOrderItemModificationAdjustmentAmount...')
	DROP FUNCTION [validation].[validationudf_CalculateOrderItemModificationAdjustmentAmount]
END

GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_RetrieveSuccessfulOrderPaymentTotal') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_RetrieveSuccessfulOrderPaymentTotal...')
	DROP FUNCTION [validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal]
END

GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_RetrieveProductPriceAtOrderTime') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_RetrieveProductPriceAtOrderTime...')
	DROP FUNCTION [validation].[validationudf_RetrieveProductPriceAtOrderTime]
END

GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_RetrieveOrderEffectiveDate') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_RetrieveOrderEffectiveDate...')
	DROP FUNCTION [validation].[validationudf_RetrieveOrderEffectiveDate]
END

GO

-----------------------------------------------------------------------------------------------------------------
------------------------------------------- STORED PROCEDURES ---------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = object_id(N'[validation].[validation_UnkitKit]') AND OBJECTPROPERTY(ID, N'IsProcedure') = 1)
BEGIN
	PRINT('Dropping validation procedure validation_UnkitKit...')
	DROP PROCEDURE [validation].[validation_UnkitKit]
END

GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = object_id(N'[validation].[validation_RecalculateOrder]') AND OBJECTPROPERTY(ID, N'IsProcedure') = 1)
BEGIN
	PRINT('Dropping validation procedure validation_RecalculateOrder...')
	DROP PROCEDURE [validation].[validation_RecalculateOrder]
END

GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = object_id(N'[validation].[validation_ValidateOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	PRINT('Dropping validation procedure validation_ValidateOrder...')
	DROP PROCEDURE [validation].[validation_ValidateOrder]
END

GO

-----------------------------------------------------------------------------------------------------------------
------------------------------------------------ SCHEMA ---------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'validation')
BEGIN
	PRINT('Dropping validation schema...')
	EXEC('DROP SCHEMA validation')
END
