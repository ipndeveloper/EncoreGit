--*******************************************************************************
-- Table of Contents - Start
--*******************************************************************************
-- [dbo].[usp_get_downline]
-- 
--*******************************************************************************
-- Table of Contents - End
--*******************************************************************************

--*******************************************************************************
-- [dbo].[usp_get_downline]
--*******************************************************************************

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_get_downline]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_get_downline]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_downline]
	@PeriodID INT,
	@PlanID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--DECLARE @PeriodID INT;
	--SELECT TOP 1 @PeriodID = PeriodID FROM Periods
	
	DECLARE @OpenPeriodID INT;
	EXEC @OpenPeriodID = dbo.udf_get_open_period;
	
	DECLARE @Locations TABLE
	(
		AccountID INT NOT NULL,
		City NVARCHAR(200) NULL,
		[State] NVARCHAR(100) NULL
	)
	INSERT INTO @Locations 
		SELECT AccountID, City, [State] 
			FROM Accounts a 
			OUTER APPLY udf_get_account_address(a.AccountID, 1, 1)

	SELECT DISTINCT a.AccountID,
		CAST(1 AS INT) AS Level,			 -- Added to generate properties for calculated values on the C# side - JHE
		CAST(1 AS INT) AS FlatDownlineCount, -- Added to generate properties for calculated values on the C# side - JHE
		AccountNumber,
		FirstName,
		LastName,
		EmailAddress,
		pat.TitleID AS PaidAsTitle,
		ct.TitleID AS CurrentTitle,
		pv.Value AS PV,
		gv.Value AS GV,
		EnrollmentDate,
		SponsorID,
		CAST((l.City + ', ' + l.State) AS NVARCHAR(1000)) AS Location,
		AccountTypeID
		--(SELECT City + ', ' + [State] FROM udf_get_account_address(a.AccountID, 1, 1)) AS Location
	FROM Accounts a	
	
	-- I replace the code below(to get the max Title to rediuce rows returned - JHE
	--LEFT OUTER JOIN AccountTitles pat
	--	ON pat.AccountID = a.AccountID AND pat.PeriodID = @PeriodID	
	LEFT OUTER JOIN (SELECT AccountID, PeriodID, MAX(TitleID) FROM AccountTitles GROUP BY AccountID, PeriodID) pat (AccountID, PeriodID, TitleID)
		ON pat.AccountID = a.AccountID AND pat.PeriodID = @PeriodID 
				
	-- I replace the code below(to get the max Title to rediuce rows returned - JHE
	--LEFT OUTER JOIN AccountTitles ct
	--	ON ct.AccountID = a.AccountID AND ct.PeriodID = @OpenPeriodID
	LEFT OUTER JOIN (SELECT AccountID, PeriodID, MAX(TitleID) FROM AccountTitles GROUP BY AccountID, PeriodID) ct (AccountID, PeriodID, TitleID)
		ON ct.AccountID = a.AccountID AND ct.PeriodID = @OpenPeriodID 
		
	LEFT OUTER JOIN Calculations pv
		ON pv.AccountID = a.AccountID AND pv.PeriodID = @PeriodID AND pv.CalculationTypeID = 1 --Sales Volume
	LEFT OUTER JOIN Calculations gv
		ON gv.AccountID = a.AccountID AND gv.PeriodID = @PeriodID AND gv.CalculationTypeID = 11 -- Personal Team Volume
	LEFT OUTER JOIN @Locations l
		ON l.AccountID = a.AccountID
	WHERE a.AccountTypeID = 1 
	AND a.Active = 1 -- TODO: Change this to use the active flag - JHE
END

GO

--*******************************************************************************
-- 
--*******************************************************************************

GO