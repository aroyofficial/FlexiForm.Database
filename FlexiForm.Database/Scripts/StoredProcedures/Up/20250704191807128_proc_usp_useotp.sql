-- Script Type    : proc
-- Name           : 20250704191807128_proc_usp_useotp.sql
-- Created At     : 2025-07-04 19:18:07 UTC (Arijit Roy)
-- Script ID      : 20250704191807128
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_UseOTP;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_UseOTP @value VARCHAR(64),
                                @userid UNIQUEIDENTIFIER
    WITH ENCRYPTION
    AS
    BEGIN
        BEGIN TRY
            SET NOCOUNT ON;

            DECLARE @L_Value VARCHAR(64) = @value;
            DECLARE @L_UserId UNIQUEIDENTIFIER = @userid;
            DECLARE @L_CurrentUTC DATETIME = GETUTCDATE();
            DECLARE @L_New TINYINT = 1;
            DECLARE @L_Consumed TINYINT = 2;

            UPDATE tblOTP
            SET Status = @L_Consumed,
                ConsumedAt = @L_CurrentUTC,
                UpdatedAt = @L_CurrentUTC,
                UpdatedBy = @L_UserId
            WHERE Value = @L_Value
            AND UserId = @L_UserId
            AND @L_CurrentUTC BETWEEN CreatedAt AND ExpiredAt
            AND Status = @L_New;

            SET NOCOUNT OFF;
        END TRY
        BEGIN CATCH
            SET NOCOUNT OFF;
            DECLARE @L_ErrorMessage VARCHAR(512) = Error_message();
            RAISERROR(@L_ErrorMessage, 16, 1);
        END CATCH
    END';

    EXEC sp_executesql @L_SQL;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
