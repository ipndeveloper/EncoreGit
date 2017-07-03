
-- This is not finished yet - JHE

-- Generic get audited changes on all audited tables - JHE
select act.Name as ChangeType, at.Name as TableName, 
a.PK, atc.ColumnName, a.OldValue, a.NewValue, 
a.DateChanged, u.Username, asu.Name as SqlUserName, am.Name as MachineName, ap.Name as ApplicationName
from AuditLogs a
join AuditTables at on at.AuditTableID = a.AuditTableID
join AuditChangeTypes act on act.AuditChangeTypeID = a.AuditChangeTypeID
join AuditMachineNames am on a.AuditMachineNameID = am.AuditMachineNameID
join AuditSqlUserNames asu on a.AuditSqlUserNameID = asu.AuditSqlUserNameID
join AuditTableColumns atc on a.AuditTableColumnID = atc.AuditTableColumnID 
left join Users u on u.UserID = a.UserID
left join Applications ap on a.ApplicationID = ap.ApplicationID




-- Account Changes - JHE
SELECT act.Name AS ChangeType, at.Name AS TableName, 
a.PK, atc.ColumnName,

CASE 
     WHEN atc.ColumnName = 'AccountStatusChangeReasonID' THEN ISNULL((SELECT Name FROM AccountStatusChangeReasons WHERE AccountStatusChangeReasonID = a.OldValue), '') 
     WHEN atc.ColumnName = 'AccountStatusID' THEN ISNULL((SELECT Name FROM AccountStatuses WHERE AccountStatusID = a.OldValue), '') 
     WHEN atc.ColumnName = 'AccountTypeID' THEN ISNULL((SELECT Name FROM AccountTypes WHERE AccountTypeID = a.OldValue), '') 
     WHEN atc.ColumnName = 'GenderID' THEN ISNULL((SELECT Name FROM Genders WHERE GenderID = a.OldValue), '') 
     WHEN atc.ColumnName = 'DefaultLanguageID' THEN ISNULL((SELECT Name FROM Languages WHERE LanguageID = a.OldValue), '') 
     ELSE a.OldValue
END AS OldValue,
CASE 
     WHEN atc.ColumnName = 'AccountStatusChangeReasonID' THEN ISNULL((SELECT Name FROM AccountStatusChangeReasons WHERE AccountStatusChangeReasonID = a.NewValue), '') 
     WHEN atc.ColumnName = 'AccountStatusID' THEN ISNULL((SELECT Name FROM AccountStatuses WHERE AccountStatusID = a.NewValue), '') 
     WHEN atc.ColumnName = 'AccountTypeID' THEN ISNULL((SELECT Name FROM AccountTypes WHERE AccountTypeID = a.NewValue), '') 
     WHEN atc.ColumnName = 'GenderID' THEN ISNULL((SELECT Name FROM Genders WHERE GenderID = a.NewValue), '') 
     WHEN atc.ColumnName = 'DefaultLanguageID' THEN ISNULL((SELECT Name FROM Languages WHERE LanguageID = a.NewValue), '') 
     ELSE a.NewValue
END AS NewValue,

a.DateChanged, u.Username, asu.Name AS SqlUserName, am.Name AS MachineName, ap.Name AS ApplicationName
FROM AuditLogs a
JOIN AuditTables at ON at.AuditTableID = a.AuditTableID
JOIN AuditChangeTypes act ON act.AuditChangeTypeID = a.AuditChangeTypeID
JOIN AuditMachineNames am ON a.AuditMachineNameID = am.AuditMachineNameID
JOIN AuditSqlUserNames asu ON a.AuditSqlUserNameID = asu.AuditSqlUserNameID
JOIN AuditTableColumns atc ON a.AuditTableColumnID = atc.AuditTableColumnID 
LEFT JOIN Users u ON u.UserID = a.UserID
LEFT JOIN Applications ap ON a.ApplicationID = ap.ApplicationID
WHERE at.Name = 'Accounts' AND a.PK = 9



-- Order Changes - JHE
SELECT act.Name AS ChangeType, at.Name AS TableName, 
a.PK, atc.ColumnName,

CASE 
     WHEN atc.ColumnName = 'OrderStatusID' THEN ISNULL((SELECT Name FROM OrderStatuses WHERE OrderStatusID = a.OldValue), '') 
     WHEN atc.ColumnName = 'OrderTypeID' THEN ISNULL((SELECT Name FROM OrderTypes WHERE OrderTypeID = a.OldValue), '') 
     WHEN atc.ColumnName = 'CurrencyID' THEN ISNULL((SELECT Name FROM Currencies WHERE CurrencyID = a.OldValue), '') 
     WHEN atc.ColumnName = 'ReturnTypeID' THEN ISNULL((SELECT Name FROM ReturnTypes WHERE ReturnTypeID = a.OldValue), '') 
     WHEN atc.ColumnName = 'ReturnReasonID' THEN ISNULL((SELECT Name FROM ReturnReasons WHERE ReturnReasonID = a.OldValue), '') 
     ELSE a.OldValue
END AS OldValue,
CASE 
     
     WHEN atc.ColumnName = 'OrderStatusID' THEN ISNULL((SELECT Name FROM OrderStatuses WHERE OrderStatusID = a.NewValue), '') 
     WHEN atc.ColumnName = 'OrderTypeID' THEN ISNULL((SELECT Name FROM OrderTypes WHERE OrderTypeID = a.NewValue), '') 
     WHEN atc.ColumnName = 'CurrencyID' THEN ISNULL((SELECT Name FROM Currencies WHERE CurrencyID = a.NewValue), '') 
     WHEN atc.ColumnName = 'ReturnTypeID' THEN ISNULL((SELECT Name FROM ReturnTypes WHERE ReturnTypeID = a.NewValue), '') 
     WHEN atc.ColumnName = 'ReturnReasonID' THEN ISNULL((SELECT Name FROM ReturnReasons WHERE ReturnReasonID = a.NewValue), '') 
     ELSE a.NewValue
END AS NewValue,

a.DateChanged, u.Username, asu.Name AS SqlUserName, am.Name AS MachineName, ap.Name AS ApplicationName
FROM AuditLogs a
JOIN AuditTables at ON at.AuditTableID = a.AuditTableID
JOIN AuditChangeTypes act ON act.AuditChangeTypeID = a.AuditChangeTypeID
JOIN AuditMachineNames am ON a.AuditMachineNameID = am.AuditMachineNameID
JOIN AuditSqlUserNames asu ON a.AuditSqlUserNameID = asu.AuditSqlUserNameID
JOIN AuditTableColumns atc ON a.AuditTableColumnID = atc.AuditTableColumnID 
LEFT JOIN Users u ON u.UserID = a.UserID
LEFT JOIN Applications ap ON a.ApplicationID = ap.ApplicationID
WHERE at.Name = 'Orders' AND a.PK = 9

