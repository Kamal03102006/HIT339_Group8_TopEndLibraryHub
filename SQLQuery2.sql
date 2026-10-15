
DECLARE @BackupPath nvarchar(4000);

SELECT TOP (1)
    @BackupPath =
        LEFT(
            physical_name,
            LEN(physical_name)
            - CHARINDEX('\', REVERSE(physical_name))
        )
        + '\TopEndLibraryHub_before_integration.bak'
FROM sys.database_files
WHERE type_desc = 'ROWS';

IF @BackupPath IS NULL
    THROW 50000, 'Could not determine backup path.', 1;

DECLARE @Sql nvarchar(max) =
    N'BACKUP DATABASE ' + QUOTENAME(DB_NAME())
    + N' TO DISK = N'''
    + REPLACE(@BackupPath, '''', '''''')
    + N''' WITH COPY_ONLY, INIT;';

EXEC (@Sql);

SELECT @BackupPath AS BackupFile;