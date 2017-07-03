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
----------------------------------------------- FUNCTIONS -------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_RetrieveOrderEffectiveDate') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_RetrieveOrderEffectiveDate...')
	DROP FUNCTION [validation].[validationudf_RetrieveOrderEffectiveDate]
END

GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_RetrieveProductPriceAtOrderTime') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_RetrieveProductPriceAtOrderTime...')
	DROP FUNCTION [validation].[validationudf_RetrieveProductPriceAtOrderTime]
END

GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_RetrieveSuccessfulOrderPaymentTotal') AND xtype IN (N'FN', N'IF', N'TF'))
BEGIN
	PRINT('Dropping validation function validationudf_RetrieveSuccessfulOrderPaymentTotal...')
	DROP FUNCTION [validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal]
END

GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'validation.validationudf_CalculateOrderPrimaryCommissionableUnitPrice') AND xtype IN (N'FN', N'IF', N'TF'))
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

PRINT('Creating validation function validationudf_RetrieveOrderEffectiveDate...')

GO

CREATE FUNCTION [validation].[validationudf_RetrieveOrderEffectiveDate] 
	(
		@orderID int
	)
RETURNS DateTime
AS
BEGIN
	DECLARE @return DateTime
	SELECT @return = ISNULL((SELECT CommissionDateUTC from Orders WHERE OrderID = @orderID), GETDATE())
	RETURN @return
END

GO

PRINT('Creating validation function validationudf_RetrieveProductPriceAtOrderTime...')

GO

CREATE FUNCTION [validation].[validationudf_RetrieveProductPriceAtOrderTime] 
	(
		@productID int,
		@productPriceTypeID int,
		@orderDate DateTime
	)
RETURNS money
AS
BEGIN
	DECLARE @return money
	SELECT @return = (SELECT TOP 1 Price FROM [dbo].[Commissions_ProductPrices] WHERE ProductID = @productID and ProductPriceTypeID = @productPriceTypeID and EffectiveDate <= @orderDate order by EffectiveDate desc)
	RETURN @return
END

GO

PRINT('Creating validation function validationudf_RetrieveSuccessfulOrderPaymentTotal...')

GO

CREATE FUNCTION [validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal] 
	(
		@orderID int
	)
RETURNS money
AS
BEGIN
	DECLARE @return money
	SELECT @return = (
						SELECT 
							SUM(op.Amount) 
						FROM 
							[dbo].[OrderPayments] op
							JOIN 
								[dbo].[Orders] o ON o.OrderID = op.OrderID
						WHERE 
							op.OrderID = @orderID and 
							op.OrderPaymentStatusID = 2
						GROUP BY
							o.ShippingTotal,
							o.TaxAmountTotal,
							o.HandlingTotal
					)
	RETURN @return
END

GO

PRINT('Creating validation function validationudf_CalculateOrderItemModificationAdjustmentAmount...')

GO

CREATE FUNCTION [validation].[validationudf_CalculateOrderItemModificationAdjustmentAmount] 
	(
		@originalUnitValue money,
		@orderLineOperationID int,
		@operand money
	)
RETURNS money
AS
BEGIN
	DECLARE @return money
	SELECT @return = 
	CASE 
		WHEN (@orderLineOperationID = 3)
			THEN @operand * @originalUnitValue
		WHEN (@orderLineOperationID = 2) -- Flat Amount
			THEN @operand
		ELSE -- Added Item modification, no value modification
			 0
	END
	RETURN @return
END

GO

PRINT('Creating validation function validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice...')

GO

CREATE FUNCTION [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice] 
	(
		@orderItemID int
	)
