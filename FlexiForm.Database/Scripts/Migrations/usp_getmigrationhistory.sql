BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_GetMigrationHistory
    WITH ENCRYPTION
    AS
        BEGIN
            BEGIN TRY
                SET NOCOUNT ON;

                SELECT Id,
                       RowId,
                       ScriptName,
                       ScriptHash,
                       [Type],
                       RanAt
                FROM tblMigrationHistory;

                SET NOCOUNT OFF;

                RETURN;
            END TRY
            BEGIN CATCH
                SET NOCOUNT OFF;
                DECLARE @L_ErrorMessage VARCHAR(512) = Error_message();
                RAISERROR(@L_ErrorMessage,16,1);
                RETURN;
            END CATCH
        END';

    EXEC sp_executesql @L_SQL;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
