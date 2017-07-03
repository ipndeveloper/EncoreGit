-----------------------------------------------------------------------------------------------------------------
------------------------------------------------ SCHEMA ---------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'validation')
BEGIN
	PRINT('Creating validation schema')
	EXEC('CREATE SCHEMA validation')
END

GO

-----------------------------------------------------------------------------------------------------------------
---------------------------------------CLIENT SETTINGS FUNCTIONS ------------------------------------------------
-----------------------------------------------------------------------------------------------------------------



IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Creating validation function validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity...')

	EXEC sp_executesql @statement = N'
	CREATE FUNCTION [validation].[validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity]
	()
	RETURNS bit
	AS
	BEGIN
		RETURN 1
	END'
END

GO

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_ClientSettings_GetClientCurrencyPriceTypeArray') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Creating validation function validationudf_ClientSettings_GetClientCurrencyPriceTypeArray...')

	EXEC sp_executesql @statement = N'
	CREATE FUNCTION [validation].[validationudf_ClientSettings_GetClientCurrencyPriceTypeArray]
	()
	RETURNS varchar(50)
	AS
	BEGIN
		RETURN N''1,22''
	END'
END

GO

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Creating validation function validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID...')

	EXEC sp_executesql @statement = N'
	CREATE FUNCTION [validation].[validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID]
	()
	RETURNS int
	AS
	BEGIN
		RETURN 21
	END'
END

GO
