GO
/****** Object:  StoredProcedure [dbo].[usp_get_audit_trail]    Script Date: 07/06/2010 12:13:50 ******/
-- =============================================
-- Author:		John Egbert
-- Create date: 07-06-2010
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_get_audit_trail] 
	@tableName nvarchar(255),
	@pk int,
	@pageIndex int = 0,
	@pageSize int = 25
AS 
BEGIN 
    SET NOCOUNT ON 
  
  SELECT TOP (@pageSize) *
  FROM (
    SELECT act.Name AS ChangeType, at.Name AS TableName, 
	a.PK, atc.ColumnName,

	CASE 
		 WHEN atc.ColumnName = 'AccountStatusChangeReasonID' THEN ISNULL((SELECT Name FROM AccountStatusChangeReasons WHERE AccountStatusChangeReasonID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'AccountStatusID' THEN ISNULL((SELECT Name FROM AccountStatuses WHERE AccountStatusID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'AccountTypeID' THEN ISNULL((SELECT Name FROM AccountTypes WHERE AccountTypeID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'GenderID' THEN ISNULL((SELECT Name FROM Genders WHERE GenderID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'DefaultLanguageID' THEN ISNULL((SELECT Name FROM Languages WHERE LanguageID = a.OldValue), '') 
		 
		 WHEN atc.ColumnName = 'OrderStatusID' THEN ISNULL((SELECT Name FROM OrderStatuses WHERE OrderStatusID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'OrderTypeID' THEN ISNULL((SELECT Name FROM OrderTypes WHERE OrderTypeID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'CurrencyID' THEN ISNULL((SELECT Name FROM Currencies WHERE CurrencyID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'ReturnTypeID' THEN ISNULL((SELECT Name FROM ReturnTypes WHERE ReturnTypeID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'ReturnReasonID' THEN ISNULL((SELECT Name FROM ReturnReasons WHERE ReturnReasonID = a.OldValue), '') 
		 
		 WHEN atc.ColumnName = 'CreatedByUserID' THEN ISNULL((SELECT Username FROM Users WHERE UserID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'ModifiedByUserID' THEN ISNULL((SELECT Username FROM Users WHERE UserID = a.OldValue), '') 
		 WHEN atc.ColumnName = 'ModifiedByApplicationID' THEN ISNULL((SELECT Name FROM Applications WHERE ApplicationID = a.OldValue), '')
		 
		 ELSE a.OldValue
	END AS OldValue,
	CASE 
		 WHEN atc.ColumnName = 'AccountStatusChangeReasonID' THEN ISNULL((SELECT Name FROM AccountStatusChangeReasons WHERE AccountStatusChangeReasonID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'AccountStatusID' THEN ISNULL((SELECT Name FROM AccountStatuses WHERE AccountStatusID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'AccountTypeID' THEN ISNULL((SELECT Name FROM AccountTypes WHERE AccountTypeID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'GenderID' THEN ISNULL((SELECT Name FROM Genders WHERE GenderID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'DefaultLanguageID' THEN ISNULL((SELECT Name FROM Languages WHERE LanguageID = a.NewValue), '') 
		 
		 WHEN atc.ColumnName = 'OrderStatusID' THEN ISNULL((SELECT Name FROM OrderStatuses WHERE OrderStatusID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'OrderTypeID' THEN ISNULL((SELECT Name FROM OrderTypes WHERE OrderTypeID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'CurrencyID' THEN ISNULL((SELECT Name FROM Currencies WHERE CurrencyID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'ReturnTypeID' THEN ISNULL((SELECT Name FROM ReturnTypes WHERE ReturnTypeID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'ReturnReasonID' THEN ISNULL((SELECT Name FROM ReturnReasons WHERE ReturnReasonID = a.NewValue), '') 
		 
		 WHEN atc.ColumnName = 'CreatedByUserID' THEN ISNULL((SELECT Username FROM Users WHERE UserID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'ModifiedByUserID' THEN ISNULL((SELECT Username FROM Users WHERE UserID = a.NewValue), '') 
		 WHEN atc.ColumnName = 'ModifiedByApplicationID' THEN ISNULL((SELECT Name FROM Applications WHERE ApplicationID = a.NewValue), '')
		 ELSE a.NewValue
	END AS NewValue,

	a.DateChanged, u.Username, asu.Name AS SqlUserName, am.Name AS MachineName, ap.Name AS ApplicationName,
	row_number() over(Order by a.DateChanged) as [row_number] 
	FROM AuditLogs a
	JOIN AuditTables at ON at.AuditTableID = a.AuditTableID
	JOIN AuditChangeTypes act ON act.AuditChangeTypeID = a.AuditChangeTypeID
	JOIN AuditMachineNames am ON a.AuditMachineNameID = am.AuditMachineNameID
	JOIN AuditSqlUserNames asu ON a.AuditSqlUserNameID = asu.AuditSqlUserNameID
	JOIN AuditTableColumns atc ON a.AuditTableColumnID = atc.AuditTableColumnID 
	LEFT JOIN Users u ON u.UserID = a.UserID
	LEFT JOIN Applications ap ON a.ApplicationID = ap.ApplicationID
	WHERE at.Name = @tableName AND a.PK = @pk
	) a
	WHERE a.row_number > (@pageIndex * @pageIndex)
END