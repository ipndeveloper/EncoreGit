
GO

DECLARE @CLIENT NVARCHAR(50) = '{CLIENT}' 
DECLARE @VERSION NVARCHAR(50) = '{VERSION}'
DECLARE @CODE NVARCHAR(50) = '{CODE}'
DECLARE @DESCRIPTION NVARCHAR(255) = '{DESCRIPTION}'
DECLARE @FILENAME NVARCHAR(255) = '{FILENAME}'
{DECLARESQL}

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
    
IF EXISTS (SELECT * 
               FROM [ver].[SchemaMigrations] 
               WHERE [Client] = @CLIENT
				 AND [Version] = @VERSION
				 AND [Code] = @CODE
				 AND [Succeeded] = 1
			   )
BEGIN

	PRINT 'Script "' + @FILENAME + '" already executed.'

	SET NOEXEC ON	
END
ELSE
BEGIN

	BEGIN TRAN SchemaUpdateTran{TranCount}

	PRINT 'Script "' + @FILENAME + '" executed.'
	
	SET NOEXEC OFF	
END

BEGIN TRY

{EXECUTESQL}
	
	--LOG SUCCESS
	INSERT [ver].[SchemaMigrations] ([Client], [Version], [Code], [Description], [Succeeded]) VALUES (@CLIENT, @VERSION, @CODE, @DESCRIPTION, 1)			
	
	COMMIT TRAN SchemaUpdateTran{TranCount}
	
END TRY	    
BEGIN CATCH

	DECLARE @ErrMsg NVARCHAR(4000) 
	DECLARE @ErrSeverity INT 

	SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY()

	IF @@TRANCOUNT > 0 
	BEGIN
		ROLLBACK TRAN SchemaUpdateTran{TranCount}
	END

	--LOG FAILED
	INSERT [ver].[SchemaMigrations] ([Client], [Version], [Code], [Description], [Succeeded], [ErrorMessage]) VALUES (@CLIENT, @VERSION, @CODE, @DESCRIPTION, 0, ERROR_MESSAGE())

	IF ( LEN(@ErrMsg) > 0 ) 
	BEGIN			
		RAISERROR(@ErrMsg, @ErrSeverity, 1) 
	END

END CATCH	    

SET NOEXEC OFF

GO

