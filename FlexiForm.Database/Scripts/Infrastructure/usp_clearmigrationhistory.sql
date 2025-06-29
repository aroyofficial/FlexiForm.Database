BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_ClearMigrationHistory;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_ClearMigrationHistory
    WITH ENCRYPTION
    AS
        BEGIN
            BEGIN TRY
                SET NOCOUNT ON;

                DELETE FROM
                tblMigrationHistory;

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
