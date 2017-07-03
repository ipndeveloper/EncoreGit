IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Accounts].[tfnGetAccountSponsorships]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [Accounts].[tfnGetAccountSponsorships]
GO--GO
/******************************************************************************
	Author:			Jeremy Lundy
	Create date:	4/20/2013
	Description:	Gets Sponsorships (Level 1 Downline) by Date
******************************************************************************/
CREATE FUNCTION [Accounts].[tfnGetAccountSponsorships]
(	
	@SponsorID int,
	@EffectiveDateUTC datetime2 = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		SP.AccountID,
		SP.AccountTypeID,
		SP.AccountStatusID,
		SP.SponsorID
	FROM Accounts.AccountHistory SP
	OUTER APPLY (
		SELECT TOP 1
			_DN.EffectiveDateUTC
		FROM Accounts.AccountHistory _DN
		WHERE _DN.AccountID = SP.AccountID
			AND _DN.EffectiveDateUTC <= ISNULL(@EffectiveDateUTC, SYSUTCDATETIME())
		ORDER BY _DN.EffectiveDateUTC DESC
	) AS DN
	WHERE SP.SponsorID = @SponsorID
		AND SP.EffectiveDateUTC = DN.EffectiveDateUTC
)
