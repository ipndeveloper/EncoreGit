--**************************************************************
--BEGIN TRANSACTION
--**************************************************************

UPDATE {COMM_DBNAME}.ver.SchemaMigrations SET Code = REPLICATE('0', 6 - len(Code)) + Code
UPDATE {MAIL_DBNAME}.ver.SchemaMigrations SET Code = REPLICATE('0', 6 - len(Code)) + Code
UPDATE {CORE_DBNAME}.ver.SchemaMigrations SET Code = REPLICATE('0', 6 - len(Code)) + Code

GO
PRINT '-- DISABLE COMMISSIONS UtTableSafety DB TRIGGER --'
USE [{COMM_DBNAME}]
GO
DISABLE TRIGGER [UtTableSafety] ON DATABASE
GO

BEGIN TRAN SQLTransMogrifierDeployment

--**************************************************************
--BEGIN TEMPLATE DATA
--**************************************************************

{TEMPLATEDATA}

--**************************************************************
--COMMIT TRANSACTION
--**************************************************************

	GOTO TRAN_COMMIT

TRAN_ABORT:

	PRINT ''
	PRINT '-- SCRIPT ABORTED --'

	IF (@@TRANCOUNT > 0)
	BEGIN

		PRINT ''
		PRINT 'ROLLBACK SQLTransMogrifierDeployment'

		ROLLBACK TRAN SQLTransMogrifierDeployment
	END

	GOTO FINISH

TRAN_COMMIT:

	IF @@TRANCOUNT > 0 
	BEGIN	
	
		PRINT ''
		PRINT 'COMMIT SQLTransMogrifierDeployment'
		
		COMMIT TRAN SQLTransMogrifierDeployment
	END

FINISH:

	PRINT ''
	PRINT '-- SCRIPT FINISHED --'

GO
PRINT '-- ENABLE COMMISSIONS UtTableSafety DB TRIGGER --'
USE [{COMM_DBNAME}]
GO
ENABLE TRIGGER [UtTableSafety] ON DATABASE
GO