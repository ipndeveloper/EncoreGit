IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Agreements')
BEGIN
	EXEC('CREATE SCHEMA [Agreements] AUTHORIZATION [GO]');
END

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agreements].[AgreementKinds]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Agreements].[AgreementKinds](
		[AgreementKindID] [int] IDENTITY(1,1) NOT NULL,
		[TermName] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_AgreementKinds] PRIMARY KEY CLUSTERED 
	(
		[AgreementKindID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agreements].[Agreements]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Agreements].[Agreements](
		[AgreementID] [int] IDENTITY(1,1) NOT NULL,
		[TermName] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_Agreements] PRIMARY KEY CLUSTERED 
	(
		[AgreementID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agreements].[AgreementsToAgreementKinds]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Agreements].[AgreementsToAgreementKinds](
		[AgreementID] [int] NOT NULL,
		[AgreementKindID] [int] NOT NULL,
	 CONSTRAINT [PK_AgreementsToAgreementKinds] PRIMARY KEY CLUSTERED 
	(
		[AgreementID] ASC,
		[AgreementKindID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [Agreements].[AgreementsToAgreementKinds]  WITH CHECK ADD  CONSTRAINT [FK_AgreementsToAgreementKinds_Agreements] FOREIGN KEY([AgreementID])
	REFERENCES [Agreements].[Agreements] ([AgreementID])

	ALTER TABLE [Agreements].[AgreementsToAgreementKinds] CHECK CONSTRAINT [FK_AgreementsToAgreementKinds_Agreements]
	
	ALTER TABLE [Agreements].[AgreementsToAgreementKinds]  WITH CHECK ADD  CONSTRAINT [FK_AgreementsToAgreementKinds_AgreementKinds] FOREIGN KEY([AgreementKindID])
	REFERENCES [Agreements].[AgreementKinds] ([AgreementKindID])

	ALTER TABLE [Agreements].[AgreementsToAgreementKinds] CHECK CONSTRAINT [FK_AgreementsToAgreementKinds_AgreementKinds]
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agreements].[AgreementsToAccountKinds]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Agreements].[AgreementsToAccountKinds](
		[AgreementID] [int] NOT NULL,
		[AccountKindID] [int] NOT NULL,
	 CONSTRAINT [PK_AgreementsToAccountKinds] PRIMARY KEY CLUSTERED 
	(
		[AgreementID] ASC,
		[AccountKindID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [Agreements].[AgreementsToAccountKinds]  WITH CHECK ADD  CONSTRAINT [FK_AgreementsToAccountKinds_Agreements] FOREIGN KEY([AgreementID])
	REFERENCES [Agreements].[Agreements] ([AgreementID])

	ALTER TABLE [Agreements].[AgreementsToAccountKinds] CHECK CONSTRAINT [FK_AgreementsToAccountKinds_Agreements]
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agreements].[AgreementVersions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Agreements].[AgreementVersions](
		[AgreementVersionID] [int] IDENTITY(1,1) NOT NULL,
		[AgreementID] [int] NOT NULL,
		[VersionNumber] [varchar](10) NOT NULL,
		[DateReleasedUTC] [datetime] NOT NULL,
		[FilePath] [varchar](250) NULL,
		[AgreementText] [nvarchar](MAX) NULL,
	 CONSTRAINT [PK_AgreementVersions_1] PRIMARY KEY CLUSTERED 
	(
		[AgreementVersionID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [Agreements].[AgreementVersions]  WITH CHECK ADD  CONSTRAINT [FK_AgreementVersions_Agreements] FOREIGN KEY([AgreementID])
	REFERENCES [Agreements].[Agreements] ([AgreementID])

	ALTER TABLE [Agreements].[AgreementVersions] CHECK CONSTRAINT [FK_AgreementVersions_Agreements]
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agreements].[AgreementAcceptanceLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Agreements].[AgreementAcceptanceLog](
		[AccountID] [int] NOT NULL,
		[AgreementVersionID] [int] NOT NULL,
		[DateAcceptedUTC] [datetime] NOT NULL,
	 CONSTRAINT [PK_AgreementAcceptanceLog] PRIMARY KEY CLUSTERED 
	(
		[AccountID] ASC,
		[AgreementVersionID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [Agreements].[AgreementAcceptanceLog]  WITH CHECK ADD  CONSTRAINT [FK_AgreementAcceptanceLog_Accounts] FOREIGN KEY([AccountID])
	REFERENCES [dbo].[Accounts] ([AccountID])

	ALTER TABLE [Agreements].[AgreementAcceptanceLog] CHECK CONSTRAINT [FK_AgreementAcceptanceLog_Accounts]

	ALTER TABLE [Agreements].[AgreementAcceptanceLog]  WITH CHECK ADD  CONSTRAINT [FK_AgreementAcceptanceLog_AgreementVersions] FOREIGN KEY([AgreementVersionID])
	REFERENCES [Agreements].[AgreementVersions] ([AgreementVersionID])

	ALTER TABLE [Agreements].[AgreementAcceptanceLog] CHECK CONSTRAINT [FK_AgreementAcceptanceLog_AgreementVersions]
END