RETURNS money
AS
BEGIN
	DECLARE @return money
	SELECT @return = CASE
						WHEN (oi.ParentOrderItemID IS NOT NULL)
							THEN 0
						ELSE 
							CASE 
								WHEN (oi.DiscountPercent is not null) THEN ([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)) - ([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)) * (1 - oi.DiscountPercent)))
								WHEN (oi.Discount is not null) THEN (([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID))) - (oi.Discount/ISNULL(oi.Quantity,1)))
								ELSE ([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)))
							END
						END
    FROM 
		OrderItems oi 
		JOIN
			OrderCustomers oc on oc.OrderCustomerID = oi.OrderCustomerID
		JOIN
			Orders o on o.OrderID = oc.OrderID
	WHERE
		oi.OrderItemID = @orderItemID

	DECLARE @targetPropertyName varchar(255)
	SELECT @targetPropertyName = Name FROM [dbo].[ProductPriceTypes] WHERE ProductPriceTypeID = (SELECT ProductPriceTypeID FROM OrderItems WHERE OrderItemID = @orderItemID)

	DECLARE @adjustment_OperationID int, @adjustment_DecimalValue money

	DECLARE adjustments_Cursor CURSOR FORWARD_ONLY
	FOR
		SELECT 
			adjs.ModificationOperationID,
			adjs.ModificationDecimalValue 
		FROM 
			[Order].[OrderAdjustmentOrderLineModifications] adjs
		WHERE
			adjs.OrderItemID = @orderItemID AND 
			adjs.PropertyName = @targetPropertyName AND 
			adjs.ModificationOperationID IN (2, 3) -- from the OrderAdjustmentOrderLineOperations table... sorry.
	OPEN adjustments_Cursor
	
	WHILE (1=1)
	BEGIN
		FETCH NEXT FROM adjustments_Cursor
			INTO @adjustment_OperationID, @adjustment_DecimalValue
		IF (@@FETCH_STATUS <> 0) BREAK
		SET @return = @return - [validation].[validationudf_CalculateOrderItemModificationAdjustmentAmount](@return, @adjustment_OperationID, @adjustment_DecimalValue)
	END

	RETURN @return 
END

GO

PRINT('Creating validation function validationudf_CalculateOrderPrimaryCommissionableUnitPrice...')

GO

CREATE FUNCTION [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice] 
	(
		@orderItemID int,
		@primaryCommissionPriceTypeID int
	)
RETURNS money
AS
BEGIN
	DECLARE @return money

	-- child items return 0 price... or they should.  Stupid.
	SELECT @return = CASE
						WHEN (oi.HostessRewardRuleID IS NOT NULL)
							THEN 0
						WHEN (oi.ParentOrderItemID IS NOT NULL)
							THEN 0
						ELSE
							(
							CASE 
								WHEN (oi.Discount IS NOT NULL)
									THEN 0
								WHEN (oi.DiscountPercent IS NOT NULL)
									THEN 0
								WHEN 
									oi.ItemPriceActual IS NOT NULL
									THEN 
									[validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)) * -- original price
									(oi.ItemPriceActual / NULLIF([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID), 0))
								ELSE
									[validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)) * -- original price
									(
										[validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID) / 
										NULLIF([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 0)
									)
							END
							)
						END 
	FROM 
		OrderItems oi
		JOIN
			OrderCustomers oc on oc.OrderCustomerID = oi.OrderCustomerID
		JOIN
			Orders o on o.OrderID = oc.OrderID
	WHERE
		oi.OrderItemID = @orderItemID
	RETURN @return 
END

GO

