CREATE PROC [dbo].[UspErrorLogsInsert]   
    @LogDateUTC datetime,  
    @ExceptionTypeName varchar(255),  
    @Source varchar(500),  
    @Message varchar(5000)  
AS   
  
   
 INSERT INTO [dbo].[ErrorLogs] ([LogDateUTC], [ExceptionTypeName], [Source], [Message])  
 SELECT SYSDATETIME(), @ExceptionTypeName, @Source, @Message  
GO
CREATE PROC [dbo].[UspLogisticsCommunicationInsert] @Message NVARCHAR(MAX)  
AS   
    BEGIN  
  
        INSERT  INTO LogisticsCommunication  
                ( Message  
                ,CreatedOn  
                ,UpdatedOn  
                )  
                SELECT  @Message  
                       ,SYSDATETIME()  
                       ,SYSDATETIME()  
    END  
GO
CREATE PROC [dbo].[UspLogisticsCommunicationUpdate] @Message NVARCHAR(MAX)  
AS   
    BEGIN  
        UPDATE  LogisticsCommunication  
        SET     Message = @Message  
               ,UpdatedOn = SYSDATETIME()  
    END  
GO
CREATE TABLE [dbo].[LogisticsCommunication](
	[LogisticsCommunicationID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PkLogisticsCommunications] PRIMARY KEY CLUSTERED 
(
	[LogisticsCommunicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO


SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordan Gurney
-- Create date: 3/30/2011
-- Description:	Select from VwReadyShipperImport view to select order line item information
-- =============================================
ALTER PROC [dbo].[UspSelectLogisticsProviderOrderItems]
AS
SELECT  [DistributorID]
       ,[CustomerNumber]
       ,[PrimaryID]      
       ,[Product]
       ,[Quantity]
       ,[Weight]
       ,[MerchandiseDescription]
       ,[Price]
       ,[Title]
       ,[CommodityCode]
       ,[PerkPoints]
       ,[CountryOfOrigin]
       ,[DeliveryConfirmation]
       ,[Warehouse]
       ,[URL] 
       ,v.OrderItemID
FROM    [dbo].[VwLogisticsProviderImport] V


GO


SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jordan Gurney
-- Create date: 3/30/2011
-- Description:	Select from VwReadyShipperImport view grouping by the Order Header data only.
-- =============================================
ALTER PROC [dbo].[UspSelectVwLogisticsProviderOrderHeader]
AS
SELECT  [DistributorID]
       ,[CustomerNumber]
       ,[PrimaryID]
       ,[BillAddress1]
       ,[BillAddress2]
       ,[BillCity]
       ,[Billcountry]
       ,[BillFirst]
       ,[BillLast]
       ,[BillCompany]
       ,[BillZip]
       ,[BillState]
       ,[BillPhone]
       ,[BillEmail]
       ,[ShipAddress1]
       ,[ShipAddress2]
       ,[ShipCity]
       ,[ShipCountry]
       ,[ShipFirst]
       ,[ShipLast]
       ,[ShipCompany]
       ,[ShipZip]
       ,[ShipState]
       ,[ShipPhone]
       ,[ShipEmail]
       ,[ShipType]
       ,[ShipVia]
       ,[BilledShipping]
       ,[Tax]
       ,[DeclaredValue]
       ,[TotalSale]
       ,[DateofSale]
       ,[MESSAGE]
      -- ,[Product]
       --,[Quantity]
       --,[Weight]
       --,[MerchandiseDescription]
       --,[Price]
       ,[PaymentMethod]
       ,[AutoShip]
       --,[Title]
       ,[Residential]
       --,[CommodityCode]
       --,[PerkPoints]
       --,[CountryOfOrigin]
       --,[DeliveryConfirmation]
       --,[Warehouse]
       --,[URL] 
FROM    [dbo].[VwLogisticsProviderImport] V

--      WHERE v.[PrimaryID] IN (796326,797082,793864,794858,793865,794732,793866)
GROUP BY [DistributorID]
       ,[CustomerNumber]
       ,[PrimaryID]
       ,[BillAddress1]
       ,[BillAddress2]
       ,[BillCity]
       ,[Billcountry]
       ,[BillFirst]
       ,[BillLast]
       ,[BillCompany]
       ,[BillZip]
       ,[BillState]
       ,[BillPhone]
       ,[BillEmail]
       ,[ShipAddress1]
       ,[ShipAddress2]
       ,[ShipCity]
       ,[ShipCountry]
       ,[ShipFirst]
       ,[ShipLast]
       ,[ShipCompany]
       ,[ShipZip]
       ,[ShipState]
       ,[ShipPhone]
       ,[ShipEmail]
       ,[ShipType]
       ,[ShipVia]
       ,[BilledShipping]
       ,[Tax]
       ,[DeclaredValue]
       ,[TotalSale]
       ,[DateofSale]
       ,[MESSAGE]
       --,[Product]
       --,[Quantity]
       --,[Weight]
       --,[MerchandiseDescription]
       --,[Price]
       ,[PaymentMethod]
       ,[AutoShip]
      -- ,[Title]
       ,[Residential]
       


GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jordan Gurney
-- Create date: 3/30/2011
-- Description:	Select from VwReadyShipperImport view to select order line item information
-- =============================================
ALTER PROC [dbo].[UspSelectVwLogisticsProviderOrderItemsByOrderID] @OrderID INT
AS
SELECT  [DistributorID]
       ,[CustomerNumber]
       ,[PrimaryID]      
       ,[Product]
       ,[Quantity]
       ,[Weight]
       ,[MerchandiseDescription]
       ,[Price]
       ,[Title]
       ,[CommodityCode]
       ,[PerkPoints]
       ,[CountryOfOrigin]
       ,[DeliveryConfirmation]
       ,[Warehouse]
       ,[URL] 
       ,v.OrderItemID
FROM    [dbo].[VwLogisticsProviderImport] V
WHERE v.[PrimaryID] = @OrderID

GO


SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jordan Gurney
-- Create date: 4/15/2011
-- Description:	Takes a @Values input parameter that takes in a series of records separated by a pipe | 
--				that has 4 different values separated by commas
--				Each Record Contains the following Data:
--			    * OrderShipmentIndex - a sequential number of each order shipment starting at 1
--              * OrderItemID
--				* QuantityShipped
--				* DateShipped
--              * Tracking Number
-- =============================================
-- EXEC UspUpdateOrderShipments 186, 967867, '1,2270680,2,03/03/2011|1,2270681,5,03/03/2011|2,2270682,1,03/04/2011'
ALTER PROCEDURE [dbo].[UspUpdateOrderShipments]
    @ModifiedByUserID INT
   ,@OrderID INT
   ,@Values VARCHAR(MAX)
AS 
    SET NOCOUNT ON ;
--DECLARE @ModifiedByUserID INT = 186
--DECLARE @OrderID INT = 967867
--DECLARE @Values VARCHAR(MAX) = '1,2270680,2,03/03/2011|1,2270681,5,03/03/2011|2,2270682,1,03/04/2011'

    DECLARE @List TABLE
        (
         [Key] VARCHAR(MAX)
        ,OrderShipmentIndex INT
        ,OrderItemID INT
        ,QuantityShipped INT
        ,DateShipped DATETIME2
        ,TrackingNumber VARCHAR(MAX)
        ) ;
    
-- split the pipe-delimited list of comma-delimited values and then split the comma-delimited values for each record.
    WITH    SplitItems
              AS ( SELECT   S.Value AS [Key]
                           ,S2.VALUE
                           ,S3.VALUE AS Quantity
                           ,S4.VALUE AS DateShipped
                           ,S5.VALUE AS TrackingNumber
                           ,Row_Number() OVER ( PARTITION BY S.Position ORDER BY S2.Position ) AS ElementNum
                   FROM     dbo.[UdfSplitString](@Values, '|') AS S
                            OUTER APPLY dbo.[UdfSplitString](S.Value, ',') AS S2
                            OUTER APPLY dbo.[UdfSplitString](S.Value, ',') AS S3
                            OUTER APPLY dbo.[UdfSplitString](S.Value, ',') AS S4
                            OUTER APPLY dbo.[UdfSplitString](S.Value, ',') AS S5
                 )
        INSERT  INTO @List
                ( 
                 [Key]
                ,OrderShipmentIndex
                ,OrderItemID
                ,QuantityShipped
                ,DateShipped
                ,TrackingNumber
                )
                SELECT  [Key]
                       ,MIN(CASE WHEN S.ElementNum = 1 THEN S.Value
                            END) AS ListKey
                       ,MIN(CASE WHEN S.ElementNum = 2 THEN S.Value
                            END) AS ListValue
                       ,MIN(CASE WHEN S.ElementNum = 3 THEN S.Value
                            END) AS Quantity
                       ,MIN(CASE WHEN S.ElementNum = 4 THEN S.Value
                            END) AS DateShipped
                       ,MIN(CASE WHEN S.ElementNum = 4 THEN S.Value
                            END) AS TrackingNumber
                FROM    SplitItems AS S
                GROUP BY [Key]
            
            /* For Debugging
            SELECT * FROM @List
            */
            
-- use the @List table with the split list to create any additional Order Shipments and assign the OrderItems belonging to them.
    DECLARE @Counter INT = 1
       ,@MaxNumber INT = ( SELECT   MAX(OrderShipmentIndex)
                           FROM     @List
                         )
    WHILE @Counter <= @MaxNumber 
        BEGIN
            IF @MaxNumber = 1 
                BEGIN
                    UPDATE  os
                    SET     os.TrackingNumber = ( SELECT    MAX(l.[TrackingNumber])
                                                  FROM      [dbo].[Orders] O
                                                            INNER JOIN [dbo].[OrderCustomers] OC ON oc.OrderID = o.OrderID
                                                            INNER JOIN [dbo].[OrderItems] OI ON oi.OrderCustomerID = oc.OrderCustomerID
                                                            INNER JOIN @List l ON l.[OrderItemID] = oi.OrderItemID
                                                  WHERE     o.OrderID = os.OrderID
                                                )
                           ,os.[DateShippedUTC] = ( SELECT  MAX(l.[DateShipped])
                                                    FROM    [dbo].[Orders] O
                                                            INNER JOIN [dbo].[OrderCustomers] OC ON oc.OrderID = o.OrderID
                                                            INNER JOIN [dbo].[OrderItems] OI ON oi.OrderCustomerID = oc.OrderCustomerID
                                                            INNER JOIN @List l ON l.[OrderItemID] = oi.OrderItemID
                                                    WHERE   o.OrderID = os.OrderID
                                                  )
                    FROM    [dbo].[OrderShipments] OS
                    WHERE   os.[OrderID] = @OrderID
				
                    INSERT  INTO [dbo].[OrderShipmentItems]
                            ( 
                             [OrderShipmentID]
                            ,[OrderItemID]
                            ,[QuantityShipped]
                            )
                            SELECT  os.[OrderShipmentID]
                                   ,l.[OrderItemID]
                                   ,l.[QuantityShipped]
                            FROM    [dbo].[OrderItems] OI
                                    INNER JOIN @List l ON l.OrderItemID = oi.[OrderItemID]
                                    INNER JOIN [dbo].[OrderShipments] OS ON os.[OrderID] = @OrderID
                            WHERE   l.[OrderShipmentIndex] = 1
                END
            ELSE 
                BEGIN
                    DECLARE @ScopeIden INT 
				-- if the shipment was split, insert a new Ordershipment record for each shipment
                    INSERT  INTO [dbo].[OrderShipments]
                            ( 
                             [OrderID]
                            ,[OrderCustomerID]
                            ,[ShippingMethodID]
                            ,[OrderShipmentStatusID]
                            ,[FirstName]
                            ,[LastName]
                            ,[Attention]
                            ,[Name]
                            ,[Address1]
                            ,[Address2]
                            ,[Address3]
                            ,[City]
                            ,[County]
                            ,[State]
                            ,[StateProvinceID]
                            ,[PostalCode]
                            ,[CountryID]
                            ,[Email]
                            ,[DayPhone]
                            ,[EveningPhone]
                            ,[TrackingNumber]
                            ,[TrackingURL]
                            ,[DateShippedUTC]
                            ,[GovernmentReceiptNumber]
                            ,[IsDirectShipment]
                            ,[IsWillCall]
                            ,[ModifiedByUserID]
			        
                            )
                            SELECT  [OrderID]
                                   ,[OrderCustomerID]
                                   ,[ShippingMethodID]
                                   ,[OrderShipmentStatusID]
                                   ,[FirstName]
                                   ,[LastName]
                                   ,[Attention]
                                   ,[Name]
                                   ,[Address1]
                                   ,[Address2]
                                   ,[Address3]
                                   ,[City]
                                   ,[County]
                                   ,[State]
                                   ,[StateProvinceID]
                                   ,[PostalCode]
                                   ,[CountryID]
                                   ,[Email]
                                   ,[DayPhone]
                                   ,[EveningPhone]
                                   ,( SELECT    MAX(l.TrackingNumber)
                                      FROM      @List l
                                      WHERE     l.OrderShipmentIndex = @Counter
                                    )
                                   ,[TrackingURL]
                                   ,( SELECT    MAX(l.[DateShipped])
                                      FROM      @List l
                                      WHERE     l.OrderShipmentIndex = @Counter
                                    )
                                   ,[GovernmentReceiptNumber]
                                   ,[IsDirectShipment]
                                   ,[IsWillCall]
                                   ,@ModifiedByUserID
                            FROM    [dbo].[OrderShipments] OS
                            WHERE   os.[OrderID] = @OrderID
                                    AND os.[OrderShipmentID] = ( SELECT MIN([OrderShipmentID])
                                                                 FROM   [dbo].[OrderShipments]
                                                                 WHERE  [OrderID] = @OrderID
                                                               )
                    SET @ScopeIden = SCOPE_IDENTITY()
                    INSERT  INTO [dbo].[OrderShipmentItems]
                            ( 
                             [OrderShipmentID]
                            ,[OrderItemID]
                            ,[QuantityShipped]
                            )
                            SELECT  @ScopeIden
                                   ,l.[OrderItemID]
                                   ,l.[QuantityShipped]
                            FROM    [dbo].[OrderItems] OI
                                    INNER JOIN @List l ON l.OrderItemID = oi.[OrderItemID]
                            WHERE   l.[OrderShipmentIndex] = @Counter
                END
            SET @Counter = @Counter + 1 
        END	
GO

