DECLARE @recipients NVARCHAR(MAX), @contextKeys NVARCHAR(MAX), @messageKind int, @identityKind int, @ids NVARCHAR(MAX), @listSeperator NVARCHAR(1)
SET @recipients = '127.0.0.1:65000'
SET @messageKind = 1
--SET @identityKind = 0 --Int32
SET @identityKind = 1 --Int64
--SET @identityKind = 2 --String
--SET @identityKind = 3 --Guid
SET @contextKeys = 'Test,Test-Again'
SET @ids = N''
--DECLARE @i int
--SET @i = 0
--WHILE @i < 25000
--BEGIN
--	SELECT @i = @i + 1, @ids = @ids + CAST(@i as NVARCHAR(MAX)) + N',' -- Int32 or Int64 or String
--	--SELECT @i = @i + 1, @ids = @ids + CAST(NEWID() as NVARCHAR(MAX)) + N',' -- Guid or String
--END

--SET @ids = SUBSTRING(@ids, 0, LEN(@ids))
SET @ids = ''
SET @listSeperator = ','

EXEC dbo.Notify @recipients, @contextKeys, @messageKind, @identityKind, @ids, @listSeperator