-----------------------------------------------------------------------------------------------------------------
------------------------------------------- STORED PROCEDURES ---------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = object_id(N'[validation].[validation_UnkitKit]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	PRINT('Dropping validation procedure validation_UnkitKit...')
	DROP PROCEDURE [validation].[validation_UnkitKit]
END

GO

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = object_id(N'[validation].[validation_RecalculateOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
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

PRINT('Creating validation procedure validation_UnkitKit...')

GO

CREATE PROCEDURE [validation].[validation_UnkitKit]
	@orderID int,
	@badKitOrderItemID int	
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		oi
	SET
		oi.ParentOrderItemID = NULL
	FROM
			Orders o
			JOIN
				OrderCustomers oc ON o.OrderID = oc.OrderID
			JOIN
				OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
			LEFT JOIN
				ProductPriceTypes ppt on oi.ProductPriceTypeID = ppt.ProductPriceTypeID
			LEFT JOIN
				[MicheCommissions].[dbo].[udfGetOrderItemVolume](@orderID) com on oi.OrderItemID = com.OrderItemID
	WHERE
		o.OrderID = @orderID AND
		oi.ParentOrderItemID = @badKitOrderItemID

	DELETE
		oip
	FROM
			OrderItemPrices oip 
			JOIN
				OrderItems oi on oi.OrderItemID = oip.OrderItemID
			JOIN
				OrderCustomers oc ON oi.OrderCustomerID = oc.OrderCustomerID
			JOIN
				Orders o on o.OrderID = oc.OrderID
			LEFT JOIN
				ProductPriceTypes ppt on oi.ProductPriceTypeID = ppt.ProductPriceTypeID
			LEFT JOIN
				[MicheCommissions].[dbo].[udfGetOrderItemVolume](@orderID) com on oi.OrderItemID = com.OrderItemID
	WHERE
		o.OrderID = @orderID AND
		oi.OrderItemID = @badKitOrderItemID

	DELETE
		oi
	FROM
			OrderItems oi
			JOIN
				OrderCustomers oc ON oi.OrderCustomerID = oc.OrderCustomerID
			JOIN
				Orders o on o.OrderID = oc.OrderID
			LEFT JOIN
				ProductPriceTypes ppt on oi.ProductPriceTypeID = ppt.ProductPriceTypeID
			LEFT JOIN
				[MicheCommissions].[dbo].[udfGetOrderItemVolume](@orderID) com on oi.OrderItemID = com.OrderItemID
	WHERE
		o.OrderID = @orderID AND
		oi.OrderItemID = @badKitOrderItemID
    
END

GO
PRINT('Creating validation procedure validation_RecalculateOrder...')

GO

CREATE PROCEDURE [validation].[validation_RecalculateOrder]
	@orderID int,
	@currencyPriceTypeIDArray varchar(255) = null,
	@primaryCommissionPriceTypeID int = null,
	@shouldMultiplyOrderItemPricesByQuantity bit = null
AS
BEGIN
	SET NOCOUNT ON;
	
	
	IF (@currencyPriceTypeIDArray IS NULL)
	BEGIN
		SET @currencyPriceTypeIDArray = [validation].[validationudf_ClientSettings_GetClientCurrencyPriceTypeArray]()
	END

	IF (@primaryCommissionPriceTypeID IS NULL)
	BEGIN
		SET @primaryCommissionPriceTypeID = [validation].[validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID]()
	END

	IF (@shouldMultiplyOrderItemPricesByQuantity IS NULL)
	BEGIN
		SET @shouldMultiplyOrderItemPricesByQuantity = [validation].[validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity]()
	END

	--------------------------- 
	----------------Order Items
	---------------------------
	UPDATE 
		oi
	SET
		oi.ItemPrice = Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2),
		oi.CommissionableTotal = Round([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity,2)
	FROM
			Orders o
			JOIN
				OrderCustomers oc ON o.OrderID = oc.OrderID
			JOIN
				OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
			LEFT JOIN
				ProductPriceTypes ppt on oi.ProductPriceTypeID = ppt.ProductPriceTypeID
			LEFT JOIN
				[MicheCommissions].[dbo].[udfGetOrderItemVolume](@orderID) com on oi.OrderItemID = com.OrderItemID
	WHERE
		o.OrderID = @orderID

	--------------------------- 
	----------Order Item Prices
	---------------------------

	UPDATE
		oip
	SET
		oip.OriginalUnitPrice = Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2)  * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END),
		oip.UnitPrice = ISNULL
						(
							CASE
								WHEN CAST(oip.ProductPriceTypeID as varchar)  IN (@currencyPriceTypeIDArray)  -- is a currency price type
									THEN
										CASE
											WHEN oi.ItemPriceActual IS NOT NULL
												THEN (Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.ItemPriceActual / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))  * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END)
											ELSE
												(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (Round([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID),2) / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))  * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END)
										END
								ELSE
									CASE -- is a commission price type
											WHEN oi.CommissionableTotalOverride IS NOT NULL
												THEN (Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.CommissionableTotalOverride / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))
											WHEN oi.ItemPriceActual IS NOT NULL
												THEN (Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.ItemPriceActual / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))
											ELSE
												(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (Round([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID),2) / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))
										END
							END
						, 0)
		FROM 
			Orders o
			JOIN
				OrderCustomers oc ON o.OrderID = oc.OrderID
			JOIN
				OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
			JOIN
				OrderItemPrices oip on oip.OrderItemID = oi.OrderItemID
			JOIN
				ProductPriceTypes ppt ON oip.ProductPriceTypeID = ppt.ProductPriceTypeID
		WHERE
			o.OrderID = @orderID

	--------------------------- 
	------------Order Customers
	--------------------------- 

	UPDATE 
		oc
	SET
		oc.Subtotal = oisums.ordersubtotal,
		oc.CommissionableTotal = oisums.ordercommissionabletotal
	FROM 
		Orders o
		JOIN
			OrderCustomers oc ON o.OrderID = oc.OrderID
		JOIN
			OrderCustomerTypes oct on oc.OrderCustomerTypeID = oct.OrderCustomerTypeID
		JOIN
			(
				SELECT 
					OrderCustomerID,
					Round(SUM(ISNULL(ItemPriceActual, [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](OrderItemID)) * Quantity), 2) as ordersubtotal,
					Round(SUM(ISNULL(CommissionableTotalOverride, [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](OrderItemID, @primaryCommissionPriceTypeID) * Quantity)), 2) as ordercommissionabletotal
				FROM OrderItems
				GROUP BY 
					OrderCustomerID
			) AS oiSUMS ON oiSUMS.OrderCustomerID = oc.OrderCustomerID
	WHERE
		o.OrderID = @orderID

	--------------------------- 
	---------------Order Totals
	--------------------------- 

	UPDATE
		o
	SET
		o.Subtotal = oc.Subtotal,
		o.CommissionableTotal = oc.CommissionableTotal
	FROM
		Orders o
		JOIN
			(
				SELECT 
					OrderID,
					SUM(Subtotal) AS Subtotal,
					SUM(CommissionableTotal) AS CommissionableTotal
				FROM
					ORDERCUSTOMERS
				GROUP BY 
					OrderID
			) oc ON o.OrderID = oc.OrderID
	WHERE
		o.OrderID = @orderID
    
