
-- Remove tables first to create schema
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND type in (N'U'))
DROP TABLE [dbo].[AuditLogs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditTables]') AND type in (N'U'))
DROP TABLE [dbo].[AuditTables]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditMachineNames]') AND type in (N'U'))
DROP TABLE [dbo].[AuditMachineNames]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditSqlUserNames]') AND type in (N'U'))
DROP TABLE [dbo].[AuditSqlUserNames]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditTableColumns]') AND type in (N'U'))
DROP TABLE [dbo].[AuditTableColumns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditChangeTypes]') AND type in (N'U'))
DROP TABLE [dbo].[AuditChangeTypes]
GO






if dbo.ObjectExists('ta', 'AuditLogs', '') = 0
BEGIN
	CREATE TABLE [dbo].[AuditLogs](
		[AuditLogID] [bigint] IDENTITY(1,1) NOT NULL,
		[AuditTableID] [smallint] NOT NULL,
		[AuditChangeTypeID] [tinyint] NOT NULL,
		[AuditMachineNameID] [smallint] NOT NULL,
		[AuditSqlUserNameID] [smallint] NOT NULL,
		[AuditTableColumnID] [smallint] NOT NULL,
		[ApplicationID] [smallint] NOT NULL,
		[UserID] [int] NULL,
		[PK] [int] NOT NULL,
		[PKs] [varchar](255) NULL,
		[OldValue] [nvarchar](4000) NULL,
		[NewValue] [nvarchar](4000) NULL,
		[DateChanged] [datetime] NOT NULL,
	 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED 
	(
		[AuditLogID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


if dbo.ObjectExists('ta', 'AuditTables', '') = 0
BEGIN
	CREATE TABLE [dbo].[AuditTables](
		[AuditTableID] [smallint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_AuditTables] PRIMARY KEY CLUSTERED 
	(
		[AuditTableID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


if dbo.ObjectExists('ta', 'AuditMachineNames', '') = 0
BEGIN
	CREATE TABLE [dbo].[AuditMachineNames](
		[AuditMachineNameID] [smallint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_AuditMachineNames] PRIMARY KEY CLUSTERED 
	(
		[AuditMachineNameID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


if dbo.ObjectExists('ta', 'AuditSqlUserNames', '') = 0
BEGIN
	CREATE TABLE [dbo].[AuditSqlUserNames](
		[AuditSqlUserNameID] [smallint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_AuditSqlUserNames] PRIMARY KEY CLUSTERED 
	(
		[AuditSqlUserNameID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


if dbo.ObjectExists('ta', 'AuditTableColumns', '') = 0
BEGIN
	CREATE TABLE [dbo].[AuditTableColumns](
		[AuditTableColumnID] [smallint] IDENTITY(1,1) NOT NULL,
		[AuditTableID] [smallint] NOT NULL,
		[ColumnName] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_AuditTableColumns] PRIMARY KEY CLUSTERED 
	(
		[AuditTableColumnID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


if dbo.ObjectExists('ta', 'Applications', '') = 0
BEGIN
	CREATE TABLE [dbo].[Applications](
		[ApplicationID] [smallint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Name] [nchar](100) NOT NULL,
		[Description] [nvarchar](255) NULL,
		[Active] [bit] NOT NULL,
	 CONSTRAINT [PK_ApplicationSources] PRIMARY KEY CLUSTERED 
	(
		[ApplicationID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[Applications] ADD  CONSTRAINT [DF_Applications_Active]  DEFAULT ((1)) FOR [Active]
END


if dbo.ObjectExists('ta', 'AuditChangeTypes', '') = 0
BEGIN
	CREATE TABLE [dbo].[AuditChangeTypes](
		[AuditChangeTypeID] [tinyint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_AuditChangeTypes] PRIMARY KEY CLUSTERED 
	(
		[AuditChangeTypeID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

truncate table AuditChangeTypes

IF NOT EXISTS (SELECT * FROM AuditChangeTypes WHERE Name = 'Insert')
BEGIN
	INSERT INTO AuditChangeTypes (Name) VALUES ('Insert')
END
IF NOT EXISTS (SELECT * FROM AuditChangeTypes WHERE Name = 'Update')
BEGIN
	INSERT INTO AuditChangeTypes (Name) VALUES ('Update')
END
IF NOT EXISTS (SELECT * FROM AuditChangeTypes WHERE Name = 'Delete')
BEGIN
	INSERT INTO AuditChangeTypes (Name) VALUES ('Delete')
END
