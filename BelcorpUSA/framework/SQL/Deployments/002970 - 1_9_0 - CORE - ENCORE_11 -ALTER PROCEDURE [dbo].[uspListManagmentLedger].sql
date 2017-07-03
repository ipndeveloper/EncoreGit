-- ====================================================
-- **GO STATEMENTS**
-- Plain GO statements will not execute properly!!!!!
-- Please avoid using GO statements, or if you need to
-- use a GO then append a --GO to the end of the GO 
-- statement without any spaces as shown below.
--
-- i.e.   GO--GO
--
-- **TRANSACTIONS**
-- Transactions are NOT Supported! The run once logic
-- will roll back if there is a problem.
--
-- **TEMP TABLES**
-- If you want to use temporary tables please use 
-- global temp tables. 
-- 
-- i.e.   ##
-- ====================================================


USE [BelcorpBRACommissions]
GO
/****** Object:  StoredProcedure [dbo].[uspListManagmentLedger]    Script Date: 25/05/2017 10:07:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[uspListManagmentLedger]
(
@GetAll                    BIT,
@AccountID          INT  ,
@PeriodID           INT = NULL,
@BonusTypeID INT = NULL,
@StartDate          DATE = NULL,
@EndDate            DATE = NULL,
@EntryAmount DECIMAL(8,2)=NULL,
@EntryReasonID      INT = NULL,
@EntryOriginID      INT = NULL,
@EntryTypeID        INT = NULL,
@PageSize           INT,
@PageNumber         INT,
@Colum              VARCHAR(50),
@Order              VARCHAR(4),
@RowsCount          INT=0 OUT
)
AS
BEGIN
	SET NOCOUNT ON;
 
	DECLARE @SQL varchar(max)
 
	IF @Colum = ''
	BEGIN
	 SET @Colum = 'AccountID'
	END
 
 /*  AGREGADO por HUNDRED =>Inicio : 25052017  */
		if (@StartDate IS NULL)
			SET @StartDate ='1752-01-01'
		if (@EndDate IS NULL)
			SET @EndDate =GETDATE()