END

GO

PRINT('Creating validation procedure validation_ValidateOrder...')

GO

CREATE PROCEDURE [validation].[validation_ValidateOrder]
	@orderID int,
	@currencyPriceTypeIDArray varchar(255) = null,
	@primaryCommissionPriceTypeID int = null,
	@shouldMultiplyOrderItemPricesByQuantity bit = null
AS
BEGIN
	SET NOCOUNT ON;

	IF (@currencyPriceTypeIDArray IS NULL)
	BEGIN
		SET @currencyPriceTypeIDArray = [validation].[validationudf_ClientSettings_GetClientCurrencyPriceTypeArray]()
	END

	IF (@primaryCommissionPriceTypeID IS NULL)
	BEGIN
		SET @primaryCommissionPriceTypeID = [validation].[validationudf_ClientSettings_GetClientPrimaryVolumePriceTypeID]()
	END

	IF (@shouldMultiplyOrderItemPricesByQuantity IS NULL)
	BEGIN
		SET @shouldMultiplyOrderItemPricesByQuantity = [validation].[validationudf_ClientSettings_GetBoolShouldMultiplyOrderItemPricesByQuantity]()
	END
	--------------------------- 
	---------------Order Totals
	--------------------------- 

	SELECT 
		o.OrderID,
		o.CommissionDateUTC,
		Round(o.Subtotal, 2) AS Subtotal,
		Round(SUM(ISNULL(oi.ItemPriceActual, ([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID))) * oi.Quantity),2) as CALC_OrderSubtotal,
		Round([validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal](o.OrderID),2) - Round(o.ShippingTotal, 2) - Round(o.HandlingTotal, 2) - Round(o.TaxAmountTotal, 2) as ProductPayment,
		Round(o.ShippingTotal, 2) as ShippingTotal,
		Round(o.HandlingTotal, 2) as HandlingTotal,
		Round(o.TaxAmountTotal, 2) as TaxAmountTotal,
		Round([validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal](o.OrderID),2) as ProductPaymentTotal,
		SUM(comm.AdjustedPrice) as COMM_OrderSubtotal,
		CASE 
			WHEN 
				(Round(o.SubTotal,2) <> Round(SUM(ISNULL(oi.ItemPriceActual, [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID)) * oi.Quantity),2)) OR -- Calculated total doesn't match
  				(Round(o.SubTotal,2) <> Round(SUM(comm.AdjustedPrice),2)) OR -- Commission DB total doesn't match
				(Round(o.SubTotal,2) <> Round([validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal](o.OrderID),2) - Round(o.ShippingTotal, 2) - Round(o.HandlingTotal, 2) - Round(o.TaxAmountTotal, 2)) -- Payment Total doesn't match
				THEN 
					(SELECT
						(	
							'FAIL - ' +
							CASE WHEN (Round(o.SubTotal,2) <> Round(SUM(ISNULL(oi.ItemPriceActual, [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID)) * oi.Quantity),2)) THEN '(CALC)' ELSE '' END +
							CASE WHEN (Round(o.SubTotal,2) <> Round(SUM(comm.AdjustedPrice),2)) THEN '(COMM)' ELSE '' END +
							CASE WHEN (Round(o.SubTotal,2) <> Round([validation].[validationudf_RetrieveSuccessfulOrderPaymentTotal](o.OrderID),2) - Round(o.ShippingTotal, 2) - Round(o.HandlingTotal, 2) - Round(o.TaxAmountTotal, 2)) THEN '(PAYMENTS)' ELSE '' END +
							''
						)
					)
			ELSE 'PASS' 
		END AS CurrencyValid,
		o.CommissionableTotal,
		Round(SUM(ISNULL(oi.CommissionableTotalOverride, [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity)),2) as CalculatedOrderCommissionTotal,
		SUM(comm.CommissionableTotal) as COMMDB_OrderCommissionTotal,
		CASE 
			WHEN 
				(o.CommissionableTotal <> Round(SUM(ISNULL(oi.CommissionableTotalOverride, [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity)),2)) OR -- Calculated total doesn't match
  				(o.CommissionableTotal <> SUM(comm.CommissionableTotal)) -- Commission DB total doesn't match
				THEN 
					(SELECT
						(	
							'FAIL - ' +
							CASE WHEN (o.CommissionableTotal <> Round(SUM([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity),2)) THEN '(CALC)' ELSE '' END +
							CASE WHEN (o.CommissionableTotal <> SUM(comm.CommissionableTotal)) THEN '(COMM)' ELSE '' END 
						)
					)
			ELSE 'PASS' 
		END AS VAL_OrderCommissionableTotal
	FROM 
		Orders o
		JOIN
			OrderCustomers oc ON o.OrderID = oc.OrderID
		JOIN
			OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
		LEFT JOIN
			[MicheCommissions].[dbo].[udfGetOrderItemVolume](@orderID) comm ON comm.OrderItemID = oi.OrderItemID
	WHERE
		o.OrderID = @orderID
	GROUP BY
		o.OrderID,
		o.CommissionDateUTC,
		o.Subtotal,
		o.CommissionableTotal,
		o.ShippingTotal,
		o.HandlingTotal,
		o.TaxAmountTotal

	--------------------------- 
	------------Order Customers
	--------------------------- 

	SELECT 
		oc.OrderCustomerID,
		oc.OrderID,
		oc.AccountID,
		oct.Name AS CustomerType,
		COUNT(oi.OrderItemID) AS OrderLineCount,
		Round(oc.Subtotal,2) as CustomerSubtotal,
		Round(SUM(ISNULL(oi.ItemPriceActual, [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID)) * oi.Quantity), 2) as CALC_Subtotal,
		CASE WHEN (Round(oc.SubTotal,2) = ISNULL(Round(SUM(ISNULL(oi.ItemPriceActual, [validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID)) * oi.Quantity),2),0)) THEN 'PASS' ELSE 'FAIL' END AS VAL_Subtotal,
		oc.CommissionableTotal as CommissionableTotal,
		Round(SUM(ISNULL(oi.CommissionableTotalOverride, [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity)), 2) as CALC_CommissionableTotal,
		CASE WHEN (oc.CommissionableTotal = ISNULL(Round(SUM(ISNULL(oi.CommissionableTotalOverride, [validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity)), 2),0)) THEN 'PASS' ELSE 'FAIL' END AS VAL_CommissionableTotal
	FROM 
		Orders o
		JOIN
			OrderCustomers oc ON o.OrderID = oc.OrderID
		LEFT JOIN
			OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
		JOIN
			OrderCustomerTypes oct on oc.OrderCustomerTypeID = oct.OrderCustomerTypeID
	WHERE
		o.OrderID = @orderID
	GROUP BY
		oc.OrderID,
		oc.AccountID,
		oct.Name,
		o.Subtotal,
		o.CommissionableTotal,
		oc.OrderCustomerID,
		oc.Subtotal,
		oc.CommissionableTotal

	DECLARE @orderCustomerID int

	DECLARE orderCustomers_CURSOR CURSOR FORWARD_ONLY
	FOR
		SELECT 
			oc.OrderCustomerID
		FROM 
			[dbo].[OrderCustomers] oc
			JOIN 
				[dbo].[OrderItems] oi ON oc.OrderCustomerID = oi.OrderCustomerID
		WHERE
			oc.OrderID = @orderId
		GROUP BY
			oc.OrderCustomeriD

	OPEN orderCustomers_CURSOR
	WHILE (1=1)
	BEGIN
		FETCH NEXT FROM orderCustomers_CURSOR
			INTO @orderCustomerID
		IF (@@FETCH_STATUS <> 0) BREAK
	
		--------------------------- 
		-------Order Customer Title
		--------------------------- 

		SELECT 'Customer ID: ' + CAST(@orderCustomerID AS VARCHAR) as ORDER_CUSTOMER_ID

		--------------------------- 
		----------------Order Items
		--------------------------- 

		SELECT 
			oi.OrderItemID,
			oi.ParentOrderItemID,
			oi.ProductID,
			oi.SKU,
			oi.Quantity,
			CASE 
				WHEN oi.ParentOrderItemID IS NOT NULL THEN 'KIT ITEM'
				WHEN oi.HostessRewardRuleID IS NOT NULL THEN 'HOSTESS REW'
				WHEN (SELECT Count(1) FROM [Order].[OrderAdjustmentOrderLineModifications] WHERE OrderItemID = oi.OrderItemID) > 0 THEN 'ADJUSTED'
				ELSE ''
			END AS OrderItemType,
			ppt.Name AS PriceType,
			oi.ItemPrice as ItemPrice,
			oi.ItemPriceActual as OverridePrice,
			ISNULL(oi.ItemPriceActual,ISNULL(Round([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID),2),0)) as CALC_UnitPrice,
			CASE
				WHEN
					ISNULL(oi.ItemPriceActual,ISNULL(Round([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID),2),0)) <> ISNULL(oi.ItemPriceActual,ISNULL(Round(oi.ItemPrice,2),0))
					THEN 
						CASE
							WHEN
								oi.ItemPriceActual IS NOT NULL
								THEN 'FAIL - OVERRIDE'
							WHEN
								oi.ParentOrderItemID IS NOT NULL
								THEN 'FAIL - KIT ITEM'
							WHEN
								oi.HostessRewardRuleID IS NOT NULL
								THEN 'FAIL - HOST REW'
							ELSE
								'FAIL'
						END
				ELSE
					CASE
						WHEN
							oi.ItemPriceActual IS NOT NULL
							THEN 'PASS - OVERRIDE'
						WHEN
							oi.ParentOrderItemID IS NOT NULL
							THEN 'PASS - KIT ITEM'
						WHEN
							oi.HostessRewardRuleID IS NOT NULL
							THEN 'PASS - HOST REW'
						ELSE
						'PASS'
					END
			END as VAL_TotalPrice,
			oi.CommissionableTotal AS TotalComm,
			oi.CommissionableTotalOverride AS OverrideComm,
			Round([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID),2) AS CALC_UnitComm,
			Round([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity,2) as CALC_TotalComm,
			CASE
				WHEN
					ISNULL(oi.CommissionableTotalOverride,Round(([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID) * oi.Quantity),2)) <> ISNULL(oi.CommissionableTotalOverride,oi.CommissionableTotal)
					THEN 
						CASE
							WHEN
								oi.CommissionableTotalOverride IS NOT NULL
								THEN 'FAIL - OVERRIDE'
							WHEN
								oi.ParentOrderItemID IS NOT NULL
								THEN 'FAIL - KIT ITEM'
							WHEN
								oi.HostessRewardRuleID IS NOT NULL
								THEN 'FAIL - HOST REW'
							ELSE
								'FAIL'
						END
				ELSE
					CASE
						WHEN
							oi.CommissionableTotalOverride IS NOT NULL
							THEN 'PASS - OVERRIDE'
						WHEN
							oi.ParentOrderItemID IS NOT NULL
							THEN 'PASS - KIT ITEM'
						WHEN
							oi.HostessRewardRuleID IS NOT NULL
							THEN 'PASS - HOST REW'
						ELSE
						'PASS'
					END
			END as VAL_PrimaryCommissionTotal
		FROM 
			Orders o
			JOIN
				OrderCustomers oc ON o.OrderID = oc.OrderID
			JOIN
				OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
			JOIN
				ProductPriceTypes ppt on oi.ProductPriceTypeID = ppt.ProductPriceTypeID
		WHERE
			oc.OrderCustomerID = @orderCustomerID

		--------------------------- 
		----------Order Item Prices
		---------------------------

		SELECT
			oip.OrderItemPriceID,
			oi.OrderItemID,
			ppt.Name as PriceType,
			oip.OriginalUnitPrice,
			Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END) as CALC_OriginalUnitPrice,
			CASE
				WHEN
					[validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)) is null
					THEN
						'NO HISTORICAL PRICE'
				WHEN
					oip.OriginalUnitPrice = ISNULL(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END), 0)
					THEN 
						'PASS'
				ELSE
					'FAIL'
				END as VAL_OriginalUnitPrice,
			oip.UnitPrice,
			ROUND(CASE
				WHEN CAST(oip.ProductPriceTypeID as varchar) IN (@currencyPriceTypeIDArray) -- is a currency price type
					THEN
						CASE
							WHEN oi.ItemPriceActual IS NOT NULL
								THEN (Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.ItemPriceActual / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0))) * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END)
							ELSE
								(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (Round([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID),2) / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0))) * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END)
						END
				ELSE
					CASE -- is a commission price type
							WHEN oi.CommissionableTotalOverride IS NOT NULL
								THEN (Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.CommissionableTotalOverride / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))
							WHEN oi.ItemPriceActual IS NOT NULL
								THEN (Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.ItemPriceActual / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))
							ELSE
								(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (Round([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID),2) / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))
						END
			END,2) AS CALC_UnitPrice,
			CASE
				WHEN CAST(oip.ProductPriceTypeID as varchar) IN (@currencyPriceTypeIDArray) -- is a currency price type
					THEN
						CASE
							WHEN
								[validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)) is null
								THEN
									'NO HISTORICAL PRICE'
							WHEN
								oi.ItemPriceActual IS NOT NULL
								THEN 
									CASE 
										WHEN
											oip.UnitPrice = ISNULL((Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.ItemPriceActual / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0)))  * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END),0)
											THEN 'PASS'
										ELSE
											'FAIL'
									END
							ELSE
								CASE 
									WHEN
										oip.UnitPrice = ISNULL((Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (Round([validation].[validationudf_CalculateOrderItemPrimaryCurrencyUnitPrice](oi.OrderItemID),2) / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0))) * (CASE WHEN @shouldMultiplyOrderItemPricesByQuantity = 1 THEN oi.Quantity ELSE 1 END),0)
										THEN 'PASS'
									ELSE
										'FAIL'
									END
							END 
				ELSE
					CASE
						WHEN
							oi.CommissionableTotalOverride IS NOT NULL
							THEN 
								CASE 
									WHEN
										oip.UnitPrice = ISNULL((Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.CommissionableTotalOverride / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0))),0)
										THEN 'PASS'
									ELSE
										'FAIL'
								END
						WHEN
							oi.ItemPriceActual IS NOT NULL
							THEN 
								CASE 
									WHEN
										oip.UnitPrice = ISNULL((Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (oi.ItemPriceActual / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oi.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0))),0)
										THEN 'PASS'
									ELSE
										'FAIL'
								END
						ELSE
							CASE 
								WHEN
									oip.UnitPrice = ISNULL((Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, oip.ProductPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2) * (Round([validation].[validationudf_CalculateOrderPrimaryCommissionableUnitPrice](oi.OrderItemID, @primaryCommissionPriceTypeID),2) / NULLIF(Round([validation].[validationudf_RetrieveProductPriceAtOrderTime](oi.ProductID, @primaryCommissionPriceTypeID, [validation].[validationudf_RetrieveOrderEffectiveDate](o.OrderID)), 2), 0))),0)
									THEN 'PASS'
								ELSE
									'FAIL'
								END
						END 
			END as VAL_UnitPrice
		FROM 
			Orders o
			JOIN
				OrderCustomers oc ON o.OrderID = oc.OrderID
			JOIN
				OrderItems oi on oc.OrderCustomerID = oi.OrderCustomerID
			JOIN
				OrderItemPrices oip on oip.OrderItemID = oi.OrderItemID
			JOIN
				ProductPriceTypes ppt ON oip.ProductPriceTypeID = ppt.ProductPriceTypeID
		WHERE
			oc.OrderCustomerID = @orderCustomerID

	END

	CLOSE orderCustomers_CURSOR
	DEALLOCATE orderCustomers_CURSOR

END

