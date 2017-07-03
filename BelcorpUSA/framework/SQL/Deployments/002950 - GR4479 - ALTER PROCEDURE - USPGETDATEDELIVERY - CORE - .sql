-- ====================================================
-- **GO STATEMENTS**
-- Plain GO statements will not execute properly!!!!!
-- Please avoid using GO statements, or if you need to
-- use a GO then append a --GO to the end of the GO 
-- statement without any spaces as shown below.
--
-- i.e.   GO--GO
--
-- **TRANSACTIONS**
-- Transactions are NOT Supported! The run once logic
-- will roll back if there is a problem.
--
-- **TEMP TABLES**
-- If you want to use temporary tables please use 
-- global temp tables. 
-- 
-- i.e.   ##
-- ====================================================

USE BelcorpBRACore

GO

/******************************************************************************      
Author Name       : ***      
Created Date      : ***      
Description       : Obtener la fecha Delivery      
      
Modifications      
------------------------------------------------------------------------------      
Date (mm/dd/yyyy) Developer Name  Description      
------------------------------------------------------------------------------      
31/08/2016      Karina Torres  Comentar el uspGetEstimatedDeliveryDate1  y traer lo que hace este mismo pero defrente en el store      
15/09/2016      Karina Torres  Agregar la validacion que no tome  dias festivos.    
29/09/2016      Karina Torres  Ajustar la logica de no tomar en cuenta los dias , sabados , domingos, y los de holiday. 
16/12/2016      Carlos Arcos   Se añade codigo de Belcorp para los holidays que aplican a todos.  - GR4479
******************************************************************************/          
ALTER PROCEDURE [dbo].[uspGetDateDelivery]
	@LogisticsProviderID INT,
	@ShippingRateGroupID INT,
	@PostalCode VARCHAR(25),
	@OrderTypeID INT,
	@ShippingOrderTypeID INT,
	@fechaOut VARCHAR(50) = NULL OUTPUT
AS
	DECLARE @GetDate             DATETIME = (
	            SELECT DATEADD(DAY, +0, GETDATE())
	        ),
	        @Street              VARCHAR(150),
	        @DaysForDelivery     INT = 0,
	        @NumDia              INT = 0,
	        @StatusWork          BIT,
	        @EvalueDay           INT = 0,
	        @EstimatedDate       DATETIME,
	        @EvalueDate          DATETIME = (
	            SELECT DATEADD(DAY, +0, GETDATE())
	        ),
	        @StateProvinceID     INT,
	        @DateEnd             DATETIME          
	
	CREATE TABLE #tmpValidDate
	(
		PostalCode          INT,
		RouteID             INT,
		monday              INT,
		tuesday             INT,
		Wednesday           INT,
		Thursday            INT,
		friday              INT,
		saturday            INT,
		sunday              INT,
		WorkInSaturdays     INT,
		WorkInSundays       INT,
		WorkInHolidays      INT,
		DaysForDelivery     INT,
		OrderTypeId         INT
	)          
	
	INSERT INTO #tmpValidDate
	--EXEC uspGetEstimatedDeliveryDate1 @LogisticsProviderID,@ShippingOrderTypeID,@PostalCode         
	SELECT @PostalCode,
	       17                      RouteID,
	       1                       monday,
	       1                       tuesday,
	       1                       Wednesday,
	       1                       Thursday,
	       1                       friday,
	       lp2.WorkInSaturdays     saturday,
	       lp2.WorkInSundays       sunday,
	       lp2.WorkInSaturdays,
	       lp2.WorkInSundays,
	       lp2.WorkInHolidays,
	       sot.DaysForDelivery,
	       sot.OrderTypeId
	FROM   LogisticsProviders lp2
	       JOIN SHippingOrderTypes sot
	            ON  sot.ShippingOrderTypeID = @ShippingOrderTypeID
	            AND sot.logisticsProviderID = lp2.LogisticsProviderID
	WHERE  lp2.LogisticsProviderID = @LogisticsProviderID        
	
	
	DELETE #tmpValidDate
	WHERE  OrderTypeId <> @OrderTypeID          
	
	SELECT @NumDia = DaysForDelivery
	FROM   #tmpValidDate             
	
	
	SELECT RouteID,
	       Dia,
	       WORK 
	       INTO     #PIVOT
	FROM   (
	           SELECT TOP 1 RouteID,
	                  monday,
	                  tuesday,
	                  Wednesday,
	                  Thursday,
	                  friday,
	                  saturday,
	                  sunday
	           FROM   #tmpValidDate
	       ) P 
	       UNPIVOT(
	           WORK FOR Dia IN (monday, tuesday, Wednesday, Thursday, friday, 
	                           saturday, sunday)
	       )     AS UNPVT          
	
	SET @DaysForDelivery = (
	        SELECT TOP 1 DaysForDelivery
	        FROM   #tmpValidDate
	    )          
	
	SELECT @EstimatedDate = @GetDate + @DaysForDelivery          
	
	SELECT ROW_NUMBER() OVER(ORDER BY RouteID) Num,
	       * 
	       INTO #tmpResult
	FROM   #PIVOT 
	--Dias Festivos          
	SELECT @StateProvinceID = StateProvinceID
	FROM   StateProvinces
	WHERE  StateAbbreviation IN (SELECT StateAbbreviation
	                             FROM   TaxCache
	                             WHERE  PostalCode = @PostalCode)           
	
	
	
	SELECT CONVERT(DATETIME, DateHoliday) DateHoliday 
	       INTO               #tmpFestiveDate
	FROM   Holiday
	WHERE  CountryID       = 73
	       AND StateID     = @StateProvinceID
	       OR  StateID IS     NULL            
	
	SET @DateEnd = (
	        SELECT @EvalueDate + @NumDia
	    )           
	
	DECLARE @NumDayAdd     INT = 0          
	DECLARE @NewDate       DATETIME = (
	            SELECT DATEADD(DAY, +0, GETDATE())
	        ) 
	
	PRINT '@NumDia:' + CAST(@NumDia AS VARCHAR) 
	PRINT '@@NumDayAdd:' + CAST(@NumDayAdd AS VARCHAR) 
	
	DECLARE @fc DATE             
	WHILE @NumDia > 0
	BEGIN
	    SET @fc = DATEADD(DAY, 1, CAST(@NewDate AS DATE))       
	    SET @NewDate = DATEADD(DAY, 1, @NewDate) 
	    
	    IF (
	           (DATEPART(dw, @NewDate) != 6)
	           AND (DATEPART(dw, @NewDate) != 7)
	           AND NOT EXISTS(
	                   SELECT *
	                   FROM   Holiday
	                   WHERE  ACTIVE = 1
	                          AND CountryID = 73
	                          AND (
	                                  StateID = @StateProvinceID
	                                  OR StateID IS NULL
	                                  OR StateID = 105 -- INI/FIN - GR4479
	                              )
	                          AND CAST(DateHoliday AS DATE) = @fc
	               )
	       ) -- si es un dia normal va agregando los dias ,de lo contrario restamos el dia
	    BEGIN
	        SET @NumDia = @NumDia -1
	    END
	END 
	
	SELECT @fechaOut = CONVERT(VARCHAR, @NewDate, 105)           
	SELECT CONVERT(VARCHAR, @NewDate, 105) 
	RETURN     


