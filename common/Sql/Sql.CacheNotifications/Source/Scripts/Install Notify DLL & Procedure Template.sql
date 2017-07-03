/*
	Find & Replace DATABASE_NAME with Actual Database Name
	Find & Replace PATH_TO_DLL with disk location of NetSteps.Sql.CacheNotifications.dll
*/

IF NOT EXISTS(SELECT * FROM sys.configurations WHERE name = 'clr enabled' AND value = 1)
BEGIN
	EXEC SP_CONFIGURE 'clr enabled', 1
	RECONFIGURE
END
GO

DECLARE @isTrustworthy BIT
SELECT @isTrustworthy = is_trustworthy_on FROM sys.databases WHERE name = N'DATABASE_NAME';
IF @isTrustworthy = 0
BEGIN
	ALTER DATABASE [DATABASE_NAME] SET TRUSTWORTHY ON
END
GO

USE [DATABASE_NAME]
GO

/****** Object:  StoredProcedure [dbo].[Notify]    Script Date: 02/15/2013 18:28:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notify]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Notify]
GO

IF  EXISTS (SELECT * FROM sys.assemblies asms WHERE asms.name = N'CacheNotifications' and is_user_defined = 1)
DROP ASSEMBLY [CacheNotifications]
GO

CREATE ASSEMBLY CacheNotifications FROM 'PATH_TO_DLL\NetSteps.Sql.CacheNotifications.dll' WITH PERMISSION_SET = UNSAFE
GO

CREATE PROCEDURE Notify(@recipients NVARCHAR(MAX), @contextKeys NVARCHAR(MAX), @messageKind int, @identityKind int, @ids NVARCHAR(MAX), @listSeperator NVARCHAR(1))  
AS EXTERNAL NAME CacheNotifications.SqlNotifier.Notify
GO

