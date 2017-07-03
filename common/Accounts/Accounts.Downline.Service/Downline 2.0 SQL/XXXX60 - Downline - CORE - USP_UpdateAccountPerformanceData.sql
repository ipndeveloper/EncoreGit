IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Accounts].[USP_UpdateAccountPerformanceData]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [Accounts].[USP_UpdateAccountPerformanceData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
	  Object name       : Accounts.USP_UpdateAccountPerformanceData

	  Database name     : Core
	  Author name       : Jeremy Lundy
	  Created date      : 5/23/2013
	  Description       : Populates Accounts.AccountPerformanceData.

	  Sample execute    : EXEC Accounts.USP_UpdateAccountPerformanceData 201301
******************************************************************************/
CREATE PROCEDURE [Accounts].[USP_UpdateAccountPerformanceData]
	@PeriodID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Start a duration timer
	DECLARE @StartTime DATETIME = GETDATE()
	DECLARE @Duration CHAR(12)
	DECLARE @RowCount INT

	-- Remove existing Accounts.AccountPerformanceDataPrep table
	IF OBJECT_ID(N'Accounts.AccountPerformanceDataPrep','U') IS NOT NULL
		DROP TABLE Accounts.AccountPerformanceDataPrep

	-- Log status
	SELECT @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Building Accounts.AccountPerformanceDataPrep...', 0, 1, @Duration) WITH NOWAIT;
	
	SELECT
		PeriodId = ISNULL(@PeriodId, 0),
		AccountId = TREE.AccountId,
		SponsorId = TREE.SponsorId,
		TreeLevel = TREE.TreeLevel,
		LeftAnchor = 0,
		RightAnchor = 0,
		NodeNumber = TREE.NodeNumber,
		NodeCount = 0,
		Upline = TREE.Upline,
		CalculatedDateUtc = SYSUTCDATETIME(),
		AccountNumber = a.AccountNumber,
		AccountNumberSortable = i.AccountNumberSortable,
		FirstName = a.FirstName,
		LastName = a.LastName,
		AccountTypeId = a.AccountTypeID,
		AccountStatusId = a.AccountStatusID,
		EmailAddress = a.EmailAddress,
		Birthday = CAST(a.BirthdayUTC AS DATE),
		Address1 = i.Address1,
		City = i.City,
		StateProvinceId = i.StateProvinceID,
		StateAbbreviation = i.StateAbbreviation,
		PostalCode = i.PostalCode,
		CountryId = i.CountryID,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		PhoneNumber = i.PhoneNumber,
		EnrollmentDateUtc = a.EnrollmentDateUTC,
		LastRenewalUtc = a.LastRenewalUTC,
		NextRenewalUtc = a.NextRenewalUTC,
		LastOrderCommissionDateUtc = i.LastOrderCommissionDateUtc,
		NextAutoshipRunDate = i.NextAutoshipRunDate,
		PwsUrl = i.PwsUrl,
		SponsorAccountNumber = i.SponsorAccountNumber,
		SponsorFirstName = i.SponsorFirstName,
		SponsorLastName = i.SponsorLastName,
		EnrollerId = a.EnrollerID,
		EnrollerAccountNumber = i.EnrollerAccountNumber,
		EnrollerFirstName = i.EnrollerFirstName,
		EnrollerLastName = i.EnrollerLastName,
		CareerTitleId = CAST(NULL AS INT),
		PaidAsTitleId = CAST(NULL AS INT),
		IsCommissionQualified = ISNULL(CAST(0 AS BIT), 0),
		PQV = ISNULL(CAST(0 AS MONEY), 0),
		PCV = ISNULL(CAST(0 AS MONEY), 0)
	INTO Accounts.AccountPerformanceDataPrep
	FROM Accounts.tfnGetSponsorHierarchy(@PeriodId) TREE
		JOIN dbo.Accounts a ON TREE.AccountId = a.AccountID
		JOIN Accounts.AccountInfoCache i ON TREE.AccountId = i.AccountID
	OPTION (MAXRECURSION 2000) -- Up to 2000 levels deep

	-- Log status
	SELECT @RowCount = @@ROWCOUNT, @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Inserted %u rows. Building nested sets...', 0, 1, @Duration, @RowCount) WITH NOWAIT;

	DECLARE @HTally TABLE (N INT PRIMARY KEY)
	INSERT @HTally
	SELECT TOP 2000 -- Up to 2000 levels deep
		N = (ROW_NUMBER() OVER (ORDER BY (SELECT NULL))-1)*4+1
	FROM master.sys.all_columns ac1
		CROSS JOIN master.sys.all_columns ac2

	DECLARE @LeftAnchor INT
	;WITH cteNodeCounts AS (
		SELECT
			apd.PeriodId,
			AccountId = CAST(SUBSTRING(apd.Upline, t.N, 4) AS INT), 
			NodeCount = COUNT(*)
		FROM Accounts.AccountPerformanceDataPrep apd,
			@HTally t
		WHERE t.N BETWEEN 1 AND DATALENGTH(apd.Upline)
		GROUP BY apd.PeriodId, SUBSTRING(apd.Upline, t.N, 4)
	)
	UPDATE apd SET
		@LeftAnchor = apd.LeftAnchor = 2 * apd.NodeNumber - apd.TreeLevel,
		apd.RightAnchor = (nc.NodeCount - 1) * 2 + @LeftAnchor + 1,
		apd.NodeCount = nc.NodeCount
	FROM Accounts.AccountPerformanceDataPrep apd
		JOIN cteNodeCounts nc ON apd.PeriodId = nc.PeriodId AND apd.AccountId = nc.AccountId

	-- Log status
	SELECT @RowCount = @@ROWCOUNT, @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Updated %u rows. Building indexes...', 0, 1, @Duration, @RowCount) WITH NOWAIT;

	ALTER TABLE Accounts.AccountPerformanceDataPrep
		ADD CONSTRAINT PK_AccountPerformanceDataPrep
		PRIMARY KEY CLUSTERED (PeriodId, LeftAnchor, RightAnchor) WITH FILLFACTOR = 100
		ON psPeriodMonths(PeriodId)
	CREATE UNIQUE INDEX IX_AccountPerformanceDataPrep_PeriodId_AccountId
		ON Accounts.AccountPerformanceDataPrep (PeriodId, AccountId) WITH FILLFACTOR = 100
		ON psPeriodMonths(PeriodId)

	-- Log status
	SELECT @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Getting commissions data...', 0, 1, @Duration) WITH NOWAIT;

	--DECLARE @CT_TitleTypeId INT = dbo.Commissions_sfnLkpTitleTypeID('CT')
	--DECLARE @PAT_TitleTypeId INT = dbo.Commissions_sfnLkpTitleTypeID('PAT')	
	
	--UPDATE apd SET
	--	CareerTitleId = ct.TitleId,
	--	PaidAsTitleId = pat.TitleId,
	--	IsCommissionQualified = ISNULL(cq.Value, 0),
	--	PQV = ISNULL(pqv.Value, 0),
	--	PCV = ISNULL(pcv.Value, 0)
	--FROM Accounts.AccountPerformanceDataPrep apd
	--	OUTER APPLY dbo.Commissions_tfnGetTitleByAccount(apd.AccountID, apd.PeriodId, @CT_TitleTypeId) ct
	--	OUTER APPLY dbo.Commissions_tfnGetTitleByAccount(apd.AccountID, apd.PeriodId, @PAT_TitleTypeId) pat
	--	OUTER APPLY dbo.Commissions_tfnGetCalculationByCode(a.AccountID, apd.PeriodId, 'PQV') pqv
	--	OUTER APPLY dbo.Commissions_tfnGetCalculationByCode(a.AccountID, apd.PeriodId, 'PCV') pcv
	--	OUTER APPLY dbo.Commissions_tfnGetCalculationByCode(a.AccountID, apd.PeriodId, 'CQ') cq

	-- Log status
	SELECT @RowCount = @@ROWCOUNT, @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Updated %u rows. Preparing target tables...', 0, 1, @Duration, @RowCount) WITH NOWAIT;

	-- Ensure Accounts.AccountPerformanceData exists
	IF OBJECT_ID(N'Accounts.AccountPerformanceData','U') IS NULL
	BEGIN
		-- Copy table schema only
		SELECT TOP 0 *
			INTO Accounts.AccountPerformanceData
			FROM Accounts.AccountPerformanceDataPrep
		-- Same indexes as prep
		ALTER TABLE Accounts.AccountPerformanceData
			ADD CONSTRAINT PK_AccountPerformanceData
			PRIMARY KEY CLUSTERED (PeriodId, LeftAnchor, RightAnchor) WITH FILLFACTOR = 100
			ON psPeriodMonths(PeriodId)
		CREATE UNIQUE INDEX IX_AccountPerformanceData_PeriodId_AccountId
			ON Accounts.AccountPerformanceData (PeriodId, AccountId) WITH FILLFACTOR = 100
			ON psPeriodMonths(PeriodId)
	END

	-- Ensure Accounts.AccountPerformanceDataDiscard doesn't exist
	IF OBJECT_ID(N'Accounts.AccountPerformanceDataDiscard','U') IS NOT NULL
		DROP TABLE Accounts.AccountPerformanceDataDiscard
	
	-- Create Accounts.AccountPerformanceDataDiscard
	SELECT TOP 0 *
		INTO Accounts.AccountPerformanceDataDiscard
		FROM Accounts.AccountPerformanceDataPrep
	-- Same clustered index as prep, but no other indexes
	ALTER TABLE Accounts.AccountPerformanceDataDiscard
		ADD CONSTRAINT PK_AccountPerformanceDataDiscard
		PRIMARY KEY CLUSTERED (PeriodId, LeftAnchor, RightAnchor) WITH FILLFACTOR = 100
		ON psPeriodMonths(PeriodId)

	-- Log status
	SELECT @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Switching data into Accounts.AccountPerformanceData...', 0, 1, @Duration) WITH NOWAIT;

	BEGIN TRY
		BEGIN TRAN
			-- Out with the old...
			ALTER TABLE Accounts.AccountPerformanceData SWITCH PARTITION $PARTITION.pfPeriodMonths(@PeriodID)
				TO Accounts.AccountPerformanceDataDiscard PARTITION $PARTITION.pfPeriodMonths(@PeriodID)
			-- ...in with the new
			ALTER TABLE Accounts.AccountPerformanceDataPrep SWITCH PARTITION $PARTITION.pfPeriodMonths(@PeriodID)
				TO Accounts.AccountPerformanceData PARTITION $PARTITION.pfPeriodMonths(@PeriodID)
		COMMIT
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK
		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT
			@ErrMsg = CONVERT(CHAR(12), GETDATE() - @StartTime, 114) + ' ' + ERROR_MESSAGE(),
			@ErrSeverity = ERROR_SEVERITY()
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH

	-- Log status
	SELECT @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Removing prep and discard tables...', 0, 1, @Duration) WITH NOWAIT;
	
	DROP TABLE Accounts.AccountPerformanceDataPrep
	DROP TABLE Accounts.AccountPerformanceDataDiscard

	-- Log status
	SELECT @Duration = CONVERT(CHAR(12), GETDATE() - @StartTime, 114);
	RAISERROR('%s Finished.', 0, 1, @Duration) WITH NOWAIT;
END
