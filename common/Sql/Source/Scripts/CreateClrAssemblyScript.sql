-- UPDATE CLR Assembly Script
sp_configure 'clr enabled', 1
go
reconfigure
go

DROP ASSEMBLY NetStepsSql;
GO
CREATE ASSEMBLY NetStepsSql
FROM 'C:\Development\NS3\Framework\Libraries\NetSteps.Sql\bin\Debug\NetSteps.Sql.dll'
WITH PERMISSION_SET = SAFE;