/*  AGREGADO por HUNDRED =>fin : 25052017  */
 
 
	SELECT
			AL.AccountID,
			ISNULL(AC.FirstName + AC.LastName, 'Actualizar Nombre')					ConsultantName,
			ISNULL(CAST(AL.PeriodId AS VARCHAR),'')									PeriodId,
			ISNULL(BT.[TermName],'')                                                BonusType,
			CAST(ISNULL(AL.EntryAmount,0) AS VARCHAR)								EntryAmount,
			--CAST(ISNULL(AL.EndingBalance,0) AS VARCHAR)								EndingBalance ,
			0 as								EndingBalance ,
			AL.EntryDate ,
			AL.EffectiveDate,
			ISNULL(CAST(AL.UserID AS VARCHAR),'')									UserID,
			ISNULL(AL.[EntryNotes],'')                                              EntryNotes,
			ISNULL(ER.[TermName],'')                                                EntryReason ,
			ISNULL(EO.[TermName],'')                                                EntryOrigin ,
			ISNULL(ET.[TermName],'')                                                EntryType,
			ISNULL(AL.[EntryDescription],'')                                        EntryDescription,
			ISNULL(CAST(AL.DisbursementID AS VARCHAR),'')							DisbursementID,
			ISNULL(DIS.TermName, '')                                                DisbursementStatusTermName
	INTO	#tmpFinal
	FROM	AccountLedger AL INNER JOIN Accounts AC
	ON		AL.AccountID = AC.AccountID INNER JOIN LedgerEntryReasons ER
	ON		AL.[EntryReasonID] = ER.[EntryReasonID] INNER JOIN LedgerEntryOrigins  EO
	ON		AL.[EntryOriginID] = EO.[EntryOriginID] INNER JOIN LedgerEntryTypes ET
	ON		AL.[EntryTypeID]   = ET.[LedgerEntryTypeID] LEFT OUTER JOIN BonusTypes BT
	ON		AL.[BonusTypeID]   = BT.[BonusTypeID] LEFT JOIN Disbursements DIT
	ON      AL.DisbursementID = DIT.DisbursementID LEFT JOIN DisbursementStatuses DIS
	ON      DIT.DisbursementStatusID = DIS.DisbursementStatusID
	WHERE	
			( @AccountID = 0   or AL.AccountID  = @AccountID )  AND
			( @PeriodID        = 0  or AL.PeriodID = @PeriodID )  AND
	--		/*AND
	--		(AL.BonusTypeID                = @BonusTypeID                      OR @BonusTypeID    = 0 )  AND
	--		(AL.EntryReasonID       = @EntryReasonID             OR @EntryReasonID  = 0)   AND
	--		(AL.EntryOriginID       = @EntryOriginID             OR @EntryOriginID  = 0 )  AND
	--		(AL.EntryTypeID                = @EntryTypeID                      OR @EntryTypeID    = 0 )  AND
	--		(AL.EntryAmount          = @EntryAmount                      OR @EntryAmount    = 0 )  AND
	--		(
	--			(@StartDate is not null and @EndDate is not null and CONVERT(DATE,AL.EffectiveDate)  between CONVERT(DATE,@StartDate,103) and CONVERT(DATE,@EndDate,103) )
	--			or
	--			(@StartDate is null and @EndDate is null )
	--		)*/

	--		/*  Modificado por HUNDRED =>Inicio : 25052017 => la consulta previa omitida los filtros */
		    
			(@BonusTypeID    = 0 or AL.BonusTypeID= @BonusTypeID)  AND
			(@EntryReasonID  = 0 or AL.EntryReasonID = @EntryReasonID )   AND
			(@EntryOriginID  = 0 or AL.EntryOriginID       = @EntryOriginID )  AND
			(@EntryTypeID    = 0 or AL.EntryTypeID = @EntryTypeID)  AND
			(@EntryAmount    = 0 or AL.EntryAmount= @EntryAmount)  AND

			/*  Modificado por HUNDRED =>inicio : 25052017 => la consulta previa omitida los filtros */
			 CONVERT(DATE,AL.EffectiveDate,103)  between  CONVERT(DATE,@StartDate,103)   and  (CONVERT(DATE,@EndDate,103))
			 --(CONVERT(DATE,AL.EffectiveDate,103)  between  @StartDate  and @EndDate)


	SET @RowsCount  = (SELECT COUNT(1) FROM #tmpFinal)
 --SET @RowsCount  = (SELECT top 1 MONTH(EntryDate) FROM #tmpFinal)
 
	SET @SQL= +
	 'SELECT ROW_NUMBER() OVER (ORDER BY '+ @Colum+ ' ' + CAST(@Order AS VARCHAR)+' ) AS RowMaster,
			 AccountID,
		     ConsultantName,
			 PeriodId,
			 BonusType,
			 EntryAmount,
			 EndingBalance,
			 EntryDate,
			 EffectiveDate,
			 UserID,
			 EntryNotes,
			 EntryReason,
			 EntryOrigin,
			 EntryType,
			 EntryDescription,
			 DisbursementID,
			 DisbursementStatusTermName
	  INTO #tmpResulFinal
	  FROM #tmpFinal 
  
	  SELECT   AccountID ,
			   ConsultantName,
			   PeriodId,
			   BonusType,
			   EntryAmount,
			   0,
			   EntryDate,
			   EffectiveDate,
			   UserID,
			   EntryNotes,
			   EntryReason,
			   EntryOrigin,
			   EntryType,
			   EntryDescription,
			   DisbursementID,
			   DisbursementStatusTermName
	  FROM #tmpResulFinal'
 
	IF @GetAll = 0
		SET @SQL += ' WHERE RowMaster BETWEEN '+ CAST((@PageSize * @PageNumber + 1) AS VARCHAR) + ' AND ' + CAST((@PageSize * (@PageNumber + 1)) AS VARCHAR) + '' 
 
	EXEC (@SQL)
 
	SET NOCOUNT OFF;
END