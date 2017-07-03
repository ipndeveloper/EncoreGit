
GO
CREATE TRIGGER [dbo].[tr_Insert_Accounts_Set_AccountNumber]
ON [dbo].[Accounts]
FOR INSERT
AS
BEGIN
	UPDATE a
		SET AccountNumber = CASE 
								WHEN i.AccountNumber = '' THEN CONVERT(nvarchar, a.AccountID)
								WHEN i.AccountNumber is null THEN CONVERT(nvarchar, a.AccountID)
								ELSE i.AccountNumber 
							END
	FROM [dbo].[Accounts] a
	JOIN inserted i on a.AccountID = i.AccountID
END


GO
CREATE TRIGGER [dbo].[tr_Insert_Orders_Set_OrderNumber]
ON [dbo].[Orders]
FOR INSERT
AS
BEGIN
	UPDATE a
		SET OrderNumber = CASE 
								WHEN i.OrderNumber = '' THEN CONVERT(nvarchar, a.OrderID)
								WHEN i.OrderNumber is null THEN CONVERT(nvarchar, a.OrderID)
								ELSE i.OrderNumber 
							END
	FROM [dbo].[Orders] a
	JOIN inserted i on a.OrderID = i.OrderID
END