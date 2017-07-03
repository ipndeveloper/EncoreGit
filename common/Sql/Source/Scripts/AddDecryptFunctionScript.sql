

 DROP FUNCTION nsDecrypt
 DROP FUNCTION nsEncrypt
 DROP FUNCTION ComputeHash
 DROP FUNCTION ComputeMd5Hash


GO
CREATE FUNCTION dbo.nsDecrypt (@value as nvarchar(1000))
RETURNS nvarchar(1000) 
AS EXTERNAL NAME NetStepsSql.[UserDefinedFunctions].nsDecrypt 

GO
CREATE FUNCTION dbo.nsEncrypt (@value as nvarchar(1000))
RETURNS nvarchar(1000) 
AS EXTERNAL NAME NetStepsSql.[UserDefinedFunctions].nsEncrypt 



GO
CREATE FUNCTION dbo.ComputeHash (@value as nvarchar(1000))
RETURNS nvarchar(1000) 
AS EXTERNAL NAME NetStepsSql.[UserDefinedFunctions].ComputeHash 

GO
CREATE FUNCTION dbo.ComputeMd5Hash (@value as nvarchar(1000))
RETURNS nvarchar(1000) 
AS EXTERNAL NAME NetStepsSql.[UserDefinedFunctions].ComputeMd5Hash 


-- Test Example - JHE
SELECT TOP 10 AccountID, TaxNumber, dbo.nsDecrypt(TaxNumber) AS 'DecryptedTaxNumber' 
FROM Accounts WHERE dbo.nsDecrypt(TaxNumber) like '%123%'