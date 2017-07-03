﻿
USE {DBNAME}

DECLARE @CLIENT{UNIQUEID} NVARCHAR(50) = '{CLIENT}' 
DECLARE @VERSION{UNIQUEID} NVARCHAR(50) = '{VERSION}'
DECLARE @CODE{UNIQUEID} NVARCHAR(50) = '{CODE}'
DECLARE @DESCRIPTION{UNIQUEID} NVARCHAR(255) = '{DESCRIPTION}'
DECLARE @FILENAME{UNIQUEID} NVARCHAR(255) = '{FILENAME}'
{DECLARESQL}

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

IF EXISTS (SELECT * 
               FROM [ver].[SchemaMigrations] 
               WHERE [Client] = @CLIENT{UNIQUEID}
				 AND [Version] = @VERSION{UNIQUEID}
				 AND [Code] = @CODE{UNIQUEID}
				 AND [Succeeded] = 1
			   )
BEGIN

	PRINT 'ALREADY EXECUTED - "' + @FILENAME{UNIQUEID} + '".'

	SET NOEXEC ON
END
ELSE
BEGIN

	PRINT 'EXECUTED         - "' + @FILENAME{UNIQUEID} + '".'
	
	SET NOEXEC OFF
END

BEGIN TRY

{EXECUTESQL}
	
	--LOG SUCCESS	

	INSERT [ver].[SchemaMigrations] ([Client], [Version], [Code], [Description], [Succeeded]) VALUES (@CLIENT{UNIQUEID}, @VERSION{UNIQUEID}, @CODE{UNIQUEID}, @DESCRIPTION{UNIQUEID}, 1)

END TRY	    
BEGIN CATCH

	DECLARE @ErrMsg{UNIQUEID} NVARCHAR(4000) 
	DECLARE @ErrSeverity{UNIQUEID} INT 

	SELECT @ErrMsg{UNIQUEID} = ERROR_MESSAGE(), @ErrSeverity{UNIQUEID} = ERROR_SEVERITY()

	IF ( LEN(@ErrMsg{UNIQUEID}) > 0 ) 
		BEGIN			
			RAISERROR(@ErrMsg{UNIQUEID}, @ErrSeverity{UNIQUEID}, 1) 
		END
	ELSE
		BEGIN
			
			RAISERROR('UNKNOWN ERROR FOR SCRIPT', 1, 1) 

		END

	PRINT ''
	PRINT 'TRANSACTION ABORTED AT SCRIPT - ' + @DESCRIPTION{UNIQUEID}

	GOTO TRAN_ABORT

END CATCH	    

SET NOEXEC OFF
