IF NOT EXISTS (SELECT * FROM sys.schemas WHERE NAME = 'ver')
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA [ver] AUTHORIZATION [dbo];'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ver].[SchemaMigrations]') AND type in (N'U'))
BEGIN
CREATE TABLE [ver].[SchemaMigrations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateAppliedUTC] [datetime] NOT NULL,
	[Client] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Succeeded] [BIT] NOT NULL,
	[ErrorMessage] [nvarchar](MAX) NULL
	CONSTRAINT [PK_SchemaMigrations] PRIMARY KEY CLUSTERED ([Id] ASC)
	WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[ver].[DF_SchemaMigrations_DateAppliedUTC]') AND type = 'D')
BEGIN
	ALTER TABLE [ver].[SchemaMigrations] ADD CONSTRAINT [DF_SchemaMigrations_DateAppliedUTC] DEFAULT (getutcdate()) FOR [DateAppliedUTC]
END

GO

CREATE UNIQUE NONCLUSTERED INDEX [uqSchemaMigrations] ON [ver].[SchemaMigrations] 
(
	[Client], [Version], [Code], [Succeeded] ASC
	
)
WHERE Succeeded = 1
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, 
DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

