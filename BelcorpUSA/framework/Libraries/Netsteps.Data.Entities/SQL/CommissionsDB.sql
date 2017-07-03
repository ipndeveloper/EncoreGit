
CREATE PROCEDURE [dbo].[usp_get_account_ledger_balance_by_accountid]
	@AccountID  int		
AS
BEGIN
	SELECT dbo.udf_get_account_ledger_balance(@AccountID, GETDATE(), null) as 'CurrentEndingBalance'	
END