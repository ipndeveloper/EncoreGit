-- This script will generate a SQL script to drop all the audit triggers re-create/update the CLR assembly in the DB and them recreate the Triggers. - JHE

DECLARE @NewLineChar AS VARCHAR(MAX) = CHAR(13) + CHAR(10)
	DECLARE @tableName sysname, @triggerName sysname, @createScript AS VARCHAR(MAX)
	DECLARE @createTriggerSQL AS VARCHAR(MAX) = '', @dropTriggerSQL AS VARCHAR(MAX) = '', @dropAssemblySQL AS VARCHAR(MAX) = ''

	DECLARE rename_cursor CURSOR FOR 
	WITH AllPKs AS (
		SELECT Name AS TriggerName, OBJECT_NAME(parent_id) AS TableName FROM sys.triggers WHERE type_desc = 'CLR_TRIGGER' AND Name like 'Audit%'
	)
	SELECT * FROM AllPKs

	OPEN rename_cursor

	FETCH NEXT FROM rename_cursor 
	INTO @triggerName, @tableName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @dropTriggerSQL = @dropTriggerSQL + ' DROP TRIGGER ' + @triggerName + @NewLineChar		
		SELECT @createTriggerSQL = @createTriggerSQL + ' GO ' + @NewLineChar + ' CREATE TRIGGER ' + @triggerName + ' ON [dbo].[' + @tableName + '] FOR INSERT,DELETE,UPDATE AS EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger' + @NewLineChar
			
	FETCH NEXT FROM rename_cursor 
	INTO @triggerName, @tableName
	END
	CLOSE rename_cursor
	DEALLOCATE rename_cursor
				
	-- STEP 1: Start the transaction
	BEGIN TRANSACTION
	
	SELECT @dropAssemblySQL = 'DROP ASSEMBLY NetStepsSql; ' + @NewLineChar + 'GO' + @NewLineChar + 'CREATE ASSEMBLY NetStepsSql FROM ''' + 'D:\SqlClr\NetSteps.Sql.dll' + ''' WITH PERMISSION_SET = SAFE;'
	
	DECLARE @SQL AS NVARCHAR(MAX)
	DECLARE @SQLScript AS VARCHAR(MAX) -- Just for printing more chars - JHE
	DECLARE @intTableCount INT
	SELECT @SQL = @dropTriggerSQL + @NewLineChar + @dropAssemblySQL + @NewLineChar + @NewLineChar + @createTriggerSQL
	SELECT @SQLScript = @SQL
	--SELECT @SQL
	PRINT '----- Running Script -----' + @NewLineChar + @NewLineChar
	PRINT @SQLScript + @NewLineChar
	PRINT '----- End of Script -----' + @NewLineChar + @NewLineChar
		
	--EXEC sp_executesql @SQL, N'@intTableCount INT OUTPUT', @intTableCount OUTPUT
	
	IF (@intTableCount <> 0)
	BEGIN
		PRINT 'Rolling Back the Transaction.'
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		COMMIT TRANSACTION
		PRINT 'Inserted Successfully.'
	END