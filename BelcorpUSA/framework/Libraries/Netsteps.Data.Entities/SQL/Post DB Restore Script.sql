

ALTER DATABASE NSFramework_Test SET ENABLE_BROKER
ALTER DATABASE NSFramework_Test SET NEW_BROKER WITH ROLLBACK IMMEDIATE; 
Select databasepropertyex('NSFramework_Test', 'IsBrokerEnabled')  




GO

/****** Object:  User [siteuser]    Script Date: 10/01/2010 16:10:20 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'siteuser')
DROP USER [siteuser]
GO


CREATE USER [siteuser] FOR LOGIN [siteuser]
GO
USE [NSFramework_Test]
GO
EXEC sp_addrolemember N'db_owner', N'siteuser'
GO


UPDATE ProxyLinks SET URL = 'http://workstation.netstepsdemo.com/Login' WHERE Name ='nsBackOffice'

UPDATE SiteURls SET IsPrimaryUrl = 1 WHERE Url like '%demo%'
UPDATE SiteURls SET IsPrimaryUrl = 0 WHERE Url not like '%demo%'