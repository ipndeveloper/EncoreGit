IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Accounts].[tfnGetSponsorHierarchy]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [Accounts].[tfnGetSponsorHierarchy]
GO--GO
/******************************************************************************
	Author:			Jeremy Lundy
	Create date:	4/20/2013
	Description:	Gets sponsor hierarchy (complete downline) by period
	
	Always use the following MAXRECURSION option when calling this function:
	OPTION (MAXRECURSION 2000) -- Up to 2000 levels deep
******************************************************************************/
CREATE FUNCTION [Accounts].[tfnGetSponsorHierarchy]
(	
	@PeriodId int = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	WITH ctePeriod AS (
		SELECT EndDateUtc = ISNULL((SELECT EndDateUtc FROM dbo.Commissions_Periods (NOLOCK) WHERE PeriodId = @PeriodId), SYSUTCDATETIME())
	),
	cteBuildAccounts AS (
		SELECT TOP 1
			SP.AccountId,
			SP.SponsorId,
			TreeLevel = 1,
			Upline = CAST(CAST(SP.AccountId AS BINARY(4)) AS VARBINARY(8000)) -- Up to 2000 levels deep
		FROM Accounts.AccountHistory SP
		WHERE SP.SponsorId IS NULL -- The root node is the first distributor account with a NULL SponsorId
			AND SP.AccountTypeId = 1 -- Distributor
		ORDER BY SP.AccountId
		UNION ALL
		SELECT
			DN.AccountId,
			DN.SponsorId,
			TreeLevel = cte.TreeLevel + 1,
			Upline = CAST(cte.Upline + CAST(DN.AccountId AS BINARY(4)) AS VARBINARY(8000)) -- Up to 2000 levels deep
		FROM cteBuildAccounts cte
			CROSS APPLY Accounts.tfnGetAccountSponsorships(cte.AccountId, (SELECT EndDateUtc FROM ctePeriod)) DN
		WHERE DN.AccountTypeId IN (1,2) -- Distributor & Preferred Customer
			AND DN.AccountStatusId <> 3 -- Begun Enrollment
	)
	SELECT
		AccountId = ISNULL(cte.AccountId, 0),
		cte.SponsorId,
		TreeLevel = ISNULL(cte.TreeLevel, 0),
		NodeNumber = ISNULL(CAST(ROW_NUMBER() OVER (ORDER BY cte.Upline) AS INT), 0),
		Upline = ISNULL(cte.Upline, 0)
	FROM cteBuildAccounts cte
)
