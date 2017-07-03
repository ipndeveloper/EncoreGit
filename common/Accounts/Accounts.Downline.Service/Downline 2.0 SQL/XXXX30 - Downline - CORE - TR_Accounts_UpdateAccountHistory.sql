IF OBJECT_ID(N'Accounts.AccountHistory','U') IS NULL
BEGIN
	CREATE TABLE Accounts.AccountHistory (
		[AccountId] INT NOT NULL,
		[EffectiveDateUtc] DATETIME2(7) NOT NULL,
		[AccountTypeId] SMALLINT NOT NULL,
		[AccountStatusId] SMALLINT NOT NULL,
		[SponsorId] INT NULL,
		[EnrollerId] INT NULL,
		CONSTRAINT [PK_AccountHistory] PRIMARY KEY CLUSTERED([AccountId] ASC, [EffectiveDateUtc] ASC)
		ON [PRIMARY]
	) ON [PRIMARY]

	-- This is a pretty basic way to populate the table. Probably needs improvement.
	DECLARE @EffectiveDateUtc datetime2(7) = '00010101'
	INSERT Accounts.AccountHistory (
		AccountId,
		EffectiveDateUtc,
		AccountTypeId,
		AccountStatusId,
		SponsorId,
		EnrollerId
	)
	SELECT
		a.AccountId,
		@EffectiveDateUtc,
		a.AccountTypeId,
		a.AccountStatusId,
		a.SponsorId,
		a.EnrollerId
	FROM dbo.Accounts a

	ALTER TABLE [Accounts].[AccountHistory]  WITH CHECK ADD  CONSTRAINT [FK_AccountHistory_AccountStatuses] FOREIGN KEY([AccountStatusId])
	REFERENCES [dbo].[AccountStatuses] ([AccountStatusId])

	ALTER TABLE [Accounts].[AccountHistory]  WITH CHECK ADD  CONSTRAINT [FK_AccountHistory_AccountTypes] FOREIGN KEY([AccountTypeId])
	REFERENCES [dbo].[AccountTypes] ([AccountTypeId])

	CREATE NONCLUSTERED INDEX IX_AccountHistory_SponsorId ON Accounts.AccountHistory
		(SponsorId)
		INCLUDE (AccountTypeId, AccountStatusId)
		ON [FG_INDEX_01]
END
GO--GO

IF OBJECT_ID('[dbo].[TR_Accounts_UpdateAccountHistory]') IS NOT NULL
	DROP TRIGGER [dbo].[TR_Accounts_UpdateAccountHistory]
GO--GO
/******************************************************************************
	Author:			Jeremy Lundy
	Create date:	4/19/2013
	Description:	On INSERT and UPDATE for certain columns, adds a row to
					Accounts.AccountHistory.
******************************************************************************/
CREATE TRIGGER dbo.TR_Accounts_UpdateAccountHistory
	ON dbo.Accounts
	AFTER INSERT, UPDATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (
		UPDATE(AccountTypeId)
		OR UPDATE(AccountStatusId)
		OR UPDATE(SponsorId)
		OR UPDATE(EnrollerId)
	)
	BEGIN
		DECLARE @EffectiveDateUtc datetime2(7) = SYSUTCDATETIME()

		INSERT Accounts.AccountHistory (
			AccountId,
			EffectiveDateUtc,
			AccountTypeId,
			AccountStatusId,
			SponsorId,
			EnrollerId
		)
		SELECT
			i.AccountId,
			@EffectiveDateUtc,
			i.AccountTypeId,
			i.AccountStatusId,
			i.SponsorId,
			i.EnrollerId
		FROM inserted i
	END
END
